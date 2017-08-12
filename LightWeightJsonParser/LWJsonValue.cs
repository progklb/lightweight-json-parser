using System;
using System.Collections.Generic;

namespace LightWeightJsonParser
{
    /// <summary>
    /// Represents a simple value as part of a key-value pair.
    /// <para>
    /// { key : value }
    /// </para>
    /// <para>
    /// (e.g. { "name" : "Kevin" })
    /// </para>
    /// </summary>
    public class LWJsonValue : LWJson
    {
        #region TYPE
        internal enum DataType
        {
            String,
            Boolean,
            Integer,
            Double,
        }
        #endregion


        #region PROPERTIES
        internal DataType Type { get; private set; }
        public string Value { get; set; }
        #endregion


        #region CONSTRUCTORS
        public LWJsonValue(string value)
        {
            Value = $"\"{value}\"";
            Type = DataType.String;
        }

        public LWJsonValue(bool value)
        {
            Value = value.ToString().ToLowerInvariant();
            Type = DataType.Boolean;
        }

        public LWJsonValue(int value)
        {
            Value = value.ToString();
            Type = DataType.Integer;
        }

        public LWJsonValue(double value)
        {
            // TODO Culture invariant numbers
            Value = value.ToString().Replace(',', '.');
            Type = DataType.Double;
        }
        #endregion


        #region GETTERS
        // TODO Add getters for casts to different types
        #endregion


        #region STRING HANDLING
        public override string ToString()
        {
            return Value;
        }
        #endregion
    }
}
