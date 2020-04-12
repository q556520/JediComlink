using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JediComlink
{
    public class Block55 : BlockLong
    {
        private byte[] _contents;
        public Span<byte> Contents { get => _contents; set => _contents = value.ToArray(); }

        public override int Id { get => 0x55; }
        public override string Description { get => "Personality Vector"; }

        #region Propeties
        public Block56 Block56 { get; set; }

        #endregion

        #region Definition
        /*  0  1  2  3   4  5  6  7    8  9  A  B   C  D  E  F
        0: 11 05 64 05  8A 05 B0 05   D6 05 FC 06  22 06 48 06
        1: 6E 06 94 06  BA 06 E0 07   06 07 2C 07  52 07 78 07
        2: 9E 07 C4
        */

        private const int BLOCK_56_VECTOR = 0x01;
        #endregion

        public Block55() { }

        public override void Deserialize(byte[] codeplugContents, int address)
        {
            Contents = Deserializer(codeplugContents, address);
            Block56 = Deserialize<Block56>(Contents, BLOCK_56_VECTOR, codeplugContents);
        }

        public override int Serialize(byte[] codeplugContents, int address)
        {
            var contents = Contents.ToArray().AsSpan(); //TODO
            var nextAddress = address + Contents.Length + BlockSizeAdjustment;
            nextAddress = SerializeChild(Block56, BLOCK_56_VECTOR, codeplugContents, nextAddress, contents);
            Serializer(codeplugContents, address, contents);
            return nextAddress;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(GetTextHeader());
            sb.AppendLine(Block56.ToString());

            return sb.ToString();
        }
    }
}
