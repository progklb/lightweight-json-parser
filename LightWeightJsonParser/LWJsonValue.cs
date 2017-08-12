using System;
using System.Collections.Generic;

namespace LightWeightJsonParser
{
    /// <summary>
    /// Represents a simple value as part of a key-value pair. 
    /// { key : value }
    /// (e.g. { "name" : "Kevin" })
    /// </summary>
    public class LWJsonValue : LWJson
    {
        public string Value;

        // TODO Add getters for casts to different types
    }
}
