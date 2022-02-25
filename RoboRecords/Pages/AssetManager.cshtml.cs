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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
}