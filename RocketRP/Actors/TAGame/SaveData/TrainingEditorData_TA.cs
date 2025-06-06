using RocketRP.DataTypes;
using RocketRP.DataTypes.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
    public class TrainingEditorData_TA : Core.Object
    {
        public Guid? TM_Guid { get; set; }
        public string? Code { get; set; }
        public string? TM_Name { get; set; }
        public ETrainingType? Type { get; set; }
        public EDifficulty? Difficulty { get; set; }
        public string? CreatorName { get; set; }
		public string? Description { get; set; }
        public int[]? Tags { get; set; }
        public Name? MapName { get; set; }
        public ulong? CreatedAt { get; set; }
        public ulong? UpdatedAt { get; set; }
        public UniqueNetId? CreatorPlayerID { get; set; }
        public EditorRoundData[]? Rounds { get; set; }
    }
}
