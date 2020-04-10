using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JediComlink
{
    public class Block8F : BlockLong
    {
        public override int Id { get => 0x8F; }
        public override string Description { get => "Status List"; }

        #region Propeties
        #endregion

        #region Definition
        /*  0  1  2  3   4  5  6  7    8  9  A  B   C  D  E  F
        0: 06 08 53 54  53 20 31 20   53 54 53 20  32 20 53 54
        1: 53 20 33 20  53 54 53 20   34 20 53 54  53 20 35 20
        2: 53 54 53 20  36 20 53 54   53 20 37 20  53 54 53 20
        3: 38 20 00
        */

        #endregion

        public Block8F(Block parent, int vector, byte[] codeplugContents) : base(parent, vector, codeplugContents)
        {
 
        }

        public override string ToString()
        {
            var s = new String(' ', Level * 2);
            var sb = new StringBuilder();
            sb.AppendLine(GetTextHeader());
            sb.AppendLine(s + GetStringContents(2, Contents.Length - 2));
            return sb.ToString();
        }
    }
}
