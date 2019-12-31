using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedditImageDownloader.Data.Config;
using RedditImageDownloader.Data.Entities;

namespace RedditImageDownloader.Data
{
    public class RedditImageDownloaderContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<RedditImageDownloaderContext> _logger;

        public RedditImageDownloaderContext(ILoggerFactory loggerFactory, DbContextOptions<RedditImageDownloaderContext> options)
            : base(options ?? throw new ArgumentNullException(nameof(options)))
        {
            _loggerFactory = loggerFactory ?? new LoggerFactory();
            _logger = loggerFactory.CreateLogger<RedditImageDownloaderContext>();

            Database.EnsureCreated();
        }

        #region Entities

        public virtual DbSet<Source> Sources { get; set; }

        public virtual DbSet<Entry> Entries { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite("Data Source=RedditImageDownloader.sqlite")
                .UseLoggerFactory(_loggerFactory)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.LogDebug("OnModelCreating: starting!");

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new SourceConfig());
            modelBuilder.ApplyConfiguration(new EntryConfig());

            _logger.LogDebug("OnModelCreating: end!");
        }

        public override int SaveChanges()
        {
            _logger.LogDebug("SaveChanges");
            SetUpdateState(DateTime.UtcNow);
#if DEBUG
            var added = ChangeTracker.Entries().Where(x => x.State == EntityState.Added).Select(x => x.Entity).ToList();
            var modified = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Select(x => x.Entity).ToList();
#endif
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _logger.LogDebug($"SaveChanges | acceptAllChanges[{acceptAllChangesOnSuccess}]");
            SetUpdateState(DateTime.UtcNow);
#if DEBUG
            var added = ChangeTracker.Entries().Where(x => x.State == EntityState.Added).Select(x => x.Entity).ToList();
            var modified = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Select(x => x.Entity).ToList();
#endif
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            _logger.LogDebug($"SaveChangesAsync: Setting Update State with cancellationToken = {cancellationToken}");
            SetUpdateState(DateTime.UtcNow);
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            _logger.LogDebug(
                $"SaveChangesAsync: Setting Update State with acceptAllChangesOnSuccess = {acceptAllChangesOnSuccess}, cancellationToken = {cancellationToken}");
            SetUpdateState(DateTime.UtcNow);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public static DbContextOptions<RedditImageDownloaderContext> BuildContextOptions(ILoggerFactory loggerFactory, string connectionString)
        {
            var builder = new DbContextOptionsBuilder<RedditImageDownloaderContext>();
            builder.UseSqlite(connectionString)
                .UseLoggerFactory(loggerFactory)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();

            return builder.Options;
        }

        private void SetUpdateState(DateTime updateTime)
        {
            _logger.LogDebug($"SetUpdateState: Setting RowVersion with {updateTime:yyyy-MM-dd hh:mm:ss}");
            var entities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified)
                .Select(x => x.Entity);

            foreach (var entity in entities)
            {
                if (entity is IEntityBase e)
                {
                    e.RowVersion = updateTime;
                }
            }
        }
    }
}
