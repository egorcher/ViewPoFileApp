using System.Globalization;

namespace ContextSearchContract
{
    /// <summary>
    /// Фильтр для поиска контекста в файлах
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Путь до папки с локализацией
        /// </summary>
        public string DirPath { get; set; }

        /// <summary>
        /// Подпапка для иследуемой локалью
        /// </summary>
        public string CultureDir { get; set; }

        /// <summary>
        /// Искомый контекст
        /// </summary>
        public string ContextStr { get; set; }

        /// <summary>
        /// Исследуемая локаль
        /// </summary>
        public CultureInfo CultureInfo { get; set; }
    }
}
