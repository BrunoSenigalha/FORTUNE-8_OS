using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Utilitaries
{
    public class ValidateDecimalImput
    {
        public decimal ValidateCredits()
        {
            decimal credits;
            bool validImput;
            do
            {
                validImput = decimal.TryParse(Console.ReadLine(), out credits);

                if (!validImput)
                {
                    throw new InvalidOperationException("Wrong value for credits, please type again.");
                }
            } while (!validImput);
            return credits;
        }
    }
}
