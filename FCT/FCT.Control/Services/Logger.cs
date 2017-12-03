using Serilog;
using System;

namespace FCT.Control.Services
{
    public class Logger: Infrastructure.Interfaces.ILogger
    {
        public Logger()
        {
            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.AppSettings()
                        .CreateLogger();
        }

        public void Verbose(string message)
        {
            Log.Verbose(message);
        }

        public void Debug(string message)
        {
            Log.Debug(message);
        }

        public void Information(string message)
        {
            Log.Information(message);
        }

        public void Warining(string message)
        {
            Log.Warning(message);
        }

        public void Warning(Exception ex, string message)
        {
            Log.Warning(ex, message);
        }

        public void Error(string message)
        {
            Log.Error(message);
        }

        public void Error(Exception ex, string message)
        {
            Log.Error(ex, message);
        }

        public void Fatal(string message)
        {
            Log.Fatal(message);
        }

        public void Fatal(Exception ex, string message)
        {
            Log.Fatal(ex, message);
        }
    }
}
