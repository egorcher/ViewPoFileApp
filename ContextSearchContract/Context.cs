namespace ContextSearchContract
{
    public class Context
    {
        public string Title { get; set; }
        public CustomFile[] Files { get; set; }
        public Context Next { get; set; }
    }
}
