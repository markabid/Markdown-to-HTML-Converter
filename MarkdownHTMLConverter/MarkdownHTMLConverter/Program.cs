using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MarkdownHTMLConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                string markdownInputFile = args[0];
                if (File.Exists(markdownInputFile))
                {
                    string markdownLinkRegex = @"\[([^\[]+)\]\(([^)]*)\)";
                    string markdownHeadingRegex = @"^(#{1,6})\s(.*)";
                    string untaggedItemsRegex = @"^([^<\r\n].*[^>])+$";
                    string entireFile = File.ReadAllText(markdownInputFile);

                    //convert all markdown links 
                    entireFile = Regex.Replace(entireFile, markdownLinkRegex, delegate (Match match)
                    {
                        return "<a href=" + match.Groups[2].Value.Trim() + ">" + match.Groups[1].Value.Trim() + "</a>";
                    });

                    //convert all markdown headings
                    entireFile = Regex.Replace(entireFile, markdownHeadingRegex, delegate (Match match)
                    {
                        string headingSize = match.Groups[1].Value.Length.ToString();
                        return "<h" + headingSize + ">" + match.Groups[2].Value.Trim() + "</h" + headingSize + ">";                    
                    }, RegexOptions.Multiline);

                    //convert remaining untagged lines to html <p> tags
                    entireFile = Regex.Replace(entireFile, untaggedItemsRegex, delegate (Match match)
                    {
                        return "<p>" + match.Value.Trim() + "</p>";
                    }, RegexOptions.Multiline);

                    File.WriteAllText(@"..\..\..\..\ConvertedMarkdown.html", entireFile);
                }
                else Console.WriteLine("File does not exist at provided path.");
            }
            else Console.WriteLine("Please pass the full path to your markdown file as the only command line argument.");
        }
    }
}