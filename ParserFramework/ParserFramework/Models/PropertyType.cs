namespace ParserFramework.Models
{
    /// <summary>
    /// Supported types for autoconvertion
    /// </summary>
    public enum PropertyType
    {
        Int,

        Bool,

        Decimal,

        /// <summary>
        /// Type which contain string representation of the <see cref="HtmlNode"/>
        /// </summary>
        Html,

        String,

        /// <summary>
        /// Model which should be reccursively parsed till other primitive types
        /// </summary>
        Model,

        /// <summary>
        /// <see cref="HtmlNode"/> itself
        /// </summary>
        HtmlNode,

        IntCollection,

        BoolCollection,

        DecimalCollection,

        HtmlCollection,

        StringCollection,

        ModelCollection,

        HtmlNodeCollection
    }

}
