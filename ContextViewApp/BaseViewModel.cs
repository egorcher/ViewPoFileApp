using System.ComponentModel;

namespace ContextViewApp
{
    /// <summary>
    /// Базовая ViewModel
    /// Описывает общее для всех ViewModel поведение
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменение свойства ViewModel
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызывает событие об изменении свойства ViewModel
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
