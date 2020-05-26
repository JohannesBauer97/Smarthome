using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SmartServer.Common.Models;
using SmartServer.Ef.Models;

namespace SmartServer.Ef
{
  public class SmartServerContext : DbContext
  {
    public SmartServerContext(DbContextOptions options): base(options)
    {
      
    }
    public DbSet<StoredSmartTemperatureClient> SmartTemperatureClients { get; set; }
  }

  public class SmartServerContextFactory : IDesignTimeDbContextFactory<SmartServerContext>
  {
    public SmartServerContext CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<SmartServerContext>();
      optionsBuilder.UseSqlite("Data Source=DesignTime.db");

      return new SmartServerContext(optionsBuilder.Options);
    }
  }
}
