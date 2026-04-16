using RocketRP.DataTypes.Enums;

namespace RocketRP.DataTypes
{
	public struct ReplayPlayerStats
	{
		public UniqueNetId PlayerID { get; set; }
		public string? Name { get; set; }
		public OnlinePlatform Platform { get; set; }
		public ulong OnlineID { get; set; }
		public int Team { get; set; }
		public int Score { get; set; }
		public int Goals { get; set; }
		public int Assists { get; set; }
		public int Saves { get; set; }
		public int Shots { get; set; }
		public bool bBot { get; set; }
	}
}
