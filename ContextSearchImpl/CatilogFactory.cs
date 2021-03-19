using GetText;
using System.Globalization;
using System.Linq;

namespace ContextSearchImpl
{
    public interface ICatilogFactory
    {
        ICatalogWrapper Create(string localesDir, CultureInfo cultureInfo, string file);
    }
    public class CatilogFactory: ICatilogFactory
    {
        public ICatalogWrapper Create(string localesDir, CultureInfo cultureInfo, string file) 
            => new CatalogWrapper(new Catalog(file, localesDir, cultureInfo));

    }
}
