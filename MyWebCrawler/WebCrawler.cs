using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;
using System.Diagnostics;

namespace MyWebCrawler
{
    public class WebCrawler
    {
        private string Domain;

        private List<WebPage> Pages;

        HashSet<string> CrawledPages;
        List<string> NeedCrawl;
        
        public WebCrawler(string Site)
        {
            try
            {
                Uri myUri = new Uri(Site);
                Domain = myUri.Host;

                Pages = new List<WebPage>();

                CrawledPages = new HashSet<string>();
                NeedCrawl = new List<string>();
                NeedCrawl.Add(Site);
            }
            catch (Exception e)
            {
                Debug.WriteLine(Site +" "+ e.GetType()+"   " + e );
                throw e;
            }
            
        }

        public void Serialize()
        {
             XmlHelper.ToXmlFile(Pages, "Site.xml");
        }

        public void Run()
        {
            using (WebClient w = new WebClient())
            { 
                while (NeedCrawl.Count > 0)
                {
                    string nextLink = NeedCrawl[0];
                    Uri nextUri = new Uri(nextLink);
                    NeedCrawl.RemoveAt(0);//removed crawled link

                    if (CrawledPages.Contains(nextUri.AbsoluteUri)) continue;

                    string content = "";
                    try
                    {
                        content = w.DownloadString(nextLink);
                    }
                    catch (Exception e)
                    {
                        //problem downloading page
                        Debug.WriteLine(nextLink + "   " + e);
                    }

                    HashSet<string> pageLinks = LinkFinder.Find(content);

                    WebPage P;
                    P.URL = nextLink;
                    P.Links = pageLinks.ToList();
                    Pages.Add(P);
                    
                    CrawledPages.Add(nextUri.AbsoluteUri);

                    //add links to crawl
                    foreach (string s in pageLinks)
                    {
                        if (s.Contains(Domain) && s.IndexOf(Domain) < 15 && !CrawledPages.Contains(s))
                            NeedCrawl.Add(s);
                    }

                }
            }
        }
    }


    public static class LinkFinder
    {
        //Return links found in html file
        public static HashSet<string> Find(string fileContent)
        {
            HashSet<string> links = new HashSet<string>();

            try
            {
                // Find all matches in file.
                MatchCollection m1 = Regex.Matches(fileContent, @"(<a.*?>.*?</a>)",
                    RegexOptions.Singleline);

                // Loop over each match.
                foreach (Match m in m1)
                {
                    string value = m.Groups[1].Value;
                    string s = "";

                    // Get href attribute.
                    Match m2 = Regex.Match(value, @"href=\""(.*?)\""",
                        RegexOptions.Singleline);
                    if (m2.Success)
                    {
                        s = m2.Groups[1].Value;
                    }

                    links.Add(s);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }
            return links;
        }
    }
}
