using System;
using System.Collections.Generic;
using System.Text;

namespace LightWeightJsonParser
{
    /// <summary>
    /// Represents a a collection of LWJsonObject instances that make up an array of objects.
    /// <para>
    /// [ {...}, {...}, ..., {...} }
    /// </para>
    /// <para>
    /// (e.g. [ { "name" : "Kevin" }, { "name" : "John" }, { "name" : "Mike" } ] )
    /// </para>
    /// </summary>
    public class LWJsonArray : LWJson
    {
        #region INDEXER
        public override LWJson this[int i]
        {
            get => ArrayData[i];
            set => ArrayData[i] = (LWJsonObject)value;
        }

        public override LWJson this[string key]
        {
            get { throw new Exception("An string-based key is not a valid indexer for this data type. An integer-based index is required."); }
            set { throw new Exception("An string-based key is not a valid indexer for this data type. An integer-based index is required."); }
        }
        #endregion


        #region PROPERTIES
        public List<LWJsonObject> ArrayData { get; set; }
        #endregion


        #region CONSTRUCTOR
        public LWJsonArray(params LWJsonObject[] objects)
        {
            ArrayData = new List<LWJsonObject>();
            ArrayData.AddRange(objects);
        }
        #endregion


        #region STRING HANDLING
        public override string ToString()
        {
            var sb = new StringBuilder();
            int iterations = 0;

            sb.Append("[");
            foreach (var obj in ArrayData)
            {
                if (iterations++ != 0)
                {
                    sb.Append(",");
                }
                sb.Append($"{obj}");
            }
            sb.Append("]");

            return sb.ToString();
        }
        #endregion
    }
}
