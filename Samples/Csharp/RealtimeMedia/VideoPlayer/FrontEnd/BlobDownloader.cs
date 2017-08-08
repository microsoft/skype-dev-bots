using FrontEnd.Logging;
using FrontEnd.Media;
using Microsoft.Skype.Bots.Media;
using Microsoft.Skype.Internal.Bots.Media;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FrontEnd
{
    internal class BlobDownloader
    {
        private CloudBlockBlob _videoBlob;
        private CloudBlockBlob _audioBlob;
        private uint _nbSecondToLoad;

        private VideoFormat _videoFormat;
        private AudioFormat _audioFormat;

        private int _videoOffset;
        private int _audioOffset;

        private int _frameSize;
        private long _frameDurationInTicks;

        public BlobDownloader(uint nbSecondToLoad)
        {
            _nbSecondToLoad = nbSecondToLoad;
            _videoOffset = 0;
            _audioOffset = 44;

            _videoFormat = VideoFormat.NV12_640x360_30Fps;
            Log.Info(new CallerInfo(), LogContext.FrontEnd, $"Video format used {_videoFormat}");

            _audioFormat = AudioFormat.Pcm16K;
            Log.Info(new CallerInfo(), LogContext.FrontEnd, $"Audio format used {_audioFormat}");

            _frameSize = GetFrameSize(_videoFormat);
            _frameDurationInTicks = GetFrameDurationInTicks(_videoFormat);
        }

        public void ConnectToStorageAccount()
        {
            var config = Service.Instance.Configuration;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config.StorageAccountConnection);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            
            CloudBlobContainer videoContainer = client.GetContainerReference(config.VideoContainer);            
            CloudBlobContainer audioContainer = client.GetContainerReference(config.AudioContainer);

            _videoBlob = videoContainer.GetBlockBlobReference(Service.Instance.Configuration.VideoFile);
            _audioBlob = audioContainer.GetBlockBlobReference(Service.Instance.Configuration.AudioFile);
        }


        public List<VideoMediaBuffer> GetVideoMediaBuffers(long currentTick)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            // 1. Downlaod _nbSecondToLoad seconds of video content from the storage account
            long bufferSize = _frameSize * _videoFormat.FrameRate * _nbSecondToLoad;
            byte[] bytesToRead = new byte[bufferSize];
            var nbByteRead = _videoBlob.DownloadRangeToByteArray(bytesToRead, 0, _videoOffset, bytesToRead.Length, null, null);

            //2. Extract each video frame in a VideoMediaBuffer object
            List<VideoMediaBuffer> videoMediaBuffers = new List<VideoMediaBuffer>();

            long referenceTime = currentTick;
            for (int index = 0; index < nbByteRead; index += _frameSize)
            {
                IntPtr unmanagedBuffer = Marshal.AllocHGlobal(_frameSize);
                Marshal.Copy(bytesToRead, index, unmanagedBuffer, _frameSize);
                referenceTime += _frameDurationInTicks;

                var videoSendBuffer = new VideoSendBuffer(unmanagedBuffer, (uint)_frameSize, _videoFormat, referenceTime);
                videoMediaBuffers.Add(videoSendBuffer);
                
                _videoOffset += _frameSize;
            }
            Log.Info(new CallerInfo(), LogContext.FrontEnd, $"Loading {_nbSecondToLoad}s video took {watch.ElapsedMilliseconds}ms ({_frameSize * _videoFormat.FrameRate * _nbSecondToLoad} bytes)");

            watch.Stop();

            return videoMediaBuffers;
        }
   
        public List<AudioMediaBuffer> GetAudioMediaBuffers(long currentTick)
        {            
            Stopwatch watch = new Stopwatch();
            watch.Start();  

            // 1. Downlaod _nbSecondToLoad seconds of audio content from the storage account    
            long bufferSize = 16000 * 2 * _nbSecondToLoad; // Pcm16K is 16000 samples per seconds, each sample is 2 bytes 
            byte[] bytesToRead = new byte[bufferSize];
            var nbByteRead = _audioBlob.DownloadRangeToByteArray(bytesToRead, 0, _audioOffset, bytesToRead.Length, null, null);

            //2. Extract each audio sample in a AudioMediaBuffer object
            List<AudioMediaBuffer> audioMediaBuffers = new List<AudioMediaBuffer>();

            int audioBufferSize = (int)(16000 * 2 * 0.02); // the Real-time media platform expects audio buffer duration of 20ms        
            long referenceTime = currentTick;
            for (int index = 0; index < nbByteRead; index += audioBufferSize)
            {
                IntPtr unmanagedBuffer = Marshal.AllocHGlobal(audioBufferSize);
                Marshal.Copy(bytesToRead, index, unmanagedBuffer, audioBufferSize);
                // 10000 ticks in a ms
                referenceTime += 20 * 10000;

                var audioBuffer = new AudioSendBuffer(unmanagedBuffer, audioBufferSize, _audioFormat, referenceTime);
                audioMediaBuffers.Add(audioBuffer);
                
                _audioOffset += audioBufferSize;
            }
            Log.Info(new CallerInfo(), LogContext.FrontEnd, $"Loading {_nbSecondToLoad}s audio took {watch.ElapsedMilliseconds}ms ({16000 * 2 * _nbSecondToLoad} bytes)");

            watch.Stop();

            return audioMediaBuffers;
        }

        private static int GetFrameSize(VideoFormat videoFormat)
        {
            return (int)(videoFormat.Width * videoFormat.Height * Helper.GetBitsPerPixel(videoFormat.VideoColorFormat) / 8);
        }

        private static long GetFrameDurationInTicks(VideoFormat videoFormat)
        {
            return (long)((1000.0 / (double)videoFormat.FrameRate) * 10000.0);
        }
    }
}
