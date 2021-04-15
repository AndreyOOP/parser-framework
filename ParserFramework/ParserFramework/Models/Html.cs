namespace ParserFramework.Models
{
    /// <summary>
    /// Wrapper to avoid flg usage isHtml in XPathAttribute 
    /// </summary>
    public class Html
    {
        string html;

        public Html(string html)
        {
            this.html = html;
        }

        public override string ToString() => html;

        public static implicit operator string(Html html) => html.ToString();
    }
}
