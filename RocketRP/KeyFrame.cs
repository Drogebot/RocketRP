﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public struct KeyFrame
	{
		public float Time { get; set; }
		public uint Frame { get; set; }
		[JsonIgnore]
		public uint FilePosition { get; set; }

		public static KeyFrame Deserialize(BinaryReader br)
		{
			var keyFrame = new KeyFrame
			{
				Time = br.ReadSingle(),
				Frame = br.ReadUInt32(),
				FilePosition = br.ReadUInt32()
			};

			return keyFrame;
		}

		public void Serialize(BinaryWriter bw)
		{
			bw.Write(Time);
			bw.Write(Frame);
			bw.Write(FilePosition);
		}
	}
}
