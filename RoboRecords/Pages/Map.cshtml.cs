using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Renci.SshNet.Messages.Authentication;
// using RoboRecords.DatabaseContexts;
using RoboRecords.DbInteraction; // Initiative to move all database interactions to one place. --- Zenya
using RoboRecords.Models;


namespace RoboRecords.Pages
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class Map : RoboPageModel
    {
        public static RoboGame CurrentGame;
        public static RoboLevel CurrentLevel;

        public RoboRecord RecordToDownload { get; set; }

        public static byte[] RecordToDownloadBytes;

            // private List<RoboGame> _roboGames;

        // Initiative to move all database interactions to one place. --- Zenya
        
        //private RoboRecordsDbContext _dbContext;
        
        public Map(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        public class DownloadReplayData
        {
            public int DbId { get; set; }
        }
        
        public IActionResult OnPostReplay([FromBody] DownloadReplayData data)
        {
            if (DbSelector.TryGetRoboRecordFromDbId(data.DbId, out RoboRecord record))
            {
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = record.GetFileName(),
                    Inline = true,
                };
                
                RecordToDownload = record;
                FileManager.Read(System.IO.Path.Combine(FileManager.ReplaysDirectoryName, $"{record.DbId}.lmp"), out byte[] bytes);
                
                Logger.Log("GET success", true);
                Response.Headers.Add("Content-Disposition", cd.ToString());
                return File(bytes, "application/octet-stream", record.GetFileName());
            }

            Logger.Log("GET net success", true);
            return BadRequest("Bad request");
        }
        
        public void OnGet()
        {
            CurrentGame = new RoboGame("Invalid Game");
            CurrentLevel = new RoboLevel(-1,"Invalid level", 0);

            var gameId = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("game");
            var mapId = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("map");

            DbSelector.TryGetGameFromID(gameId, out CurrentGame);
            DbSelector.TryGetGameLevelFromMapId(gameId, mapId, out CurrentLevel);
        }
    }
}