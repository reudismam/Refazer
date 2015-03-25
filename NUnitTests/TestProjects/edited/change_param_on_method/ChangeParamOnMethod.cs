using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//e7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd
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
