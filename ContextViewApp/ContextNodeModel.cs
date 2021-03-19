using System.Collections.ObjectModel;

namespace ContextViewApp
{
    public class ContextNodeModel
    {
        public string Title { get; set; }
        public ObservableCollection<ContextNodeModel> Next { get; set; }
        public string Content { get; set; }
    }
}
