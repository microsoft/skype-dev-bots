/**
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*/

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using FrontEnd.Logging;
using FrontEnd;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {        
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        /// Keep the service running until OnStop is called 
        /// </summary>
        public override void Run()
        {
            Log.Info(new CallerInfo(), LogContext.FrontEnd, "WorkerRole is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        /// <summary>
        /// Cleanup when WorkerRole is stopped
        /// </summary>
        public override void OnStop()
        {
            try
            {
                Log.Info(new CallerInfo(), LogContext.FrontEnd, "WorkerRole is stopping");

                this.cancellationTokenSource.Cancel();
                this.runCompleteEvent.WaitOne();

                base.OnStop();

                Log.Info(new CallerInfo(), LogContext.FrontEnd, "WorkerRole has stopped");
            }
            catch (Exception e)
            {
                Log.Error(new CallerInfo(), LogContext.FrontEnd, "Exception on shutdown: {0}", e.ToString());
                throw;
            }
            finally
            {
                AppDomain.CurrentDomain.UnhandledException -= this.OnAppDomainUnhandledException;
                TaskScheduler.UnobservedTaskException -= this.OnUnobservedTaskException;
                Log.Flush();
            }
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
            }
        }

        /// <summary>
        /// Log UnObservedTaskExceptions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Log.Error(new CallerInfo(), LogContext.FrontEnd, "Unobserved task exception: " + e.Exception.ToString());
        }

        /// <summary>
        /// Log any unhandled exceptions that are raised in the service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(new CallerInfo(), FrontEnd.Logging.LogContext.FrontEnd, "Unhandled exception: " + e.ExceptionObject.ToString());
            Log.Flush(); // process may or may not be terminating so flush log just in case.
        }

        /// <summary>
        /// Initialize and start the service when workerrole is started
        /// </summary>
        /// <returns></returns>
        public override bool OnStart()
        {

            try
            {
                // Wire up exception handling for unhandled exceptions (bugs).
                AppDomain.CurrentDomain.UnhandledException += this.OnAppDomainUnhandledException;
                TaskScheduler.UnobservedTaskException += this.OnUnobservedTaskException;

                // Set the maximum number of concurrent connections
                ServicePointManager.DefaultConnectionLimit = 12;
                AzureConfiguration.Instance.Initialize();

                // Create and start the environment-independent service.
                Service.Instance.Initialize(AzureConfiguration.Instance);
                Service.Instance.Start();

                bool result = base.OnStart();
                Log.Info(new CallerInfo(), LogContext.FrontEnd, "WorkerRole has been started");

                return result;
            }
            catch (Exception e)
            {
                Log.Error(new CallerInfo(), LogContext.FrontEnd, "Exception on startup: {0}", e.ToString());
                throw;
            }
        }

        // code below is from https://blogs.msdn.microsoft.com/windowsazurestorage/2014/05/26/persisting-connections-to-microsoft-azure-files/

        [DllImport("Mpr.dll", EntryPoint = "WNetAddConnection2", CallingConvention = CallingConvention.Winapi)]
        private static extern int WNetAddConnection2(NETRESOURCE lpNetResource,
                                    string lpPassword,
                                    string lpUsername,
                                    System.UInt32 dwFlags);

        [DllImport("Mpr.dll", EntryPoint = "WNetCancelConnection2", CallingConvention = CallingConvention.Winapi)]
        private static extern int WNetCancelConnection2(string lpName,
                                                        System.UInt32 dwFlags,
                                                        System.Boolean fForce);

        [StructLayout(LayoutKind.Sequential)]
        private class NETRESOURCE
        {
            public int dwScope;
            public ResourceType dwType;
            public int dwDisplayType;
            public int dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        };

        public enum ResourceType
        {
            RESOURCETYPE_DISK = 1,
        };
    }
}
