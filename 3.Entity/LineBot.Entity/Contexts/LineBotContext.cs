using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using LineBot.Entity.Models;

#nullable disable

namespace LineBot.Entity.Contexts
{
    public partial class LineBotContext : DbContext
    {
        public LineBotContext()
        {
        }

        public LineBotContext(DbContextOptions<LineBotContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<PersonalLiability> PersonalLiabilities { get; set; }
        public virtual DbSet<RentFixedFee> RentFixedFees { get; set; }
        public virtual DbSet<Sysparam> Sysparams { get; set; }
        public virtual DbSet<UtilityFee> UtilityFees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=ec2-52-45-238-24.compute-1.amazonaws.com;Database=d8eb2jm5fse32r;Username=oeibbdsxltadus;Password=150d3b1c00b6541644a8711bb8f06dfb24b971f2e689c6f5c2491a5e0ab63a75;Sslmode=Require;Trust Server Certificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");

                entity.HasComment("人員資料表");

                entity.Property(e => e.PersonId)
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .HasComment("人員唯一ID");

                entity.Property(e => e.PersonName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("人員名稱");
            });

            modelBuilder.Entity<PersonalLiability>(entity =>
            {
                entity.HasKey(e => e.SeqNo)
                    .HasName("PersonalLiabilities_pkey");

                entity.HasComment("人員負債表");

                entity.Property(e => e.SeqNo).HasComment("流水號");

                entity.Property(e => e.ClosedOn).HasComment("結清債務時間");

                entity.Property(e => e.CreateOn).HasComment("產生債務時間");

                entity.Property(e => e.CreditorId).HasComment("債主ID");

                entity.Property(e => e.DebtorId).HasComment("欠債人ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(400)
                    .HasComment("項目名稱");

                entity.Property(e => e.Fee)
                    .HasPrecision(6, 1)
                    .HasComment("金額");

                entity.Property(e => e.IsClosed).HasComment("是否已結清");
            });

            modelBuilder.Entity<RentFixedFee>(entity =>
            {
                entity.HasKey(e => e.SeqNo)
                    .HasName("RentFixedFee_pkey");

                entity.ToTable("RentFixedFee");

                entity.HasComment("租屋固定支出表");

                entity.Property(e => e.SeqNo).HasComment("流水號");

                entity.Property(e => e.Fee)
                    .HasPrecision(6, 1)
                    .HasComment("費用");

                entity.Property(e => e.ItemId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("項目 ID");

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasComment("項目名稱");
            });

            modelBuilder.Entity<Sysparam>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Sysparam");

                entity.HasComment("系統參數");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("描述");

                entity.Property(e => e.GroupId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("參數群組代碼");

                entity.Property(e => e.ItemId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("參數 Id");

                entity.Property(e => e.ItemValue)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasComment("參數值");
            });

            modelBuilder.Entity<UtilityFee>(entity =>
            {
                entity.HasKey(e => e.SeqNo)
                    .HasName("UtilityFee_pkey");

                entity.ToTable("UtilityFee");

                entity.HasComment("租屋公共費用表");

                entity.Property(e => e.SeqNo).HasComment("流水號");

                entity.Property(e => e.ClosedOn).HasComment("結清費用的時間");

                entity.Property(e => e.CreateOn).HasComment("產生費用的時間");

                entity.Property(e => e.Fee)
                    .HasPrecision(6, 1)
                    .HasComment("費用");

                entity.Property(e => e.IsClosed).HasComment("是否已結清");

                entity.Property(e => e.ItemId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("項目 ID");

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasComment("項目名稱");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
