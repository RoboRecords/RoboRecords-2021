/*
 * UnitTest1.cs
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
using RoboRecords.Models;
using Xunit;

namespace UnitTests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("MAP01", 1)]
        [InlineData("MAPA1", 101)]
        [InlineData("MAPAA", 110)]
        [InlineData("MAPZZ", 1035)]
        [InlineData("Invalid-map-number", 1036)]
        [InlineData("Invalid-map-number", -1)]
        public void TestMaps(string mapString, int levelNumber)
        {
            Assert.Equal(mapString, RoboLevel.MakeMapString(levelNumber));
        }

        [Fact]
        public void TestVersionString()
        {
            var roboRecord = new RoboRecord(null, null);
            
            roboRecord.SetVersion(2,9);
            Assert.Equal("2.2.9", roboRecord.VersionString);
            roboRecord.MajorVersion = 3;
            Assert.Equal("2.3.9", roboRecord.VersionString);
            roboRecord.SubVersion = 0;
            Assert.Equal("2.3.0", roboRecord.VersionString);
        }

        [Theory]
        [InlineData("MAP01-amy-0.16.68.lmp", "amy", 584, 1, "MAP01-guest.lmp")] // Valid Amy replay on GFZ1
        [InlineData("MAP26-sonic-last.lmp", null, uint.MaxValue, 26, "MAP26-guest.lmp")] // Unfinished Sonic replay on BCZ2, reading stops early
        [InlineData("MAP50-time-best.lmp", "sonic", 807, 50, "MAP50-guest.lmp")] // Floral Field NiGHTS replay
        public void TestFileReading(string fileName, string character, uint tics, int mapNumber, string mapName)
        {
            var replayBytes = File.ReadAllBytes(fileName);
            var roboRecord = new RoboRecord(null, replayBytes);
            
            Assert.Equal(mapNumber, roboRecord.LevelNumber);
            Assert.Equal(tics, roboRecord.Tics);
            Assert.Equal(character, roboRecord.Character);
            Assert.Equal(mapName, roboRecord.GetFileName());
        }
    }
}
