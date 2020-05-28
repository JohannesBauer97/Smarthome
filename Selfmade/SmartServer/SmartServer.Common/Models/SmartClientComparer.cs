using System;
using System.Collections.Generic;

namespace SmartServer.Common.Models
{
  public class SmartClientComparer : IEqualityComparer<ISmartClient>
  {
    public bool Equals(ISmartClient x, ISmartClient y)
    {
      if (x == null && y == null)
        return true;

      if ((x == null) ^ (y == null))
        return false;

      return x.ChipId.Equals(y.ChipId, StringComparison.InvariantCultureIgnoreCase);
    }

    public int GetHashCode(ISmartClient obj)
    {
      return obj.ChipId.GetHashCode();
    }
  }
}