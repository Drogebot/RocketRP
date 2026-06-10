using RocketRP.Actors.Core;
using RocketRP.DataTypes.Enums;

namespace RocketRP.DataTypes
{
	public struct DemoSpawnSelectionData
	{
		public EDemoSpawnPreference SpawnPreference { get; set; }
		public EDemoSelectionState SelectionState { get; set; }
		public int PercentageTimeLeft { get; set; }
		public ObjectTarget<ClassObject> SpawnActor { get; set; }
		public int SpawnIndex { get; set; }
		public Vector SpawnLocation { get; set; }
		public Rotator SpawnRotation { get; set; }
		
		public DemoSpawnSelectionData(EDemoSpawnPreference spawnPreference, EDemoSelectionState selectionState, int percentageTimeLeft, ObjectTarget<ClassObject> spawnActor, int spawnIndex, Vector spawnLocation, Rotator spawnRotation)
		{
			SpawnPreference = spawnPreference;
			SelectionState = selectionState;
			PercentageTimeLeft = percentageTimeLeft;
			SpawnActor = spawnActor;
			SpawnIndex = spawnIndex;
			SpawnLocation = spawnLocation;
			SpawnRotation = spawnRotation;
		}
		
		public static DemoSpawnSelectionData Deserialize(BitReader br, Replay replay)
		{
			var spawnPreference = (EDemoSpawnPreference)br.ReadByte();
			var selectionState = (EDemoSelectionState)br.ReadByte();
			var percentageTimeLeft = br.ReadInt32();
			var spawnActor = ObjectTarget<ClassObject>.Deserialize(br, replay);
			var spawnIndex = br.ReadInt32();
			var spawnLocation = Vector.Deserialize(br, replay);
			var spawnRotation = Rotator.Deserialize(br, replay);
			return new DemoSpawnSelectionData(spawnPreference, selectionState, percentageTimeLeft, spawnActor, spawnIndex, spawnLocation, spawnRotation);
		}
		
		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write((byte)SpawnPreference);
			bw.Write((byte)SelectionState);
			bw.Write(PercentageTimeLeft);
			SpawnActor.Serialize(bw, replay);
			bw.Write(SpawnIndex);
			SpawnLocation.Serialize(bw, replay);
			SpawnRotation.Serialize(bw, replay);
		}
	}
}
