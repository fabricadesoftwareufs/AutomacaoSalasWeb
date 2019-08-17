using Microsoft.EntityFrameworkCore;

namespace Persistences
{
    public partial class ContextDb : DbContext
    {
        public ContextDb()
        {
        }

        public ContextDb(DbContextOptions<ContextDb> options)
            : base(options)
        {
        }

        public virtual DbSet<Bloco> Bloco { get; set; }
        public virtual DbSet<Disciplina> Disciplina { get; set; }
        public virtual DbSet<Hardware> Hardware { get; set; }
        public virtual DbSet<Horariosala> Horariosala { get; set; }
        public virtual DbSet<Organizacao> Organizacao { get; set; }
        public virtual DbSet<Sala> Sala { get; set; }
        public virtual DbSet<Tipohardware> Tipohardware { get; set; }
        public virtual DbSet<Tipousuario> Tipousuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioOrganizacoes> UsuarioOrganizacoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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

            modelBuilder.Entity<Disciplina>(entity =>
            {
                entity.ToTable("disciplina", "str_db");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);
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

                entity.HasIndex(e => e.Disciplina)
                    .HasName("fk_HorarioSala_Disciplina1_idx");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_HorarioSala_Sala1_idx");

                entity.HasIndex(e => e.Usuario)
                    .HasName("fk_HorarioSala_Usuario1_idx");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Data).HasColumnType("date");

                entity.Property(e => e.Disciplina).HasColumnType("int(10) unsigned");

                entity.Property(e => e.QtdAlunos).HasColumnType("int(11)");

                entity.Property(e => e.Sala).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Turno)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.DisciplinaNavigation)
                    .WithMany(p => p.Horariosala)
                    .HasForeignKey(d => d.Disciplina)
                    .HasConstraintName("fk_HorarioSala_Disciplina1");

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
