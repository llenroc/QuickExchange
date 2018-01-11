using System.Diagnostics;
using System.IO;
using System.Text;

namespace CryptoTools.Core.Api.Binance.Logging
{
    internal class TraceTextWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.ASCII;

        public override void WriteLine(string line)
        {
            Trace.Write(line);
        }
    }
}
