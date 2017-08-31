/**
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FrontEnd.Logging;
using FrontEnd.Ttvs;
using Microsoft.Skype.Bots.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FrontEnd.CallLogic
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
        /// The video socket created for this particular call.
        /// </summary>
        private readonly VideoSocket _videoSocket;

        /// <summary>
        /// Indicates if the call has been disposed
        /// </summary>
        private int _disposed;

        private readonly TaskCompletionSource<bool> _audioSendStatusActive;
        private readonly TaskCompletionSource<bool> _videoSendStatusActive;

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

        private readonly VideoFormat _defaultVideoFormat = VideoFormat.Rgb24_1280x720_30Fps;
        
        #endregion

        #region Public Methods
        /// <summary>
        /// Create a new instance of the MediaSession.
        /// </summary>
        public MediaSession(string id, string correlationId)
        {
            this.Id = id;
            _audioSendStatusActive = new TaskCompletionSource<bool>();
            _videoSendStatusActive = new TaskCompletionSource<bool>();

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
                _videoSocket = new VideoSocket(new VideoSocketSettings
                {
                    StreamDirections = StreamDirection.Sendrecv,
                    ReceiveColorFormat = VideoColorFormat.NV12,
                    
                    SupportedSendVideoFormats = new List<VideoFormat>() {
                        _defaultVideoFormat
                    },

                    CallId = correlationId
                });

                // subscribe to audio socket events
                _audioSocket.AudioSendStatusChanged += OnAudioSendStatusChanged;
                _videoSocket.VideoSendStatusChanged += OnVideoSendStatusChanged;

                // create the mediaconfiguration
                MediaConfiguration = MediaPlatform.CreateMediaConfiguration(_audioSocket, _videoSocket);

                Log.Info(new CallerInfo(), LogContext.FrontEnd, $"[{this.Id}]: MediaConfiguration={MediaConfiguration.ToString(Formatting.Indented)}");
               

                // async start the TTS task
                StartTTVS().ConfigureAwait(false);
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
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 1)
            {
                return;
            }

            try
            {
                Log.Info(new CallerInfo(), LogContext.FrontEnd, "Disposing Call with Id={0}.", Id);

               
                if (_audioSocket != null)
                {
                    _audioSocket.AudioSendStatusChanged -= OnAudioSendStatusChanged;
                    _audioSocket.Dispose();
                }

                if (_videoSocket != null)
                {
                    _videoSocket.VideoSendStatusChanged -= OnVideoSendStatusChanged;
                    _videoSocket.Dispose();
                }
                

                Log.Info(new CallerInfo(), LogContext.FrontEnd, "disposed videoMediaBuffers Id={0}.", Id);
            }
            catch (Exception ex)
            {
                Log.Warning(new CallerInfo(), LogContext.FrontEnd, "Ignoring exception in dispose" + ex);
            }
        }
        #endregion

        private AudioVideoFramePlayer audioVideoFramePlayer;
        private async Task StartTTVS()
        {
            // wait for both the audio and video channels to be active
            await Task.WhenAll(_audioSendStatusActive.Task, _videoSendStatusActive.Task);

            // create an audio/video frame player
            audioVideoFramePlayer = new AudioVideoFramePlayer(_audioSocket, _videoSocket, new AudioVideoFramePlayerSettings(new AudioSettings(20), new VideoSettings(), 1000));
            
            // lists of buffers to be sent to the player... we will populate this buffers with the audio/video coming from the TTVS engines
            var audioMediaBuffers = new List<AudioMediaBuffer>();
            var videoMediaBuffers = new List<VideoMediaBuffer>();

            // create the TTVS engine and generate the media buffers
            TtvsEngine ttvsEngine = new TtvsEngine(_defaultVideoFormat);

            string welcomeText = "I'm completely operational, and all my circuits are functioning perfectly!";
            ttvsEngine.SynthesizeText(welcomeText, audioMediaBuffers, videoMediaBuffers);
            
            await audioVideoFramePlayer.EnqueueBuffersAsync(audioMediaBuffers, videoMediaBuffers);
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

        /// <summary>
        /// Callback for informational updates from the media plaform about video status changes. 
        /// Once the Status becomes active, then video can be sent.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVideoSendStatusChanged(object sender, VideoSendStatusChangedEventArgs e)
        {
            Log.Info(new CallerInfo(), LogContext.Media, "OnVideoSendStatusChanged start");

            Log.Info(
                new CallerInfo(),
                LogContext.Media,
                "[VideoSendStatusChangedEventArgs(MediaSendStatus=<{0}>;PreferredVideoSourceFormat=<{1}>]",
                e.MediaSendStatus,
                e.PreferredVideoSourceFormat.VideoColorFormat);

            if (e.MediaSendStatus == MediaSendStatus.Active)
            {
                _videoSendStatusActive.SetResult(true);
            }
        }
    }
}
