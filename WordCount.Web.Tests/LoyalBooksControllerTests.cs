using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCount.Web.Controllers;

using Moq;
using WordCount.Model;
using WordCount.ServiceManagers;
using WordCount.ServiceManagers.Interfaces;
using WordCount.Web.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace WordCount.Web.Tests
{

    [TestClass]
    public class LoyalBooksDataControllerTests
    {
        [TestMethod]
        public async Task NextTenWordsTest()
        {
            string bookName = "DummyBookName";

            Mock<ISession> mockSession = new Mock<ISession>();

            mockSession.Setup(x => x.GetString("bookName")).Returns(bookName);

            Mock<HttpContext> mockHttpContxt = new Mock<HttpContext>();

            mockHttpContxt.Setup(x => x.Session).Returns(mockSession.Object);

            Mock<IHttpContextAccessor> mockAccessor = new Mock<IHttpContextAccessor>();

            mockAccessor.Setup(x => x.HttpContext).Returns(mockHttpContxt.Object);

            Mock<IServiceProvider> mockServices = new Mock<IServiceProvider>();

            mockServices.Setup(svc => svc.GetRequiredService<IHttpContextAccessor>()).Returns(mockAccessor.Object);

            IEnumerable<WordOccurance> wordOccurances = this.GetWordOccurances();

            Mock<IWebApiManager> mockManager = new Mock<IWebApiManager>();
            
            mockManager.Setup(x => x.GetIndivisualWordsCount(bookName)).Returns(Task.FromResult(wordOccurances));

            Mock<IDependencyResolver> mockDepResolver = new Mock<IDependencyResolver>();

            mockDepResolver.Setup(x => x.GetWebApiManagerByName(null)).Returns(mockManager.Object);

            LoyalBooksDataController controller = new LoyalBooksDataController(mockDepResolver.Object, mockServices.Object);

            IEnumerable<WordOccurance> result = await controller.NextTenWords();

        }

        private IEnumerable<WordOccurance> GetWordOccurances()
        {
            return new List<WordOccurance>()
            {
                new WordOccurance(){Word = "WordNumber1", Count = 1, PrimeNumberStatus = "Unit"},
                new WordOccurance(){Word = "WordNumber2", Count = 2, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber3", Count = 3, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber4", Count = 4, PrimeNumberStatus = "NO"},
                new WordOccurance(){Word = "WordNumber5", Count = 5, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber6", Count = 6, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber7", Count = 7, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber8", Count = 8, PrimeNumberStatus = "NO"},
                new WordOccurance(){Word = "WordNumber9", Count = 9, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber10", Count = 10, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber11", Count = 11, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber12", Count = 12, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber13", Count = 13, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber14", Count = 14, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber15", Count = 15, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber16", Count = 16, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber17", Count = 17, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber18", Count = 18, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber19", Count = 19, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber20", Count = 20, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber21", Count = 21, PrimeNumberStatus = "No"},

            };
        }
    }
}
