using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComplianceEnforcer.Exceptions
{
    internal class MissingStepInCodeException : InvalidOperationException
    {
        public MissingStepInCodeException(string message) : base(message)
        {

        }
    }
}
