using ContextSearchContract;
using GetText;
using System.Collections.Generic;
using System.Linq;

namespace ContextSearchImpl
{
    public interface ICatalogToCustomFileConverter
    {
        CustomFile[] Convert(string contextTitle, Dictionary<string, ICatalogWrapper> catalogs);
    }

    public class CatalogToCustomFileConverter: ICatalogToCustomFileConverter
    {
        public CustomFile[] Convert(string contextTitle, Dictionary<string, ICatalogWrapper> catalogs)
        {
            if (contextTitle == null || catalogs == null)
                return new CustomFile[0];
            

            var result =  catalogs
                    .Where(c => c.Value.Translations.Where(tr => tr.Key.Split(Catalog.CONTEXTGLUE).FirstOrDefault()==contextTitle).Any())
                    .Select(c => new CustomFile
                    {
                        Name = c.Key,
                        Messages = c.Value.Translations
                        .Where(tr => tr.Key.Split(Catalog.CONTEXTGLUE).FirstOrDefault() == contextTitle)
                        .SelectMany(x =>
                        {
                            var contextAndUntraslateWords = x.Key.Split(Catalog.CONTEXTGLUE);
                            var msgId = contextAndUntraslateWords.Length == 2 ? contextAndUntraslateWords[1] : "";
                            return x.Value.Select(vl => new MsgTranslate
                            {
                                MsgId = msgId,
                                MsgStr = vl
                            });
                        }).ToArray()
                    }).ToArray();

            return result;
        }
    }
}
