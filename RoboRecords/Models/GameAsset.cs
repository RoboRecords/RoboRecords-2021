/*
 * GameAsset.cs: The model for a game asset managed by the AssetManager
 * Copyright (C) 2022, Refrag <Refragg>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System.Text.Json.Serialization;

namespace RoboRecords.Models;

public class GameAsset : BaseAsset
{
    public virtual RoboGame Game { get; set; }
    
    [JsonIgnore]
    public override string Path => FileManager.GamesAssetsDirectoryName;
    
    public GameAsset() : base()
    {
    }

    public GameAsset(RoboGame game, string name, string fileExtension) : base(name, fileExtension)
    {
        Game = game;
    }
}