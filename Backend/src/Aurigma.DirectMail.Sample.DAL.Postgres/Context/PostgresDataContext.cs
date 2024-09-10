using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aurigma.DirectMail.Sample.DAL.Postgres.Context;

public class PostgresDataContext : DbContext
{
    public PostgresDataContext(DbContextOptions<PostgresDataContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CompanyDal>().Property(x => x.Id).ValueGeneratedNever();
        modelBuilder.Entity<CompanyDal>().Property(x => x.Name).HasMaxLength(256);

        modelBuilder.Entity<CampaignDal>().Property(x => x.Id).ValueGeneratedNever();
        modelBuilder.Entity<CampaignDal>().Property(x => x.Title).HasMaxLength(256);

        modelBuilder.Entity<ProductDal>().Property(x => x.Id).ValueGeneratedNever();
        modelBuilder.Entity<ProductDal>().Property(x => x.Title).HasMaxLength(256);

        modelBuilder.Entity<CategoryDal>().Property(x => x.Id).ValueGeneratedNever();
        modelBuilder.Entity<CategoryDal>().Property(x => x.Title).HasMaxLength(256);

        modelBuilder
            .Entity<RecipientListCampaignDal>()
            .HasKey(x => new { x.RecipientListId, x.CampaignId });

        modelBuilder
            .Entity<RecipientImageRecipientDal>()
            .HasKey(x => new { x.RecipientId, x.RecipientImageId });

        new DbInitializer(modelBuilder).Seed();
    }

    #region Database entities (table names)
    public DbSet<CompanyDal> Company { get; set; }

    public DbSet<CampaignDal> Campaign { get; set; }

    public DbSet<ProductDal> Product { get; set; }

    public DbSet<CategoryDal> Category { get; set; }

    public DbSet<LineItemDal> LineItem { get; set; }

    public DbSet<RecipientListDal> RecipientList { get; set; }

    public DbSet<RecipientDal> Recipient { get; set; }

    public DbSet<RecipientImageDal> RecipientImage { get; set; }

    public DbSet<RecipientListCampaignDal> RecipientListCampaign { get; set; }

    public DbSet<JobDal> Job { get; set; }

    public DbSet<RecipientImageRecipientDal> RecipientImageRecipientDal { get; set; }

    #endregion Database entities (table names)
}
