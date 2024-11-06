using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packfortune.Logic.Exceptions
{
    public class NegativePriceException : Exception
    {
        public NegativePriceException(string message) : base(message) { }
    }
}
