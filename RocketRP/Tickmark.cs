﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public struct Tickmark
	{
		public string Type { get; set; }
		public uint Frame { get; set; }

		public static Tickmark Deserialize(BinaryReader br)
		{
			var tickmark = new Tickmark
			{
				Type = br.ReadString()!,
				Frame = br.ReadUInt32()
			};

			return tickmark;
		}

		public void Serialize(BinaryWriter bw)
		{
			bw.Write(Type);
			bw.Write(Frame);
		}
	}
}
