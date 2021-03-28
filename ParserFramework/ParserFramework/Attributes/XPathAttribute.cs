﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ParserFramework
{
    public class XPathSourceAttribute : Attribute
    {
        public string XPath { get; private set; }
        public bool ReturnHtml { get; private set; }

        public XPathSourceAttribute(string xPath, bool returnHtml = false) 
        {
            XPath = xPath;
            ReturnHtml = returnHtml;
        }

        public XPathSourceAttribute(string xPath, string innerXPath, bool returnHtml = false) { }
    }
}