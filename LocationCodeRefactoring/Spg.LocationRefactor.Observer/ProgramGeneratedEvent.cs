using System.Collections.Generic;
using Spg.LocationRefactor.Program;

namespace Spg.LocationRefactor.Observer
{
    public class ProgramGeneratedEvent
    {

        public List<Prog> programs { get; set; }

        public ProgramGeneratedEvent(List<Prog> programs) {
            this.programs = programs;
        }
    }
}

