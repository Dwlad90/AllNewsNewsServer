using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllNewsServer.Data.Models
{
    public class ActiveSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string RefreshToken { get; set; }

        public DateTime CreationDateTime { get; set; }

        public DateTime LastRefreshDateTime { get; set; }

        public string UserAgent { get; set; }

        public string Location { get; set; }

        public string IPAddress { get; set; }

        public bool IsTerminated { get; set; }

        public int ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
