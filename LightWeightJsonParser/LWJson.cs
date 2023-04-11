using System;

namespace LightWeightJsonParser
{
	public abstract class LWJson
	{
		#region CONSTANTS
		/// <summary>
		/// Represents a null instance of this type. This can be used in place of a null value type.
		/// </summary>
		public const LWJson NULL = null;
		/// <summary>
		/// The spacer added to output when going one level into an object. (This accumulates with each level to build a tree of output).
		/// </summary>
		private static readonly string OUTPUT_SPACER = "   ";
		#endregion


		#region INDEXERS
		/// <summary>
		/// Gets or sets the item at the index specified. This is only valid for <see cref="LWJsonArray"/> types.
		/// </summary>
		/// <param name="i">Index of the array to access.</param>
		/// <returns>The value at the provided index.</returns>
		public virtual LWJson this[int i]
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets or sets the item with the specified key. This is only valid for <see cref="LWJsonObject"/> types.
		/// </summary>
		/// <param name="key">Key of the data attribute to access.</param>
		/// <returns>The value of the data matching the provided key.</returns>
		public virtual LWJson this[string key]
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}
		#endregion


		#region EVENTS
		/// <summary>
		/// Is raised during Parse operations on every item parsed. This helps identify where parsing is failing.
		/// </summary>
		public static Action<string> OnItemParsed = delegate { };
		#endregion


		#region PROPERTIES
		/// <summary>
		/// The string mode that determines the quotation marks used when outputting a <see cref="LWJson"/> item to a JSON string. Used for keys and string types.
		/// </summary>
		public static StringMode.Mode CurrentStringMode { get; set; } = StringMode.Mode.DoubleQuote;
		/// <summary>
		/// Determines how value parsing failures are handled.
		/// </summary>
		public static FailureMode CurrentFailureMode { get; set; } = FailureMode.Verbose;
		/// <summary>
		/// If set to true, parsing will ignore any closing quote characters that are preceded by a backslash.
		/// </summary>
		public static bool CheckEscapedCharacters { get; set; } = true;

		/// <summary>
		/// The data type of this item.
		/// </summary>
		public virtual JsonDataType DataType { get; protected set; }

		/// <summary>
		/// Whether this item is a string-based value.
		/// { "key" : "string" }
		/// </summary>
		public bool IsString { get => DataType == JsonDataType.String; }
		/// <summary>
		/// Whether this item is a boolean-based value.
		/// { "key" : true }
		/// </summary>
		public bool IsBoolean { get => DataType == JsonDataType.Boolean; }
		/// <summary>
		/// Whether this item is an integer-based value.
		/// { "key" : 25 }
		/// </summary>
		public bool IsInteger { get => DataType == JsonDataType.Integer; }
		/// <summary>
		/// Whether this item is a double-based value.
		/// { "key" : 1.8458 }
		/// </summary>
		public bool IsDouble { get => DataType == JsonDataType.Double; }
		/// <summary>
		/// Whether this item is an an object representing a set of key-value pairs.
		/// { key : { ... } }
		/// </summary>
		public bool IsObject { get => DataType == JsonDataType.Object; }
		/// <summary>
		/// Whether this item is an array of sub-items.
		/// { "key" : [{...},...] }
		/// </summary>
		public bool IsArray { get => DataType == JsonDataType.Array; }
		/// <summary>
		/// Whether this item is a simple value. Note that this is a convenience method for performing more specific type checks, 
		/// as a <see cref="LWJsonValue"/> can represent any of <see cref="string"/>, <see cref="bool"/>, <see cref="int"/>, or <see cref="double"/>.
		/// { "key" : value }
		/// </summary>
		public bool IsValue { get => (this is LWJsonValue); }
		#endregion


		#region CASTING
		/// <summary>
		/// Returns this item as a string value.
		/// </summary>
		/// <returns>The string representation of this item.</returns>
		public virtual string AsString() => throw new InvalidCastException();
		/// <summary>
		/// Returns this item as a boolean value.
		/// </summary>
		/// <returns>The boolean representation of this item.</returns>
		public virtual bool AsBoolean() => throw new InvalidCastException();
		/// <summary>
		/// Returns this item as an integer value.
		/// </summary>
		/// <returns>The integer representation of this item.</returns>
		public virtual int AsInteger() => throw new InvalidCastException();
		/// <summary>
		/// Returns this item as a double value.
		/// </summary>
		/// <returns>The double representation of this item.</returns>
		public virtual double AsDouble() => throw new InvalidCastException();
		/// <summary>
		/// Returns this item as a JSON object.
		/// </summary>
		/// <returns>The JSON object representation of this item.</returns>
		public virtual LWJsonObject AsObject() => this is LWJsonObject ? this as LWJsonObject : throw new InvalidCastException($"Cannot cast object of type {DataType} to Object");
		/// <summary>
		/// Returns this item as a JSON array.
		/// </summary>
		/// <returns>The JSON array representation of this item.</returns>
		public virtual LWJsonArray AsArray() => this is LWJsonArray ? this as LWJsonArray : throw new InvalidCastException($"Cannot cast object of type {DataType} to Array");
		/// <summary>
		/// Returns this item as a <see cref="LWJsonValue"/>. This is a container type for any of <see cref="string"/>, <see cref="bool"/>, <see cref="int"/>, or <see cref="double"/>. 
		/// </summary>
		/// <returns>The <see cref="LWJsonValue"/> representation of this item.</returns>
		public virtual LWJsonValue AsValue() => this is LWJsonValue ? this as LWJsonValue : throw new InvalidCastException($"Cannot cast object of type {DataType} to Value");
		#endregion


		#region PARSING
		/// <summary>
		/// Accepts a valid JSON string, producing and returning a corresponding <see cref="LWJSon"/> JSON object.
		/// </summary>
		/// <param name="jsonString">A valid JSON string.</param>
		/// <returns>A corresponding LWJSon object that represents the provided string.</returns>
		public static LWJson Parse(string jsonString)
		{
			// Sanitise the ends of the string
			jsonString = jsonString.Trim();

			LWJson json;

			// Check what object type to start with.
			switch (jsonString[0])
			{
				case '{':
					json = new LWJsonObject();
					(json as LWJsonObject).Parse(jsonString);
					break;
				case '[':
					json = new LWJsonArray();
					(json as LWJsonArray).Parse(jsonString);
					break;
				default:
					throw new LWJPException($"Invalid initial character of JSON string: \"{jsonString[0]}\"");
			}

			return json;
		}

		/// <summary>
		/// Provided a valid string chunk (object, array, or value type) this will parse the chunk and assign
		/// a corresponding value to the provided <paramref name="value"/>.
		/// </summary>
		/// <param name="value">The object which to assign the parse chunk's value to.</param>
		/// <param name="chunk">The string of JSON to parse. This must be trimmed correctly beforehand.</param>
		/// <param name="outputSpacer">An optional parameter that inserts spacing for formatting output of the <see cref="OnItemParsed"/> event.</param>
		public void ParseChunk(out LWJson value, string chunk, string outputSpacer = "")
		{
			if (chunk[0] == '{')
			{
				value = new LWJsonObject();
				(value as LWJsonObject).Parse(chunk, outputSpacer + OUTPUT_SPACER);
			}
			else if (chunk[0] == '[')
			{
				value = new LWJsonArray();
				(value as LWJsonArray).Parse(chunk, outputSpacer + OUTPUT_SPACER);
			}
			else if (chunk == "null")
			{
				value = null;
				OnItemParsed($"{outputSpacer}{OUTPUT_SPACER}null");
			}
			else
			{
				value = new LWJsonValue();
				if (!(value as LWJsonValue).Parse(chunk))
				{
					if (LWJson.CurrentFailureMode == FailureMode.Nullify)
					{
						value = null;
					}
				}

				OnItemParsed($"{outputSpacer}{OUTPUT_SPACER}{value.ToString()}");
			}
		}

		/// <summary>
		/// Provided a JSON string and starting index (which must correspond to an opening array or object character, opening string quote, or first character of a value), 
		/// this automatically determines if it should be chunked as an object, array, string, or primitive value. The chunked value is returned.
		/// </summary>
		/// <param name="jsonString">The JSON string to extract an array, object, string, or primitive value from.</param>
		/// <param name="startingIdx">The index of the opening character of the array, object, or string (quote), or the first character of a primitive value.</param>
		/// <returns>The extracted item.</returns>
		internal static string Chunk(string jsonString, int startingIdx)
		{
			if (jsonString[startingIdx] == '{' || jsonString[startingIdx] == '[')
			{
				return ChunkBlock(jsonString, startingIdx);
			}
			else
			{
				return ChunkValue(jsonString, startingIdx);
			}
		}

		/// <summary>
		/// Provided a JSON string and a starting index (which must correspond to an opening array or object character), 
		/// this will return the array or object in its entirety up to the corresponding closing character.
		/// </summary>
		/// <param name="jsonString">The JSON string to extract an array or object from.</param>
		/// <param name="startingIdx">The index of the opening character of the array or object.</param>
		/// <returns>The extracted object or array block.</returns>
		internal static string ChunkBlock(string jsonString, int startingIdx)
		{
			char openingChar = jsonString[startingIdx];
			char closingChar;

			switch (openingChar)
			{
				case '{':
					closingChar = '}';
					break;
				case '[':
					closingChar = ']';
					break;
				default:
					throw new LWJPException($"Invalid opening character of JSON object/array: \"{openingChar}\"" +
						"\nPlease provide a start index that corresponds to the opening of an object/array.");
			}

			// Iterate through the string starting at the specified starting index.
			// We count the number of opening tags and closing tags, returning when the root object/array ends.
			for (int i = startingIdx, openTags = 0; ; ++i)
			{
				if (jsonString[i] == openingChar)
				{
					openTags++;
				}
				else if (jsonString[i] == closingChar)
				{
					openTags--;
				}

				// Check if we have reached the end of the object/array
				if (openTags == 0)
				{
					return jsonString.Substring(startingIdx, i - startingIdx + 1);
				}
			}
		}

		/// <summary>
		/// Provided a JSON string and a starting index (which must correspond to an opening string quote or first character of a primitive value), 
		/// this will return the string or primitive value in its entirety up to the corresponding closing character.
		/// </summary>
		/// <param name="jsonString">The JSON string to extract an array or object from.</param>
		/// <param name="startingIdx">The index of the opening character of the array or object.</param>
		/// <returns>The extracted string or primitive value.</returns>
		internal static string ChunkValue(string jsonString, int startingIdx)
		{
			char openingChar = jsonString[startingIdx];

			// If string
			if (IsStringQuote(openingChar))
			{
				for (int i = startingIdx + 1; i < jsonString.Length; ++i)
				{
					if (jsonString[i] == openingChar && (CheckEscapedCharacters && jsonString[i - 1] != '\\'))
					{
						return jsonString.Substring(startingIdx, i - startingIdx + 1);
					}
				}
			}
			// If primitive value
			else
			{
				for (int i = startingIdx + 1; i < jsonString.Length; ++i)
				{
					if (!char.IsLetterOrDigit(jsonString[i]) && jsonString[i] != '.' && jsonString[i] != '+' && jsonString[i] != '-')
					{
						return jsonString.Substring(startingIdx, i - startingIdx);
					}
				}
			}

			throw new LWJPException($"Failed to chunk the provided string - no appropriate closing character found. String provided: '{jsonString}'. Starting char: '{openingChar}'");
		}
		#endregion


		#region ESCAPE SEQUENCE HANDLING
		/// <summary>
		/// Provided a string, this will convert any escape characters made up of two characters into their single character equivalent.
		/// </summary>
		/// <param name="text">String to convert all escape characters.</param>
		/// <returns>Modified string.</returns>
		public static string ConvertEscapeCharacters(string text)
		{
			string sequence;
			for (int i = 0; i < text.Length - 1; i++)
			{
				if (text[i] == '\\')
				{
					sequence = string.Format("{0}{1}", text[i], text[i + 1]);
					text = text.Remove(i, 2);
					text = text.Insert(i, ConvertEscapeCharacterSequence(sequence));
				}
			}

			return text;
		}
		/// <summary>
		/// Provided a two-character sequence, this converts the sequence to a singular escaped character equivalent.
		/// If the escape sequence is not recognized, the provided sequence will be returned unmodified.
		/// </summary>
		/// <param name="sequence">Sequence to convert.</param>
		/// <returns>The converted character.</returns>
		internal static string ConvertEscapeCharacterSequence(string sequence)
		{
			switch (sequence)
			{
				case @"\""":
					return @"""";
				case @"\'":
					return "\'";

				case @"\\":
					return "\\";
				case @"\n":
					return "\n";
				case @"\r":
					return "\r";
				case @"\t":
					return "\t";
				case @"\b":
					return "\b";
				case @"\f":
					return "\f";

				default:
					return sequence;
			}
		}
		#endregion


		#region HELPERS
		/// <summary>
		/// Outputs this object as a formatted JSON string.
		/// </summary>
		/// <returns>Formatted JSON representation of this object.</returns>
		new public abstract string ToString();

		/// <summary>
		/// Checks whether the provided string is a valid JSON string quote character.
		/// </summary>
		/// <param name="character">String element to check.</param>
		/// <returns>True if the provided string element is a string-type quote character.</returns>
		internal static bool IsStringQuote(char character)
		{
			return character == '\'' || character == '\"';
		}

		/// <summary>
		/// Adds enclosing quote characters to the provided string and returns.
		/// The type of quote added is based on the value of <see cref="CurrentStringMode"/>.
		/// </summary>
		/// <param name="val">The string to add quotes to.</param>
		/// <returns>The provided string wrapped in quotes.</returns>
		internal static string WrapInQuotes(string val)
		{
			var quote = CurrentStringMode == StringMode.Mode.SingleQuote ? StringMode.SINGLE_QUOTE : StringMode.DOUBLE_QUOTE;
			return $"{quote}{val}{quote}";
		}

		/// <summary>
		/// Raises the <see cref="OnItemParsed"/> event with the provided text.
		/// </summary>
		/// <param name="text">Text to pass with event.</param>
		protected void RaiseOnItemParsed(string text)
		{
			OnItemParsed(text);
		}
		#endregion
	}
}
