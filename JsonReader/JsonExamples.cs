using System.IO;

namespace JsonReader
{
	public static class JsonExamples
	{
		#region PROPERTIES
		public static string SimpleKVPObject { get => ReadFromFile("Examples/Simple_KVP_Object.json"); }
		public static string SimpleMixedObject { get => ReadFromFile("Examples/Simple_Mixed_Object.json"); }
		public static string SimpleMixedEmptyObject { get => ReadFromFile("Examples/Simple_Mixed_Empty_Object.json"); }

		public static string SimpleKVPArray { get => ReadFromFile("Examples/Simple_KVP_Array.json"); }
		public static string SimpleMixedArray1 { get => ReadFromFile("Examples/Simple_Mixed_Array_1.json"); }
		public static string SimpleMixedArray2 { get => ReadFromFile("Examples/Simple_Mixed_Array_2.json"); }

		public static string ComplexMixedObject { get => ReadFromFile("Examples/Complex_Mixed_Object.json"); }
		public static string ComplexDiffTypesObject { get => ReadFromFile("Examples/Complex_Diff_Types_Object.json"); }

		public static string InvalidObject { get => ReadFromFile("Examples/Invalid_Object.json"); }
		#endregion


		#region HELPER FUNCTIONS
		/// <summary>
		/// Reads a text file containing JSON, converting it to a <see cref="LWJson"/> object and outputing the results to console.
		/// </summary>
		/// <param name="filename"></param>
		static string ReadFromFile(string filename)
		{
			return File.ReadAllText(filename);
		}
		#endregion
	}
}
