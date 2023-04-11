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
	public sealed class LWJsonArray : LWJson
	{
		#region INDEXER
		public override LWJson this[int i]
		{
			get => ArrayData[i];
			set => ArrayData[i] = (LWJsonObject)value;
		}

		public override LWJson this[string key]
		{
			get { throw new LWJPException("An string-based key is not a valid indexer for this data type. An integer-based index is required."); }
			set { throw new LWJPException("An string-based key is not a valid indexer for this data type. An integer-based index is required."); }
		}
		#endregion


		#region PROPERTIES
		public override JsonDataType DataType { get { return JsonDataType.Array; } }

		/// <summary>
		/// Holds the set of objects contained by this array.
		/// </summary>
		public List<LWJson> ArrayData { get; set; }
		/// <summary>
		/// The number of items currently held in this array.
		/// </summary>
		public int Count { get { return ArrayData.Count; } }
		#endregion


		#region CONSTRUCTOR
		public LWJsonArray(params LWJson[] objects)
		{
			ArrayData = new List<LWJson>();
			Add(objects);
		}
		#endregion


		#region PUBLIC API
		/// <summary>
		/// Adds the set of provided items to the array.
		/// </summary>
		/// <param name="objects">Items to add to the array.</param>
		/// <returns>This instance for chaining.</returns>
		public LWJsonArray Add(params LWJson[] objects)
		{
			ArrayData.AddRange(objects);
			return this;
		}

		/// <summary>
		/// Removes the set of provided items from the array.
		/// </summary>
		/// <param name="objects">Items to remove from the array.</param>
		/// <returns>This instance for chaining.</returns>
		public LWJsonArray Remove(params LWJson[] objects)
		{
			foreach (var obj in objects)
			{
				ArrayData.Remove(obj);
			}
			return this;
		}
		/// <summary>
		/// Removes the item in the array at the specified index.
		/// </summary>
		/// <param name="index">Index to remove.</param>
		/// <returns>This instance for chaining.</returns>
		public LWJsonArray RemoveAt(int index)
		{
			ArrayData.RemoveAt(index);
			return this;
		}

		/// <summary>
		/// Clears all items contained by this array.
		/// </summary>
		public void Clear()
		{
			ArrayData.Clear();
		}
		#endregion


		#region STRING HANDLING
		internal void Parse(string jsonChunk, string outputSpacer = "")
		{
			CheckChunkValidity(jsonChunk);

			ArrayData = new List<LWJson>();

			int i = 0;

			do
			{
				// Because we start at index i=0 which is '[', 
				// or loop back here on an encountered comma separator ',',
				// iterate before checking if the character is whitespace.
				do
				{ i++; } while (char.IsWhiteSpace(jsonChunk[i]));

				// Check if empty and only process if not.
				if (jsonChunk[i] != ']')
				{
					LWJson value;

					var chunk = Chunk(jsonChunk, i);
					i += chunk.Length;

					ParseChunk(out value, chunk, outputSpacer);

					ArrayData.Add(value);

					while (char.IsWhiteSpace(jsonChunk[i]))
					{ i++; }
				}
			} while (jsonChunk[i] == ',');

			if (i != jsonChunk.Length - 1)
			{
				string failedCharPreview = (jsonChunk.Length - i > 5) ?
					$"{jsonChunk[i - 2]}{jsonChunk[i - 1]}'{jsonChunk[i]}'{jsonChunk[i + 1]}{jsonChunk[i + 2]}" :
					$"'{jsonChunk[i]}'";

				throw new LWJPException("Parsing from JSON failed. Expected final character of array but this is not the case! " +
					$"(final index = {jsonChunk.Length - 1}, current index = {i}) " +
					$"(current index = {failedCharPreview})");
			}
		}

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
				sb.Append($"{obj.ToString()}");
			}
			sb.Append("]");

			return sb.ToString();
		}

		/// <summary>
		/// Checks that the provided JSON chunk begins with an opening square bracket '['
		/// and ends with a closed square bracket ']'. Thus, the expected format is '[...]'.
		/// </summary>
		/// <param name="jsonChunk"></param>
		private void CheckChunkValidity(string jsonChunk)
		{
			if (jsonChunk[0] != '[' || jsonChunk[jsonChunk.Length - 1] != ']')
			{
				throw new LWJPException($"Invalid starting/ending character provided for parsing by {nameof(LWJsonArray)}:" +
					$"Starting=\"{jsonChunk[0]}\" " +
					$"Ending=\"{jsonChunk[jsonChunk.Length - 1]}\"");
			}
		}
		#endregion
	}
}
