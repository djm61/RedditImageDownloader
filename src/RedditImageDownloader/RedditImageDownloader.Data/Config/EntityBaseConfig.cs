using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedditImageDownloader.Data.Entities;

namespace RedditImageDownloader.Data.Config
{
    public class EntityBaseConfig<T> : IEntityTypeConfiguration<T> where T : EntityBase
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Properties
            builder.Property(t => t.Active)
                .IsRequired()
                .HasMaxLength(1);

            builder.Property(t => t.Deleted)
                .IsRequired()
                .HasMaxLength(1);

            builder.Property(e => e.RowVersion)
                .IsRequired()
                .IsRowVersion()
                //.IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("DATETIME('now')")
                .HasColumnType("datetime");

            // Column Mappings
            builder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(t => t.Active).HasColumnName("Active");
            builder.Property(t => t.Deleted).HasColumnName("Deleted");
            builder.Property(t => t.RowVersion).HasColumnName("RowVersion");
        }
    }
}
