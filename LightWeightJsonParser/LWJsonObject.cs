using System;
using System.Collections.Generic;

namespace LightWeightJsonParser
{
    /// <summary>
    /// Represents a a collection of key-value pairs that makes up an object. Note that each value
    /// can be of any type LWJsonValue, LWJsonObject, or LWJsonArray.
    /// { key1 : ..., key2 : ..., keyN : ... }
    /// (e.g. { "name" : "Kevin", "location" : {...}, categories" : [...],  } )
    /// </summary>
    public class LWJsonObject : LWJson
    {
        Dictionary<string, LWJson> Object;
    }
}
