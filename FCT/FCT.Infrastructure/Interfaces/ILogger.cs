using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.Infrastructure.Interfaces
{
    public interface ILogger
    {
        void Verbose(string message);

        void Debug(string message);

        void Information(string message);

        void Warining(string message);

        void Warning(Exception ex, string message);

        void Error(string message);

        void Error(Exception ex, string message);

        void Fatal(string message);

        void Fatal(Exception ex, string message);
    }
}
