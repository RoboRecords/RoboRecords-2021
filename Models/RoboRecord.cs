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
using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace RoboRecords.Models
{
    public class RoboRecord
    {
        public int Index;
        public RoboUser Uploader;
        public int Tics;
        public int Rings;
        public int Score;

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
        public RoboLevel Level;
        
        public RoboRecord(RoboUser uploader, byte[] fileBytes)
        {
            Uploader = uploader;
            UploadTime = DateTime.Now;
            FileBytes = fileBytes;

            // Read the file and get tics and shit
            

        }
        
        private const string DefaultNameString = "guest";
        public string GetFileName()
        {
            return Level.MapString + "-" + DefaultNameString + ".lmp";
        }
    }
}