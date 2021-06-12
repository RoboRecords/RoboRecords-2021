/*
 * RoboRecord.cs
 * Copyright (C) 2021, Ors <Riku-S>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;
using System.IO;

namespace RoboRecords.Models
{
    public static class DataReader
    {
        public static string ReadByteString(ref int currentByte, int length, byte[] demoBytes)
        {
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = (char)demoBytes[currentByte + i];
            }

            currentByte += length;
            return new string(chars);
        }
        
        public static byte ReadUInt8(ref int currentByte, byte[] demoBytes)
        {
            byte[] bytes = new byte[1];
            Stream stream = new MemoryStream(demoBytes);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                reader.BaseStream.Seek(currentByte, SeekOrigin.Begin);
                reader.Read(bytes, 0, 1);
            }

            currentByte++;
            return bytes[0];
        }

        public static Int16 ReadSInt16(ref int currentByte, byte[] demoBytes)
        {
            byte[] bytes = new byte[2];
            Stream stream = new MemoryStream(demoBytes);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                reader.BaseStream.Seek(currentByte, SeekOrigin.Begin);
                reader.Read(bytes, 0, 2);
            }

            currentByte += 2;
            return BitConverter.ToInt16(bytes, 0);
        }

        public static sbyte ReadSInt8(ref int currentByte, byte[] demoBytes)
        {
            byte[] bytes = new byte[1];
            Stream stream = new MemoryStream(demoBytes);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                reader.BaseStream.Seek(currentByte, SeekOrigin.Begin);
                reader.Read(bytes, 0, 1);
            }

            currentByte++;
            return (sbyte)bytes[0];
        }

        public static UInt16 ReadUInt16(ref int currentByte, byte[] demoBytes)
        {
            byte[] bytes = new byte[2];
            Stream stream = new MemoryStream(demoBytes);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                reader.BaseStream.Seek(currentByte, SeekOrigin.Begin);
                reader.Read(bytes, 0, 2);
            }

            currentByte += 2;
            return BitConverter.ToUInt16(bytes, 0);
        }

        public static UInt32 ReadUInt32(ref int currentByte, byte[] demoBytes)
        {
            byte[] bytes = new byte[4];
            Stream stream = new MemoryStream(demoBytes);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                reader.BaseStream.Seek(currentByte, SeekOrigin.Begin);
                reader.Read(bytes, 0, 4);
            }

            currentByte += 4;
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static Int32 ReadSInt32(ref int currentByte, byte[] demoBytes)
        {
            byte[] bytes = new byte[4];
            Stream stream = new MemoryStream(demoBytes);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                reader.BaseStream.Seek(currentByte, SeekOrigin.Begin);
                reader.Read(bytes, 0, 4);
            }

            currentByte += 4;
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}