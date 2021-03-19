using ContextSearchContract;
using System.Globalization;
using System.Linq;
using System;
using System.IO;

namespace ContextSearchImpl
{
    public class ContextSearcher : IContextSearcher
    {
        private readonly ICatilogFactory _catalogFactory;
        private readonly IIOProvider _ioProvider;
        private readonly ICatalogToCustomFileConverter _catalogToCustomFileConverter;
        public const string Pattern = "*.po";
        public const string RusDir = "ru_Ru";
        public ContextSearcher(ICatilogFactory catalogFactory, IIOProvider ioProvider, ICatalogToCustomFileConverter catalogToCustomFileConverter)
        {
            _catalogFactory = catalogFactory ?? throw new ArgumentNullException(nameof(catalogFactory));
            _ioProvider = ioProvider ?? throw new ArgumentNullException(nameof(ioProvider));
            _catalogToCustomFileConverter = catalogToCustomFileConverter ?? throw new ArgumentNullException(nameof(catalogToCustomFileConverter));
        }

        public Context Search(Filter filter)
        {
            if (filter == null)
                return new Context();

            if (string.IsNullOrWhiteSpace(filter.ContextStr))
                return new Context();

            var contexts = filter.ContextStr.Split('.').ToArray();
            if (contexts.Length == 0)
                return new Context();

            var context = new Context { Title = contexts[0] };
            var temp = context;
            for (var i = 1; i < contexts.Length; i++)
            {
                temp.Next = new Context { Title = contexts[i] };
                temp = temp.Next;
            }

            var catalogs = _ioProvider
                .GetNameFiles(Path.Combine(filter.DirPath, filter.CultureDir), Pattern)
                .Select(f => new { filleName = f, catalog = _catalogFactory.Create(filter.DirPath, filter.CultureInfo, f) })
                .ToDictionary(info => info.filleName, info => info.catalog);

            if (catalogs.Count == 0)
                return context;

            temp = context;
            while(temp!=null)
            {
                temp.Files = _catalogToCustomFileConverter.Convert(temp.Title, catalogs);
                temp = temp.Next;
            }

            return context;
        }
    }
}
