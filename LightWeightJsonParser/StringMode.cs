namespace LightWeightJsonParser
{
    /// <summary>
    /// The two available types of string enclosing characters.
    /// </summary>
    public class StringMode
    {
        #region CONSTANTS
        public const string SINGLE_QUOTE = "'";
        public const string DOUBLE_QUOTE = "\"";
        #endregion


        #region TYPES
        public enum Mode
        {
            SingleQuote,
            DoubleQuote
        }
        #endregion
    }
}
