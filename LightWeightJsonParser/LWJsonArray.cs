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
        /// Holds the set of objects contained by this array.
        public List<LWJsonObject> ArrayData { get; set; }
        #endregion


        #region CONSTRUCTOR
        public LWJsonArray(params LWJsonObject[] objects)
        {
            ArrayData = new List<LWJsonObject>();
            Add(objects);
        }
        #endregion


        #region PUBLIC API
        /// <summary>
        /// Adds the set of provided objects to the array.
        /// </summary>
        /// <param name="objects">Objects to add to the array.</param>
        /// <returns>This instance for chaining.</returns>
        public LWJsonArray Add(params LWJsonObject[] objects)
        {
            ArrayData.AddRange(objects);
            return this;
        }

        /// <summary>
        /// Removes the set of provided objects from the array.
        /// </summary>
        /// <param name="objects">Objects to remove from the array.</param>
        /// <returns>This instance for chaining.</returns>
        public LWJsonArray Remove(params LWJsonObject[] objects)
        {
            foreach (var obj in objects)
            {
                ArrayData.Remove(obj);
            }
            return this;
        }
        /// <summary>
        /// Removes the object in the array at the specified index.
        /// </summary>
        /// <param name="index">Index to remove.</param>
        /// <returns>This instance for chaining.</returns>
        public LWJsonArray RemoveAt(int index)
        {
            ArrayData.RemoveAt(index);
            return this;
        }

        /// <summary>
        /// Clears all objects contained by this array.
        /// </summary>
        public void Clear()
        {
            ArrayData.Clear();
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
