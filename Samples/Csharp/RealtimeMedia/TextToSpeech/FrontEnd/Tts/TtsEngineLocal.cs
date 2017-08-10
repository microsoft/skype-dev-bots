using System.IO;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using FrontEnd.Tts;

namespace FrontEnd.Tts
{
    /// <summary>
    /// TTS engine that uses the local Speech Synthesizer APIs, 
    /// see documentation: https://msdn.microsoft.com/en-us/library/hh362831(v=office.14).aspx
    /// </summary>
    internal class TtsEngineLocal : ITtsEngine
    {
        private readonly SpeechSynthesizer _synth;

        public TtsEngineLocal()
        {
            // initialize the Speech Synthesizer with a female voice
            _synth = new SpeechSynthesizer();
            _synth.SelectVoiceByHints(VoiceGender.Female);
        }
        
        public MemoryStream SynthesizeText(string text)
        {
            var audioStream = new MemoryStream();

            // set the synthesizer output to the stream, make sure the output format is matching the audio socket settings
            _synth.SetOutputToAudioStream(audioStream,
                new SpeechAudioFormatInfo(samplesPerSecond: 16000, bitsPerSample: AudioBitsPerSample.Sixteen, channel: AudioChannel.Mono));

            _synth.Speak(text);

            return audioStream;
        }
    }
}
