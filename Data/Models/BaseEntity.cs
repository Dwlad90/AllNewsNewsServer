using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllNewsServer.Data.Models {
  public class BaseEntity {
    public DateTime CreationDateTime { get; set; }
    public string CreationUser { get; set; }
    public DateTime ModificationDateTime { get; set; }
    public string ModificationUser { get; set; }
    public bool IsActive { get; set; } = true;
  }
}