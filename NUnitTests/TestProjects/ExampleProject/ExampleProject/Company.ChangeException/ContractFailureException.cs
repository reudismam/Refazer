using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExampleProject.Company.ChangeException
{
    class ContractFailureException : Exception
    {
        public ContractFailureException(string message)
        {
            // TODO: Complete member initialization
            this.Message = message;
        }
    }
}
