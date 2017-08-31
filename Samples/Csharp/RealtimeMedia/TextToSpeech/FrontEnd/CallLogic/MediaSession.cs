/**
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*/

using FrontEnd.Http;
using FrontEnd.Logging;
using FrontEnd.Media;
using Microsoft.Bing.Speech;
using Microsoft.Skype.Bots.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CorrelationId = FrontEnd.Logging.CorrelationId;
using FrontEnd.Tts;

namespace FrontEnd.Call
{
    /// <summary>
    /// This class handles media related logic for a call.
    /// </summary>
    internal class MediaSession : IDisposable
    {
        #region Fields
        /// <summary>
        /// The audio socket created for this particular call.
        /// </summary>
        private readonly AudioSocket _audioSocket;

        /// <summary>
        /// Indicates if the call has been disposed
        /// </summary>
        private int _disposed;

        private readonly TaskCompletionSource<bool> _audioSendStatusActive;

        #endregion

        #region Properties
        /// <summary>
        /// The Id of this call.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The opaque media configuration object sent back to the Skype platform when answering a call.
        /// </summary>
        public JObject MediaConfiguration { get; private set; }
        
        #endregion

        #region Public Methods
        /// <summary>
        /// Create a new instance of the MediaSession.
        /// </summary>
        public MediaSession(string id, string correlationId)
        {
            this.Id = id;
            _audioSendStatusActive = new TaskCompletionSource<bool>();

            Log.Info(new CallerInfo(), LogContext.FrontEnd, $"[{this.Id}]: Call created");

            try
            {
                // create the audio socket
                _audioSocket = new AudioSocket(new AudioSocketSettings
                {
                    StreamDirections = StreamDirection.Sendrecv,
                    SupportedAudioFormat = AudioFormat.Pcm16K,
                    CallId = correlationId
                });

                Log.Info(new CallerInfo(), LogContext.FrontEnd, $"[{this.Id}]:Created AudioSocket");

                // subscribe to audio socket events
                _audioSocket.AudioSendStatusChanged += OnAudioSendStatusChanged;
                
                // create the mediaconfiguration with only audio channel
                MediaConfiguration = MediaPlatform.CreateMediaConfiguration(_audioSocket);

                Log.Info(new CallerInfo(), LogContext.FrontEnd, $"[{this.Id}]: MediaConfiguration={MediaConfiguration.ToString(Formatting.Indented)}");
                
                // async start the TTS task
                StartTTS().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error(new CallerInfo(), LogContext.FrontEnd, "Error in MediaSession creation" + ex.ToString());
                Dispose();
                throw;
            }
        }

        /// <summary>
        /// Unsubscribes all audio send/receive-related events, cancels tasks and disposes sockets
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
                {
                    Log.Info(new CallerInfo(), LogContext.FrontEnd, $"[{this.Id}]: Disposing Call");

                    if (_audioSocket != null)
                    {
                        _audioSocket.AudioSendStatusChanged -= OnAudioSendStatusChanged;
                        _audioSocket.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning(new CallerInfo(), LogContext.FrontEnd, $"[{this.Id}]: Ignoring exception in dispose {ex}");
            }
        }
        #endregion

        private async Task StartTTS()
        {
            // wait for the audio channel to be active
            await Task.WhenAll(_audioSendStatusActive.Task);

            // create an audio/video frame player with audio channel only
            AudioVideoFramePlayer audioVideoFramePlayer = new AudioVideoFramePlayer(_audioSocket, null, new AudioVideoFramePlayerSettings(new AudioSettings(20), new VideoSettings(), 1000));

            // list of AudioMediaBuffer to be sent to the player... we will populate this buffers with the audio coming from the two TTS engines
            var audioMediaBuffers = new List<AudioMediaBuffer>();
            // reference time tick for the audio buffers
            var referenceTimeTicks = DateTime.Now.Ticks;

            String text = "I'm completely operational, and all my circuits are functioning perfectly.";

            // create the local TTS engine
            ITtsEngine localTts = new TtsEngineLocal();
            referenceTimeTicks = populateAudioBuffersFromStream(localTts.SynthesizeText(text), audioMediaBuffers, referenceTimeTicks);

            // create the TTS service engine
            ITtsEngine serviceTts = new TtsEngineService();
            populateAudioBuffersFromStream(serviceTts.SynthesizeText(text), audioMediaBuffers, referenceTimeTicks);

            // enqueue the audio buffers, passing an empty list of video buffers since our player is audio only
            await audioVideoFramePlayer.EnqueueBuffersAsync(audioMediaBuffers, new List<VideoMediaBuffer>());
        }

        /// <summary>
        /// Create audio buffers from a stream and populate the given list starting at the reference time tick.
        /// </summary>
        /// <param name="stream">The stream to read audio from</param>
        /// <param name="audioBuffers">The list of audio buffers to be populated</param>
        /// <param name="referenceTimeTick">The reference starting time tick</param>
        private long populateAudioBuffersFromStream(Stream stream, List<AudioMediaBuffer> audioBuffers, long referenceTimeTick)
        {
            // a 20ms buffer is 640 bytes
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
                var audioBuffer = new AudioSendBuffer(unmanagedBuffer, bufferSize, AudioFormat.Pcm16K, referenceTimeTick);
                audioBuffers.Add(audioBuffer);
            }

            // return the reference time tick of the last buffer so we can queue new data if needed
            return referenceTimeTick;
        }


        /// <summary>
        /// Callback for informational updates from the media plaform about audio status changes.
        /// </summary>
        private void OnAudioSendStatusChanged(object sender, AudioSendStatusChangedEventArgs e)
        {
            Log.Info(
                new CallerInfo(),
                LogContext.Media,
                $"[{this.Id}]: AudioSendStatusChangedEventArgs(MediaSendStatus={e.MediaSendStatus})"
            );

            if (e.MediaSendStatus == MediaSendStatus.Active)
            {
                _audioSendStatusActive.SetResult(true);
            }
        }
    }
}
