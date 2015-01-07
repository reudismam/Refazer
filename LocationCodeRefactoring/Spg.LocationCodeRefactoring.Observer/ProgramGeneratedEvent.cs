using Spg.LocationRefactor.Program;
using System.Collections.Generic;

namespace Spg.LocationCodeRefactoring.Observer
{
    public class ProgramGeneratedEvent
    {

        public List<Prog> programs { get; set; }

        public ProgramGeneratedEvent(List<Prog> programs) {
            this.programs = programs;
        }
    }
}