# VideoPlayer sample

## Description

Video Player bot is a sample playing a video and audio files stored as an Azure blob.

This sample is based on the AudiovideoPlayerBot sample from the botFramework (https://github.com/Microsoft/BotBuilder-RealTimeMediaCalling/tree/master/Samples/AudioVideoPlayerBot).

## Test the bot

To add the bot to your account, use this link: https://join.skype.com/bot/87310219-6701-44cd-986e-14514a330aff
Once added, start a video or audio call with the bot. The bot will begin streaming the video and audio from the Azure blobs specified in the configuration.

## Deploy the bot sample
Prerequisites and instructions for deploying are [here](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-real-time-deploy-visual-studio). Update the configuration before deploying the sample per the instructions above.

## Update Config

-	In app.config of the WorkerRole, replace $BotHandle$, $MicrosoftAppId$ and $BotSecret$ with values obtained during bot registration.

```xml
<appSettings>
    <!-- update these with your BotId, Microsoft App Id and your Microsoft App Password-->
    <add key="BotId" value="$BotHandle$"/>
    <add key="MicrosoftAppId" value="$MicrosoftAppId$"/>
    <add key="MicrosoftAppPassword" value="$BotSecret$"/>
 </appSettings>
```

-	Substitute the $xxx$ in service configuration (ServiceConfiguration.Cloud.cscfg file) with appropriate values in the config.
```xml
<Setting name="DefaultCertificate" value="$CertificateThumbprint$" />
```

-   Modify the video and audio files name and location (container) to point to the wav file for audio and YUV file for video
```xml
<Setting name="AudioContainer" value="audio" />
<Setting name="AudioFile" value="$audioFileName$" />
<Setting name="VideoContainer" value="video" />
<Setting name="VideoFile" value="$videoFileName$" />
```

## How it works:

Video frames and audio samples are donwloaded from the Azure storage account through the blob API. 
In this example we downlaod 2s of video and 2s of audio then send those frames and samples directly to the audio/video sockets.
The cloud service never stores on disk the actual data. This make it easier to scale and update content.
The quality of the video is limited by the bandwidth available between the cloud service (the bot) and the storage account.
In this example we use:
Video (NV12): 640x360 at 30fps
Audio: PCM 16k

The location of the video and audio files (and the containers) are specified in the ServiceConfiguration.Cloud.cscfg file (see "Update config" section).

To convert an existing video to raw video et and audio format, you can use ffmpeg tool (https://www.ffmpeg.org/)

Command to convert the video: 
```
ffmpeg -i myvideo.mp4 -s 640x360 -r 30 -f rawvideo -pix_fmt nv12 video.yuv
```

Command to convert the audio: 
```
ffmpeg -i myvideo.mp4 -acodec pcm_s16le -ac 1 -ar 16000 audio.wav
```