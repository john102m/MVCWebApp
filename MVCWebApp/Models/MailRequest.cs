using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models
{
    public class MailRequest
    {
        [Required]
        [EmailAddress]  
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }      
        public List<IFormFile> Attachments { get; set; }
    }
}
