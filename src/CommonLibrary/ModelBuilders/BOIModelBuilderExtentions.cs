using CommonLibrary.Core;
using CommonLibrary.Logging;
using Microsoft.EntityFrameworkCore;

namespace CommonLibrary.ModelBuilders;

public static class BOIModelBuilderExtentions
{
    public static ModelBuilder BuildCommonLibrary(
        this ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<LogHandle>()
        //     .HasOne(s => s.Object)
        //     .WithOne(t => t.LogHandle)
        //     .HasForeignKey<LogHandle>(s => s.Id);
        // modelBuilder.Entity<IIObject>()
        //     .HasOne(s => s.LogHandle)
        //     .WithOne(s => s.Object)
        //     .HasForeignKey<LogHandle>(s => s.Id);
        return modelBuilder;
    }
}