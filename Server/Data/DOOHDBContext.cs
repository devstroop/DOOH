using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DOOH.Server.Models.DOOHDB;

namespace DOOH.Server.Data
{
    public partial class DOOHDBContext : DbContext
    {
        public DOOHDBContext()
        {
        }

        public DOOHDBContext(DbContextOptions<DOOHDBContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DOOH.Server.Models.DOOHDB.CampaignAdboard>().HasKey(table => new {
                table.CampaignId, table.AdboardId
            });

            builder.Entity<DOOH.Server.Models.DOOHDB.Adboard>()
              .HasOne(i => i.Category)
              .WithMany(i => i.Adboards)
              .HasForeignKey(i => i.CategoryId)
              .HasPrincipalKey(i => i.CategoryId);

            builder.Entity<DOOH.Server.Models.DOOHDB.Adboard>()
              .HasOne(i => i.City)
              .WithMany(i => i.Adboards)
              .HasForeignKey(i => i.CityName)
              .HasPrincipalKey(i => i.CityName);

            builder.Entity<DOOH.Server.Models.DOOHDB.Adboard>()
              .HasOne(i => i.Country)
              .WithMany(i => i.Adboards)
              .HasForeignKey(i => i.CountryName)
              .HasPrincipalKey(i => i.CountryName);

            builder.Entity<DOOH.Server.Models.DOOHDB.Adboard>()
              .HasOne(i => i.Display)
              .WithMany(i => i.Adboards)
              .HasForeignKey(i => i.DisplayId)
              .HasPrincipalKey(i => i.DisplayId);

            builder.Entity<DOOH.Server.Models.DOOHDB.Adboard>()
              .HasOne(i => i.Motherboard)
              .WithMany(i => i.Adboards)
              .HasForeignKey(i => i.MotherboardId)
              .HasPrincipalKey(i => i.MotherboardId);

            builder.Entity<DOOH.Server.Models.DOOHDB.Adboard>()
              .HasOne(i => i.Provider)
              .WithMany(i => i.Adboards)
              .HasForeignKey(i => i.ProviderId)
              .HasPrincipalKey(i => i.ProviderId);

            builder.Entity<DOOH.Server.Models.DOOHDB.Adboard>()
              .HasOne(i => i.State)
              .WithMany(i => i.Adboards)
              .HasForeignKey(i => i.StateName)
              .HasPrincipalKey(i => i.StateName);

            builder.Entity<DOOH.Server.Models.DOOHDB.AdboardImage>()
              .HasOne(i => i.Adboard)
              .WithMany(i => i.AdboardImages)
              .HasForeignKey(i => i.AdboardId)
              .HasPrincipalKey(i => i.AdboardId);

            builder.Entity<DOOH.Server.Models.DOOHDB.AdboardNetwork>()
              .HasOne(i => i.Adboard)
              .WithMany(i => i.AdboardNetworks)
              .HasForeignKey(i => i.AdboardId)
              .HasPrincipalKey(i => i.AdboardId);

            builder.Entity<DOOH.Server.Models.DOOHDB.AdboardWifi>()
              .HasOne(i => i.Adboard)
              .WithMany(i => i.AdboardWifis)
              .HasForeignKey(i => i.AdboardId)
              .HasPrincipalKey(i => i.AdboardId);

            builder.Entity<DOOH.Server.Models.DOOHDB.Advertisement>()
              .HasOne(i => i.Attachment)
              .WithMany(i => i.Advertisements)
              .HasForeignKey(i => i.AttachmentKey)
              .HasPrincipalKey(i => i.AttachmentKey);

            builder.Entity<DOOH.Server.Models.DOOHDB.Advertisement>()
              .HasOne(i => i.Campaign)
              .WithMany(i => i.Advertisements)
              .HasForeignKey(i => i.CampaignId)
              .HasPrincipalKey(i => i.CampaignId);

            builder.Entity<DOOH.Server.Models.DOOHDB.Analytic>()
              .HasOne(i => i.Adboard)
              .WithMany(i => i.Analytics)
              .HasForeignKey(i => i.AdboardId)
              .HasPrincipalKey(i => i.AdboardId);

            builder.Entity<DOOH.Server.Models.DOOHDB.Analytic>()
              .HasOne(i => i.Advertisement)
              .WithMany(i => i.Analytics)
              .HasForeignKey(i => i.AdvertisementId)
              .HasPrincipalKey(i => i.AdvertisementId);

            builder.Entity<DOOH.Server.Models.DOOHDB.Billing>()
              .HasOne(i => i.Analytic)
              .WithMany(i => i.Billings)
              .HasForeignKey(i => i.AnalyticId)
              .HasPrincipalKey(i => i.AnalyticId);

            builder.Entity<DOOH.Server.Models.DOOHDB.CampaignAdboard>()
              .HasOne(i => i.Adboard)
              .WithMany(i => i.CampaignAdboards)
              .HasForeignKey(i => i.AdboardId)
              .HasPrincipalKey(i => i.AdboardId);

            builder.Entity<DOOH.Server.Models.DOOHDB.CampaignAdboard>()
              .HasOne(i => i.Campaign)
              .WithMany(i => i.CampaignAdboards)
              .HasForeignKey(i => i.CampaignId)
              .HasPrincipalKey(i => i.CampaignId);

            builder.Entity<DOOH.Server.Models.DOOHDB.City>()
              .HasOne(i => i.State)
              .WithMany(i => i.Cities)
              .HasForeignKey(i => i.StateName)
              .HasPrincipalKey(i => i.StateName);

            builder.Entity<DOOH.Server.Models.DOOHDB.Company>()
              .HasOne(i => i.City)
              .WithMany(i => i.Companies)
              .HasForeignKey(i => i.CityName)
              .HasPrincipalKey(i => i.CityName);

            builder.Entity<DOOH.Server.Models.DOOHDB.Company>()
              .HasOne(i => i.Country)
              .WithMany(i => i.Companies)
              .HasForeignKey(i => i.CountryName)
              .HasPrincipalKey(i => i.CountryName);

            builder.Entity<DOOH.Server.Models.DOOHDB.Company>()
              .HasOne(i => i.State)
              .WithMany(i => i.Companies)
              .HasForeignKey(i => i.StateName)
              .HasPrincipalKey(i => i.StateName);

            builder.Entity<DOOH.Server.Models.DOOHDB.Display>()
              .HasOne(i => i.Brand)
              .WithMany(i => i.Displays)
              .HasForeignKey(i => i.BrandId)
              .HasPrincipalKey(i => i.BrandId);

            builder.Entity<DOOH.Server.Models.DOOHDB.Earning>()
              .HasOne(i => i.Adboard)
              .WithMany(i => i.Earnings)
              .HasForeignKey(i => i.AdboardId)
              .HasPrincipalKey(i => i.AdboardId);

            builder.Entity<DOOH.Server.Models.DOOHDB.Earning>()
              .HasOne(i => i.Analytic)
              .WithMany(i => i.Earnings)
              .HasForeignKey(i => i.AnalyticId)
              .HasPrincipalKey(i => i.AnalyticId);

            builder.Entity<DOOH.Server.Models.DOOHDB.Earning>()
              .HasOne(i => i.Provider)
              .WithMany(i => i.Earnings)
              .HasForeignKey(i => i.ProviderId)
              .HasPrincipalKey(i => i.ProviderId);

            builder.Entity<DOOH.Server.Models.DOOHDB.Motherboard>()
              .HasOne(i => i.Brand)
              .WithMany(i => i.Motherboards)
              .HasForeignKey(i => i.BrandId)
              .HasPrincipalKey(i => i.BrandId);

            builder.Entity<DOOH.Server.Models.DOOHDB.Provider>()
              .HasOne(i => i.City)
              .WithMany(i => i.Providers)
              .HasForeignKey(i => i.CityName)
              .HasPrincipalKey(i => i.CityName);

            builder.Entity<DOOH.Server.Models.DOOHDB.Provider>()
              .HasOne(i => i.Country)
              .WithMany(i => i.Providers)
              .HasForeignKey(i => i.CountryName)
              .HasPrincipalKey(i => i.CountryName);

            builder.Entity<DOOH.Server.Models.DOOHDB.Provider>()
              .HasOne(i => i.State)
              .WithMany(i => i.Providers)
              .HasForeignKey(i => i.StateName)
              .HasPrincipalKey(i => i.StateName);

            builder.Entity<DOOH.Server.Models.DOOHDB.State>()
              .HasOne(i => i.Country)
              .WithMany(i => i.States)
              .HasForeignKey(i => i.CountryName)
              .HasPrincipalKey(i => i.CountryName);

            builder.Entity<DOOH.Server.Models.DOOHDB.Tax>()
              .HasOne(i => i.Tax1)
              .WithMany(i => i.Taxes1)
              .HasForeignKey(i => i.ParentTaxId)
              .HasPrincipalKey(i => i.TaxId);

            builder.Entity<DOOH.Server.Models.DOOHDB.Adboard>()
              .Property(p => p.Latitude)
              .HasDefaultValueSql(@"((0.0))");

            builder.Entity<DOOH.Server.Models.DOOHDB.Adboard>()
              .Property(p => p.Longitude)
              .HasDefaultValueSql(@"((0.00))");

            builder.Entity<DOOH.Server.Models.DOOHDB.Adboard>()
              .Property(p => p.IsActive)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<DOOH.Server.Models.DOOHDB.Adboard>()
              .Property(p => p.CreatedAt)
              .HasDefaultValueSql(@"(sysdatetime())");

            builder.Entity<DOOH.Server.Models.DOOHDB.AdboardWifi>()
              .Property(p => p.CreatedAt)
              .HasDefaultValueSql(@"(sysdatetime())");

            builder.Entity<DOOH.Server.Models.DOOHDB.Advertisement>()
              .Property(p => p.CreatedAt)
              .HasDefaultValueSql(@"(sysdatetime())");

            builder.Entity<DOOH.Server.Models.DOOHDB.Analytic>()
              .Property(p => p.StoppedAt)
              .HasDefaultValueSql(@"(sysdatetime())");

            builder.Entity<DOOH.Server.Models.DOOHDB.Analytic>()
              .Property(p => p.UpdatedAt)
              .HasDefaultValueSql(@"(sysdatetime())");

            builder.Entity<DOOH.Server.Models.DOOHDB.Attachment>()
              .Property(p => p.CreatedAt)
              .HasDefaultValueSql(@"(sysdatetime())");

            builder.Entity<DOOH.Server.Models.DOOHDB.Campaign>()
              .Property(p => p.StartDate)
              .HasDefaultValueSql(@"(getdate())");

            builder.Entity<DOOH.Server.Models.DOOHDB.Campaign>()
              .Property(p => p.IsActive)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<DOOH.Server.Models.DOOHDB.Campaign>()
              .Property(p => p.IsSuspended)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<DOOH.Server.Models.DOOHDB.Campaign>()
              .Property(p => p.CreatedAt)
              .HasDefaultValueSql(@"(sysdatetime())");

            builder.Entity<DOOH.Server.Models.DOOHDB.Category>()
              .Property(p => p.Commission)
              .HasDefaultValueSql(@"((0.00))");

            builder.Entity<DOOH.Server.Models.DOOHDB.Earning>()
              .Property(p => p.TotalDuration)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<DOOH.Server.Models.DOOHDB.Faq>()
              .Property(p => p.UpdatedAt)
              .HasDefaultValueSql(@"(sysdatetime())");

            builder.Entity<DOOH.Server.Models.DOOHDB.Provider>()
              .Property(p => p.IsActive)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<DOOH.Server.Models.DOOHDB.Provider>()
              .Property(p => p.CreatedAt)
              .HasDefaultValueSql(@"(sysdatetime())");

            builder.Entity<DOOH.Server.Models.DOOHDB.Tax>()
              .Property(p => p.TaxRate)
              .HasDefaultValueSql(@"((0.00))");

            builder.Entity<DOOH.Server.Models.DOOHDB.Adboard>()
              .Property(p => p.CreatedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.Adboard>()
              .Property(p => p.UpdatedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.AdboardWifi>()
              .Property(p => p.ConnectedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.AdboardWifi>()
              .Property(p => p.CreatedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.AdboardWifi>()
              .Property(p => p.UpdatedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.Advertisement>()
              .Property(p => p.CreatedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.Advertisement>()
              .Property(p => p.UpdatedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.Advertisement>()
              .Property(p => p.VerifiedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.Analytic>()
              .Property(p => p.StartedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.Analytic>()
              .Property(p => p.StoppedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.Analytic>()
              .Property(p => p.UpdatedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.Attachment>()
              .Property(p => p.CreatedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.Attachment>()
              .Property(p => p.UpdatedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.Campaign>()
              .Property(p => p.CreatedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.Campaign>()
              .Property(p => p.UpdatedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.Faq>()
              .Property(p => p.UpdatedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.Provider>()
              .Property(p => p.CreatedAt)
              .HasColumnType("datetime");

            builder.Entity<DOOH.Server.Models.DOOHDB.Provider>()
              .Property(p => p.UpdatedAt)
              .HasColumnType("datetime");
            this.OnModelBuilding(builder);
        }

        public DbSet<DOOH.Server.Models.DOOHDB.Adboard> Adboards { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.AdboardImage> AdboardImages { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.AdboardNetwork> AdboardNetworks { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.AdboardWifi> AdboardWifis { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Advertisement> Advertisements { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Analytic> Analytics { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Attachment> Attachments { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Billing> Billings { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Brand> Brands { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Campaign> Campaigns { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.CampaignAdboard> CampaignAdboards { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Category> Categories { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.City> Cities { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Company> Companies { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Country> Countries { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Display> Displays { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Earning> Earnings { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Faq> Faqs { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Motherboard> Motherboards { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Page> Pages { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Provider> Providers { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.State> States { get; set; }

        public DbSet<DOOH.Server.Models.DOOHDB.Tax> Taxes { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}