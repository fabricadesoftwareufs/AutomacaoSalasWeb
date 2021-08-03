using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Persistence.str_db
{
    public partial class str_dbContext : DbContext
    {
        public str_dbContext()
        {
        }

        public str_dbContext(DbContextOptions<str_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Hardwaredesala> Hardwaredesala { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=1234;database=str_db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hardwaredesala>(entity =>
            {
                entity.ToTable("hardwaredesala", "str_db");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_Hardware_Sala1_idx");

                entity.HasIndex(e => e.TipoHardware)
                    .HasName("fk_HardwareDeSala_TipoHardware1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Ip)
                    .HasColumnName("ip")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Mac)
                    .IsRequired()
                    .HasColumnName("mac")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Sala)
                    .HasColumnName("sala")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.TipoHardware)
                    .HasColumnName("tipoHardware")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasColumnName("token")
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Uuid)
                    .HasColumnName("uuid")
                    .HasMaxLength(75)
                    .IsUnicode(false);
            });
        }
    }
}
