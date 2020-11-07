using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialConstantDef
    {
        public uint NameHash;
        public string Name;
        public float[] Literal = new float[4];

        public T6MaterialConstantDef(BinaryReader br)
        {
            this.NameHash = br.ReadUInt32();
            this.Name = new string(br.ReadChars(12));
            for (int i = 0; i < this.Literal.Length; i++)
            {
                this.Literal[i] = br.ReadSingle();
            }
        }
    }
}
