using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS_Tests.Utilitaries
{
    public class ConsoleInputCapture : IDisposable
    {
        private readonly System.IO.StringReader stringReader;
        private readonly System.IO.TextReader originalInput;

        public ConsoleInputCapture(params string[] inputLines)
        {
            stringReader = new System.IO.StringReader(string.Join(Environment.NewLine, inputLines));
            originalInput = Console.In;
            Console.SetIn(stringReader);
        }

        public void Dispose()
        {
            Console.SetIn(originalInput);
            stringReader.Dispose();
        }
    }
}
