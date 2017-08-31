/**
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*/

using System;
using System.IO;
using System.Net;
using System.Text;
using FrontEnd.Http;

namespace FrontEnd.Tts
{
    /// <summary>
    /// TTS engine that uses the Bing Cognitive Service Text to Speech APIs, 
    /// see documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/speech/api-reference-rest/bingvoiceoutput
    /// </summary>
    internal class TtsEngineService : ITtsEngine
    {
        private const string RequestUri = @"https://speech.platform.bing.com/synthesize";
        // key and name of the Bing Speech Api azure subscription
        private const string BingSpeechApiKey = @"f094c9c69b294f66874101dfa83e6136";
        private const string BingSpeechAppName = @"ttsbot";
        // make sure the output format is matching the audio socket settings
        private const string AudioOutputFormat = @"raw-16khz-16bit-mono-pcm";

        // access token
        private readonly string _token;
        // an ID that uniquely identifies the client application. 
        private readonly string _appId = Guid.NewGuid().ToString().Replace("-", "");
        // an ID that uniquely identifies an application instance for each installation.
        private readonly string _clientId = Guid.NewGuid().ToString().Replace("-", "");


        public TtsEngineService()
        {
            // retrieve the access token
            var authentication = new CognitiveServicesAuthentication(BingSpeechApiKey);
            _token = authentication.GetAccessToken();
        }

        public MemoryStream SynthesizeText(string text)
        {
            // create the http request to the Speech Api service
            var request = (HttpWebRequest) HttpWebRequest.Create(RequestUri);

            request.Method = "POST";
            request.ProtocolVersion = HttpVersion.Version11;
            request.ContentType = "application/ssml+xml";
            request.UserAgent = BingSpeechAppName;
            request.Headers["X-Microsoft-OutputFormat"] = AudioOutputFormat;
            request.Headers["X-Search-AppId"] = _appId;
            request.Headers["X-Search-ClientID"] = _clientId;
            request.Headers["Authorization"] = "Bearer " + _token;

            // build the SSML string for the given input, 
            // see reference here https://www.w3.org/TR/speech-synthesis/
            var ssmlBuilder = new StringBuilder();
            ssmlBuilder.Append(@"<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xmlns:mstts='http://www.w3.org/2001/mstts' xml:lang='en-US'>");
                ssmlBuilder.Append("<voice xml:lang='en-US' name='Microsoft Server Speech Text to Speech Voice (en-US, JessaRUS)'>");
                    ssmlBuilder.Append(text);
                ssmlBuilder.Append("</voice>");
            ssmlBuilder.Append("</speak>");

            // send the request
            byte[] requestBody = Encoding.UTF8.GetBytes(ssmlBuilder.ToString());
            using (var stream = request.GetRequestStream())
            {
                stream.Write(requestBody, 0, requestBody.Length);
                stream.Flush();
            }

            // copy the response stream to the local memory stream
            var audioStream = new MemoryStream();
            var responseStream = request.GetResponse().GetResponseStream();
           
            responseStream?.CopyTo(audioStream);

            return audioStream;
        }
    }
}
