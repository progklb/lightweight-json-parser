using System;
using System.Text;
using LightWeightJsonParser;

namespace JsonReader
{
    class Program
    {
        #region ENTRY POINT
        static void Main(string[] args)
        {
            //ExamineJson(JsonExamples.SIMPLE_KVP_OBJECT, "Simple key-value object");
            //ExamineJson(JsonExamples.SIMPLE_MIXED_OBJECT, "Simple mixed object");

            //ExamineJson(JsonExamples.SIMPLE_KVP_ARRAY, "Simple key-value array");
            //ExamineJson(JsonExamples.SIMPLE_MIXED_ARRAY, "Simple mixed array");

            //ExamineJson(JsonExamples.COMPLEX_MIXED_OBJECT, "Complex mixed string");

            //ExamineJson(JsonExamples.INVALID_OBJECT, "Invalid object");

            CreateJson();

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

            // Catch any exceptions thrown while processing.
            // These will be thrown if invalid JSON formatting is encountered.
            strB.Append("Processing... ");
            try
            {
                jsonObj = LWJson.Parse(jsonString);
            }
            catch (Exception e)
            {
                Console.WriteLine(strB.ToString());
                Console.WriteLine($"Invalid JSON provided. Exception thrown: {e.Message}");
                return;
            }
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

            strB.AppendLine("Chunked:").AppendLine(LWJson.Chunk(jsonString, 9));

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
        #endregion

    }


}
