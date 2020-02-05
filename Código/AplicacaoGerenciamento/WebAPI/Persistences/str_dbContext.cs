using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Persistences
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

        public virtual DbSet<Bloco> Bloco { get; set; }
        public virtual DbSet<Hardware> Hardware { get; set; }
        public virtual DbSet<Horariosala> Horariosala { get; set; }
        public virtual DbSet<Minhasala> Minhasala { get; set; }
        public virtual DbSet<Organizacao> Organizacao { get; set; }
        public virtual DbSet<Planejamento> Planejamento { get; set; }
        public virtual DbSet<Sala> Sala { get; set; }
        public virtual DbSet<Tipohardware> Tipohardware { get; set; }
        public virtual DbSet<Tipousuario> Tipousuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioOrganizacoes> UsuarioOrganizacoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=123456;database=str_db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bloco>(entity =>
            {
                entity.ToTable("bloco", "str_db");

                entity.HasIndex(e => e.Organizacao)
                    .HasName("fk_Bloco_Organizacao1_idx");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Organizacao).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.OrganizacaoNavigation)
                    .WithMany(p => p.Bloco)
                    .HasForeignKey(d => d.Organizacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Bloco_Organizacao1");
            });

            modelBuilder.Entity<Hardware>(entity =>
            {
                entity.ToTable("hardware", "str_db");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_Hardware_Sala1_idx");

                entity.HasIndex(e => e.TipoHardware)
                    .HasName("fk_Hardware_TipoHardware1_idx");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Mac)
                    .IsRequired()
                    .HasColumnName("MAC")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Sala).HasColumnType("int(10) unsigned");

                entity.Property(e => e.TipoHardware).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.SalaNavigation)
                    .WithMany(p => p.Hardware)
                    .HasForeignKey(d => d.Sala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Hardware_Sala1");

                entity.HasOne(d => d.TipoHardwareNavigation)
                    .WithMany(p => p.Hardware)
                    .HasForeignKey(d => d.TipoHardware)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Hardware_TipoHardware1");
            });

            modelBuilder.Entity<Horariosala>(entity =>
            {
                entity.ToTable("horariosala", "str_db");

                entity.HasIndex(e => e.SalaId)
                    .HasName("fk_HorarioSala_Sala1_idx");

                entity.HasIndex(e => e.UsuarioId)
                    .HasName("fk_HorarioSala_Usuario1_idx");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Objetivo)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SalaId)
                    .HasColumnName("Sala_Id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Situacao)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioId)
                    .HasColumnName("Usuario_Id")
                    .HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.Sala)
                    .WithMany(p => p.Horariosala)
                    .HasForeignKey(d => d.SalaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_HorarioSala_Sala1");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Horariosala)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_HorarioSala_Usuario1");
            });

            modelBuilder.Entity<Minhasala>(entity =>
            {
                entity.HasKey(e => e.IdMinhaSala);

                entity.ToTable("minhasala", "str_db");

                entity.HasIndex(e => e.SalaId)
                    .HasName("fk_SalasPessoais_Sala1_idx");

                entity.HasIndex(e => e.UsuarioId)
                    .HasName("fk_SalasPessoais_Usuario1_idx");

                entity.Property(e => e.IdMinhaSala)
                    .HasColumnName("idMinhaSala")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.SalaId)
                    .HasColumnName("Sala_Id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.UsuarioId)
                    .HasColumnName("Usuario_Id")
                    .HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.Sala)
                    .WithMany(p => p.Minhasala)
                    .HasForeignKey(d => d.SalaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SalasPessoais_Sala1");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Minhasala)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SalasPessoais_Usuario1");
            });

            modelBuilder.Entity<Organizacao>(entity =>
            {
                entity.ToTable("organizacao", "str_db");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Cnpj)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.RazaoSocial)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Planejamento>(entity =>
            {
                entity.HasKey(e => e.IdPlanejamento);

                entity.ToTable("planejamento", "str_db");

                entity.HasIndex(e => e.SalaId)
                    .HasName("fk_Planejamento_Sala1_idx");

                entity.HasIndex(e => e.UsuarioId)
                    .HasName("fk_Planejamento_Usuario1_idx");

                entity.Property(e => e.IdPlanejamento)
                    .HasColumnName("idPlanejamento")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.DataFim)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.DataInicio)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.DiaSemana)
                    .IsRequired()
                    .HasColumnType("enum('SEG','TER','QUA','QUI','SEX','SAB','DOM')");

                entity.Property(e => e.Objetivo)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SalaId)
                    .HasColumnName("Sala_Id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.UsuarioId)
                    .HasColumnName("Usuario_Id")
                    .HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.Sala)
                    .WithMany(p => p.Planejamento)
                    .HasForeignKey(d => d.SalaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Planejamento_Sala1");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Planejamento)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Planejamento_Usuario1");
            });

            modelBuilder.Entity<Sala>(entity =>
            {
                entity.ToTable("sala", "str_db");

                entity.HasIndex(e => e.Bloco)
                    .HasName("fk_Sala_Bloco1_idx");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Bloco).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.HasOne(d => d.BlocoNavigation)
                    .WithMany(p => p.Sala)
                    .HasForeignKey(d => d.Bloco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Sala_Bloco1");
            });

            modelBuilder.Entity<Tipohardware>(entity =>
            {
                entity.ToTable("tipohardware", "str_db");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tipousuario>(entity =>
            {
                entity.ToTable("tipousuario", "str_db");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario", "str_db");

                entity.HasIndex(e => e.Cpf)
                    .HasName("Cpf_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.TipoUsuario)
                    .HasName("fk_Usuario_TipoUsuario1_idx");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Cpf)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.DataNascimento).HasColumnType("date");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.TipoUsuario).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.TipoUsuarioNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.TipoUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Usuario_TipoUsuario1");
            });

            modelBuilder.Entity<UsuarioOrganizacoes>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Organizacao, e.Usuario });

                entity.ToTable("usuario_organizacoes", "str_db");

                entity.HasIndex(e => e.Organizacao)
                    .HasName("fk_Organizacao_has_Usuario_Organizacao1_idx");

                entity.HasIndex(e => e.Usuario)
                    .HasName("fk_Organizacao_has_Usuario_Usuario1_idx");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10) unsigned")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Organizacao).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Usuario).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.OrganizacaoNavigation)
                    .WithMany(p => p.UsuarioOrganizacoes)
                    .HasForeignKey(d => d.Organizacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Organizacao_has_Usuario_Organizacao1");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.UsuarioOrganizacoes)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Organizacao_has_Usuario_Usuario1");
            });
        }
    }
}
