namespace RocketRP.DataTypes
{
	public struct AppliedBreakoutDamage
	{
		public byte Id { get; set; }
		public Vector Location { get; set; }
		public int DamageIndex { get; set; }
		public int TotalDamage { get; set; }

		public AppliedBreakoutDamage(byte id, Vector location, int damageIndex, int totalDamage)
		{
			Id = id;
			Location = location;
			DamageIndex = damageIndex;
			TotalDamage = totalDamage;
		}

		public static AppliedBreakoutDamage Deserialize(BitReader br, Replay replay)
		{
			var id = br.ReadByte();
			var location = Vector.Deserialize(br, replay);
			var damageIndex = br.ReadInt32();
			var totalDamage = br.ReadInt32();

			return new AppliedBreakoutDamage(id, location, damageIndex, totalDamage);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(Id);
			Location.Serialize(bw, replay);
			bw.Write(DamageIndex);
			bw.Write(TotalDamage);
		}
	}
}
