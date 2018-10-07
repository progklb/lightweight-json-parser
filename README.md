# Lightweight JSON Parser


[![NuGet](https://img.shields.io/nuget/v/LightWeightJsonParser.svg?style=flat-square)](https://www.nuget.org/packages/LightWeightJsonParser/)
[![license](https://img.shields.io/github/license/mashape/apistatus.svg?style=flat-square)](https://github.com/0bl1v1on/vs-lightweight-json-parser/blob/master/LICENSE.md)

LWJP is a JSON parser written in C# that has only `System` as a dependency.


## Technical

LWJP is JSON-standard compliant and should work with any valid JSON string. Because of the aim of "as light as possible" there is no serialization/deserialization support for converting JSON to .NET compliant objects. `LWJson` objects act as simple containers for strings or other `LWJson`-derived objects. There are 4 types of objects available:

 - `LWJson` - An abstract base class for more specific types. 
 - `LWJsonObject` - Concrete type that contains a key:value pairs. These are stored internally in a `Dictionary<string, LWJson>`
 - `LWJsonArray` - Concrete type that contains a set of `LWJson` objects. These are stored internally in a `List<LWJson>`
 - `LWJsonValue` - Concrete type that contains a simple value, stored internally as a `string`

Although `LWJsonValue` objects store their value internally as a string, the API allows setting and getting the value as any of the following primitive types: `string, bool, int, double`. Conversion of floating point numbers to strings and vice versa is culturally aware and will be automatically handled.

The `LWJson` base class has a number of convenience methods and options for parsing, as well as an event that can be subscribed to that is raised each time an element of a JSON string is parsed (for debugging purposes).

As an example, let's say we have the simple JSON object below:
```
{
  "Person" :
   {
      "FirstName" : "Sterling",
      "LastName" : "Archer",
      "Age" : 36 
   }, 
   "Fears": 
    [
        "Crocodiles", 
        "Alligators", 
        "Aneurysms"
    ],
    "Notes":null
}
```

When parsed, this JSON will be returned as an `LWJsonObject` because the root of the JSON is of object-type. This `LWJsonObject` will have 2 key/value pairs: *Person*:`LWJsonObject` and *Fears*:`LWJsonArray`. The *Person* contains a further 3 key/value pairs, where each value is a simple `LWJsonValue` type. The *Fears* key points to a `LWJsonArray` of `LWJsonValue` objects.

To illustrate this, below is a breakdown of each type :
```
LWJsonObject
    "Person":LWJsonObject
              "FirstName":LWJsonValue"
              "LastName":LWJsonValue"
              "Age":LWJsonValue"
    "Fears":LWJsonArray
              LWJsonValue
              LWJsonValue
              LWJsonValue
    "Notes":LWJson
```

Because `LWJsonObject` and `LWJsonArray` both wrap the native `Dictionary` and `List` types, the backing data object has been left accessible for less common use-case. To access the backing data object of object, simple use the properties `LWJsonObject.ObjectData` and `LWJsonArray.ArrayData`.

**Note:** Also included in the solution is a console-based test application with a number of fixed JSON strings that are parsed with output, and the creation of a new LWJson object to output as JSON.
 
## Usage

Please note that the examples below make use of the JSON above.

### Parsing

The `LWJson` base class has a static `Parse(string jsonString)` method that can be used to generate a new `LWJson` object from the provided JSON string. The returned `LWJson` object can be of either `LWJsonObject` or `LWJsonArray` type, depending on the provided JSON string.

```
var jsonString = " ... ";
var jsonObject = LWJson.Parse(jsonString);
```

### Object traversal

Once an object instance has been created, we can access its contents by using the index operators.
- Because `LWJsonObject` stores key/value pairs, we provide the `string` key to access its values.
- Because `LWJsonArray` stores an array of values, we provide a numberical index to access its elements.

Note that if a key is provided to an array, or an index is provided to an object, an appropriate exception will be thrown.

Because each index request will return a `LWJson` derived object, these operators can be chained in order to drill down into the JSON structure.

Let's get Archer's age and his primary fear:
```
var age = jsonObject["Person"]["Age"];
var fear = jsonObject["Fears"][0];
```

We have now extracted these values, but note that they are still of `LWJson` type.

### Type handling

If we are unsure of what a JSON response contains, there are a number of available calls that will return a boolean value indicating an objects type. 
 - To check if the object is of type `LWJsonObject` or `LWJsonArray` type, we make use of: `IsObject` and `IsArray`.
 - To check if the object is of type `LWJsonValue` we use `IsValue`
 - Because `LWJsonValue` can store various value types, we check the type by making use of: `IsString`, `IsBoolean`, `IsInteger`, and `IsDouble`.

Note that each object (once parsed) keeps track of its own type, and this can be accessed through the `DataType` property.

Because all objects are derived from `LWJson`, in order to retrieve an object as its concrete type there are number of convenience conversion methods:
 - `AsObject`, `AsArray`, `AsString`, `AsBoolean`, `AsInteger`, and `AsDouble`

Let us assume that we don't know if a response will return a single object of a person, or an array. The following example handles this:

```
string firstName;

if (jsonObject.IsObject)
{
    firstName = jsonObject["Person"]["FirstName].AsString();
}
else if (jsonObject.IsArray && jsonObject.AsArray().Count > 0)
{
    firstName = jsonObject[0]["Person"]["FirstName].AsString();
}
```

As an alternative, we can switch over the `DataType` property:
```
string firstName;

switch (jsonObject.DataType)
{
  case JsonDataType.Object:
    firstName = jsonObject["Person"]["FirstName].AsString();
    break;
  case JsonDataType.Array:
    if (jsonObject.AsArray().Count > 0)
    {
       firstName = jsonObject[0]["Person"]["FirstName].AsString();
    }
    break;
}
```

Null objects are handled by simply checking for `null` using the standard equality operators `==` or `!=`. Any null JSON objects are represented by a null `LWJson` instance, and there is a convenience constant at `LWJson.NULL`.

```
string notes = "There are no notes on this person";

if (jsonObject["Notes"] != null)
{
    notes = jsonObject["Notes"].AsString();
}
```

### Object creation

We can create our own `LWJson` objects and output them as JSON string. To do this, we create a root element (array or object) and then build from there. Note that adding entries to either `LWJsonObject`s or `LWJsonArray`s will return the object for chaining. For example, to build the example above we would make the following calls:

```
// Standard usage
var root = new LWJsonObject();
root.Add("Person", new LWJsonObject()
            .Add("FirstName", "Sterling")
            .Add("LastName", "Archer")
            .Add("Age", 36);
root.Add("Fears", new LWJsonArray().Add("Crocodiles", "Aligators", "Aneurysms");
root.Add("Notes", LWJson.NULL);

// Using the backing object directly
root.ObjectData.Add("FavouriteFood", new LWJsonValue("Eggs Woodhouse"));
```

We can also modify the data easily:

```
// Standard usage
root.Remove("Notes");
root["FirstName"] = "Sterling Malory";

// Using the backing object directly
root.ObjectData.Remove("FavouriteFood");
```

We can output this object as a valid JSON string by calling `LWJson.ToString()`.
```
var jsonString = root.ToString();
```


### Additional functionality

There are options on the `LWJson` option that affect how certain operations are handled.

#### String output
 - `LWJson.CurrentStringMode` can be set to determine whether we assign single or double quotes to strings during output to JSON. Note that this does not affect parsing.
 - `LWJson.CurrentFailureMode` affects how value parsing failures are handled.
 - `LWJson.CheckEscapedCharacters` considers escaped quotes `\"` or `\'`. This is because certain APIs respond with two characters that make up an escape sequence instead of a single character (for example '\' + '"' instead of '\"'). If this is encountered, it could cause the parser to misinterpret a quote in a string as the end of the string and thus this check for explicitely escaped sequences.
 
#### Chunking
There is an exposed method `LWJson.ParseChunk` that given an `LWJson` object and a string will iterate over the string and assign a representation of it to the object. This requires that the first index of the string is an object character of an object or array (`{` or `[`) or is the start of a string value, number, boolean, or null. The provided `LWJson` object's type is irrelavent as it will be internally reassigned to what is required.

```
var jsonString = "{ 'key' : 'value' }";
LWJson obj;
LWJson.ParseChunk(out obj, jsonString); // obj is now a LWJsonObject with the information provided

var jsonValue = "12.51248";
LWJson obj;
LWJson.ParseChunk(out obj, jsonValue);  // obj is now a LWJsonValue with the information provided
```

The power of this is that we can extract a substring from a JSON string, such as:
```
var jsonValue = "[ { 'key1' : 'value' }, { 'key2' : { 'keyA' : 'value' } }, { 'key3' : 'value' }]";
LWJson obj;
LWJson.ParseChunk(out obj, jsonValue.Substring(36)); // obj is now a LWJsonObject containing { 'keyA' : 'value' };
```

#### Escape character conversion
`LWJson.CheckEscapedCharacters` will not do any explicit convertion from two-character to single-character representations of escape characters in strings. There is a convenience method `LWJson.ConvertEscapeCharacters` that will convert escape sequences in a provided string to their single-character representation.



## Notes

Although `LWJson.CurrentFailureMode` determines how parsing of values is handled, if the parsing of the JSON string itself fails (due to an invalid JSON string or a case that has not been accounted for by this parser) an exception will be thrown. It is wise to always wrap parse opertions in a `try/catch` structure and handle failures appropriately. 

Exceptions should be thrown with information regarding what went wrong, but it can be hard to determine exactly what the cause was (because the failure will often occur further down the string caused by a problem previously encountered which put the parser out-of-state for the current section it is parsing).

For debugging purposes there is an event `LWJson.OnItemParsed` that is raised on successfully parsing a chunk of JSON and carries with it the string that was most recently parsed. This can be subscribed to in order to see how far the parser got and determine roughly where the failure is happening.
