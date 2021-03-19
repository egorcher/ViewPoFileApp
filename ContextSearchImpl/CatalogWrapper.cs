using GetText;
using System;
using System.Collections.Generic;

namespace ContextSearchImpl
{
    public interface ICatalogWrapper
    {
        IDictionary<string, string[]> Translations { get;}
    }

    public class CatalogWrapper: ICatalogWrapper
    {
        private readonly Catalog _catalog;

        public CatalogWrapper(Catalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }
        public IDictionary<string, string[]> Translations { get =>_catalog.Translations;}
    }
}
