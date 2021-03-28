﻿using HtmlAgilityPack;
using ParserFramework.Abstractions;

namespace ParserFramework
{
    public class HtmlNodeSource : IHtmlNodeSource
    {
        private string html;
        private HtmlNode htmlNode;

        public HtmlNodeSource(string html)
        {
            this.html = html;
        }

        public HtmlNode HtmlDocument 
        {
            get
            {
                if (htmlNode == null){
                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);
                    htmlNode = htmlDocument.DocumentNode;
                }
                // ToCheck: maybe return clone - otherwise it may have inpact on original node
                return htmlNode; 
            }
        }
    }
}
