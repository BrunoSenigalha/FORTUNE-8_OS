using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS_Tests.Utilitaries
{
    public class ConsoleOutputCapture : IDisposable
    {
        private readonly System.IO.StringWriter stringWriter;
        private readonly System.IO.TextWriter originalOutput;

        public ConsoleOutputCapture()
        {
            stringWriter = new System.IO.StringWriter();
            originalOutput = Console.Out;
            Console.SetOut(stringWriter);
        }

        public void Dispose()
        {
            Console.SetOut(originalOutput);
            stringWriter.Dispose();
        }
    }
}
