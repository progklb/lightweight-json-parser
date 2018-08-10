using System;
using System.Text;
using LightWeightJsonParser;

namespace JsonReader
{
	/// <summary>
	/// A simple terminal application to test the <see cref="LWJson"/> library.
	/// </summary>
    class Program
    {
        #region ENTRY POINT
        static void Main(string[] args)
        {
            LWJson.CurrentStringMode = StringMode.Mode.DoubleQuote;
            LWJson.CurrentFailureMode = FailureMode.Silent;
			LWJson.CheckEscapedCharacters = true;

            LWJson.OnItemParsed += (s) => { Console.WriteLine("- " + s); };

			/* In order to test parsing, enable the below method calls.*/

			//ExamineJson(JsonExamples.SimpleKVPObject, "Simple key-value object");
			//ExamineJson(JsonExamples.SimpleMixedObject, "Simple mixed object");
			//ExamineJson(JsonExamples.SimpleMixedEmptyObject, "Simple mixed empty object");

			//ExamineJson(JsonExamples.SimpleKVPArray, "Simple key-value array");
			//ExamineJson(JsonExamples.SimpleMixedArray1, "Simple mixed array 1");
			//ExamineJson(JsonExamples.SimpleMixedArray2, "Simple mixed array 2");

			//ExamineJson(JsonExamples.ComplexMixedObject, "Complex mixed object");
			//ExamineJson(JsonExamples.ComplexDiffTypesObject, "Complex different types object");

			// ExamineJson(JsonExamples.InvalidObject, "Invalid object");

			//CreateJson();

			Console.Read();
        }
        #endregion


        #region HELPERS
        /// <summary>
        /// Parses the provided JSON string and outputs formatted information regarding it.
        /// </summary>
        /// <param name="jsonString">JSON string to parse.</param>
        /// <param name="name">Name to specify in the header of the output.</param>
        static void ExamineJson(string jsonString, string name = "Unspecified string")
        {
            var strB = new StringBuilder();
            LWJson jsonObj;

            // Provide a header and preview of the JSON that will be processed
            strB.AppendLine("---------------------------------------")
                .AppendLine($"Start Examination ({name})")
                .AppendLine("---------------------------------------");

            strB.AppendLine("Preview of JSON string provided:")
                .Append(jsonString.Substring(0, Math.Min(jsonString.Length, 100))).AppendLine("...").AppendLine();

            // Exceptions will be thrown if invalid JSON formatting is encountered.
            strB.Append("Processing... ");
            jsonObj = LWJson.Parse(jsonString);
            strB.AppendLine("Done!").AppendLine();

            // Display details about the processed JSON
            strB.AppendLine("Details:")
                .AppendLine($" - IsString: {jsonObj.IsString}")
                .AppendLine($" - IsBoolean: {jsonObj.IsBoolean}")
                .AppendLine($" - IsInteger: {jsonObj.IsInteger}")
                .AppendLine($" - IsDouble: {jsonObj.IsDouble}")
                .AppendLine($" - IsObject: {jsonObj.IsObject}")
                .AppendLine($" - IsArray: {jsonObj.IsArray}")
                .AppendLine();

            strB.AppendLine("Processed JSON:").AppendLine(jsonObj.ToString());

            Console.WriteLine(strB.ToString());
        }

        /// <summary>
        /// Creates a custom JSON object and then accesses its data for output of the values contained within.
        /// </summary>
        static void CreateJson()
        {
            var strB = new StringBuilder();

            strB.AppendLine("---------------------------------------")
                .AppendLine($"Start Creation")
                .AppendLine("---------------------------------------");

            strB.AppendLine("Creating... ");

            // The JSON that we will build below
            /*{
                 "name" : "Kevin",
                 "age" : 25,
                 "has_camera" : true,
                 "photos_per_day" : 1.735845,
                 "optional_extras" : null,
                 "cameras" : [
                    {
                        "make" : "Canon",
                        "model" : "650D"
                    },
                    {
                        "make" : "Canon",
                        "model" : "AE-1"
                    }
                 ]
            }*/

            // Create a root object and push data into it.
            var root = new LWJsonObject();
            root.Add("name", "Kevin");
            root.Add("age", 25);
            root.Add("has_camera", true);
            root.Add("photos_per_day", 1.735845);
            root.Add("optional_extras", LWJson.NULL);
            // For an array, we specify the key and add a new array with sub objects.
            // Note that LWJsonObject allows chaining of commands.
            root.Add("cameras", 
                new LWJsonArray(
                    new LWJsonObject().Add("make", "Canon").Add("model", "650D"),
                    new LWJsonObject().Add("make", "Canon").Add("model", "AE-1")
                )
            );

            strB.AppendLine("Done!").AppendLine();
            
            // Print out the generated JSON object as a JSON string
            strB.AppendLine(root.ToString()).AppendLine();

            // Access some of the values contained within the JSON object
            var name = root["name"];
            var age = root["age"];
            var hasCam = root["has_camera"];
            var photosPerDay = root["photos_per_day"];
            var cameras = root["cameras"];
            var camera2model = root["cameras"][1]["model"];

            strB.AppendLine($"Name = {name}")
                .AppendLine($"Age = {age}")
                .AppendLine($"Has camera = {hasCam}")
                .AppendLine($"Photos per day = {photosPerDay}")
                .AppendLine($"Cameras = {cameras}")
                .AppendLine($"Camera 2 make = {camera2model}");

            Console.WriteLine(strB.ToString());
        }

		/// <summary>
		/// Reads a text file containing JSON, converting it to a <see cref="LWJson"/> object and outputing the results to console.
		/// </summary>
		/// <param name="filename"></param>
        static void ReadFromFile(string filename)
        {
			var json = System.IO.File.ReadAllText(filename);
			Console.WriteLine(json);

			var jsonObj = LWJson.Parse(json);
			Console.WriteLine(jsonObj.ToString());
		}
		#endregion

	}
}
