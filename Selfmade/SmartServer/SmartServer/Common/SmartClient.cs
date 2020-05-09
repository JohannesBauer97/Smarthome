using System;

namespace SmartServer.Common
{
  public abstract class SmartClient
  {
    public SmartClient(string chipId)
    {
      ChipId = chipId;
    }

    public string ChipId { get; }

    public DateTime Discovered { get; set; } = DateTime.Now;
  }
}