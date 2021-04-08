using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Persistence
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
        public virtual DbSet<Codigoinfravermelho> Codigoinfravermelho { get; set; }
        public virtual DbSet<Equipamento> Equipamento { get; set; }
        public virtual DbSet<Hardwaredebloco> Hardwaredebloco { get; set; }
        public virtual DbSet<Hardwaredesala> Hardwaredesala { get; set; }
        public virtual DbSet<Horariosala> Horariosala { get; set; }
        public virtual DbSet<Monitoramento> Monitoramento { get; set; }
        public virtual DbSet<Operacao> Operacao { get; set; }
        public virtual DbSet<Organizacao> Organizacao { get; set; }
        public virtual DbSet<Planejamento> Planejamento { get; set; }
        public virtual DbSet<Sala> Sala { get; set; }
        public virtual DbSet<Salaparticular> Salaparticular { get; set; }
        public virtual DbSet<Tipohardware> Tipohardware { get; set; }
        public virtual DbSet<Tipousuario> Tipousuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Usuarioorganizacao> Usuarioorganizacao { get; set; }

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
            modelBuilder.Entity<Bloco>(entity =>
            {
                entity.ToTable("bloco", "str_db");

                entity.HasIndex(e => e.Organizacao)
                    .HasName("fk_Bloco_Organizacao1_idx");

                entity.Property(e => e.Id).HasColumnType("int unsigned");

                entity.Property(e => e.Organizacao).HasColumnType("int unsigned");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.OrganizacaoNavigation)
                    .WithMany(p => p.Bloco)
                    .HasForeignKey(d => d.Organizacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Bloco_Organizacao1");
            });

            modelBuilder.Entity<Codigoinfravermelho>(entity =>
            {
                entity.ToTable("codigoinfravermelho", "str_db");

                entity.HasIndex(e => e.Equipamento)
                    .HasName("fk_CodigoInfravermelho_Equipamento1_idx");

                entity.HasIndex(e => e.Operacao)
                    .HasName("fk_CodigoInfravermelho_Operacao1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasColumnType("mediumtext");

                entity.HasOne(d => d.EquipamentoNavigation)
                    .WithMany(p => p.Codigoinfravermelho)
                    .HasForeignKey(d => d.Equipamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CodigoInfravermelho_Equipamento1");

                entity.HasOne(d => d.OperacaoNavigation)
                    .WithMany(p => p.Codigoinfravermelho)
                    .HasForeignKey(d => d.Operacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CodigoInfravermelho_Operacao1");
            });

            modelBuilder.Entity<Equipamento>(entity =>
            {
                entity.ToTable("equipamento", "str_db");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_Equipamento_Sala1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Marca)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Modelo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Sala).HasColumnType("int unsigned");

                entity.Property(e => e.TipoEquipamento)
                    .IsRequired()
                    .HasColumnType("enum('CONDICIONADOR','LUZES')")
                    .HasDefaultValueSql("_utf8mb4\\'CONDICIONADOR\\'");

                entity.HasOne(d => d.SalaNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.Sala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Equipamento_Sala1");
            });

            modelBuilder.Entity<Hardwaredebloco>(entity =>
            {
                entity.ToTable("hardwaredebloco", "str_db");

                entity.HasIndex(e => e.Bloco)
                    .HasName("fk_HardwareDeBloco_Bloco1_idx");

                entity.HasIndex(e => e.Id)
                    .HasName("idHardwareDeBloco_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.TipoHardware)
                    .HasName("fk_HardwareDeBloco_TipoHardware1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bloco).HasColumnType("int unsigned");

                entity.Property(e => e.Mac)
                    .IsRequired()
                    .HasColumnName("MAC")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.TipoHardware).HasColumnType("int unsigned");

                entity.HasOne(d => d.BlocoNavigation)
                    .WithMany(p => p.Hardwaredebloco)
                    .HasForeignKey(d => d.Bloco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_HardwareDeBloco_Bloco1");

                entity.HasOne(d => d.TipoHardwareNavigation)
                    .WithMany(p => p.Hardwaredebloco)
                    .HasForeignKey(d => d.TipoHardware)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_HardwareDeBloco_TipoHardware1");
            });

            modelBuilder.Entity<Hardwaredesala>(entity =>
            {
                entity.ToTable("hardwaredesala", "str_db");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_Hardware_Sala1_idx");

                entity.HasIndex(e => e.TipoHardware)
                    .HasName("fk_HardwareDeSala_TipoHardware1_idx");

                entity.Property(e => e.Id).HasColumnType("int unsigned");

                entity.Property(e => e.Ip)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Mac)
                    .IsRequired()
                    .HasColumnName("MAC")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Sala).HasColumnType("int unsigned");

                entity.Property(e => e.TipoHardware).HasColumnType("int unsigned");

                entity.HasOne(d => d.SalaNavigation)
                    .WithMany(p => p.Hardwaredesala)
                    .HasForeignKey(d => d.Sala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Hardware_Sala1");

                entity.HasOne(d => d.TipoHardwareNavigation)
                    .WithMany(p => p.Hardwaredesala)
                    .HasForeignKey(d => d.TipoHardware)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_HardwareDeSala_TipoHardware1");
            });

            modelBuilder.Entity<Horariosala>(entity =>
            {
                entity.ToTable("horariosala", "str_db");

                entity.HasIndex(e => e.Planejamento)
                    .HasName("fk_HorarioSala_Planejamento1_idx");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_HorarioSala_Sala1_idx");

                entity.HasIndex(e => e.Usuario)
                    .HasName("fk_HorarioSala_Usuario1_idx");

                entity.Property(e => e.Id).HasColumnType("int unsigned");

                entity.Property(e => e.Objetivo)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Planejamento).HasColumnType("int unsigned");

                entity.Property(e => e.Sala).HasColumnType("int unsigned");

                entity.Property(e => e.Situacao)
                    .IsRequired()
                    .HasColumnType("enum('PENDENTE','APROVADA','REPROVADA','CANCELADA')")
                    .HasDefaultValueSql("APROVADA");

                entity.Property(e => e.Usuario).HasColumnType("int unsigned");

                entity.HasOne(d => d.PlanejamentoNavigation)
                    .WithMany(p => p.Horariosala)
                    .HasForeignKey(d => d.Planejamento)
                    .HasConstraintName("fk_HorarioSala_Planejamento1");

                entity.HasOne(d => d.SalaNavigation)
                    .WithMany(p => p.Horariosala)
                    .HasForeignKey(d => d.Sala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_HorarioSala_Sala1");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.Horariosala)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_HorarioSala_Usuario1");
            });

            modelBuilder.Entity<Monitoramento>(entity =>
            {
                entity.ToTable("monitoramento", "str_db");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_Sala_Id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Sala).HasColumnType("int unsigned");

                entity.HasOne(d => d.SalaNavigation)
                    .WithMany(p => p.Monitoramento)
                    .HasForeignKey(d => d.Sala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Sala_Id");
            });

            modelBuilder.Entity<Operacao>(entity =>
            {
                entity.ToTable("operacao", "str_db");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Organizacao>(entity =>
            {
                entity.ToTable("organizacao", "str_db");

                entity.Property(e => e.Id).HasColumnType("int unsigned");

                entity.Property(e => e.Cnpj)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RazaoSocial)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Planejamento>(entity =>
            {
                entity.ToTable("planejamento", "str_db");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_Planejamento_Sala1_idx");

                entity.HasIndex(e => e.Usuario)
                    .HasName("fk_Planejamento_Usuario1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.DataFim).HasColumnType("date");

                entity.Property(e => e.DataInicio).HasColumnType("date");

                entity.Property(e => e.DiaSemana)
                    .IsRequired()
                    .HasColumnType("enum('SEG','TER','QUA','QUI','SEX','SAB','DOM')");

                entity.Property(e => e.Objetivo)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Sala).HasColumnType("int unsigned");

                entity.Property(e => e.Usuario).HasColumnType("int unsigned");

                entity.HasOne(d => d.SalaNavigation)
                    .WithMany(p => p.Planejamento)
                    .HasForeignKey(d => d.Sala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Planejamento_Sala1");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.Planejamento)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Planejamento_Usuario1");
            });

            modelBuilder.Entity<Sala>(entity =>
            {
                entity.ToTable("sala", "str_db");

                entity.HasIndex(e => e.Bloco)
                    .HasName("fk_Sala_Bloco1_idx");

                entity.Property(e => e.Id).HasColumnType("int unsigned");

                entity.Property(e => e.Bloco).HasColumnType("int unsigned");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.BlocoNavigation)
                    .WithMany(p => p.Sala)
                    .HasForeignKey(d => d.Bloco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Sala_Bloco1");
            });

            modelBuilder.Entity<Salaparticular>(entity =>
            {
                entity.ToTable("salaparticular", "str_db");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_MinhaSala_Sala1_idx");

                entity.HasIndex(e => e.Usuario)
                    .HasName("fk_MinhaSala_Usuario1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Sala).HasColumnType("int unsigned");

                entity.Property(e => e.Usuario).HasColumnType("int unsigned");

                entity.HasOne(d => d.SalaNavigation)
                    .WithMany(p => p.Salaparticular)
                    .HasForeignKey(d => d.Sala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_MinhaSala_Sala1");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.Salaparticular)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_MinhaSala_Usuario1");
            });

            modelBuilder.Entity<Tipohardware>(entity =>
            {
                entity.ToTable("tipohardware", "str_db");

                entity.Property(e => e.Id).HasColumnType("int unsigned");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tipousuario>(entity =>
            {
                entity.ToTable("tipousuario", "str_db");

                entity.Property(e => e.Id).HasColumnType("int unsigned");

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

                entity.Property(e => e.Id).HasColumnType("int unsigned");

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
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoUsuario).HasColumnType("int unsigned");

                entity.HasOne(d => d.TipoUsuarioNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.TipoUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Usuario_TipoUsuario1");
            });

            modelBuilder.Entity<Usuarioorganizacao>(entity =>
            {
                entity.ToTable("usuarioorganizacao", "str_db");

                entity.HasIndex(e => e.Organizacao)
                    .HasName("fk_Organizacao_has_Usuario_Organizacao1_idx");

                entity.HasIndex(e => e.Usuario)
                    .HasName("fk_Organizacao_has_Usuario_Usuario1_idx");

                entity.Property(e => e.Id).HasColumnType("int unsigned");

                entity.Property(e => e.Organizacao).HasColumnType("int unsigned");

                entity.Property(e => e.Usuario).HasColumnType("int unsigned");

                entity.HasOne(d => d.OrganizacaoNavigation)
                    .WithMany(p => p.Usuarioorganizacao)
                    .HasForeignKey(d => d.Organizacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Organizacao_has_Usuario_Organizacao1");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.Usuarioorganizacao)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Organizacao_has_Usuario_Usuario1");
            });
        }
    }
}
