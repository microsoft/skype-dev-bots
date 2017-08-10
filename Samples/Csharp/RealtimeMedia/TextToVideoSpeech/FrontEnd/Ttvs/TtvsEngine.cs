using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using FrontEnd.Logging;
using FrontEnd.Media;
using Microsoft.Skype.Bots.Media;
using Microsoft.Skype.Internal.Bots.Media;

namespace FrontEnd.Ttvs
{
    /// <summary>
    /// TTVS engine that uses the local Speech Synthesizer APIs to generate audio and viseme ids, 
    /// see documentation: https://msdn.microsoft.com/en-us/library/hh362831(v=office.14).aspx
    /// </summary>
    internal class TtvsEngine
    {
        private readonly SpeechSynthesizer _synth;
        private readonly Dictionary<int, byte[]> _visemeBitmaps = new Dictionary<int, byte[]>();
        private readonly VideoFormat _videoFormat;

        public TtvsEngine(VideoFormat videoFormat)
        {
            // initialize the Speech Synthesizer with a female voice
            _synth = new SpeechSynthesizer();
            _synth.SelectVoiceByHints(VoiceGender.Female);
            _videoFormat = videoFormat;

            PreloadVisemes();
        }

        public void SynthesizeText(string text,
            List<AudioMediaBuffer> audioMediaBuffers,
            List<VideoMediaBuffer> videoMediaBuffers)
        {
            // stream for the output audio
            var audioStream = new MemoryStream();

            // set the synthesizer output to the stream, make sure the output format is matching the audio socket settings
            _synth.SetOutputToAudioStream(audioStream,
                new SpeechAudioFormatInfo(samplesPerSecond: 16000, bitsPerSample: AudioBitsPerSample.Sixteen,
                    channel: AudioChannel.Mono));

            // observe the synthesizer to generate the visemes timeline  
            VisemesTimeline timeline = new VisemesTimeline();
            _synth.VisemeReached += (sender, visemeReachedEventArgs) =>
            {
                timeline.Add(visemeReachedEventArgs.Viseme, visemeReachedEventArgs.Duration.Milliseconds);
            };

            // synthesize the text -> audio and visemes are generated
            _synth.Speak(text);

            // geneate the buffers and synchronize them with the current time
            long referenceTimeTick = DateTime.Now.Ticks;

            CreateAudioBuffers(audioStream, audioMediaBuffers, referenceTimeTick);
            CreateVideoBuffers(timeline, videoMediaBuffers, referenceTimeTick);
        }


        /// <summary>
        /// Create audio buffers from a stream and populate the given list starting at the reference time tick.
        /// </summary>
        /// <param name="stream">The stream to read audio from</param>
        /// <param name="audioBuffers">The list of audio buffers to be populated</param>
        /// <param name="referenceTimeTick">The reference starting time tick</param>
        private void CreateAudioBuffers(Stream stream, List<AudioMediaBuffer> audioBuffers, long referenceTimeTick)
        {
            // a 20ms buffer is 640 bytes at PCM16k
            int bufferSize = 640;
            byte[] bytesToRead = new byte[bufferSize];

            // reset the input stream to initial position
            stream.Position = 0;

            // create 20ms buffers from the input stream
            while (stream.Read(bytesToRead, 0, bytesToRead.Length) >= bufferSize)
            {
                IntPtr unmanagedBuffer = Marshal.AllocHGlobal(bufferSize);
                Marshal.Copy(bytesToRead, 0, unmanagedBuffer, bufferSize);

                // move the reference time by 20ms (there are 10.000 ticks in a milliseconds)
                referenceTimeTick += 20 * 10000;

                // create the audio buffer and add it to the list
                var audioBuffer = new AudioSendBuffer(unmanagedBuffer, bufferSize, AudioFormat.Pcm16K,
                    referenceTimeTick);
                audioBuffers.Add(audioBuffer);
            }
            Log.Info(
                new CallerInfo(),
                LogContext.Media,
                "created {0} AduioMediaBuffers frames", audioBuffers.Count);
        }


        /// <summary>
        /// Create video buffers from a viseme timeline and populate the given list starting at the reference time tick.
        /// </summary>
        /// <param name="visemesTimeline">The viseme timeline</param>
        /// <param name="videoBuffers">The list of video buffers to be populated</param>
        /// <param name="referenceTimeTick">The reference starting time tick</param>
        private void CreateVideoBuffers(VisemesTimeline visemesTimeline, List<VideoMediaBuffer> videoBuffers,
            long referenceTimeTick)
        {
            // compute the frame buffer size in bytes for the current video format
            var frameSize = (int) (_videoFormat.Width * _videoFormat.Height *
                                   Helper.GetBitsPerPixel(_videoFormat.VideoColorFormat) / 8);

            // compute the frame duration for the current framerate
            var frameDurationInMs = (int) (1000.0 / (double) _videoFormat.FrameRate);

            var durationInMs = 0;

            // create video frames for the whole viseme timeline lenght
            while (durationInMs < visemesTimeline.Length)
            {
                // get the current viseme
                byte[] visemeBitmap = _visemeBitmaps[visemesTimeline.Get(durationInMs)];

                // create the buffer
                IntPtr unmanagedBuffer = Marshal.AllocHGlobal(frameSize);
                Marshal.Copy(visemeBitmap, 0, unmanagedBuffer, frameSize);

                // increase the current duration by one frame
                durationInMs += frameDurationInMs;

                // create the video buffer and add it to the list
                var videoSendBuffer = new VideoSendBuffer(unmanagedBuffer, (uint) frameSize,
                    _videoFormat, referenceTimeTick + durationInMs * 10000);
                videoBuffers.Add(videoSendBuffer);
            }

            Log.Info(
                new CallerInfo(),
                LogContext.Media,
                "created {0} VideoMediaBuffers frames", videoBuffers.Count);
        }


        /// <summary>
        /// Preload the byte array for the visemes.
        /// Full list of viseme ids here https://msdn.microsoft.com/en-us/library/office/system.speech.synthesis.speechsynthesizer.visemereached.aspx
        /// </summary>
        private void PreloadVisemes()
        {
            _visemeBitmaps.Add(0, Utilities.BitmapToByteArray(Properties.Resources.avatar, _videoFormat));

            byte[] viseme = Utilities.BitmapToByteArray(Properties.Resources.avatar_AH, _videoFormat);
            _visemeBitmaps.Add(1, viseme);
            _visemeBitmaps.Add(2, viseme);
            _visemeBitmaps.Add(9, viseme);
            _visemeBitmaps.Add(11, viseme);

            viseme = Utilities.BitmapToByteArray(Properties.Resources.avatar_OOH, _videoFormat);
            _visemeBitmaps.Add(3, viseme);
            _visemeBitmaps.Add(8, viseme);
            _visemeBitmaps.Add(10, viseme);
            
            viseme = Utilities.BitmapToByteArray(Properties.Resources.avatar_EH, _videoFormat);
            _visemeBitmaps.Add(4, viseme);
            _visemeBitmaps.Add(5, viseme);
            _visemeBitmaps.Add(6, viseme);
            _visemeBitmaps.Add(20, viseme);

            viseme = Utilities.BitmapToByteArray(Properties.Resources.avatar_FV, _videoFormat);
            _visemeBitmaps.Add(7, viseme);
            _visemeBitmaps.Add(18, viseme);
            
            viseme = Utilities.BitmapToByteArray(Properties.Resources.avatar_S_D_H_Z, _videoFormat);
            _visemeBitmaps.Add(12, viseme);
            _visemeBitmaps.Add(13, viseme);
            _visemeBitmaps.Add(15, viseme);
            _visemeBitmaps.Add(16, viseme);

            viseme = Utilities.BitmapToByteArray(Properties.Resources.avatar_L_TH, _videoFormat);
            _visemeBitmaps.Add(17, viseme);
            _visemeBitmaps.Add(14, viseme);
            _visemeBitmaps.Add(19, viseme);

            _visemeBitmaps.Add(21, Utilities.BitmapToByteArray(Properties.Resources.avatar_MM, _videoFormat));
        }
    }
}