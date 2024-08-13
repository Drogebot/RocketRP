using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes.Enums
{
	public enum OnlinePlatform : byte
	{
		OnlinePlatform_Unknown,
		OnlinePlatform_Steam,
		OnlinePlatform_PS4,
		OnlinePlatform_PS3,
		OnlinePlatform_Dingo,	//XboxOne
		OnlinePlatform_QQ,		//DEPRICATED
		OnlinePlatform_OldNNX,
		OnlinePlatform_NNX,		//Switch
		OnlinePlatform_PsyNet,
		OnlinePlatform_Deleted,
		OnlinePlatform_WeGame,  //DEPRICATED
		OnlinePlatform_Epic,
		OnlinePlatform_END,
	}
}
