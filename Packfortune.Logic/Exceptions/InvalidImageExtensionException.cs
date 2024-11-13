using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packfortune.Logic.Exceptions
{
    public class InvalidImageExtensionException : Exception
    {
        public InvalidImageExtensionException(string message) : base(message) { }
    }
}
