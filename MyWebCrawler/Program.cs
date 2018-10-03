using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;



namespace MyWebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            
            WebCrawler w = new WebCrawler(args[0]);
            
            w.Run();

            w.Serialize();

        }
    }




}
