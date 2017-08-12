using System;

namespace LightWeightJsonParser
{
    public abstract class LWJson
    {
        #region PROPERTIES
        /// <summary>
        /// Whether this object is a simple value.
        /// { key : value }
        /// </summary>
        public bool IsValue { get { return (this is LWJsonValue); } }
        /// <summary>
        /// Whether this object is an array of subobjects of key-value pairs.
        /// { key : [{...},...] }
        /// </summary>
        public bool IsArray { get { return (this is LWJsonArray); } }
        /// <summary>
        /// Whether this object is an an object represent a set of key-value pairs.
        /// { key : { ... } }
        /// </summary>
        public bool IsObject { get { return (this is LWJsonObject); } }
        #endregion


        #region PARSING
        public static LWJson Parse(string jsonString)
        {
            return null;
            if (jsonString[0] == '[')
            {
                return new LWJsonArray();
            }
            else if (jsonString[0] == '{')
            {
                return new LWJsonObject();
            }
            
            return null;
        }
        #endregion
    }
}
