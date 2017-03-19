using System;
using System.Collections.Generic;
using WordCount.Web.Controllers;
using Xunit;
using Moq;
using WordCount.Model;
using WordCount.ServiceManagers;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.Web.Tests
{
    public class LoyalBooksControllerTests
    {
        [Fact]
        public void Test1()
        {
            IEnumerable<WordOccurance> wordOccurances = this.GetWordOccurances();

            MockRepository factory = new MockRepository(MockBehavior.Default);
            var mockManager = factory.Create<IWebApiManager>();

            mockManager.Setup(x => x.GetIndivisualWordsCount("")).Returns(wordOccurances);

            Mock<IDependencyResolver> mockDepResolver = new Mock<IDependencyResolver>();

            



            mockDepResolver.Setup(x => x.GetWebApiManagerByName(null)).Returns(mockApiManager);

        }

        private IEnumerable<WordOccurance> GetWordOccurances()
        {
            throw new NotImplementedException();
        }
    }
}
