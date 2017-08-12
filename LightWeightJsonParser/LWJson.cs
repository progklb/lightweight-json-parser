using System;
using DataType = LightWeightJsonParser.LWJsonValue.DataType;

namespace LightWeightJsonParser
{
    public abstract class LWJson
    {
        #region INDEXERS
        /// <summary>
        /// Gets or sets the item at the index specified. This is only valid for <see cref="LWJsonArray"/> types.
        /// </summary>
        /// <param name="i">Index of the array to access.</param>
        /// <returns>The value at the provided index.</returns>
        public virtual LWJson this[int i]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the item with the specified key. This is only valid for <see cref="LWJsonObject"/> types.
        /// </summary>
        /// <param name="key">Key of the data attribute to access.</param>
        /// <returns>The value of the data matching the provided key.</returns>
        public virtual LWJson this[string key]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        #endregion


        #region PROPERTIES
        /// <summary>
        /// Whether this object is a string-based value.
        /// { "key" : "string" }
        /// </summary>
        public bool IsString { get { return (this.IsValue ? (this as LWJsonValue).Type == DataType.String : false); } }
        /// <summary>
        /// Whether this object is a boolean-based value.
        /// { "key" : true }
        /// </summary>
        public bool IsBoolean { get { return (this.IsValue ? (this as LWJsonValue).Type == DataType.Boolean : false); } }
        /// <summary>
        /// Whether this object is an integer-based value.
        /// { "key" : 25 }
        /// </summary>
        public bool IsInteger { get { return (this.IsValue ? (this as LWJsonValue).Type == DataType.Integer : false); } }
        /// <summary>
        /// Whether this object is a double-based value.
        /// { "key" : 1.8458 }
        /// </summary>
        public bool IsDouble { get { return (this.IsValue ? (this as LWJsonValue).Type == DataType.Double : false); } }
        /// <summary>
        /// Whether this object is an array of sub-objects of key-value pairs.
        /// { "key" : [{...},...] }
        /// </summary>
        public bool IsArray { get { return (this is LWJsonArray); } }
        /// <summary>
        /// Whether this object is an an object representing a set of key-value pairs.
        /// { key : { ... } }
        /// </summary>
        public bool IsObject { get { return (this is LWJsonObject); } }

        /// <summary>
        /// Whether this object is a simple value. Note that this is a convenience method for performing more specific type checks.
        /// { "key" : value }
        /// </summary>
        private bool IsValue { get { return (this is LWJsonValue); } }
        #endregion


        #region PARSING
        public static LWJson Parse(string jsonString)
        {
            // Sanitise the ends of the string
            jsonString = jsonString.Trim();

            // Check what object type to start with.
            switch (jsonString[0])
            {
                case '{':       return new LWJsonObject();
                case '[':       return new LWJsonArray();
                default:        throw new Exception($"Invalid initial character of JSON string: \"{jsonString[0]}\"");
            }
        }

        internal static LWJson ParseTest()
        {
            return null;
        }

        /// <summary>
        /// Provided a JSON string and a starting index (which must correspond to an opening array or object character), 
        /// this will return the array or object in its entirety up to the corresponding closing character.
        /// </summary>
        /// <param name="jsonString">The JSON string to extract an array or object from.</param>
        /// <param name="startingIdx">The index of the opening character of the array or object.</param>
        /// <returns></returns>
        public static string Chunk(string jsonString, int startingIdx)
        {
            char openingChar = jsonString[startingIdx];
            char closingChar;

            switch (openingChar)
            {
                case '{':       closingChar = '}';      break;
                case '[':       closingChar = ']';      break;
                default:        throw new Exception($"Invalid opening character of JSON object/array: \"{openingChar}\"" + 
                                    "\nPlease provide a start index that corresponds to the opening of an object/array.");
            }

            // Iterate through the string starting at the specified starting index.
            // We count the number of opening tags and closing tags, returning when the root object/array ends.
            for (int i = startingIdx, openTags = 0; ; ++i)
            {
                if (jsonString[i] == openingChar)
                {
                    openTags++;
                }
                else if (jsonString[i] == closingChar)
                {
                    openTags--;
                }

                // Check if we have reached the end of the object/array
                if (openTags == 0)
                {
                    return jsonString.Substring(startingIdx, i - startingIdx + 1);
                }
            }
        }
        #endregion
    }
}
