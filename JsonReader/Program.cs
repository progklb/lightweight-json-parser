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
            ExamineJson(JsonExamples.jsonStringSimple);

            Console.Read();
        }
        #endregion


        #region HELPERS
        static void ExamineJson(string jsonString)
        {
            var sb = new StringBuilder();

            sb  .AppendLine("---------------------------------------")
                .AppendLine("Start Examination")
                .AppendLine("---------------------------------------");

            sb.Append("Processing... ");
            var obj = LWJson.Parse(jsonString);
            sb.Append("Done!");

            sb  .AppendLine("JSON string provided:")
                .AppendFormat(" - IsValue {0}", obj.IsValue).AppendLine()
                .AppendFormat(" - IsObject {0}", obj.IsObject).AppendLine()
                .AppendFormat(" - IsArray {0}", obj.IsArray).AppendLine();

            Console.WriteLine(sb.ToString());
        }
        #endregion

    }


}
