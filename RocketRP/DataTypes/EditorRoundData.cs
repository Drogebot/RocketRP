using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
    public struct EditorRoundData
    {
        public float? TimeLimit { get; set; }
        public string[]? SerializedArchetypes { get; set; }
    }
}
