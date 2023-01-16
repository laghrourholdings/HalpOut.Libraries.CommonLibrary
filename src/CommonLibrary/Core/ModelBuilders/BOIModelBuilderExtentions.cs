using Microsoft.EntityFrameworkCore;

namespace CommonLibrary.Core;

public static class BOIModelBuilderExtentions
{
    /// <summary>
    /// Builds the EFCore relationship between base BOIs
    /// </summary>
    public static ModelBuilder BuildCommonLibrary(
        this ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<LogHandle>()
        //     .HasOne(s => s.ObjectId)
        //     .WithOne(t => t)
        //     .HasForeignKey<LogHandle>(s => s.Id);
        // modelBuilder.Entity<IIObject>()
        //     .HasOne(s => s.LogHandle)
        //     .WithOne(s => s.Object)
        //     .HasForeignKey<LogHandle>(s => s.Id);
        return modelBuilder;
    }
}
