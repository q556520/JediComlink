﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JediComlink
{
    public class Block01 : Block
    {
        #region Propeties
        public string Serial { get; set; }
        public string Model { get; set; }
        public Block30 Block30 { get; set; }
        public Block02 Block02 { get; set; }
        public Block56 Block56 { get; set; }
        public Block10 Block10 { get; set; }
        public byte[] AuthCode { get; private set; }


        #endregion

        #region Definition
        /*  0  1  2  3   4  5  6  7    8  9  A  B   C  D  E  F
        0: 02 00 34 36  36 41 57 41   32 38 36 37  48 30 31 55
        1: 43 44 36 50  57 31 42 4E   00 00 00 00  00 0F 02 00
        2: 00 01 00 00  00 3D 01 99   00 00 00 00  01 DC 01 00
        3: 03 22 BB C8  89 54 02 69   30 B0 
        */

        private const int BLOCK_30_VECTOR = 0x00;
        private const int SERIAL = 0x02;
        private const int SERIAL_LEN = 10;
        private const int MODEL = 0x0C;
        private const int MODEL_LEN = 16;

        private const int BLOCK_02_VECTOR = 0x24;
        private const int BLOCK_56_VECTOR = 0x26;
        private const int BLOCK_10_VECTOR = 0x2C;

        private const int AUTH_CODE = 0x30;
        #endregion

        public Block01(Codeplug codeplug)
        {
            Codeplug = codeplug;
            Level = 1;

            StartAddress = 0x0000;
            Length = codeplug.Contents[StartAddress];
            EndAddress = StartAddress + Length;

            Id = 0x01;
            Description = "Internal Radio";            

            Serial = GetStringContents(SERIAL, SERIAL_LEN);
            Model = GetStringContents(MODEL, MODEL_LEN);
            AuthCode = Contents.Slice(AUTH_CODE, 10).ToArray();

            Block30 = new Block30(this, BLOCK_30_VECTOR);
            Block02 = new Block02(this, BLOCK_02_VECTOR);
            Block56 = new Block56(this, BLOCK_56_VECTOR);
            Block10 = new Block10(this, BLOCK_10_VECTOR);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(GetTextHeader());
            sb.AppendLine($"Serial: {Serial}");
            sb.AppendLine($"Model: {Model}");
            sb.AppendLine($"Auth Code: {FormatHex(AuthCode)}");
            sb.AppendLine(Block30.ToString());
            sb.AppendLine(Block02.ToString());
            sb.AppendLine(Block56.ToString());
            sb.AppendLine(Block10.ToString());
            return sb.ToString();
        }
    }
}
