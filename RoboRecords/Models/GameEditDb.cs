using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoboRecords.Models
{
    public class GameEditDb
    {
        [Required] [Display(Name = "Name")] public string Name;
        [Required] [Display(Name = "Url name")] public string UrlName;

        [Required] [Display(Name = "Level Groups")]
        public IList<LevelGroup> LevelGroups;
    }
}