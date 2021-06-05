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
    }
}
