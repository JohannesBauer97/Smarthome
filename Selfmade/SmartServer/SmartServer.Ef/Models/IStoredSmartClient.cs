using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartServer.Ef.Models
{
  public interface IStoredSmartClient
  {
    [Key]
    public string ChipId { get; set; }

  }
}
