﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public struct DebugString
	{
		public uint FrameNumber { get; set; }
		public string? Username { get; set; }
		public string? Text { get; set; }

		public static DebugString Deserialize(BinaryReader br)
		{
			var debugString = new DebugString
			{
				FrameNumber = br.ReadUInt32(),
				Username = br.ReadString(),
				Text = br.ReadString()
			};

			return debugString;
		}

		public void Serialize(BinaryWriter bw)
		{
			bw.Write(FrameNumber);
			bw.Write(Username);
			bw.Write(Text);
		}
	}
}
