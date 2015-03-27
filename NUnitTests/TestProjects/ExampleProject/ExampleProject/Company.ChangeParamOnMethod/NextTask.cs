using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExampleProject.Company.ChangeParamOnMethod
{
    class NextTask
    {
        internal void ContinueWith(Func<System.Threading.Tasks.Task, object, TResult> ReportFatalError, object continuationFunction, System.Threading.CancellationToken cancellationToken, object p, System.Threading.Tasks.TaskScheduler taskScheduler)
        {
            throw new NotImplementedException();
        }
    }
}
