﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct BreakoutDamageState
	{
		public byte State { get; set; }
		public ObjectTarget Causer { get; set; }
		public Vector DamageLocation { get; set; }
		public bool bDirectDamage { get; set; }
		public bool bImmediate { get; set; }

		public BreakoutDamageState(byte state, ObjectTarget causer, Vector damageLocation, bool directDamage, bool immediate)
		{
			State = state;
			Causer = causer;
			DamageLocation = damageLocation;
			bDirectDamage = directDamage;
			bImmediate = immediate;
		}

		public static BreakoutDamageState Deserialize(BitReader br, Replay replay)
		{
			var state = br.ReadByte();
			var causer = ObjectTarget.Deserialize(br);
			var damageLocation = Vector.Deserialize(br, replay);
			var directDamage = br.ReadBit();
			var immediate = br.ReadBit();

			return new BreakoutDamageState(state, causer, damageLocation, directDamage, immediate);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(State);
			Causer.Serialize(bw);
			DamageLocation.Serialize(bw, replay);
			bw.Write(bDirectDamage);
			bw.Write(bImmediate);
		}
	}
}
