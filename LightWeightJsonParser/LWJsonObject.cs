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
        public Dictionary<string, LWJson> ObjectData = new Dictionary<string, LWJson>();
        #endregion


        #region PUBLIC API
        public LWJsonObject Add(string key, string value)
        {
            ObjectData.Add($"\"{key}\"", new LWJsonValue(value));
            return this;
        }

        public LWJsonObject Add(string key, bool value)
        {
            ObjectData.Add($"\"{key}\"", new LWJsonValue(value));
            return this;
        }

        public LWJsonObject Add(string key, int value)
        {
            ObjectData.Add($"\"{key}\"", new LWJsonValue(value));
            return this;
        }

        public LWJsonObject Add(string key, double value)
        {
            ObjectData.Add($"\"{key}\"", new LWJsonValue(value));
            return this;
        }

        public LWJsonObject Add(string key, LWJson value)
        {
            ObjectData.Add($"\"{key}\"", value);
            return this;
        }
        #endregion


        #region STRING HANDLING
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
