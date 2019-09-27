using System;
using System.Diagnostics;
using log4net;

namespace Plateforme.AlloTabib.CrossCuttingLayer.Logging
{
    public static class Logger
    {
        private static object _lock = new object();

        public static ILog Log
        {
            get
            {
                var callStack = new StackTrace(true);
                var stackFrame = callStack.GetFrame(2);
                GlobalContext.Properties["f"] = stackFrame.GetFileName();
                GlobalContext.Properties["l"] = stackFrame.GetFileLineNumber();
                GlobalContext.Properties["c"] = stackFrame.GetFileColumnNumber();
                GlobalContext.Properties["pid"] = Process.GetCurrentProcess().Id;

                var machineName = Environment.MachineName;
                var getHostName = System.Net.Dns.GetHostName();
                var computerName = System.Environment.GetEnvironmentVariable("COMPUTERNAME");

                return LogManager.GetLogger(String.Format("{0}::{1} :: {2} {3} {4} {5}", stackFrame.GetMethod().DeclaringType, stackFrame.GetMethod().Name, "Machine - DNS - Computer name :", machineName, getHostName, computerName));
            }
        }

        public static void LogInfo(string message)
        {
            lock (_lock)
            {
                Log.Info(message);
            }

        }

        public static void LogInfo(string format, params object[] args)
        {
            lock (_lock)
            {
                Log.Info(String.Format(format, args));
            }
        }

        public static void LogExceptionInfo(string message, Exception exception)
        {
            lock (_lock)
            {
                Log.Info(message, exception);
            }
        }

        public static void LogError(string message)
        {
            lock (_lock)
            {
                Log.Error(message);
            }
        }
        public static void LogError(string format, params object[] args)
        {
            lock (_lock)
            {
                Log.Error(String.Format(format, args));
            }
        }
        public static void LogExceptionError(string message, Exception exception)
        {
            lock (_lock)
            {
                Log.Error(message, exception);
            }
        }

        public static void LogFatal(string message)
        {
            lock (_lock)
            {
                Log.Fatal(message);
            }
        }

        public static void LogFatal(string format, params object[] args)
        {
            lock (_lock)
            {
                Log.Fatal(String.Format(format, args));
            }
        }

        public static void LogExceptionFatal(string message, Exception exception)
        {
            lock (_lock)
            {
                Log.Fatal(message, exception);
            }
        }

        public static void LogWarn(string message)
        {
            lock (_lock)
            {
                Log.Warn(message);
            }
        }

        public static void LogWarn(string format, params object[] args)
        {
            lock (_lock)
            {
                Log.Warn(String.Format(format, args));
            }
        }

        public static void LogExceptionWarn(string message, Exception exception)
        {
            lock (_lock)
            {
                Log.Warn(message, exception);
            }
        }

        public static void LogDebug(string message)
        {
            lock (_lock)
            {
                Log.Debug(message);
            }
        }

        public static void LogDebug(string format, params object[] args)
        {
            lock (_lock)
            {
                Log.Debug(String.Format(format, args));
            }
        }

        public static void LogExceptionDebug(string message, Exception exception)
        {
            lock (_lock)
            {
                Log.Debug(message, exception);
            }
        }
    }
}
