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

namespace RoboRecords.Models
{
    public class RoboRecord
    {
        public int DbId;
        
        public virtual RoboUser Uploader { get; set; }
        public long Tics;
        public long Rings;
        public long Score;
        public virtual RoboCharacter Character { get; set; }

        // 202 -> 2.2
        private int _majorVersion;
        // 9 in 2.2.9
        private int _subVersion;
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
        public string VersionString
        {
            get => GetVersionString();
        }
        public void SetVersion(int majorVersion, int subVersion)
        {
            _majorVersion = majorVersion;
            _subVersion = subVersion;
            // Major version: 202, Minor version: 9 -> Version string: 2.2.9
        }

        public string GetVersionString()
        {
            return _majorVersion / 100 + "." + _majorVersion % 100 + "." + _subVersion;
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
            SetVersion(_majorVersion, _subVersion);
            
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

        public static string GetTimeFromTics(long tics)
        {
            long minutes = tics / 2100;
            long seconds = tics / 35 % 60;
            var centiseconds = (tics * 100 / 35) % 100;
            return minutes > 0
                ? minutes + ":" + seconds.ToString("D2") + "." + centiseconds.ToString("D2")
                : seconds + "." + centiseconds.ToString("D2");
        }
        
        // Needed for the database context
        public RoboRecord()
        {
            UploadTime = DateTime.Now;
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
                Logger.Log("Error: Couldn't read bytes.", Logger.LogLevel.Error, true);
            }
            // Read the file and get tics and shit
            switch (readStatus)
            {
                case ReadStatus.Success:
                    FileBytes = fileBytes;
                    Logger.Log("File read successfully!", true);
                    break;
                case ReadStatus.ErrorInvalid:
                    Logger.Log("Error: Invalid demo file.", Logger.LogLevel.Error, true);
                    break;
                case ReadStatus.ErrorUnfinished:
                    Logger.Log("Error: Unfinished demo.", Logger.LogLevel.Error, true);
                    break;
                case ReadStatus.ErrorNotSrb2:
                    Logger.Log("Error, not an SRB2 demo!", Logger.LogLevel.Error, true);
                    break;
                case ReadStatus.ErrorReading:
                    Logger.Log("Error, couldn't read the demo!", Logger.LogLevel.Error, true);
                    break;
                default:
                    Logger.Log("Error: Unknown error.", Logger.LogLevel.Error, true);
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