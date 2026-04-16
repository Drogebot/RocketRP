namespace RocketRP.Actors.TAGame
{
	public class SaveData_GameEditor_Training_TA : Core.Object
	{
		public ObjectTarget<TrainingEditorData_TA> TrainingData { get; set; }
		public int PlayerTeamNumber { get; set; }
		public bool bUnowned { get; set; }
		public bool bPerfectCompleted { get; set; }
		public int ShotsCompleted { get; set; }
	}
}
