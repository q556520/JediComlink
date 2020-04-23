﻿using System;
using System.ComponentModel;
using System.Text;

namespace JediCodeplug
{
    public class Block30 : Block
    {
        private byte[] _contents;
        public Span<byte> Contents { get => _contents; set => _contents = value.ToArray(); }

        public override int Id { get => 0x30; }
        public override string Description { get => "External Radio"; }

        #region Definition
        /*  0  1  2  3   4  5  6  7    8  9  A  B   C  D  E  F
        0: 3E 00 34 33  32 43 44 4E   30 30 30 32  48 30 31 4B
        1: 44 44 39 50  57 31 42 4E   00 00 00 00  11 02 24 14
        2: 42 03 00 1C  07 71 02 D1   03 0A 04 B7  05 1F 06 65
        3: 06 74 06 E4  07 16 08 36   08 3E 08 82  09 82 09 9B
        4: 00 00 00 00  00 00 00 00   00 00 09 AB  00 00
        */

        private const int UNKNOWN1 = 0x00; //01
        private const int SERIAL = 0x02; //03 04 05 06 07 08 09 0A 0B
        private const int MODEL = 0x0C; //0D 0E 0F 10 11 12 13 14 15 16 17 18 19 1A 1B
        private const int TIMESTAMP = 0x1c; //1D 1E 1F 20
        private const int UNKNOWN2 = 0x21; //22 23
        private const int EXTERNAL_CODEPLUG_SIZE = 0x24; //25
        private const int BLOCK_31_VECTOR = 0x26; //27
        private const int BLOCK_3D_VECTOR = 0x28; //29
        private const int BLOCK_36_VECTOR = 0x2A; //2B
        private const int BLOCK_55_VECTOR = 0x2C; //2D
        private const int BLOCK_54_VECTOR = 0x2E; //2F
        private const int BLOCK_51_VECTOR = 0x30; //31
        private const int UNKNOWN3 = 0x32; //33
        private const int BLOCK_39_VECTOR = 0x34; //35
        private const int BLOCK_3B_VECTOR = 0x36; //37
        private const int BLOCK_34_VECTOR = 0x38; //39
        private const int BLOCK_35_VECTOR = 0x3A; //3B
        private const int BLOCK_3C_VECTOR = 0x3C; //3D
        private const int BLOCK_73_VECTOR = 0x3E; //3F
        private const int UNKNOWN4 = 0x40; //41 42 43 44 45 46 47 48 49 4A 4B
        private const int UNKNOWN5 = 0x4C; //4D
        #endregion

        #region Propeties
        public byte[] Unknown1
        {
            get => Contents.Slice(UNKNOWN1, 2).ToArray();
            //set => XYZ = value; //TODO
        }

        public string Serial
        {
            get => GetStringContents(Contents, SERIAL, 10);
            //set => XYZ = value; //TODO
        }

        public string Model
        {
            get => GetStringContents(Contents, MODEL, 16);
            //set => XYZ = value; //TODO
        }

        public DateTime TimeStamp
        {
            get => new DateTime(2000 + GetDigits(Contents[TIMESTAMP]),
                                    GetDigits(Contents[TIMESTAMP + 1]),
                                    GetDigits(Contents[TIMESTAMP + 2]),
                                    GetDigits(Contents[TIMESTAMP + 3]),
                                    GetDigits(Contents[TIMESTAMP + 4]),
                                    0);
            //set => XYZ = value; //TODO
        }

        [TypeConverter(typeof(HexByteArrayTypeConverter))]
        public byte[] Unknown2
        {
            get => Contents.Slice(UNKNOWN2, 3).ToArray();
            //set => XYZ = value; //TODO
        }
        
        [TypeConverter(typeof(HexIntValueTypeConverter))]
        public int ExternalCodeplugSize
        {
            get => Contents[EXTERNAL_CODEPLUG_SIZE] * 0x100 + Contents[EXTERNAL_CODEPLUG_SIZE + 1];
            set
            {
                if (value < 0 || value > 0xFFFF) throw new ArgumentException("Out of range 0x0000 to 0xFFFF");
                Contents[EXTERNAL_CODEPLUG_SIZE] = (byte)(value / 0x100);
                Contents[EXTERNAL_CODEPLUG_SIZE+1] = (byte)(value % 0x100);
            }
        }

        public Block31 Block31 { get; set; }
        public Block3D Block3D { get; set; }
        public Block36 Block36 { get; set; }
        public Block55 Block55 { get; set; }
        public Block54 Block54 { get; set; }
        public Block51 Block51 { get; set; }

        [TypeConverter(typeof(HexByteArrayTypeConverter))]
        public byte[] Unknown3
        {
            get => Contents.Slice(UNKNOWN3, 3).ToArray();
            //set => XYZ = value; //TODO
        }

        public Block39 Block39 { get; set; }
        public Block3B Block3B { get; set; }
        public Block34 Block34 { get; set; }
        public Block35 Block35 { get; set; }
        public Block3C Block3C { get; set; }
        public Block73 Block73 { get; set; }

        [TypeConverter(typeof(HexByteArrayTypeConverter))]
        public byte[] Unknown4
        {
            get => Contents.Slice(UNKNOWN4, 12).ToArray();
            //set => XYZ = value; //TODO
        }

        [TypeConverter(typeof(HexByteArrayTypeConverter))]
        public byte[] Unknown5
        {
            get => Contents.Slice(UNKNOWN5, 2).ToArray();
            //set => XYZ = value; //TODO
        }
        #endregion

        public Block30() { }

        public override void Deserialize(byte[] codeplugContents, int address)
        {
            Contents = Deserializer(codeplugContents, address);
            Block31 = Deserialize<Block31>(Contents, BLOCK_31_VECTOR, codeplugContents);
            Block3D = Deserialize<Block3D>(Contents, BLOCK_3D_VECTOR, codeplugContents);
            Block36 = Deserialize<Block36>(Contents, BLOCK_36_VECTOR, codeplugContents);
            Block55 = Deserialize<Block55>(Contents, BLOCK_55_VECTOR, codeplugContents);
            Block54 = Deserialize<Block54>(Contents, BLOCK_54_VECTOR, codeplugContents);
            Block51 = Deserialize<Block51>(Contents, BLOCK_51_VECTOR, codeplugContents);
            Block39 = Deserialize<Block39>(Contents, BLOCK_39_VECTOR, codeplugContents);
            Block3B = Deserialize<Block3B>(Contents, BLOCK_3B_VECTOR, codeplugContents);
            Block34 = Deserialize<Block34>(Contents, BLOCK_34_VECTOR, codeplugContents);
            Block35 = Deserialize<Block35>(Contents, BLOCK_35_VECTOR, codeplugContents);
            Block3C = Deserialize<Block3C>(Contents, BLOCK_3C_VECTOR, codeplugContents);
            Block73 = Deserialize<Block73>(Contents, BLOCK_73_VECTOR, codeplugContents);
        }

        public override int Serialize(byte[] codeplugContents, int address)
        {
            var contents = Contents.ToArray().AsSpan(); //TODO
            var nextAddress = address + Contents.Length + BlockSizeAdjustment;
            nextAddress = SerializeChild(Block31, BLOCK_31_VECTOR, codeplugContents, nextAddress, contents);
            nextAddress = SerializeChild(Block3D, BLOCK_3D_VECTOR, codeplugContents, nextAddress, contents);
            nextAddress = SerializeChild(Block36, BLOCK_36_VECTOR, codeplugContents, nextAddress, contents);
            nextAddress = SerializeChild(Block55, BLOCK_55_VECTOR, codeplugContents, nextAddress, contents);
            nextAddress = SerializeChild(Block54, BLOCK_54_VECTOR, codeplugContents, nextAddress, contents);
            nextAddress = SerializeChild(Block51, BLOCK_51_VECTOR, codeplugContents, nextAddress, contents);
            nextAddress = SerializeChild(Block39, BLOCK_39_VECTOR, codeplugContents, nextAddress, contents);
            nextAddress = SerializeChild(Block3B, BLOCK_3B_VECTOR, codeplugContents, nextAddress, contents);
            nextAddress = SerializeChild(Block34, BLOCK_34_VECTOR, codeplugContents, nextAddress, contents);
            nextAddress = SerializeChild(Block35, BLOCK_35_VECTOR, codeplugContents, nextAddress, contents);
            nextAddress = SerializeChild(Block3C, BLOCK_3C_VECTOR, codeplugContents, nextAddress, contents);
            nextAddress = SerializeChild(Block73, BLOCK_73_VECTOR, codeplugContents, nextAddress, contents);
            Serializer(codeplugContents, address, contents);
            return nextAddress;
        }
    }
}
