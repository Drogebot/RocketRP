using RocketRP.Actors.TAGame.SaveData;

namespace RocketRP.Actors.TAGame
{
	public class SaveData_GameEditor_Training_TA : SaveData_GameEditor_TA
	{
		public ObjectTarget<TrainingEditorData_TA> TrainingData { get; set; }
		public int PlayerTeamNumber { get; set; }
		public bool bUnowned { get; set; }
		public bool bPerfectCompleted { get; set; }
		public int ShotsCompleted { get; set; }
	}
}
