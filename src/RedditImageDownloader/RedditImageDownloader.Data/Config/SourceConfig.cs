using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedditImageDownloader.Data.Entities;

namespace RedditImageDownloader.Data.Config
{
    public class SourceConfig : EntityBaseConfig<Source>
    {
        public override void Configure(EntityTypeBuilder<Source> builder)
        {
            base.Configure(builder);

            builder.ToTable("Source", "dbo");

            builder.HasIndex(e => new { e.Active, e.Deleted })
                .HasName("IX_Source_ActiveDeleted");

            builder.HasIndex(e => e.FeedUrl).IsUnique().HasName("UQ_Source_FeedUrl");

            builder.Property(e => e.Name).HasMaxLength(50).IsUnicode();
            builder.Property(e => e.NiceName).HasMaxLength(100).IsUnicode();
            builder.Property(e => e.Url).HasMaxLength(100).IsUnicode();
            builder.Property(e => e.FeedUrl).HasMaxLength(100).IsUnicode();

            builder.HasMany(e => e.Entries)
                .WithOne(e => e.Source)
                .HasForeignKey(e => e.SourceId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
