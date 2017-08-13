using System;
using System.Collections.Generic;
using System.Text;

namespace LightWeightJsonParser
{
    /// <summary>
    /// Represents a a collection of key-value pairs that makes up an object. Note that each value
    /// can be of any type <see cref="LWJsonValue"/>,  <see cref="LWJsonObject"/>, or  <see cref="LWJsonArray"/>.
    /// <para>
    /// { key1 : ..., key2 : ..., keyN : ... }
    /// </para>
    /// <para>
    /// (e.g. { "name" : "Kevin", "location" : {...}, categories" : [...],  } )
    /// </para>
    /// </summary>
    public class LWJsonObject : LWJson
    {
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
                sb.Append($"{pair.Key}:{pair.Value}");
            }
            sb.Append("}");

            return sb.ToString();
        }
        #endregion
    }
}
