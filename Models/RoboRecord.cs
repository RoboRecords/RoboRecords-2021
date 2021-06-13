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
using System.Diagnostics;
using System.IO;
using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace RoboRecords.Models
{
    public class RoboRecord
    {
        public int Index;
        public RoboUser Uploader;
        public uint Tics;
        public UInt16 Rings;
        public uint Score;
        public RoboCharacter Character; 
        
        
        // X in 2.X
        private int _majorVersion;
        // 9 in 2.2.9
        private int _subVersion;
        // "v2.2.9", for example
        private string _versionString;
        public int MajorVersion
        {
            get => _majorVersion;
            set => SetVersion(value, _subVersion);
        }
        public int SubVersion
        {
            get => _subVersion;
            set => SetVersion(_majorVersion, value);
        }
        // Read-only outside the class. Edit by changing the version numbers
        public string VersionString => _versionString;
        public void SetVersion(int majorVersion, int subVersion)
        {
            _majorVersion = majorVersion;
            _subVersion = subVersion;
            _versionString = "2." + majorVersion + "." + subVersion;
        }

        public byte[] FileBytes;
        // Time the time is uploaded
        public DateTime UploadTime;
        
        // Level the record is played on
        public int LevelNumber;
        //public RoboLevel Level;
        
        private const uint MaxSize = uint.MaxValue;
        //private static readonly char[] DemoHeaderBytes = { (char)0xF0, 'S', 'R', 'B', '2', 'R', 'e', 'p', 'l', 'a', 'y', (char)0x0F }; // hackish method to spell out "ðSRB2Replay", required for file verification
        private static readonly string DemoHeader = char.ToString((char)0xF0) + "SRB2Replay" + char.ToString((char)0x0F);

        enum  ReadStatus
        {
            Success,
            ErrorNotSrb2,
            ErrorInvalid,
            ErrorUnfinished,
            ErrorReading
        }
        private ReadStatus ReadDemo(byte[] bytes)
        {
            FileBytes = bytes;
            int curByte = 0;

            // read demo header
            string demoHeader = DataReader.ReadByteString(ref curByte, 12, bytes);
            if (demoHeader != DemoHeader)
            {
                return ReadStatus.ErrorNotSrb2;
            }
            Debug.WriteLine(demoHeader);
            // Demo Version
            _majorVersion = DataReader.ReadUInt8(ref curByte, bytes);
            _subVersion = DataReader.ReadUInt8(ref curByte, bytes);
            UInt16 demoVersion = DataReader.ReadUInt16(ref curByte, bytes);

            // Demo Checksum
            curByte += 16;

            // Player or Metal demo
            string playMetal = DataReader.ReadByteString(ref curByte, 4, bytes);

            if (playMetal != "PLAY")
            {
                return ReadStatus.ErrorInvalid;
            }

            // Map Info
            LevelNumber = DataReader.ReadUInt16(ref curByte, bytes);
            
            //Map checksum
            curByte += 16;

            // Demo Flag. 03 is default, 05 for NiGHTS
            byte demoFlag = DataReader.ReadUInt8(ref curByte, bytes);
            
            if (demoFlag == 2 || demoFlag == 3) // Regular demo. 2 means no ghost data, 3 means ghost data.
            {
                Tics = DataReader.ReadUInt32(ref curByte, bytes);
                Score = DataReader.ReadUInt32(ref curByte, bytes);
                Rings = DataReader.ReadUInt16(ref curByte, bytes);
            }
            else if (demoFlag == 4 || demoFlag == 5)  // NiGHTs demos don't have a ring stat. For now, we just leave it at 0.
            {
                Tics = DataReader.ReadUInt32(ref curByte, bytes);
                Score = DataReader.ReadUInt32(ref curByte, bytes);
            }

            if (Tics == MaxSize)
            {
                return ReadStatus.ErrorUnfinished;
            }

            // Demo Seed
            curByte += sizeof(UInt32);

            // Player Info
            // Player name
            curByte += 16;
            Character = CharacterManager.GetCharacterById(DataReader.ReadByteString(ref curByte, 16, bytes).TrimEnd('\0'));

            return ReadStatus.Success;
        }

        public static string GetTimeFromTics(uint tics)
        {
            uint minutes = tics / 2100;
            uint seconds = tics / 35 % 60;
            var centiseconds = (tics * 100 / 35) % 100;
            return minutes > 0
                ? minutes + ":" + seconds.ToString("D2") + "." + centiseconds.ToString("D2")
                : seconds + "." + centiseconds.ToString("D2");
        }

        
        public RoboRecord(RoboUser uploader, byte[] fileBytes)
        {
            Uploader = uploader;
            UploadTime = DateTime.Now;
            
            if (fileBytes == null)
                return;

            ReadStatus readStatus;
            try
            {
                readStatus = ReadDemo(fileBytes);
            }
            catch
            {
                readStatus = ReadStatus.ErrorReading;
            }
            // Read the file and get tics and shit
            switch (readStatus)
            {
                case ReadStatus.Success:
                    FileBytes = fileBytes;
                    break;
                case ReadStatus.ErrorInvalid:
                    Debug.WriteLine("Error: Invalid demo file.");
                    break;
                case ReadStatus.ErrorUnfinished:
                    Debug.WriteLine("Error: Unfinished demo.");
                    break;
                case ReadStatus.ErrorNotSrb2:
                    Debug.WriteLine("Error, not an SRB2 demo!");
                    break;
                case ReadStatus.ErrorReading:
                    Debug.WriteLine("Error, couldn't read the demo!");
                    break;
                default:
                    Debug.WriteLine("Error: Unknown error.");
                    break;
            }
        }
        
        private const string DefaultNameString = "guest";
        public string GetFileName()
        {
            return RoboLevel.MakeMapString(LevelNumber) + "-" + DefaultNameString + ".lmp";
        }
    }
}