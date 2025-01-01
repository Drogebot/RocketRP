using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class BinaryReader : System.IO.BinaryReader
	{
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="input"><inheritdoc/></param>
		public BinaryReader(Stream input) : base(input) { }

		/// <summary>
		/// Reads a string from the current stream. The string is prefixed with the length, encoded as a 32-bit integer. The string is postfixed with a null character.
		/// </summary>
		/// <returns>The string being read.</returns>
		public override string ReadString()
		{
			// TODO: This is the same implementation as BitReader's ReadString()
			var length = ReadInt32();
			if (length > 0)
			{
				var bytes = ReadBytes(length);
				return CodePagesEncodingProvider.Instance.GetEncoding(1252).GetString(bytes, 0, length - 1);
			}
			else if (length < 0)
			{
				var bytes = ReadBytes(length * -2);
				return Encoding.Unicode.GetString(bytes, 0, (length * -2) - 2);
			}
			return String.Empty;
		}
	}

	public class BinaryWriter : System.IO.BinaryWriter
	{
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="input"><inheritdoc/></param>
		public BinaryWriter(Stream input) : base(input) { }

		/// <summary>
		/// Writes a length-prefixed, null-postfixed string to this stream, and advances the current position of the stream in accordance with the encoding used and the specific characters being written to the stream.
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public override void Write(string value)
		{
			// TODO: This is the same implementation as BitWriter's Write(string)
			if (value == String.Empty)
			{
				Write((int)0);
				return;
			}

			var length = value.Length + 1;
			bool isUnicode = value.Any(c => c > 255);
			if (isUnicode)
			{
				length *= -1;
			}
			Write(length);

			if (!isUnicode)
			{
				Write(CodePagesEncodingProvider.Instance.GetEncoding(1252).GetBytes(value));
				Write((byte)0);
			}
			else
			{
				Write(Encoding.Unicode.GetBytes(value));
				Write((byte)0);
				Write((byte)0);
			}
		}
	}
}
