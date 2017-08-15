using System;

namespace LightWeightJsonParser
{
    public abstract class LWJson
    {
        #region CONSTANTS
        /// <summary>
        /// Represents a null instance of this type.
        /// </summary>
        public const LWJson NULL = null;
        #endregion


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
        /// The string mode that determines the quotation marks used when outputting a <see cref="LWJson"/> object to a JSON string.
        /// </summary>
        public static StringMode.Mode CurrentStringMode { get; set; } = StringMode.Mode.DoubleQuote;
        /// <summary>
        /// Determines how value parsing failures are handled.
        /// </summary>
        public static FailureMode CurrentFailureMode { get; set; } = FailureMode.Verbose;

        /// <summary>
        /// The data type of this object.
        /// </summary>
        public virtual JsonDataType DataType { get; protected set; }

        /// <summary>
        /// Whether this object is a string-based value.
        /// { "key" : "string" }
        /// </summary>
        public bool IsString { get { return DataType == JsonDataType.String; } }
        /// <summary>
        /// Whether this object is a boolean-based value.
        /// { "key" : true }
        /// </summary>
        public bool IsBoolean { get { return DataType == JsonDataType.Boolean; } }
        /// <summary>
        /// Whether this object is an integer-based value.
        /// { "key" : 25 }
        /// </summary>
        public bool IsInteger { get { return DataType == JsonDataType.Integer; } }
        /// <summary>
        /// Whether this object is a double-based value.
        /// { "key" : 1.8458 }
        /// </summary>
        public bool IsDouble { get { return DataType == JsonDataType.Double; } }
        /// <summary>
        /// Whether this object is an an object representing a set of key-value pairs.
        /// { key : { ... } }
        /// </summary>
        public bool IsObject { get { return DataType == JsonDataType.Object; } }
        /// <summary>
        /// Whether this object is an array of sub-objects of key-value pairs.
        /// { "key" : [{...},...] }
        /// </summary>
        public bool IsArray { get { return DataType == JsonDataType.Array; } }
        /// <summary>
        /// Whether this object is a simple value. Note that this is a convenience method for performing more specific type checks, 
        /// as a <see cref="LWJsonValue"/> can represent any of <see cref="string"/>, <see cref="bool"/>, <see cref="int"/>, or <see cref="double"/>.
        /// { "key" : value }
        /// </summary>
        public bool IsValue { get { return (this is LWJsonValue); } }
        #endregion


        #region CASTING
        public virtual string AsString() => throw new InvalidCastException();
        public virtual bool AsBoolean() => throw new InvalidCastException();
        public virtual int AsInteger() => throw new InvalidCastException();
        public virtual double AsDouble() => throw new InvalidCastException();
        public virtual LWJsonObject AsObject() => this is LWJsonObject ? this as LWJsonObject : throw new InvalidCastException($"Cannot cast object of type {DataType} to Object");
        public virtual LWJsonArray AsArray() => this is LWJsonArray ? this as LWJsonArray : throw new InvalidCastException($"Cannot cast object of type {DataType} to Array");
        public virtual LWJsonValue AsValue() => this is LWJsonValue ? this as LWJsonValue : throw new InvalidCastException($"Cannot cast object of type {DataType} to Value");
        #endregion


        #region PARSING
        /// <summary>
        /// Accepts a valid JSON string, producing and returning a corresponding <see cref="LWJSon"/> JSON object.
        /// </summary>
        /// <param name="jsonString">A valid JSON string.</param>
        /// <returns>A corresponding LWJSon object that represents the provided string.</returns>
        public static LWJson Parse(string jsonString)
        {
            // Sanitise the ends of the string
            jsonString = jsonString.Trim();

            LWJson json;

            // Check what object type to start with.
            switch (jsonString[0])
            {
                case '{':
                    json = new LWJsonObject();
                    (json as LWJsonObject).Parse(jsonString);
                    break;
                case '[':
                    json = new LWJsonArray();
                    (json as LWJsonArray).Parse(jsonString);
                    break;
                default:
                    throw new Exception($"Invalid initial character of JSON string: \"{jsonString[0]}\"");
            }

            return json;
        }

        /// <summary>
        /// Provided a JSON string and starting index (which must correspond to an opening array or object character, opening string quote, or first character of a value), 
        /// this automatically determines if it should be chunked as an object, array, string, or primitive value. The chunked value is returned.
        /// </summary>
        /// <param name="jsonString">The JSON string to extract an array, object, string, or primitive value from.</param>
        /// <param name="startingIdx">The index of the opening character of the array, object, or string (quote), or the first character of a primitive value.</param>
        /// <returns>The extracted item.</returns>
        internal static string Chunk(string jsonString, int startingIdx)
        {
            if (jsonString[startingIdx] == '{' || jsonString[startingIdx] == '[')
            {
                return ChunkBlock(jsonString, startingIdx);
            }
            else
            {
                return ChunkValue(jsonString, startingIdx);
            }
        }

        /// <summary>
        /// Provided a JSON string and a starting index (which must correspond to an opening array or object character), 
        /// this will return the array or object in its entirety up to the corresponding closing character.
        /// </summary>
        /// <param name="jsonString">The JSON string to extract an array or object from.</param>
        /// <param name="startingIdx">The index of the opening character of the array or object.</param>
        /// <returns>The extracted object or array block.</returns>
        internal static string ChunkBlock(string jsonString, int startingIdx)
        {
            char openingChar = jsonString[startingIdx];
            char closingChar;

            switch (openingChar)
            {
                case '{':
                    closingChar = '}';
                    break;
                case '[':
                    closingChar = ']';
                    break;
                default:
                    throw new Exception($"Invalid opening character of JSON object/array: \"{openingChar}\"" +
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

        /// <summary>
        /// Provided a JSON string and a starting index (which must correspond to an opening string quote or first character of a primitive value), 
        /// this will return the string or primitive value in its entirety up to the corresponding closing character.
        /// </summary>
        /// <param name="jsonString">The JSON string to extract an array or object from.</param>
        /// <param name="startingIdx">The index of the opening character of the array or object.</param>
        /// <returns>The extracted string or primitive value.</returns>
        internal static string ChunkValue(string jsonString, int startingIdx)
        {
            char openingChar = jsonString[startingIdx];

            // If string
            if (IsStringQuote(openingChar))
            {
                for (int i = startingIdx + 1; i < jsonString.Length; ++i)
                {
                    if (jsonString[i] == openingChar)
                    {
                        return jsonString.Substring(startingIdx, i - startingIdx + 1);
                    }
                }
            }
            // If primitive value
            else
            {
                for (int i = startingIdx + 1; i < jsonString.Length; ++i)
                {
                    if (!char.IsLetterOrDigit(jsonString[i]) && jsonString[i] != '.' && jsonString[i] != '+' && jsonString[i] != '-')
                    {
                        return jsonString.Substring(startingIdx, i - startingIdx);
                    }
                }
            }

            throw new Exception($"Failed to chunk the provided string - no appropriate closing character found.");
        }
        #endregion


        #region HELPERS
        /// <summary>
        /// Outputs this object as a formatted JSON string.
        /// </summary>
        /// <returns>Formatted JSON representation of this object.</returns>
        new public abstract string ToString();

        /// <summary>
        /// Checks whether the provided string is a valid JSON string quote character.
        /// </summary>
        /// <param name="character">String element to check.</param>
        /// <returns>True if the provided string element is a string-type quote character.</returns>
        internal static bool IsStringQuote(char character)
        {
            return character ==  '\'' || character == '\"';
        }

        /// <summary>
        /// Adds enclosing quote characters to the provided string and returns.
        /// The tyoe of quote added is based on the value of <see cref="CurrentStringMode"/>.
        /// </summary>
        /// <param name="val">The string to add quotes to.</param>
        /// <returns>The provided string wrapped in quotes.</returns>
        internal string WrapInQuotes(string val)
        {
            var quote = LWJson.CurrentStringMode == StringMode.Mode.SingleQuote ? StringMode.SINGLE_QUOTE : StringMode.DOUBLE_QUOTE;
            return $"{quote}{val}{quote}";
        }
        #endregion
    }
}
