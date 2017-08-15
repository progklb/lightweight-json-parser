namespace LightWeightJsonParser
{
    /// <summary>
    /// Determines the response of the parser when a parsing failure occurs.
    /// </summary>
    public enum FailureMode
    {
        /// <summary>
        /// Failured type-checks will still assign the value as a string type.
        /// </summary>
        Silent,
        /// <summary>
        /// Failed type-checks will replace the value with a string indicating falure.
        /// </summary>
        Verbose,
        /// <summary>
        /// Failed type-checks will not assign the value, resulting in a null object.
        /// </summary>
        Nullify,
        /// <summary>
        /// Failed type-checks will throw an exception.
        /// </summary>
        Exception
    }
}
