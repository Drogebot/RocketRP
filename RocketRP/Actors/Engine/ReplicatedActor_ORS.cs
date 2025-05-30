using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.Engine
{
    public class ReplicatedActor_ORS : Actor
    {
        public ObjectTarget<Actor> ReplicatedOwner { get; set; }
    }
}
