using ContextSearchContract;
using ContextSearchImpl;
using FluentAssertions;
using System.Globalization;
using System.IO;
using Xunit;

namespace ContextSearchImplTest
{

    public class ContextSearcherIntegrationTest
    {
        private readonly string LocalesDir = Path.Combine(Directory.GetCurrentDirectory(), "locales");
        [Fact]
        public void ShouldFindContext()
        {
            var expect = new Context
            {
                Title = "context1",
                Files = new[]
                {
                    new CustomFile
                    {
                        Name = "Test",
                        Messages = new []
                        {
                            new MsgTranslate
                            {
                                MsgId = "test3",
                                MsgStr = "тест3контекст1"
                            }
                        }
                    },
                    new CustomFile
                    {
                        Name = "Test1",
                        Messages = new[]
                        {
                            new MsgTranslate
                            {
                                MsgId = "test7",
                                MsgStr = "тест7контекст1"
                            }
                        }
                    },
                },
                Next = new Context 
                { 
                    Title = "context2",
                    Files = new []
                    {
                        new CustomFile
                        {
                            Name = "Test",
                            Messages = new []
                            {
                                new MsgTranslate
                                {
                                    MsgId = "test3",
                                    MsgStr = "тест3контекст2"
                                }
                            }
                        }
                    },
                    Next = new Context 
                    { 
                        Title = "context3",
                        Files = new[]
                        {
                            new CustomFile
                            {
                                Name = "Test1",
                                Messages = new []
                                {
                                    new MsgTranslate
                                    {
                                        MsgId = "test1",
                                        MsgStr = "тест5контекст3"
                                    }
                                }
                            }
                        }
                    } 
                } 
            };
            var target = new ContextSearcher(new CatilogFactory(), new IOProvider(), new CatalogToCustomFileConverter());
            var filter = new Filter
            {
                DirPath = LocalesDir,
                CultureDir = "ru_Ru",
                CultureInfo = CultureInfo.CurrentCulture,
                ContextStr = "context1.context2.context3"
            };

            //When
            var actual = target.Search(filter);

            //Then
            actual.Should().BeEquivalentTo(expect);
        }
    }
}
