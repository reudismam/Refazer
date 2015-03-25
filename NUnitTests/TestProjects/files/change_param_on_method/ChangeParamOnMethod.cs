using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExampleProject.Company.ChangeParamOnMethod
{
    class ChangeParamOnMethod
    {
        private NextTask nextTask;
        private Task t;
        public void Method()
        {
            nextTask.ContinueWith(ReportFatalError, continuationFunction,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default);

            nextTask.ContinueWith(ReportFatalError, continuationFunction,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default);
        }

        private TResult ReportFatalError(Task arg1, object arg2)
        {
            throw new NotImplementedException();
        }

        public object continuationFunction { get; set; }
    }
}
