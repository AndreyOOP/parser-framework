using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserFramework.Abstractions
{
    public interface IXPathAttributeMapper
    {
        public T Map<T>(IHtmlNodeSource source) where T : class, new();
        
        public T Map<T>(HtmlNode node) where T : class;
    }

    /// <summary>
    /// Uri source, html (string) source
    /// </summary>
    public interface IHtmlNodeSource
    {
        public HtmlNode HtmlDocument { get; }
    }

    /// <summary>
    /// Abstraction to avoid linking to specific html parser library
    /// If it will be too hard to manage - avoid - ?
    /// </summary>
    //public interface IHtmlDocument // remove & change to HtmlDocument - very low prob of changing parsing library at first stages...
    //{
    //    /// <summary>
    //    /// Get html document by xPath
    //    /// </summary>
    //    public IHtmlDocument FromXPath(string xPath);

    //    public string InnerText { get; }
    //    // Value, Html, InnerText, Attributes etc
    //}
}
