namespace Spg.LocationCodeRefactoring.Observer
{
    public class ProgramRefactoredEvent
    {
        public string transformation { get; set; }

        public ProgramRefactoredEvent(string transformation) {
            this.transformation = transformation;
        }
    }
}