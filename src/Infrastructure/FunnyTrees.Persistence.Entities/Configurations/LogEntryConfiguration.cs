using FunnyTrees.Persistence.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunnyTrees.Persistence.Entities.Configurations;

internal class LogEntryConfiguration : IEntityTypeConfiguration<LogEntry>
{
    public void Configure(EntityTypeBuilder<LogEntry> builder)
    {
        var tableName = nameof(LogEntry);

        builder.ToTable(tableName);

        builder.HasKey(n => n.Id);
        builder.Property(n => n.EventId).IsRequired();
        builder.Property(n => n.Timestamp).IsRequired();
        builder.Property(n => n.StackTrace).HasMaxLength(2048).IsRequired();
        builder.Property(n => n.Message).HasMaxLength(2048).IsRequired();
    }
}
