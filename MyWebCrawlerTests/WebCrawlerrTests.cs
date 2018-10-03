using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyWebCrawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace MyWebCrawler.Tests
{
    [TestClass()]
    public class WebCrawlerTests
    {
        [TestMethod()]
        public void FindTest()
        {
            string content = @"<a href=""https://www.w3schools.com"">Visit W3Schools</a>" +
                             @"<a href=""https://www.Link1.com"">Link1</a>";
            HashSet<string> pageLinks = LinkFinder.Find(content);
            bool b = pageLinks.Contains("https://www.w3schools.com");
            bool b1 = pageLinks.Contains("https://www.Link1.com");

            Assert.IsTrue(b && b1);
        }

        [TestMethod()]
        public void FindTest_negative()
        {
            string content = @"<b href=""https://www.w3schools.com"">Visit W3Schools</b>";
            HashSet<string> pageLinks = LinkFinder.Find(content);
            bool b = pageLinks.Contains("https://www.w3schools.com");

            Assert.IsFalse(b);
        }

        [TestMethod()]
        [ExpectedException(typeof(System.UriFormatException), AllowDerivedTypes = true)]
        public void Construct_bad()
        {
            WebCrawler w = new WebCrawler("tapok");

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void CrawlPage_OK()
        {
            string content = "";
            using (WebClient w = new WebClient())
            {
                content = w.DownloadString("https://wiprodigital.com/");
            }

            Assert.IsTrue(content.Length > 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(System.Net.WebException), AllowDerivedTypes = true)]
        public void CrawlPage_Bad()
        {
            string content = "";
            using (WebClient w = new WebClient())
            {
                content = w.DownloadString("https://wiprodigital.com/page_that_does_not_exists");
            }

            Assert.IsFalse(content.Length > 0);
        }

        [TestMethod()]
        public void SerializeTest()
        {
            WebPage P;
            P.URL = "www.url1.com";
            P.Links = new List<string>();
            P.Links.Add("www.url1.com/Link1");
            P.Links.Add("www.url1.com/Link2");

            WebPage P1;
            P1.URL = "www.url2.com";
            P1.Links = new List<string>();
            P1.Links.Add("www.url3.com/Link3");
            P1.Links.Add("www.url3.com/Link3");

            List<WebPage> L = new List<WebPage>();
            L.Add(P);
            L.Add(P1);

            string result = XmlHelper.ToXml(L);

            string goodResult = @"<ArrayOfWebPage>
  <WebPage>
    <URL>www.url1.com</URL>
    <Links>
      <string>www.url1.com/Link1</string>
      <string>www.url1.com/Link2</string>
    </Links>
  </WebPage>
  <WebPage>
    <URL>www.url2.com</URL>
    <Links>
      <string>www.url3.com/Link3</string>
      <string>www.url3.com/Link3</string>
    </Links>
  </WebPage>
</ArrayOfWebPage>";

            Assert.AreEqual(result, goodResult);
        }
    }
}

