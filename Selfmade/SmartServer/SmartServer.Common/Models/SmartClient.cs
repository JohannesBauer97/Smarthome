using System;

namespace SmartServer.Common.Models
{
  public abstract class SmartClient
  {
    public SmartClient(string chipId)
    {
      ChipId = chipId;
    }

    public string ChipId { get; }

  }
}