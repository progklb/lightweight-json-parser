using System;
using System.Collections.Generic;
using System.Text;

namespace LightWeightJsonParser
{
    /// <summary>
    /// Represents a a collection of key-value pairs that makes up an object. Note that each value
    /// can be of any type <see cref="LWJsonValue"/>, <see cref="LWJsonObject"/>, or <see cref="LWJsonArray"/>.
    /// <para>
    /// { key1 : ..., key2 : ..., keyN : ... }
    /// </para>
    /// <para>
    /// (e.g. { "name" : "Kevin", "location" : {...}, categories" : [...]  } )
    /// </para>
    /// </summary>
    public sealed class LWJsonObject : LWJson
    {
        public static event Action<string> OnOutput = delegate { };

        #region INDEXERS
        public override LWJson this[int i]
        {
            get { throw new Exception("An integer-based index is not a valid indexer for this data type. A string-based key is required."); }
            set { throw new Exception("An integer-based index is not a valid indexer for this data type. A string-based key is required."); }
        }

        public override LWJson this[string key]
        {
            get => ObjectData[$"\"{key}\""];
            set => ObjectData[$"\"{key}\""] = (LWJsonObject)value;
        }
        #endregion


        #region PROPERTIES
        /// Holds the set of key-value pairs of this object.
        public Dictionary<string, LWJson> ObjectData = new Dictionary<string, LWJson>();
        #endregion


        #region PUBLIC API
        /// <summary>
        /// Adds a new value-value pair in which the value is string-based. (e.g. "key" : "string")
        /// </summary>
        /// <param name="key">Key of data pair.</param>
        /// <param name="value">Value of data pair.</param>
        /// <returns>This instance for chaining.</returns>
        public LWJsonObject Add(string key, string value)
        {
            ObjectData.Add($"\"{key}\"", new LWJsonValue(value));
            return this;
        }

        /// <summary>
        /// Adds a new value-value pair in which the value is boolean-based. (e.g. "key" : true)
        /// </summary>
        /// <param name="key">Key of data pair.</param>
        /// <param name="value">Value of data pair.</param>
        /// <returns>This instance for chaining.</returns>
        public LWJsonObject Add(string key, bool value)
        {
            ObjectData.Add($"\"{key}\"", new LWJsonValue(value));
            return this;
        }

        /// <summary>
        /// Adds a new value-value pair in which the value is integer-based. (e.g. "key" : 25)
        /// </summary>
        /// <param name="key">Key of data pair.</param>
        /// <param name="value">Value of data pair.</param>
        /// <returns>This instance for chaining.</returns>
        public LWJsonObject Add(string key, int value)
        {
            ObjectData.Add($"\"{key}\"", new LWJsonValue(value));
            return this;
        }

        /// <summary>
        /// Adds a new value-value pair in which the value is double-based. (e.g. "key" : 1.789651)
        /// </summary>
        /// <param name="key">Key of data pair.</param>
        /// <param name="value">Value of data pair.</param>
        /// <returns>This instance for chaining.</returns>
        public LWJsonObject Add(string key, double value)
        {
            ObjectData.Add($"\"{key}\"", new LWJsonValue(value));
            return this;
        }

        /// <summary>
        /// Adds a new value-value pair in which the value is an object- or array-based. (e.g. "key" : {...}, or "key" : [...] )
        /// </summary>
        /// <param name="key">Key of data pair.</param>
        /// <param name="value">Value of data pair.</param>
        /// <returns>This instance for chaining.</returns>
        public LWJsonObject Add(string key, LWJson value)
        {
            ObjectData.Add($"\"{key}\"", value);
            return this;
        }

        /// <summary>
        /// Removes the data-pair corresponding to the provided key.
        /// </summary>
        /// <param name="key">Key to remove from this object.</param>
        /// <returns>This instance for chaining.</returns>
        public LWJsonObject Remove(string key)
        {
            ObjectData.Remove(key);
            return this;
        }

        /// <summary>
        /// Checks whether the provided key exists in this object.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>True if this object contains a data-pair with the provided key, false otherwise.</returns>
        public bool ContainsKey(string key)
        {
            return ObjectData.ContainsKey(key);
        }

        /// <summary>
        /// Returns the number of data-pairs contained in this object.
        /// </summary>
        /// <returns>The number of data-pairs contained in the object.</returns>
        public int Count()
        {
            return ObjectData.Count;
        }

        /// <summary>
        /// Clears all data-pairs contained within this object.
        /// </summary>
        public void Clear()
        {
            ObjectData.Clear();
        }
        #endregion


        #region STRING HANDLING
        new internal void Parse(string jsonChunk)
        {
            // Check that we have valid JSON and clear this object's data (incase of reuse)
            CheckChunkValidity(jsonChunk);
            ObjectData = new Dictionary<string, LWJson>();

            // Break into key-value pairs.
            // Note that we start after the opening '{' and end before the closing '}'

            int i = 1;

            do
            {
                OnOutput($"start: i={i}={jsonChunk[i]}");

                // Extract the key:
                int startIdx = -1, endIdx = -1;
                while (endIdx == -1)
                {
                    if (IsStringQuote(jsonChunk[i]))
                    {
                        if (startIdx == -1)
                        {
                            startIdx = i + 1;
                        }
                        else
                        {
                            endIdx = i - 1;
                        }
                    }

                    ++i;
                }

                string key = jsonChunk.Substring(startIdx, endIdx - startIdx + 1);

                OnOutput($"key: i={i}={jsonChunk[i]} (key={key})");


                // Skip over seperator and whitespace
                while (jsonChunk[i] == ' ' || jsonChunk[i] == ':') { ++i; }

                // Process the value
                LWJson value = null;

                if (jsonChunk[i] == '{')
                {
                    OnOutput($"obj : i={i}={jsonChunk[i]}");

                    var chunk = ChunkBlock(jsonChunk, i);
                    i += chunk.Length;

                    value = new LWJsonObject();
                    (value as LWJsonObject).Parse(chunk);
                }
                else if (jsonChunk[i] == '[')
                {
                    OnOutput($"arr : i={i}={jsonChunk[i]}");

                    var chunk = ChunkBlock(jsonChunk, i);
                    i += chunk.Length;

                    value = new LWJsonArray();
                    (value as LWJsonArray).Parse(chunk);
                }
                else if (!(jsonChunk.Substring(i, 4) == "null"))
                {
                    OnOutput($"val : i={i}={jsonChunk[i]}");

                    var chunk = ChunkValue(jsonChunk, i);
                    i += chunk.Length;

                    value = new LWJsonValue();
                    (value as LWJsonValue).Parse(chunk);
                }

                // Add key value pair
                Add(key, value);

                OnOutput($" -- final value : i={i}={jsonChunk[i]} (val={value.ToString()})");


                // Skip to end of whitespace
                while (char.IsWhiteSpace(jsonChunk[i])) { i++; }

            // If we have a comma there is more to process. Repeat.
            } while (jsonChunk[i] == ',');
            
            if (i != jsonChunk.Length - 1)
            {
                string failedCharPreview = (jsonChunk.Length - i > 5) ?
                    $"{jsonChunk[i - 2]}{jsonChunk[i - 1]}'{jsonChunk[i]}'{jsonChunk[i + 1]}{jsonChunk[i + 2]}" :
                    $"'{jsonChunk[i]}'";

                throw new Exception("Parsing from JSON failed. Expected final character of object but this is not the case! " +
                    $"(final index = {jsonChunk.Length - 1}, current index = {i}) " +
                    $"(current index = {failedCharPreview})");
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            int iterations = 0;

            sb.Append("{");
            foreach (var pair in ObjectData)
            {
                if (iterations++ != 0)
                {
                    sb.Append(",");
                }
                sb.AppendFormat("{0}:{1}", pair.Key, (pair.Value != null ? pair.Value.ToString() : "null"));
            }
            sb.Append("}");

            return sb.ToString();
        }
        #endregion


        #region HELPERS
        /// <summary>
        /// Checks that the provided JSON chunk begins with an opening curly brace '{'
        /// and ends with a closed curly brace '}'. Thus, the expected format is '{...}'.
        /// </summary>
        /// <param name="jsonChunk"></param>
        private void CheckChunkValidity(string jsonChunk)
        {
            if (jsonChunk[0] != '{' || jsonChunk[jsonChunk.Length - 1] != '}')
            {
                throw new Exception($"Invalid starting/ending character provided for parsing by {nameof(LWJsonObject)}:" +
                    $"Starting=\"{jsonChunk[0]}\" " +
                    $"Ending=\"{jsonChunk[jsonChunk.Length - 1]}\"");
            }
        }

        private string ExtractItem(string json, ref int startIdx)
        {
            return null;
        }
        #endregion
    }
}
