using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
    public class StructProperty
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public object GetValue()
        {
            return ToString();
        }

        public static StructProperty Deserialize(BinaryReader br, int valueLength)
        {
            var prop = new StructProperty();
            prop.Type = br.ReadString();
            prop.Value = Convert.ToHexString(br.ReadBytes(valueLength));

            return prop;
        }

        public void Serialize(BinaryWriter bw)
        {
            bw.Write(Type);
            bw.Write(Convert.FromHexString(Value));
        }

        public override string ToString()
        {
            return $"{Type}:{Value}";
        }
    }
}