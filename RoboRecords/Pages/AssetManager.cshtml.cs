using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoboRecords.Models;

namespace RoboRecords.Pages;

public class AssetManager : RoboPageModel
{
    private AssetManager _assetManager;
    
    public AssetManager(AssetManager assetManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _assetManager = assetManager;
    }
    
    public void OnGet()
    {
        
    }
}