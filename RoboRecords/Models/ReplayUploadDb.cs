using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;

namespace RoboRecords.Models
{
    public class ReplayUploadDb
    {
        [Required]
        [Display(Name="File")]
        public List<IFormFile> FormFiles { get; set; }
    }
}