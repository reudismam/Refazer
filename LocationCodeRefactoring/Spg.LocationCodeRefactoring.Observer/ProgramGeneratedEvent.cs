using System.Collections.Generic;
using LocationCodeRefactoring.Spg.LocationRefactor.Program;

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