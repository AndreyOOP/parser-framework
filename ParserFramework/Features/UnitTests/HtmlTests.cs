using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParserFramework.Models;

namespace UnitTests
{
    [TestClass]
    public class HtmlTests
    {
        [TestMethod]
        public void HtmlInstanceConvertedToStringImplicitly_Ok()
        {
            var html = new Html("<head></head>");
            
            string str = html;

            Assert.AreEqual("<head></head>", str);
        }
    }
}
