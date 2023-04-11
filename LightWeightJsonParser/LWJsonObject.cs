using System;
using System.Collections.Generic;
using System.Text;

namespace LightWeightJsonParser
{
	/// <summary>
	/// Represents a a collection of key-value pairs that makes up an object. Note that each value
	/// can be of any type <see cref="LWJsonValue"/>, <see cref="LWJsonObject"/>, or <see cref="LWJsonArray"/>.
	/// <para>
	/// { key1 : ..., key2 : ..., keyN : ... }
	/// </para>
	/// <para>
	/// (e.g. { "name" : "Kevin", "location" : {...}, categories" : [...]  } )
	/// </para>
	/// </summary>
	public sealed class LWJsonObject : LWJson
	{
		#region INDEXERS
		public override LWJson this[int i]
		{
			get { throw new LWJPException("An integer-based index is not a valid indexer for this data type. A string-based key is required."); }
			set { throw new LWJPException("An integer-based index is not a valid indexer for this data type. A string-based key is required."); }
		}

		public override LWJson this[string key]
		{
			get => m_ObjectData[key];
			set => m_ObjectData[key] = (LWJsonObject)value;
		}
		#endregion


		#region PROPERTIES
		public override JsonDataType DataType { get { return JsonDataType.Object; } }

		/// <summary>
		/// Holds the set of key-value pairs of this object.
		/// </summary>
		public Dictionary<string, LWJson> ObjectData = new Dictionary<string, LWJson>();
		#endregion


		#region VARIABLES
		private Dictionary<string, LWJson> m_ObjectData = new Dictionary<string, LWJson>();
		#endregion


		#region PUBLIC API
		/// <summary>
		/// Adds a new value-value pair in which the value is string-based. (e.g. "key" : "string")
		/// </summary>
		/// <param name="key">Key of data pair.</param>
		/// <param name="value">Value of data pair.</param>
		/// <returns>This instance for chaining.</returns>
		public LWJsonObject Add(string key, string value)
		{
			m_ObjectData.Add(key, new LWJsonValue(value));
			return this;
		}

		/// <summary>
		/// Adds a new value-value pair in which the value is boolean-based. (e.g. "key" : true)
		/// </summary>
		/// <param name="key">Key of data pair.</param>
		/// <param name="value">Value of data pair.</param>
		/// <returns>This instance for chaining.</returns>
		public LWJsonObject Add(string key, bool value)
		{
			m_ObjectData.Add(key, new LWJsonValue(value));
			return this;
		}

		/// <summary>
		/// Adds a new value-value pair in which the value is integer-based. (e.g. "key" : 25)
		/// </summary>
		/// <param name="key">Key of data pair.</param>
		/// <param name="value">Value of data pair.</param>
		/// <returns>This instance for chaining.</returns>
		public LWJsonObject Add(string key, int value)
		{
			m_ObjectData.Add(key, new LWJsonValue(value));
			return this;
		}

		/// <summary>
		/// Adds a new value-value pair in which the value is double-based. (e.g. "key" : 1.789651)
		/// </summary>
		/// <param name="key">Key of data pair.</param>
		/// <param name="value">Value of data pair.</param>
		/// <returns>This instance for chaining.</returns>
		public LWJsonObject Add(string key, double value)
		{
			m_ObjectData.Add(key, new LWJsonValue(value));
			return this;
		}

		/// <summary>
		/// Adds a new value-value pair in which the value is an object- or array-based. (e.g. "key" : {...}, or "key" : [...] )
		/// </summary>
		/// <param name="key">Key of data pair.</param>
		/// <param name="value">Value of data pair.</param>
		/// <returns>This instance for chaining.</returns>
		public LWJsonObject Add(string key, LWJson value)
		{
			m_ObjectData.Add(key, value);
			return this;
		}

		/// <summary>
		/// Removes the data-pair corresponding to the provided key.
		/// </summary>
		/// <param name="key">Key to remove from this object.</param>
		/// <returns>This instance for chaining.</returns>
		public LWJsonObject Remove(string key)
		{
			m_ObjectData.Remove(key);
			return this;
		}

		/// <summary>
		/// Checks whether the provided key exists in this object.
		/// </summary>
		/// <param name="key"></param>
		/// <returns>True if this object contains a data-pair with the provided key, false otherwise.</returns>
		public bool ContainsKey(string key)
		{
			return m_ObjectData.ContainsKey(key);
		}

		/// <summary>
		/// Returns the number of data-pairs contained in this object.
		/// </summary>
		/// <returns>The number of data-pairs contained in the object.</returns>
		public int Count()
		{
			return m_ObjectData.Count;
		}

		/// <summary>
		/// Clears all data-pairs contained within this object.
		/// </summary>
		public void Clear()
		{
			m_ObjectData.Clear();
		}
		#endregion


		#region STRING HANDLING
		internal void Parse(string jsonChunk, string outputSpacer = "")
		{
			// Check that we have valid JSON and clear this object's data (incase of reuse)
			CheckChunkValidity(jsonChunk);
			m_ObjectData = new Dictionary<string, LWJson>();

			// Break into key-value pairs.
			// Note that we start after the opening '{' and end before the closing '}'

			int i = 1;
			bool emptyObject = false;

			do
			{
				// Extract the key:
				int startIdx = -1, endIdx = -1;
				while (endIdx == -1)
				{
					// Look for start/end of key.
					if (IsStringQuote(jsonChunk[i]))
					{
						if (startIdx == -1)
						{
							startIdx = i + 1;
						}
						else
						{
							endIdx = i - 1;
						}
					}
					// Check that we don't have an empty object.
					else if (jsonChunk[i] == '}')
					{
						emptyObject = true;
						break;
					}

					++i;
				}

				if (emptyObject)
				{ break; }

				string key = jsonChunk.Substring(startIdx, endIdx - startIdx + 1);
				OnItemParsed($"{outputSpacer}{key}");

				// Skip over seperator and whitespace
				while (char.IsWhiteSpace(jsonChunk[i]) || jsonChunk[i] == ':')
				{ ++i; }

				// Process the value
				LWJson value;

				var chunk = Chunk(jsonChunk, i);
				i += chunk.Length;

				ParseChunk(out value, chunk, outputSpacer);

				// Add key value pair
				if (!m_ObjectData.ContainsKey(key))
				{
					Add(key, value);
				}

				// Skip to end of whitespace
				while (char.IsWhiteSpace(jsonChunk[i]))
				{ i++; }

				// If we have a comma there is more to process. Repeat.
			} while (jsonChunk[i] == ',');

			if (i != jsonChunk.Length - 1)
			{
				string failedCharPreview = (jsonChunk.Length - i > 5) ?
					$"{jsonChunk[i - 2]}{jsonChunk[i - 1]}{jsonChunk[i]}{jsonChunk[i + 1]}{jsonChunk[i + 2]}" :
					$"{jsonChunk[i]}";

				throw new LWJPException("Parsing from JSON failed. Expected final character of object but this is not the case! " +
					$"(final index = {jsonChunk.Length - 1}, current index = {i}, failed char = {jsonChunk[i]}) " +
					$"(current index = [{failedCharPreview}])");
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			int iterations = 0;

			sb.Append("{");
			foreach (var pair in m_ObjectData)
			{
				if (iterations++ != 0)
				{
					sb.Append(",");
				}
				sb.AppendFormat("{0}:{1}", WrapInQuotes(pair.Key), (pair.Value != null ? pair.Value.ToString() : "null"));
			}
			sb.Append("}");

			return sb.ToString();
		}
		#endregion


		#region HELPERS
		/// <summary>
		/// Checks that the provided JSON chunk begins with an opening curly brace '{'
		/// and ends with a closed curly brace '}'. Thus, the expected format is '{...}'.
		/// </summary>
		/// <param name="jsonChunk"></param>
		private static void CheckChunkValidity(string jsonChunk)
		{
			if (jsonChunk[0] != '{' || jsonChunk[jsonChunk.Length - 1] != '}')
			{
				throw new LWJPException($"Invalid starting/ending character provided for parsing by {nameof(LWJsonObject)}:" +
					$"Starting=\"{jsonChunk[0]}\" " +
					$"Ending=\"{jsonChunk[jsonChunk.Length - 1]}\"");
			}
		}
		#endregion
	}
}
