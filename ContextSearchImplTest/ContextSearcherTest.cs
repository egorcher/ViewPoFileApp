using ContextSearchContract;
using ContextSearchImpl;
using FluentAssertions;
using GetText;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Xunit;

namespace ContextSearchImplTest
{
    public class ContextSearcherTest
    {
        private ContextSearcher _target;
        private Mock<IIOProvider> _ioProvider;
        private Mock<ICatilogFactory> _factory;
        private Mock<ICatalogToCustomFileConverter> _catalogToCustomFileConverter;
        const string nullstr = null;
        private string contexts = "context1.subcontext2.subcontext3";
        private readonly Filter _filter;

        public ContextSearcherTest()
        {
            _factory = new Mock<ICatilogFactory>();
            _ioProvider = new Mock<IIOProvider>();
            _catalogToCustomFileConverter = new Mock<ICatalogToCustomFileConverter>();
            _target = new ContextSearcher(_factory.Object, _ioProvider.Object, _catalogToCustomFileConverter.Object);
            _filter = new Filter
            {
                ContextStr = "context1.subcontext2.subcontext3",
                CultureDir = "ru_Ru",
                CultureInfo = new CultureInfo("ru-Ru"),
                DirPath = "c:\\locales"
            };
        }
        
        [Theory]
        [InlineData(nullstr)]
        [InlineData("")]
        public void ShouldGetEmptyContext(string contextstr)
        {
            //Given
            _filter.ContextStr = contextstr;

            //When
            var actual = _target.Search(_filter);

            //Then
            actual.Should().BeEquivalentTo(new Context());
        }

        [Fact]
        public void ShouldGetContextWithoutCustomFileWhenFileNotFound()
        {
            //Given
            _ioProvider.Setup(x => x.GetNameFiles(_filter.DirPath, It.Is<string>(x=>x==ContextSearcher.Pattern))).Returns(new string[0]);
            var expect = new Context { Title = "context1", Next = new Context { Title = "subcontext2", Next = new Context { Title = "subcontext3" } } };

            //When
            var actual = _target.Search(_filter);

            //Then
            actual.Should().BeEquivalentTo(expect);
        }

        [Fact]
        public void ShouldSearchFileNames()
        {
            //Given
            //When
            var actual = _target.Search(_filter);

            //Then
            _ioProvider.Verify(x=>x.GetNameFiles(Path.Combine(_filter.DirPath,_filter.CultureDir), ContextSearcher.Pattern), Times.Once);
        }

        [Fact]
        public void ShouldCreateCatalogs()
        {
            //Given
            var files = new[] { "File1", "File2" };
            _ioProvider.Setup(x => x.GetNameFiles(Path.Combine(_filter.DirPath, _filter.CultureDir), ContextSearcher.Pattern)).Returns(files);


            //When
            var actual = _target.Search(_filter);

            //Then
            _factory.Verify(x => x.Create(_filter.DirPath, _filter.CultureInfo, It.Is<string>(x => x.StartsWith("File"))), Times.Exactly(files.Length));
        }

        [Fact]
        public void ShouldConvertCatalogToCustomFile()
        {
            //Given
            var files = new[] { "File1", "File2" };
            var catalog = new CatalogWrapper( new Catalog());
            _ioProvider.Setup(x => x.GetNameFiles(Path.Combine(_filter.DirPath, _filter.CultureDir), ContextSearcher.Pattern)).Returns(files);
            foreach(var file in files)
            {
                _factory.Setup(x => x.Create(_filter.DirPath, _filter.CultureInfo, It.Is<string>(x => x.StartsWith("File")))).Returns(catalog);
            }

            //When
            var actual = _target.Search(_filter);

            //Then
            foreach(var contextTitle in contexts.Split('.'))
            {
                _catalogToCustomFileConverter.Verify(x => x.Convert(contextTitle, It.Is<Dictionary<string, ICatalogWrapper>>(x => x.Values.All(x => x == catalog))), Times.Once);
            }
        }

        [Fact]
        public void ShouldGetContexts()
        {
            //Given
            var files = new[] { "File1", "File2" };
            var catalog = new CatalogWrapper(new Catalog());
            var customFiles = new[] { new CustomFile(), new CustomFile() };

            _ioProvider.Setup(x => x.GetNameFiles(Path.Combine(_filter.DirPath, _filter.CultureDir), ContextSearcher.Pattern)).Returns(files);
            foreach (var file in files)
            {
                _factory.Setup(x => x.Create(_filter.DirPath, _filter.CultureInfo, It.Is<string>(x => x.StartsWith("File")))).Returns(catalog);
            }
            foreach (var contextTitle in contexts.Split('.'))
            {
                _catalogToCustomFileConverter.Setup(x => x.Convert(contextTitle, It.Is<Dictionary<string, ICatalogWrapper>>(x => x.Values.All(x => x == catalog))))
                    .Returns(customFiles);
            }

            var expect = new Context { 
                Title = "context1",
                Files = customFiles,
                Next = new Context { 
                    Title = "subcontext2",
                    Files = customFiles,
                    Next = new Context { 
                        Title = "subcontext3",
                        Files = customFiles,
                    } } };

            //When
            var actual = _target.Search(_filter);

            //Then
            actual.Should().BeEquivalentTo(expect);
        }


    }
}
