using System;
using ContextSearchContract;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Text;

namespace ContextViewApp
{
    public class MainWindowViewModel : BaseViewModel, IMainWindowViewModel
    {
        private readonly IContextSearcher _contextSearcher;
        private string _folderPath = @"..\..\..\..\ContextSearchImplTest\locales";
        private string _cultureDir = "ru_Ru";
        private string _culture = "ru-Ru";
        private string _messageContext = "context1.context2.context3";

        public MainWindowViewModel()
        {

        }

        public MainWindowViewModel(IContextSearcher contextSearcher)
        {
            _contextSearcher = contextSearcher ?? throw new ArgumentNullException(nameof(contextSearcher));
            FindCommand = new DelegateCommand(FindCommandHandler);
            Contexts = new ObservableCollection<ContextNodeModel>();
            EnableCommand = true;
        }

        /// <summary>
        /// Путь до папки с файлами
        /// </summary>
        public string FolderPath
        {
            get => _folderPath;
            set
            {
                _folderPath = value;
                OnPropertyChanged(nameof(FolderPath));
            }
        }

        /// <summary>
        /// Подпапка для иследуемой локалью
        /// </summary>
        public string CultureDir
        {
            get => _cultureDir;
            set
            {
                _folderPath = value;
                OnPropertyChanged(nameof(CultureDir));
            }
        }

        /// <summary>
        /// Исследуемая локаль
        /// </summary>
        public string Culture
        {
            get => _culture;
            set
            {
                _folderPath = value;
                OnPropertyChanged(nameof(Culture));
            }
        }

        /// <summary>
        /// Контекст для поиска
        /// </summary>
        public string MessageContext
        {
            get => _messageContext;
            set
            {
                _messageContext = value;
                OnPropertyChanged(nameof(MessageContext));
            }
        }

        public ObservableCollection<ContextNodeModel> Contexts { get; set; }

        /// <summary>
        /// Можно ли выполнисть команду
        /// </summary>
        public bool EnableCommand { get; set; }
        /// <summary>
        /// Команда найти совпадения
        /// </summary>
        public ICommand FindCommand { get; set; }

        private void FindCommandHandler(object obj)
        {
            EnableCommand = false;

            var filter = new Filter
            {
                DirPath = FolderPath,
                ContextStr = MessageContext,
                CultureDir = CultureDir,
                CultureInfo = new CultureInfo(Culture)
            };
            var result = _contextSearcher.Search(filter);
            if (result == null)
                return;

            var tempRes = result;
            ContextNodeModel model = new ContextNodeModel();
            var tempMod = model;
            while (tempRes != null)
            {
                tempMod.Title = tempRes.Title;
                tempMod.Next = new ObservableCollection<ContextNodeModel>();

                var next = new ContextNodeModel();

                if (tempRes.Files != null)
                {
                    foreach (var file in tempRes.Files)
                    {
                        var builder = new StringBuilder();
                        foreach (var msg in file.Messages)
                        {
                            builder.AppendLine($"Перевод: {msg.MsgId} - {msg.MsgStr}");
                        }

                        tempMod.Next.Add(new ContextNodeModel { Title = "Файл: " + file.Name, Content = builder.ToString() });
                    }
                }
                
                tempMod.Next.Add(next);
                tempMod = next;
                tempRes = tempRes.Next;
            }
            Contexts.Clear();
            Contexts.Add(model);
            OnPropertyChanged(nameof(Contexts));

            EnableCommand = true;
        }

    }
}
