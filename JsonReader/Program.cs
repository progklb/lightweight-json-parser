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
            ExamineJson(JsonExamples.SIMPLE_OBJECT, "Simple string");
            ExamineJson(JsonExamples.SIMPLE_ARRAY, "Simple array");
            ExamineJson(JsonExamples.COMPLEX_OBJECT, "Complex string");
            ExamineJson(JsonExamples.INVALID_JSON, "Invalid string");

            Console.Read();
        }
        #endregion


        #region HELPERS
        static void ExamineJson(string jsonString, string name = "Unspecified string")
        {
            var strB = new StringBuilder();
            LWJson jsonObj;

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
                .AppendLine($" - IsValue: {jsonObj.IsValue}")
                .AppendLine($" - IsObject: {jsonObj.IsObject}")
                .AppendLine($" - IsArray: {jsonObj.IsArray}");

            Console.WriteLine(strB.ToString());
        }
        #endregion

    }


}
