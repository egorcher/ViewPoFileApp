using System.Windows.Input;

namespace ContextViewApp
{
    public interface IMainWindowViewModel
    {
        ICommand FindCommand { get; set; }
        string FolderPath { get; set; }
        string MessageContext { get; set; }

    }
}