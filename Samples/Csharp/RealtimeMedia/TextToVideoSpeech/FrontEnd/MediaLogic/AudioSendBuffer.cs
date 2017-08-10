/**************************************************
*                                                                                    *
*   © Microsoft Corporation. All rights reserved.  *
*                                                                                    *
**************************************************/

using System;
using System.Runtime.InteropServices;
using FrontEnd.Logging;
using Microsoft.Skype.Bots.Media;


namespace FrontEnd.Media
{
    /// <summary>
    /// Creates an Audio Buffer for Send and also implements Dispose
    /// </summary>
    class AudioSendBuffer : AudioMediaBuffer
    {
        private bool _disposed;

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public AudioSendBuffer(IntPtr mediaBuffer, long length, AudioFormat format, long timeStamp)
        {
            Data = mediaBuffer;
            Length = length;
            AudioFormat = format;
            Timestamp = timeStamp;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {              
                Marshal.FreeHGlobal(Data);
            }

            _disposed = true;
        }
    }
}
