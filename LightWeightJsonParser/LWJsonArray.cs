using System;
using System.Collections.Generic;

namespace LightWeightJsonParser
{
    /// <summary>
    /// Represents a a collection of LWJsonObject instances that make up an array of objects.
    /// [ {...}, {...}, ..., {...} } 
    /// (e.g. [ { "name" : "Kevin" }, { "name" : "John" }, { "name" : "Mike" } ] )
    /// </summary>
    public class LWJsonArray : LWJson
    {
        public List<LWJsonObject> Array { get; set; }
    }
}
