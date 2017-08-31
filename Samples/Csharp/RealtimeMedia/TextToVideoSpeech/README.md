# Text to Video Speech sample

## Description

The TextToVideoSpeech bot is a sample that generates audio and video streams on an Azure cloud service and streams the content to the user in a Skype video call. 

This sample is based on the [AudiovideoPlayerBot](https://github.com/Microsoft/BotBuilder-RealTimeMediaCalling/tree/master/Samples/AudioVideoPlayerBot) sample from the botFramework and the [TextToSpeech](https://github.com/Microsoft/skype-dev-bots/tree/master/Samples/Csharp/RealtimeMedia/TextToSpeech) bot .

## Test the bot

To add the bot to your account, use this link: https://join.skype.com/bot/cc7d0385-cd03-40e2-98c4-e967db719252
Once added, establish an audio call and you will see the bot avatar talking back to you.

## Deploy the bot sample
Prerequisites and instructions for deploying are [here](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-real-time-deploy-visual-studio). Update the configuration before deploying the sample per the instructions below.


## Update configuration

-	In app.config of the WorkerRole, replace *$BotHandle$*, *$MicrosoftAppId$* and *$BotSecret$* with values obtained during bot registration.

```xml
<appSettings>
    <!-- update these with your BotId, Microsoft App Id and your Microsoft App Password-->
    <add key="BotId" value="$BotHandle$"/>
    <add key="MicrosoftAppId" value="$MicrosoftAppId$"/>
    <add key="MicrosoftAppPassword" value="$BotSecret$"/>
 </appSettings>
```

-	Substitute the *$CertificateThumbprint$* in service configuration (```ServiceConfiguration.Cloud.cscfg``` file) with appropriate values in the config.
```xml
<Setting name="DefaultCertificate" value="$CertificateThumbprint$" />
```

## How it works:

The bot is based on the [TextToSpeech bot](https://github.com/Microsoft/skype-dev-bots/tree/master/Samples/Csharp/RealtimeMedia/TextToSpeech), we use the same [Speech Synthesis API](https://msdn.microsoft.com/en-us/library/hh362831(v=office.14).aspx) to generate an audio stream containing the synthesized audio and a timeline of *visemes* (a viseme is the visual equivalent of a phoneme – it defines the shape and position of your mouth and face when you make the sound associated with a phoneme). 

```csharp
// observe the synthesizer to generate the visemes timeline  
VisemesTimeline timeline = new VisemesTimeline();
_synth.VisemeReached += (sender, visemeReachedEventArgs) =>
{
    timeline.Add(visemeReachedEventArgs.Viseme, visemeReachedEventArgs.Duration.Milliseconds);
};
```

The visemes timeline is then used to generate the video buffers (full code in the [TTSEngine](https://github.com/Microsoft/skype-dev-bots/tree/master/Samples/Csharp/RealtimeMedia/TextToVideoSpeech/FrontEnd/Ttvs/TtvsEngine.cs) class).



```csharp
 private void CreateVideoBuffers(VisemesTimeline visemesTimeline, List<VideoMediaBuffer> videoBuffers, long referenceTimeTick)
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
    }
```