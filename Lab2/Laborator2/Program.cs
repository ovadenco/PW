using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace Laborator2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowHelp();
                return;
            }

            var option = args[0];

            switch (option)
            {
                case "-u":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("No URL specified.");
                        return;
                    }
                    var url = args[1];
                    await MakeHttpRequest(url);
                    break;

                case "-s":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("No search term specified.");
                        return;
                    }
                    var searchTerm = string.Join(" ", args, 1, args.Length - 1);
                    await Search(searchTerm);
                    break;

                case "-h":
                    ShowHelp();
                    break;

                default:
                    Console.WriteLine("Invalid option. Use -h to see available options.");
                    break;
            }
        }

        static async Task MakeHttpRequest(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(RemoveHtmlTags(content)); // ExtractTextFromHtml( content));
            }
        }

        static async Task Search(string searchTerm)
        {
            var searchEngineURL = "https://www.google.com/search?q=";
            var searchURL = searchEngineURL + Uri.EscapeDataString(searchTerm);
            await MakeHttpRequest(searchURL);
        }

        static void ShowHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("go2web -u <URL>         # make an HTTP request to the specified URL and print the response");
            Console.WriteLine("go2web -s <search-term> # make an HTTP request to search the term using your favorite search engine and print top 10 results");
            Console.WriteLine("go2web -h               # show this help");
        }
        static string ExtractBodyContent(string htmlString)
        {
            string bodyPattern = @"<body\b[^>]*>(.*?)</body>";
            Match bodyMatch = Regex.Match(htmlString, bodyPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (bodyMatch.Success)
            {
                return bodyMatch.Groups[1].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ExtractText(string html)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            string s = reg.Replace(html, " ");
            s = HttpUtility.HtmlDecode(s);
            return s;
        }

        static string ExtractTextFromHtml(string htmlString)
        {
            htmlString = ExtractBodyContent(htmlString);
            // Remove script tags and their contents
            htmlString = Regex.Replace(htmlString, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            // Remove style tags and their contents
            htmlString = Regex.Replace(htmlString, "<style[^>]*?>.*?</style>", "", RegexOptions.IgnoreCase);

            // Remove all other HTML tags
            htmlString = Regex.Replace(htmlString, "<[^>]*>", "");

            // Remove extra whitespace and newline characters
            htmlString = Regex.Replace(htmlString, @"\s+", " ");

            return htmlString;
        }

        public static string RemoveHtmlTags(string text)
        {
            Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
            return ExtractTextFromHtml(rRemScript.Replace(text, ""));
        }

    }
}
