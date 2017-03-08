using Controller.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public interface IEditFinishedObserver
    {
        void EditFinished(EditFinishedEvent @event);
    }
}
