using System;
using System.Collections.Generic;

namespace SmartServer.Common
{
  public class SmartClientComparer : IEqualityComparer<SmartClient>
  {
    public bool Equals(SmartClient x, SmartClient y)
    {
      if (x == null && y == null)
        return true;

      if ((x == null) ^ (y == null))
        return false;

      return x.ChipId.Equals(y.ChipId, StringComparison.InvariantCultureIgnoreCase);
    }

    public int GetHashCode(SmartClient obj)
    {
      return obj.ChipId.GetHashCode();
    }
  }
}