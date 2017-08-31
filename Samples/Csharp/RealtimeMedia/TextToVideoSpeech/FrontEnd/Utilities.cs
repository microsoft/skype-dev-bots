/**
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FrontEnd.Logging;
using Microsoft.Skype.Bots.Media;

namespace FrontEnd
{
    internal static class Utilities
    {
        /// <summary>
        /// Extension for Task to execute the task in background and log any exception
        /// </summary>
        /// <param name="task"></param>
        /// <param name="description"></param>
        public static async void ForgetAndLogException(this Task task, string description = null)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                //ignore
                Log.Error(new CallerInfo(),
                    LogContext.FrontEnd,
                    "Caught an Exception running the task: {0} \n StackTrace: {1}", e.Message, e.StackTrace);
            }
        }
        /// <summary>
        /// Convert the bitmap to a byte array
        /// </summary>
        public static byte[] BitmapToByteArray(Bitmap inputBitmap, VideoFormat videoFormat)
        {
            // resize bitmap to match the videoformat
            Bitmap bmp = new Bitmap(inputBitmap, videoFormat.Width, videoFormat.Height);

            BitmapData bData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            int lineSize = bData.Width * 3;
            int byteCount = lineSize * bData.Height;
            byte[] bytes = new byte[byteCount];

            IntPtr scan = bData.Scan0;
            for (int i = 0; i < bData.Height; i++)
            {
                Marshal.Copy(scan, bytes, i * lineSize, lineSize);
                scan += bData.Stride;
            }
            return bytes;
        }
        
    }
}
