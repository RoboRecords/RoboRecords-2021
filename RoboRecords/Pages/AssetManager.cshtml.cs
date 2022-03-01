/*
 * AssetManager.cshtml.cs: Backend of the Asset Manager website's page
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoboRecords.DbInteraction;
using RoboRecords.Models;

namespace RoboRecords.Pages;

public class AssetManager : RoboPageModel
{
    private Services.AssetManager _assetManager;
    
    public AssetManager(Services.AssetManager assetManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _assetManager = assetManager;
    }
    
    public void OnGet()
    {
        
    }

    public IActionResult OnGetGames()
    {
        DbSelector.TryGetGames(out List<RoboGame> games);
        return new JsonResult(games);
    }
    
    public IActionResult OnGetCharacters()
    {
        DbSelector.TryGetCharacters(out List<RoboCharacter> characters);
        return new JsonResult(characters);
    }

    public IActionResult OnGetSiteAssets()
    {
        List<SiteAsset> siteAssets = _assetManager.GetSiteAssets();
        return siteAssets is not null ? new JsonResult(siteAssets) : NotFound();
    }
    
    public IActionResult OnGetGameAssets([FromQuery] int id)
    {
        DbSelector.TryGetGameFromDbID(id, out RoboGame game);
        
        return new JsonResult(_assetManager.GetGameAssets(game));
    }
    
    public IActionResult OnGetCharacterAssets([FromQuery] int id)
    {
        DbSelector.TryGetCharacterFromDbId(id, out RoboCharacter character);
        
        return new JsonResult(_assetManager.GetCharacterAssets(character));
    }
    
    public IActionResult OnPostTest([FromBody] SiteAsset asset)
    {
        Console.WriteLine(asset.DbId);
        Console.WriteLine(asset.Name);
        Console.WriteLine(asset.ImageInfo.IsValueCreated);
        Console.WriteLine(asset.ImageInfo.Value.Width);
        return Content("");
    }
}