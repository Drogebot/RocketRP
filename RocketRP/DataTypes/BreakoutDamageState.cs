using RocketRP.Actors.TAGame;

namespace RocketRP.DataTypes
{
	public struct BreakoutDamageState
	{
		public byte State { get; set; }
		public ObjectTarget<PRI_TA> Causer { get; set; }
		public Vector DamageLocation { get; set; }
		public bool bDirectDamage { get; set; }
		public bool bImmediate { get; set; }

		public BreakoutDamageState(byte state, ObjectTarget<PRI_TA> causer, Vector damageLocation, bool directDamage, bool immediate)
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
			var causer = ObjectTarget<PRI_TA>.Deserialize(br, replay);
			var damageLocation = Vector.Deserialize(br, replay);
			var directDamage = br.ReadBit();
			var immediate = br.ReadBit();

			return new BreakoutDamageState(state, causer, damageLocation, directDamage, immediate);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(State);
			Causer.Serialize(bw, replay);
			DamageLocation.Serialize(bw, replay);
			bw.Write(bDirectDamage);
			bw.Write(bImmediate);
		}
	}
}
