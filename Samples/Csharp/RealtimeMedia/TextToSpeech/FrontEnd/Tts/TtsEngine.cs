/**
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*/

using System.IO;

namespace FrontEnd.Tts
{
    /// <summary>
    /// Interface for the TTS engine support
    /// </summary>
    internal interface ITtsEngine
    {
        /// <summary>
        /// Synthetize the text
        /// </summary>
        /// <param name="text">The string to synthetize</param>
        /// <returns>The stream containing the synthetized audio</returns>
        MemoryStream SynthesizeText(string text);
    }
}
