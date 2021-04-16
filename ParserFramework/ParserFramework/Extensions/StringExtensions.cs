namespace ParserFramework.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Swap expression 'string.Format("Template", args) to "Template".Format(args)
        /// </summary>
        public static string Format(this string str, params object[] args) 
            => string.Format(str, args);
    }
}
