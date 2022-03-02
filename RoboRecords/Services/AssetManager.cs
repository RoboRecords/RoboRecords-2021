/*
 * AssetManager.cs: Service to provide functionality to the AssetManager page and for the rest of the codebase
 * Copyright (C) 2022, Refrag <Refragg>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;
using System.Collections.Generic;
using RoboRecords.DbInteraction;
using RoboRecords.Models;

namespace RoboRecords.Services;

public class AssetManager
{

    public SiteAsset CreateSiteAsset(string name, string fileExtension, byte[] file)
    {
        SiteAsset asset = new SiteAsset(name, fileExtension);

        DbInserter.AddSiteAsset(asset);
        string path = $"{FileManager.NewAssetsDirectoryName}/{FileManager.SiteAssetsDirectoryName}/{asset.DbId}.{asset.FileExtension}";

        FileManager.Write(path, file);

        
        return asset;
    }
    
    public GameAsset CreateGameAsset(RoboGame game, string name, string fileExtension, byte[] file)
    {
        GameAsset asset = new GameAsset(game, name, fileExtension);

        DbInserter.AddGameAsset(asset);
        string path = $"{FileManager.NewAssetsDirectoryName}/{FileManager.GamesAssetsDirectoryName}/{asset.DbId}.{asset.FileExtension}";

        FileManager.Write(path, file);

        
        return asset;
    }
    
    public CharacterAsset CreateCharacterAsset(RoboCharacter character, string name, string fileExtension, byte[] file)
    {
        CharacterAsset asset = new CharacterAsset(character, name, fileExtension);

        DbInserter.AddCharacterAsset(asset);
        string path = $"{FileManager.NewAssetsDirectoryName}/{FileManager.CharactersAssetsDirectoryName}/{asset.DbId}.{asset.FileExtension}";

        FileManager.Write(path, file);

        
        return asset;
    }

    public List<SiteAsset> GetSiteAssets()
    {
        if (DbSelector.TryGetAllSiteAssets(out List<SiteAsset> siteAssets))
            return siteAssets;
        return null;
    }
    
    public List<GameAsset> GetAllGameAssets()
    {
        if (DbSelector.TryGetAllGameAssets(out List<GameAsset> gameAssets))
            return gameAssets;
        return null;
    }
    
    public List<GameAsset> GetGameAssets(RoboGame game)
    {
        if (DbSelector.TryGetGameAssetsFromGame(game, out List<GameAsset> gameAssets))
            return gameAssets;
        return null;
    }
    
    public List<CharacterAsset> GetAllCharacterAssets()
    {
        if (DbSelector.TryGetAllCharacterAssets(out List<CharacterAsset> characterAssets))
            return characterAssets;
        return null;
    }
    
    public List<CharacterAsset> GetCharacterAssets(RoboCharacter character)
    {
        if (DbSelector.TryGetCharacterAssetsFromCharacter(character, out List<CharacterAsset> characterAssets))
            return characterAssets;
        return null;
    }
}