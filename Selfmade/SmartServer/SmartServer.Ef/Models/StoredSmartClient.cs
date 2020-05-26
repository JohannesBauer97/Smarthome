using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartServer.Ef.Models
{
  public abstract class StoredSmartClient
  {
    public StoredSmartClient() { }

    [Required]
    public string ChipId { get; }

    [Key]
    public Guid DbId { get; set; }

    public DateTime Discovered { get; set; }
  }
}
