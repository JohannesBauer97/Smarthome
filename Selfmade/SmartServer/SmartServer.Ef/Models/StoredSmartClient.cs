using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartServer.Ef.Models
{
  public abstract class StoredSmartClient
  {
    public StoredSmartClient() { }

    [Key]
    public string ChipId { get; set; }

  }
}
