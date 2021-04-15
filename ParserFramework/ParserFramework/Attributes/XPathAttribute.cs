using System;

namespace ParserFramework
{
    public class XPathSourceAttribute : Attribute
    {
        public string XPath { get; private set; }

        public XPathSourceAttribute(string xPath)
        {
            XPath = xPath;
        }
    }
}
