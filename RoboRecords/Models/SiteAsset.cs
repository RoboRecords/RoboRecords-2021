/*
 * SiteAsset.cs: The model for a site asset managed by the AssetManager
 * Copyright (C) 2022, Refrag <Refragg>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;
using System.Text.Json.Serialization;
using SixLabors.ImageSharp;

namespace RoboRecords.Models;

public sealed class SiteAsset : BaseAsset
{
    [JsonIgnore]
    public override string Path => FileManager.SiteAssetsDirectoryName;
    
    public SiteAsset() : base()
    {
    }
    
    public SiteAsset(string name, string fileExtension) : base(name, fileExtension)
    {
    }
}