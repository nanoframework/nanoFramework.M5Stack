using System;

namespace nanoFramework.M5AtomLite
{
	/// <summary>
	/// RGB color class
	/// </summary>
	public class Color
	{
		/// <summary>
		/// RGB color
		/// </summary>
		/// <param name="r">red</param>
		/// <param name="g">green</param>
		/// <param name="b">blue</param>
		public Color(byte r, byte g, byte b)
		{
			R = r;
			G = g;
			B = b;
		}

		/// <summary>
		/// Blue channel
		/// </summary>
		public byte B { get; set; }

		/// <summary>
		/// Green channel
		/// </summary>
		public byte G { get; set; }

		/// <summary>
		/// Red channel
		/// </summary>
		public byte R { get; set; }
	}
}
