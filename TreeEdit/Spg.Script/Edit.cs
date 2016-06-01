namespace TreeEdit.Spg.Script
{
    public class Edit<T>
    {
        public EditOperation<T> EditOperation { get; set; }

        public Edit(EditOperation<T> operation)
        {
            EditOperation = operation;
        }
    }
}
