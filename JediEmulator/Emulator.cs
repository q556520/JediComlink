﻿using JediCommunication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediEmulator
{
    public class Emulator
    {
		private SerialPort Port;

		void Main()
		{
			try
			{
				Port = new SerialPort("COM4", 9600);
				Port.ReceivedBytesThreshold = 1024;
				Port.Open();
				ReadEvent();
				Console.ReadLine();
			}
			finally
			{
				Port.Close();
			}
		}

		private void ProcessSB9600(SB9600Message message)
		{
			if (message.DeviceType == 0x1A)
			{
				if (message.Data1 == 0x012 && message.Data2 == 0x1A && message.OpCode == 0x06)
				{
					Console.WriteLine("SmartRib Mode");
					echo = false;
					sbepMode = true;
					sribMode = true;
				}
			}
		}

		private void ProcessSbep(SbepMessage message)
		{
			Console.WriteLine("Unknown SBEP Message: " + String.Join(" ", Array.ConvertAll(message.Bytes, _ => _.ToString("X2"))));
		}

		private void ProcessSrib(SbepMessage message)
		{
			SbepMessage response = null;
			switch (message.Data[0])
			{
				case 0x04:
					if (message.Data[1] == 0x02)
					{
						Console.WriteLine("SmartRib Mode - 1st Command");
						response = new SbepMessage(0x00, 0x87, 0x00);
					}
					break;
				case 0x10:
					Console.WriteLine("SmartRib Mode - 2nd Command");
					response = new SbepMessage(0x00, 0x90, 0x56, 0x44, 0x17, 0x11, 0x10, 0xD6);
					break;
				case 0x11:
					Console.WriteLine("SmartRib Mode - Exit SmartRib Mode");
					sribMode = false;
					sbepMode = false;
					response = new SbepMessage(0x00, 0x87, 0x00);
					break;
				case 0x12:
					//12 E6 4D DE CF 42 D4
					Console.WriteLine("SmartRib Mode - 3rd Command");
					response = new SbepMessage(0x00, 0x87, 0x00);
					break;
				case 0x1A:
					//12 E6 4D DE CF 42 D4
					Console.WriteLine("SmartRib Mode - 4th Command");
					response = new SbepMessage(0x00, 0x9A, 0x00, 0x00, 0x01, 0x20, 0x52, 0x30, 0x31, 0x2E, 0x30, 0x30, 0x09, 0x20, 0x52, 0x30, 0x34, 0x2E, 0x30, 0x30, 0x03, 0x20, 0x52, 0x30, 0x36, 0x2E, 0x34, 0x30, 0x03, 0x20, 0x20, 0x52, 0x30, 0x38, 0x37, 0x31, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x89);
					Console.WriteLine("bytes: " + String.Join(" ", Array.ConvertAll(response.Bytes, _ => _.ToString("X2"))));
					break;
				case 0x20:
					//20 01 8F 95 7F 91 7E D6 4C 2C B8 54 20 AA AF EF 8E 3B E0 7D 6E A9 24 B5 CC E7 13 30 19 0D A0 F8 33 89 69 D2 FB 29 53 88 02 F5 CA FF 86 D4 C4 35 FC 04 46 A8 17 40 63 E6 D3 05 8B 70 DA 7B 18 97 B2 A5 38 73 B0 9D CB E3 E8 14 D8 FD 12 68 16 08 6B CE 3A 64 81 F7 2D BF 1B 44 57 66 A2 A7 2B D9 F4 41 71 82 51 6F B1 59 F0 3C 01 56 8D BB CF C0 AB 4D 0E 85 26 DB 1F C1 B3 B4 07 96 72 37 C8 4B EA 34 3F 45 5B 1E 00 4A C6 8A 4E 5E B6 2F 74 1D CD 03 D7 43 39 E4 C9 DF AC 47 23 7C C7 67 AE 3E 21 49 62 6D BE 42 E1 A6 93 15 9E 52 5D F9 48 2A C2 0F A3 A1 80 EE D0 B7 10 DD BD AD DE 5F 65 9C F3 E5 A4 27 6A 79 61 78 90 ED FA 0A 84 36 25 75 9B 09 5C BA EC 6C 7A 77 2E 8C 06 D5 DC 0B D1 11 FE 60 83 28 F1 9F 98 5A 4F F6 99 9A E2 32 E9 BC 58 76 87 C5 F2 92 3D 1C 50 0C 22 94 1A EB 31 C3 B9 55 4B 
					Console.WriteLine("SmartRib Mode - Write 20 Cmd");
					response = new SbepMessage(0x00, 0x87, 0x00);
					break;
				case 0x22:
					//20 01 8F 95 7F 91 7E D6 4C 2C B8 54 20 AA AF EF 8E 3B E0 7D 6E A9 24 B5 CC E7 13 30 19 0D A0 F8 33 89 69 D2 FB 29 53 88 02 F5 CA FF 86 D4 C4 35 FC 04 46 A8 17 40 63 E6 D3 05 8B 70 DA 7B 18 97 B2 A5 38 73 B0 9D CB E3 E8 14 D8 FD 12 68 16 08 6B CE 3A 64 81 F7 2D BF 1B 44 57 66 A2 A7 2B D9 F4 41 71 82 51 6F B1 59 F0 3C 01 56 8D BB CF C0 AB 4D 0E 85 26 DB 1F C1 B3 B4 07 96 72 37 C8 4B EA 34 3F 45 5B 1E 00 4A C6 8A 4E 5E B6 2F 74 1D CD 03 D7 43 39 E4 C9 DF AC 47 23 7C C7 67 AE 3E 21 49 62 6D BE 42 E1 A6 93 15 9E 52 5D F9 48 2A C2 0F A3 A1 80 EE D0 B7 10 DD BD AD DE 5F 65 9C F3 E5 A4 27 6A 79 61 78 90 ED FA 0A 84 36 25 75 9B 09 5C BA EC 6C 7A 77 2E 8C 06 D5 DC 0B D1 11 FE 60 83 28 F1 9F 98 5A 4F F6 99 9A E2 32 E9 BC 58 76 87 C5 F2 92 3D 1C 50 0C 22 94 1A EB 31 C3 B9 55 4B 
					Console.WriteLine($"SmartRib Mode - Write 22 Cmd -- {message.Data[1]:X2} {message.Data[2]:X2} {message.Data[3]:X2}");
					response = new SbepMessage(0x00, 0x87, 0x00);
					break;
				case 0x24:
					//0A 24 09 20 52 30 34 2E 30 30 64
					Console.WriteLine("SmartRib Mode - Write 24 Cmd");
					response = new SbepMessage(0x00, 0x87, 0x00);
					break;
				case 0x40:
					//0A 24 09 20 52 30 34 2E 30 30 64
					Console.WriteLine("SmartRib Mode - 40 Cmd");
					response = new SbepMessage(0x00, 0x87, 0x00);
					break;
				case 0x50:
					//20 01 8F 95 7F 91 7E D6 4C 2C B8 54 20 AA AF EF 8E 3B E0 7D 6E A9 24 B5 CC E7 13 30 19 0D A0 F8 33 89 69 D2 FB 29 53 88 02 F5 CA FF 86 D4 C4 35 FC 04 46 A8 17 40 63 E6 D3 05 8B 70 DA 7B 18 97 B2 A5 38 73 B0 9D CB E3 E8 14 D8 FD 12 68 16 08 6B CE 3A 64 81 F7 2D BF 1B 44 57 66 A2 A7 2B D9 F4 41 71 82 51 6F B1 59 F0 3C 01 56 8D BB CF C0 AB 4D 0E 85 26 DB 1F C1 B3 B4 07 96 72 37 C8 4B EA 34 3F 45 5B 1E 00 4A C6 8A 4E 5E B6 2F 74 1D CD 03 D7 43 39 E4 C9 DF AC 47 23 7C C7 67 AE 3E 21 49 62 6D BE 42 E1 A6 93 15 9E 52 5D F9 48 2A C2 0F A3 A1 80 EE D0 B7 10 DD BD AD DE 5F 65 9C F3 E5 A4 27 6A 79 61 78 90 ED FA 0A 84 36 25 75 9B 09 5C BA EC 6C 7A 77 2E 8C 06 D5 DC 0B D1 11 FE 60 83 28 F1 9F 98 5A 4F F6 99 9A E2 32 E9 BC 58 76 87 C5 F2 92 3D 1C 50 0C 22 94 1A EB 31 C3 B9 55 4B 
					Console.WriteLine($"SmartRib Mode - Read Codeplug -- {message.Data[1]:X2} Bytes at  {message.Data[2]:X2} {message.Data[3]:X2} {message.Data[4]:X2}");
					response = new SbepMessage(0x00, 0x87, 0x00);
					break;
			}

			if (response != null)
			{
				Port.Write(response.Bytes, 0, response.Bytes.Length);
			}
			else
			{
				Console.WriteLine("Unknown SRIB Message: " + String.Join(" ", Array.ConvertAll(message.Bytes, _ => _.ToString("X2"))));
			}


			//if (packet.SequenceEqual(new byte[] { 0x03, 0x04, 0x02 }))
			//{
			//	Port.Write(new byte[] { 0x50 }, 0, 1);
			//}
			//else if (packet.SequenceEqual(new byte[] { 0x02, 0x10 }))
			//{
			//	Console.WriteLine("SmartRib Mode - 3rd Command");
			//	Port.Write(new byte[] { 0x50 }, 0, 1);
			//	Port.Write(new byte[] { 0x08, 0x90, 0xCD, 0x43, 0xF1, 0xB6, 0x33, 0x90, 0xED }, 0, 9);
			//}
			//else if (packet.SequenceEqual(new byte[] { 0x08, 0x12, 0xB5, 0xB5, 0x94, 0xEE, 0x97, 0xFE }))
			//{
			//	Console.WriteLine("SmartRib Mode - 4th Command");
			//	Port.Write(new byte[] { 0x50 }, 0, 1);
			//	Port.Write(new byte[] { 0x03, 0x87, 0x00, 0x75 }, 0, 4);
			//}
			//else if (packet.SequenceEqual(new byte[] { 0x02, 0x1A }))
			//{
			//	Console.WriteLine("SmartRib Mode - 5th Command");
			//	Port.Write(new byte[] { 0x50 }, 0, 1);
			//	Port.Write(new byte[] { 0x0F, 0x00, 0x2C, 0x9A, 0x00, 0x00, 0x01, 0x20, 0x52, 0x30, 0x31, 0x2E, 0x30, 0x30, 0x09, 0x20, 0x52, 0x30, 0x34, 0x2E, 0x30, 0x30, 0x03, 0x20, 0x52, 0x30, 0x36, 0x2E, 0x34, 0x30, 0x03, 0x20, 0x20, 0x52, 0x30, 0x38, 0x37, 0x31, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x89 }, 0, 47);
			//}
			//else if (packet.SequenceEqual(new byte[] { 0x03, 0x40, 0x09 }))
			//{
			//	Console.WriteLine("SmartRib Mode - 6th Command");
			//	Port.Write(new byte[] { 0x50 }, 0, 1);
			//	Port.Write(new byte[] { 0x03, 0x87, 0x01, 0x74 }, 0, 4);
			//}
			//else if (packet.SequenceEqual(new byte[] { 0x02, 0x46 }))
			//{
			//	Console.WriteLine("SmartRib Mode - Exit");
			//	echo = true;
			//	sb9600Mode = false;
			//	Port.Write(new byte[] { 0x50 }, 0, 1);
			//}
			//else
			//{
			//	Console.WriteLine("msg  " + String.Join(" ", Array.ConvertAll(packet, _ => _.ToString("X2"))));
			//	Port.Write(new byte[] { 0x50 }, 0, 1);
			//}
		}

		private object portLock = new Object();
		private byte[] packetBuffer = new byte[1024];
		private int packetBufferCount = 0;
		private bool echo = true;
		private bool sbepMode = false;
		private bool sribMode = false;
		private Stopwatch sw = new Stopwatch();

		private void RaiseAppSerialDataEvent(byte[] readBuffer)
		{
			if (echo) Port.Write(readBuffer, 0, readBuffer.Length);
			lock (portLock)
			{
				if (sw.ElapsedMilliseconds > 50)
				{
					packetBufferCount = 0; //If it's been more than 50ms, abandon any incomplete packets
										   //Thought about a timer to return a NAK, but Firmware has same behavior
				}

				sw.Reset();

				Buffer.BlockCopy(readBuffer, 0, packetBuffer, packetBufferCount, readBuffer.Length);
				packetBufferCount += readBuffer.Length;

				if (sbepMode)
				{
					var message = new SbepMessage(packetBuffer, packetBufferCount);
					if (message.Incomplete)
					{
						sw.Restart(); //Incomplete packet. Give 50ms to complete
						return;
					}

					packetBufferCount = 0;

					if (message.Invalid)
					{
						Port.Write(new byte[] { 0x60 }, 0, 1); //Negative ACK
						return;
					}

					Port.Write(new byte[] { 0x50 }, 0, 1); //ACK

					if (sribMode)
					{
						ProcessSrib(message);
					}
					else
					{
						ProcessSbep(message);
					}

				}
				else
				{
					var message = new SB9600Message(packetBuffer, packetBufferCount);
					if (message.Incomplete)
					{
						sw.Restart(); //Incomplete packet. Give 50ms to complete
						return;
					}

					packetBufferCount = 0;
					if (message.Invalid)
					{
						Port.Write(new byte[] { 0x60 }, 0, 1); //Negative ACK
					}
					else
					{
						Port.Write(new byte[] { 0x50 }, 0, 1); //ACK
						ProcessSB9600(message);
					}
				}
			}
		}

		private void ReadEvent()
		{
			byte[] buffer = new byte[2000];
			Action kickoffRead = null;
			kickoffRead = (Action)(() => Port.BaseStream.BeginRead(buffer, 0, buffer.Length, delegate (IAsyncResult ar)
			{
				try
				{
					int count = Port.BaseStream.EndRead(ar);
					byte[] dst = new byte[count];
					Buffer.BlockCopy(buffer, 0, dst, 0, count);
					RaiseAppSerialDataEvent(dst);
				}
				catch (Exception exception)
				{
					//exception.Dump();
				}
				kickoffRead();
			}, null));
			kickoffRead();
		}
	}
}
