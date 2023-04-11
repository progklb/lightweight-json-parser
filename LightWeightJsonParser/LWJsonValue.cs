using System;
using System.Globalization;

namespace LightWeightJsonParser
{
	/// <summary>
	/// Represents a simple value as part of a key-value pair.
	/// <para>
	/// { key : value }
	/// </para>
	/// <para>
	/// (e.g. { "name" : "Kevin" })
	/// </para>
	/// </summary>
	public sealed class LWJsonValue : LWJson
	{
		#region PROPERTIES
		/// <summary>
		/// The value that this object represents in string form.
		/// </summary>
		public string Value { get; set; }
		#endregion


		#region CONSTRUCTORS
		public LWJsonValue() { }

		public LWJsonValue(string value)
		{
			Value = value;
			DataType = JsonDataType.String;
		}

		public LWJsonValue(bool value)
		{
			Value = value.ToString().ToLowerInvariant();
			DataType = JsonDataType.Boolean;
		}

		public LWJsonValue(int value)
		{
			Value = value.ToString();
			DataType = JsonDataType.Integer;
		}

		public LWJsonValue(double value)
		{
			Value = value.ToString().Replace(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator, ".");
			DataType = JsonDataType.Double;
		}
		#endregion


		#region GETTERS
		public override string AsString() => Value;

		public override bool AsBoolean() => bool.Parse(Value);

		public override int AsInteger() => int.Parse(Value);

		public override double AsDouble() => double.Parse(Value.Replace(".", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator));
		#endregion


		#region STRING HANDLING
		/// <summary>
		/// Parses the provided string and determines the type, setting the <see cref="Type"/> value
		/// of this object and saving the provided string at the value this object represents.
		/// </summary>
		/// <param name="value">String value to parse.</param>
		/// <returns>The success of parsing.</returns>
		new internal bool Parse(string value)
		{
			bool success = false;

			// Determine value type.
			if (IsStringQuote(value[0]) && IsStringQuote(value[value.Length - 1]))
			{
				// Strip quotes
				value = value.Substring(1, value.Length - 2);

				success = true;
				DataType = JsonDataType.String;
			}
			else if (bool.TryParse(value, out bool b))
			{
				success = true;
				DataType = JsonDataType.Boolean;
			}
			else if (int.TryParse(value, out int i))
			{
				success = true;
				DataType = JsonDataType.Integer;
			}
			// Attempt to parse as a double. Convert the digit separator to a culture-specific value.
			else
			{
				if (double.TryParse(value.Replace(".", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator), out double d))
				{
					success = true;
					DataType = JsonDataType.Double;
				}
			}

			// Assign value or handle failure.
			if (success)
			{
				Value = value;
			}
			else
			{
				switch (CurrentFailureMode)
				{
					case FailureMode.Silent:
						Value = value;
						break;
					case FailureMode.Verbose:
						Value = $"value_parse_failure({value})";
						break;
					case FailureMode.Exception:
					default:
						throw new LWJPException($"Failed to parse value: {value}");
				}
			}

			return success;
		}

		public override string ToString()
		{
			if (DataType == JsonDataType.String)
			{
				return WrapInQuotes(Value);
			}
			else
			{
				return Value;
			}
		}
		#endregion
	}
}
