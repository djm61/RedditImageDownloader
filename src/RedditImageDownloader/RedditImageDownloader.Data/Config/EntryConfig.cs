using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedditImageDownloader.Data.Entities;

namespace RedditImageDownloader.Data.Config
{
    public class EntryConfig : EntityBaseConfig<Entry>
    {
        public override void Configure(EntityTypeBuilder<Entry> builder)
        {
            base.Configure(builder);

            builder.ToTable("Entry", "dbo");

            builder.HasIndex(e => new { e.Active, e.Deleted })
                .HasName("IX_Entry_ActiveDeleted");

            builder.Property(e => e.PostId).HasMaxLength(25).IsUnicode();
            builder.Property(e => e.Url).HasMaxLength(100).IsUnicode();
            builder.Property(e => e.Processed).HasMaxLength(1).IsUnicode();
            builder.Property(e => e.Downloaded).HasMaxLength(1).IsUnicode();

            builder.HasIndex(e => e.SourceId).HasName("IX_Entry_SourceId");

            builder.HasOne(e => e.Source)
                .WithMany(e => e.Entries)
                .HasForeignKey(e => e.SourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Entry_Source");
        }
    }
}
