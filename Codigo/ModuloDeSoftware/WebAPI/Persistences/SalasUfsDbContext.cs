using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public partial class SalasUfsDbContext : DbContext
    {
        private const string _dbName = "db_a8a8d4_dbsalas";
        public SalasUfsDbContext()
        {
        }

        public SalasUfsDbContext(DbContextOptions<SalasUfsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bloco> Bloco { get; set; }
        public virtual DbSet<Codigoinfravermelho> Codigoinfravermelho { get; set; }
        public virtual DbSet<Equipamento> Equipamento { get; set; }
        public virtual DbSet<Hardwaredesala> Hardwaredesala { get; set; }
        public virtual DbSet<Horariosala> Horariosala { get; set; }
        public virtual DbSet<Logrequest> Logrequest { get; set; }
        public virtual DbSet<Monitoramento> Monitoramento { get; set; }
        public virtual DbSet<Operacao> Operacao { get; set; }
        public virtual DbSet<Organizacao> Organizacao { get; set; }
        public virtual DbSet<Planejamento> Planejamento { get; set; }
        public virtual DbSet<Sala> Sala { get; set; }
        public virtual DbSet<Salaparticular> Salaparticular { get; set; }
        public virtual DbSet<Solicitacao> Solicitacao { get; set; }
        public virtual DbSet<Tipohardware> Tipohardware { get; set; }
        public virtual DbSet<Tipousuario> Tipousuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Usuarioorganizacao> Usuarioorganizacao { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bloco>(entity =>
            {
                entity.ToTable("bloco");

                entity.HasIndex(e => e.Organizacao)
                    .HasName("fk_Bloco_Organizacao1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Organizacao)
                    .HasColumnName("organizacao")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasColumnName("titulo")
                    .HasMaxLength(100);

                entity.HasOne(d => d.OrganizacaoNavigation)
                    .WithMany(p => p.Bloco)
                    .HasForeignKey(d => d.Organizacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Bloco_Organizacao1");
            });

            modelBuilder.Entity<Codigoinfravermelho>(entity =>
            {
                entity.ToTable("codigoinfravermelho");

                entity.HasIndex(e => e.Equipamento)
                    .HasName("fk_CodigoInfravermelho_Equipamento1_idx");

                entity.HasIndex(e => e.Operacao)
                    .HasName("fk_CodigoInfravermelho_Operacao1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasColumnName("codigo")
                    .HasColumnType("mediumtext");

                entity.Property(e => e.Equipamento).HasColumnName("equipamento");

                entity.Property(e => e.Operacao).HasColumnName("operacao");

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
                entity.ToTable("equipamento");

                entity.HasIndex(e => e.HardwareDeSala)
                    .HasName("fk_Equipamento_HardwareDeSala1_idx");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_Equipamento_Sala1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .HasColumnName("descricao")
                    .HasMaxLength(1000);

                entity.Property(e => e.HardwareDeSala)
                    .HasColumnName("hardwareDeSala")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Marca)
                    .IsRequired()
                    .HasColumnName("marca")
                    .HasMaxLength(100);

                entity.Property(e => e.Modelo)
                    .IsRequired()
                    .HasColumnName("modelo")
                    .HasMaxLength(200);

                entity.Property(e => e.Sala)
                    .HasColumnName("sala")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.TipoEquipamento)
                    .IsRequired()
                    .HasColumnName("tipoEquipamento")
                    .HasColumnType("enum('CONDICIONADOR','LUZES')")
                    .HasDefaultValueSql("'CONDICIONADOR'");

                entity.HasOne(d => d.HardwareDeSalaNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.HardwareDeSala)
                    .HasConstraintName("fk_Equipamento_HardwareDeSala1");

                entity.HasOne(d => d.SalaNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.Sala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Equipamento_Sala1");
            });

            modelBuilder.Entity<Hardwaredesala>(entity =>
            {
                entity.ToTable("hardwaredesala");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_Hardware_Sala1_idx");

                entity.HasIndex(e => e.TipoHardware)
                    .HasName("fk_HardwareDeSala_TipoHardware1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Ip)
                    .HasColumnName("ip")
                    .HasMaxLength(15);

                entity.Property(e => e.Mac)
                    .IsRequired()
                    .HasColumnName("mac")
                    .HasMaxLength(45);

                entity.Property(e => e.Registrado).HasColumnName("registrado");

                entity.Property(e => e.Sala)
                    .HasColumnName("sala")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.TipoHardware)
                    .HasColumnName("tipoHardware")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasMaxLength(400);

                entity.Property(e => e.Uuid)
                    .HasColumnName("uuid")
                    .HasMaxLength(75);

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
                entity.ToTable("horariosala");

                entity.HasIndex(e => e.Planejamento)
                    .HasName("fk_HorarioSala_Planejamento1_idx");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_HorarioSala_Sala1_idx");

                entity.HasIndex(e => e.Usuario)
                    .HasName("fk_HorarioSala_Usuario1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Data).HasColumnName("data");

                entity.Property(e => e.HorarioFim).HasColumnName("horarioFim");

                entity.Property(e => e.HorarioInicio).HasColumnName("horarioInicio");

                entity.Property(e => e.Objetivo)
                    .IsRequired()
                    .HasColumnName("objetivo")
                    .HasMaxLength(500);

                entity.Property(e => e.Planejamento)
                    .HasColumnName("planejamento")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Sala)
                    .HasColumnName("sala")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Situacao)
                    .IsRequired()
                    .HasColumnName("situacao")
                    .HasColumnType("enum('PENDENTE','APROVADA','REPROVADA','CANCELADA')")
                    .HasDefaultValueSql("'APROVADA'");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasColumnType("int unsigned");

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

            modelBuilder.Entity<Logrequest>(entity =>
            {
                entity.ToTable("logrequest");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Input)
                    .IsRequired()
                    .HasColumnName("input");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("ip")
                    .HasMaxLength(150);

                entity.Property(e => e.Origin)
                    .IsRequired()
                    .HasColumnName("origin")
                    .HasColumnType("enum('API','ESP32')")
                    .HasDefaultValueSql("'API'");

                entity.Property(e => e.StatusCode)
                    .IsRequired()
                    .HasColumnName("statusCode")
                    .HasMaxLength(50);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Monitoramento>(entity =>
            {
                entity.ToTable("monitoramento");

                entity.HasIndex(e => e.Equipamento)
                    .HasName("fk_Equipamento_Id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Equipamento).HasColumnName("equipamento");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.HasOne(d => d.EquipamentoNavigation)
                    .WithMany(p => p.Monitoramento)
                    .HasForeignKey(d => d.Equipamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Equipamento_Id");
            });

            modelBuilder.Entity<Operacao>(entity =>
            {
                entity.ToTable("operacao");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .HasColumnName("descricao")
                    .HasMaxLength(200);

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasColumnName("titulo")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Organizacao>(entity =>
            {
                entity.ToTable("organizacao");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Cnpj)
                    .IsRequired()
                    .HasColumnName("cnpj")
                    .HasMaxLength(15);

                entity.Property(e => e.RazaoSocial)
                    .IsRequired()
                    .HasColumnName("razaoSocial")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Planejamento>(entity =>
            {
                entity.ToTable("planejamento");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_Planejamento_Sala1_idx");

                entity.HasIndex(e => e.Usuario)
                    .HasName("fk_Planejamento_Usuario1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.DataFim)
                    .HasColumnName("dataFim")
                    .HasColumnType("date");

                entity.Property(e => e.DataInicio)
                    .HasColumnName("dataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.DiaSemana)
                    .IsRequired()
                    .HasColumnName("diaSemana")
                    .HasColumnType("enum('SEG','TER','QUA','QUI','SEX','SAB','DOM')");

                entity.Property(e => e.HorarioFim).HasColumnName("horarioFim");

                entity.Property(e => e.HorarioInicio).HasColumnName("horarioInicio");

                entity.Property(e => e.Objetivo)
                    .IsRequired()
                    .HasColumnName("objetivo")
                    .HasMaxLength(500);

                entity.Property(e => e.Sala)
                    .HasColumnName("sala")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasColumnType("int unsigned");

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
                entity.ToTable("sala");

                entity.HasIndex(e => e.Bloco)
                    .HasName("fk_Sala_Bloco1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Bloco)
                    .HasColumnName("bloco")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasColumnName("titulo")
                    .HasMaxLength(100);

                entity.HasOne(d => d.BlocoNavigation)
                    .WithMany(p => p.Sala)
                    .HasForeignKey(d => d.Bloco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Sala_Bloco1");
            });

            modelBuilder.Entity<Salaparticular>(entity =>
            {
                entity.ToTable("salaparticular");

                entity.HasIndex(e => e.Sala)
                    .HasName("fk_MinhaSala_Sala1_idx");

                entity.HasIndex(e => e.Usuario)
                    .HasName("fk_MinhaSala_Usuario1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Sala)
                    .HasColumnName("sala")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasColumnType("int unsigned");

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

            modelBuilder.Entity<Solicitacao>(entity =>
            {
                entity.ToTable("solicitacao");

                entity.HasIndex(e => e.IdHardware)
                    .HasName("fk_Solicitacao_Hardware1");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.IdHardware)
                    .HasColumnName("idHardware")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Payload)
                    .IsRequired()
                    .HasColumnName("payload")
                    .HasColumnType("json");


                entity.Property(e => e.TipoSolicitacao)
                    .IsRequired()
                    .HasColumnName("tipoSolicitacao")
                    .HasColumnType("enum('MONITORAMENTO_LUZES','MONITORAMENTO_AR_CONDICIONADO','ATUALIZAR_RESERVAS')")
                    .HasDefaultValueSql("ATUALIZAR_RESERVAS");

                entity.HasOne(d => d.IdHardwareNavigation)
                    .WithMany(p => p.Solicitacao)
                    .HasForeignKey(d => d.IdHardware)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Solicitacao_Hardware1");
            });

            modelBuilder.Entity<Tipohardware>(entity =>
            {
                entity.ToTable("tipohardware");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Tipousuario>(entity =>
            {
                entity.ToTable("tipousuario");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");

                entity.HasIndex(e => e.Cpf)
                    .HasName("Cpf_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.TipoUsuario)
                    .HasName("fk_Usuario_TipoUsuario1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Cpf)
                    .IsRequired()
                    .HasColumnName("cpf")
                    .HasMaxLength(11);

                entity.Property(e => e.DataNascimento)
                    .HasColumnName("dataNascimento")
                    .HasColumnType("date");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(45);

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasColumnName("senha")
                    .HasMaxLength(100);

                entity.Property(e => e.TipoUsuario)
                    .HasColumnName("tipoUsuario")
                    .HasColumnType("int unsigned");

                entity.HasOne(d => d.TipoUsuarioNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.TipoUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Usuario_TipoUsuario1");
            });

            modelBuilder.Entity<Usuarioorganizacao>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Organizacao, e.Usuario })
                    .HasName("PRIMARY");

                entity.ToTable("usuarioorganizacao");

                entity.HasIndex(e => e.Organizacao)
                    .HasName("fk_Organizacao_has_Usuario_Organizacao1_idx");

                entity.HasIndex(e => e.Usuario)
                    .HasName("fk_Organizacao_has_Usuario_Usuario1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Organizacao)
                    .HasColumnName("organizacao")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasColumnType("int unsigned");

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
