using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tenli.Server.Data.DTOs.ActiveSession
{
    public class ActiveSessionDTO
    {
        public int Id { get; set; }

        public DateTime CreationDateTime { get; set; }

        public DateTime LastRefreshDateTime { get; set; }

        public string UserAgent { get; set; }

        public string Location { get; set; }

        public string IPAddress { get; set; }
    }
}
