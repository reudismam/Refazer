using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//cc3d32746f60ed5a9f3775ef0ec44424b03d65cf

namespace ExampleProject.Company.ChangeException
{
    class ChangeException
    {
        [DebuggerHidden]
        public static void Fail(string message = "Unexpected")
        {
            throw new ContractFailureException(message);

        }

        [DebuggerHidden]
        public static T FailWithReturn<T>(string message = "Unexpected")
        {
            throw new ContractFailureException(message);
        }
    }
}
