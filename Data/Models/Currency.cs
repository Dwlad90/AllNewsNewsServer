using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllNewsServer.Data.Models {
  public class Currency : BaseEntity {
    [Key]
    [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Key { get; set; }
    public virtual ICollection<LocalizationResource> Names { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product> ();
  }
}