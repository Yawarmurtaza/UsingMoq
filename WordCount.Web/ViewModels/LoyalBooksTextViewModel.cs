using System.Collections.Concurrent;
using System.Collections.Generic;
using WordCount.Model;

namespace WordCount.Web.ViewModels
{
    public class LoyalBooksTextViewModel
    {
        private string bookName;
        public string OperationName { get; set; }
        public string BookName
        {
            get { return this.bookName.Replace(".txt", string.Empty); }
            set { this.bookName = value; }
        }

    }
}

