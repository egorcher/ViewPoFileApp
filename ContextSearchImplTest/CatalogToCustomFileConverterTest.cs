using ContextSearchContract;
using ContextSearchImpl;
using FluentAssertions;
using GetText;
using System.Collections.Generic;
using Xunit;

namespace ContextSearchImplTest
{
    public class CatalogToCustomFileConverterTest
    {
        private readonly CatalogToCustomFileConverter target = new CatalogToCustomFileConverter();

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void ShouldConvert(string testName, string contextTitle, Dictionary<string, ICatalogWrapper> catalogs, CustomFile[] expect)
        {
            //When
            var actual = target.Convert(contextTitle, catalogs);

            //Then
            actual.Length.Should().Be(expect.Length);
            actual.Should().BeEquivalentTo(expect);
        }



        public static object[][] GetTestData() => new object[][]
        {
            new object[]
            {
                "Тест1",
                "context",
                null,
                new CustomFile[0]
            },
            new object[]
            {
                "Тест2",
                null,
                new Dictionary<string,ICatalogWrapper>(),
                new CustomFile[0]
            },
            new object[]
            {
                "Тест3",
                "context",
                new Dictionary<string,ICatalogWrapper>(),
                new CustomFile[0]
            },
            new object[]
            {
                "Тест4",
                "context",
                new Dictionary<string,ICatalogWrapper>
                {
                    { 
                        "File1", new TestableCatalog(
                        new Dictionary<string, string[]>
                        {
                            { $"context{Catalog.CONTEXTGLUE}untranslate1",new []{"translate1", "translate1" } },
                            { $"context{Catalog.CONTEXTGLUE}untranslate2",new []{"translate2", "translate2" } },
                            { $"context1{Catalog.CONTEXTGLUE}untranslate3",new []{"translate3", "translate3" } },
                            { $"context{Catalog.CONTEXTGLUE}untranslate4",new []{"", "translate4" } },
                            { $"context{Catalog.CONTEXTGLUE}",new []{"", "translate5" } }
                        })
                    },
                    {
                        "File2", new TestableCatalog(
                        new Dictionary<string, string[]>
                        {
                            { $"context{Catalog.CONTEXTGLUE}untranslate1",new []{"translate1", "translate1" } },
                            { $"context{Catalog.CONTEXTGLUE}untranslate2",new []{"translate2", "translate2" } },
                            { $"context1{Catalog.CONTEXTGLUE}untranslate3",new []{"translate3", "translate3" } },
                            { $"context{Catalog.CONTEXTGLUE}untranslate4",new []{"", "translate4" } },
                            { $"context{Catalog.CONTEXTGLUE}",new []{"", "translate5" } }
                        })
                    }
                },
                new[]
                {
                    new CustomFile
                    {
                        Name = "File1",
                        Messages = new[]
                        {
                            new MsgTranslate{ MsgId = "untranslate1", MsgStr = "translate1"},
                            new MsgTranslate{ MsgId = "untranslate1", MsgStr = "translate1"},
                            new MsgTranslate{ MsgId = "untranslate2", MsgStr = "translate2"},
                            new MsgTranslate{ MsgId = "untranslate2", MsgStr = "translate2"},
                            new MsgTranslate{ MsgId = "untranslate4", MsgStr = ""},
                            new MsgTranslate{ MsgId = "untranslate4", MsgStr = "translate4"},
                            new MsgTranslate{ MsgId = "", MsgStr = ""},
                            new MsgTranslate{ MsgId = "", MsgStr = "translate5"}
                        }
                    },
                    new CustomFile
                    {
                        Name = "File2",
                        Messages = new[]
                        {
                            new MsgTranslate{ MsgId = "untranslate1", MsgStr = "translate1"},
                            new MsgTranslate{ MsgId = "untranslate1", MsgStr = "translate1"},
                            new MsgTranslate{ MsgId = "untranslate2", MsgStr = "translate2"},
                            new MsgTranslate{ MsgId = "untranslate2", MsgStr = "translate2"},
                            new MsgTranslate{ MsgId = "untranslate4", MsgStr = ""},
                            new MsgTranslate{ MsgId = "untranslate4", MsgStr = "translate4"},
                            new MsgTranslate{ MsgId = "", MsgStr = ""},
                            new MsgTranslate{ MsgId = "", MsgStr = "translate5"}
                        }
                    }

                }
            }
        };

        private class TestableCatalog : ICatalogWrapper
        {
            IDictionary<string, string[]> _translations;
            public TestableCatalog(IDictionary<string, string[]> translations)
            {
                _translations = translations;
            }
            public IDictionary<string, string[]> Translations => _translations;
        }

    }

    
}
