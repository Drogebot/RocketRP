﻿using RocketRP.Actors.Engine;
using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class BreakOutActor_Platform_TA : Actor
	{
		public override bool HasInitialPosition => false;

		public BreakoutDamageState DamageState { get; set; }
	}
}
