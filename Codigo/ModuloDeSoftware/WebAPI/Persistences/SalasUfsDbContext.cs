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

                entity.HasIndex(e => e.Organizacao, "fk_Bloco_Organizacao1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Organizacao).HasColumnName("organizacao");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasColumnName("titulo")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.OrganizacaoNavigation)
                    .WithMany(p => p.Bloco)
                    .HasForeignKey(d => d.Organizacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Bloco_Organizacao1");
            });

            modelBuilder.Entity<Codigoinfravermelho>(entity =>
            {
                entity.ToTable("codigoinfravermelho");

                entity.HasIndex(e => e.Equipamento, "fk_CodigoInfravermelho_Equipamento1_idx");

                entity.HasIndex(e => e.Operacao, "fk_CodigoInfravermelho_Operacao1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasColumnType("mediumtext")
                    .HasColumnName("codigo")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

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

                entity.HasIndex(e => e.HardwareDeSala, "fk_Equipamento_HardwareDeSala1_idx");

                entity.HasIndex(e => e.Sala, "fk_Equipamento_Sala1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .HasColumnType("varchar(1000)")
                    .HasColumnName("descricao")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.HardwareDeSala).HasColumnName("hardwareDeSala");

                entity.Property(e => e.Marca)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasColumnName("marca")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Modelo)
                    .IsRequired()
                    .HasColumnType("varchar(200)")
                    .HasColumnName("modelo")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Sala).HasColumnName("sala");

                entity.Property(e => e.TipoEquipamento)
                    .IsRequired()
                    .HasColumnType("enum('CONDICIONADOR','LUZES')")
                    .HasColumnName("tipoEquipamento")
                    .HasDefaultValueSql("'CONDICIONADOR'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

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

                entity.HasIndex(e => e.TipoHardware, "fk_HardwareDeSala_TipoHardware1_idx");

                entity.HasIndex(e => e.Sala, "fk_Hardware_Sala1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ip)
                    .HasColumnType("varchar(15)")
                    .HasColumnName("ip")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Mac)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasColumnName("mac")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Registrado).HasColumnName("registrado");

                entity.Property(e => e.Sala).HasColumnName("sala");

                entity.Property(e => e.TipoHardware).HasColumnName("tipoHardware");

                entity.Property(e => e.Token)
                    .HasColumnType("varchar(400)")
                    .HasColumnName("token")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Uuid)
                    .HasColumnType("varchar(75)")
                    .HasColumnName("uuid")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

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

                entity.HasIndex(e => e.Planejamento, "fk_HorarioSala_Planejamento1_idx");

                entity.HasIndex(e => e.Sala, "fk_HorarioSala_Sala1_idx");

                entity.HasIndex(e => e.Usuario, "fk_HorarioSala_Usuario1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("data");

                entity.Property(e => e.HorarioFim)
                    .HasColumnType("time")
                    .HasColumnName("horarioFim");

                entity.Property(e => e.HorarioInicio)
                    .HasColumnType("time")
                    .HasColumnName("horarioInicio");

                entity.Property(e => e.Objetivo)
                    .IsRequired()
                    .HasColumnType("varchar(500)")
                    .HasColumnName("objetivo")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Planejamento).HasColumnName("planejamento");

                entity.Property(e => e.Sala).HasColumnName("sala");

                entity.Property(e => e.Situacao)
                    .IsRequired()
                    .HasColumnType("enum('PENDENTE','APROVADA','REPROVADA','CANCELADA')")
                    .HasColumnName("situacao")
                    .HasDefaultValueSql("'APROVADA'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Usuario).HasColumnName("usuario");

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

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Input)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("input")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnType("varchar(150)")
                    .HasColumnName("ip")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Origin)
                    .IsRequired()
                    .HasColumnType("enum('API','ESP32')")
                    .HasColumnName("origin")
                    .HasDefaultValueSql("'API'")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.StatusCode)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasColumnName("statusCode")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnType("varchar(250)")
                    .HasColumnName("url")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Monitoramento>(entity =>
            {
                entity.ToTable("monitoramento");

                entity.HasIndex(e => e.Equipamento, "fk_Equipamento_Id");

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
                    .HasColumnType("varchar(200)")
                    .HasColumnName("descricao")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasColumnName("titulo")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Organizacao>(entity =>
            {
                entity.ToTable("organizacao");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cnpj)
                    .IsRequired()
                    .HasColumnType("varchar(15)")
                    .HasColumnName("cnpj")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.RazaoSocial)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasColumnName("razaoSocial")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Planejamento>(entity =>
            {
                entity.ToTable("planejamento");

                entity.HasIndex(e => e.Sala, "fk_Planejamento_Sala1_idx");

                entity.HasIndex(e => e.Usuario, "fk_Planejamento_Usuario1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DataFim)
                    .HasColumnType("date")
                    .HasColumnName("dataFim");

                entity.Property(e => e.DataInicio)
                    .HasColumnType("date")
                    .HasColumnName("dataInicio");

                entity.Property(e => e.DiaSemana)
                    .IsRequired()
                    .HasColumnType("enum('SEG','TER','QUA','QUI','SEX','SAB','DOM')")
                    .HasColumnName("diaSemana")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.HorarioFim)
                    .HasColumnType("time")
                    .HasColumnName("horarioFim");

                entity.Property(e => e.HorarioInicio)
                    .HasColumnType("time")
                    .HasColumnName("horarioInicio");

                entity.Property(e => e.Objetivo)
                    .IsRequired()
                    .HasColumnType("varchar(500)")
                    .HasColumnName("objetivo")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Sala).HasColumnName("sala");

                entity.Property(e => e.Usuario).HasColumnName("usuario");

                entity.HasOne(d => d.SalaNavigation)
                    .WithMany(p => p.Planejamento )
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

                entity.HasIndex(e => e.Bloco, "fk_Sala_Bloco1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bloco).HasColumnName("bloco");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasColumnName("titulo")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.BlocoNavigation)
                    .WithMany(p => p.Sala)
                    .HasForeignKey(d => d.Bloco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Sala_Bloco1");
            });

            modelBuilder.Entity<Salaparticular>(entity =>
            {
                entity.ToTable("salaparticular");

                entity.HasIndex(e => e.Sala, "fk_MinhaSala_Sala1_idx");

                entity.HasIndex(e => e.Usuario, "fk_MinhaSala_Usuario1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Sala).HasColumnName("sala");

                entity.Property(e => e.Usuario).HasColumnName("usuario");

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

                entity.HasIndex(e => e.IdHardware, "fk_Solicitacao_Hardware1");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DataFinalizacao)
                    .HasColumnType("datetime")
                    .HasColumnName("dataFinalizacao");

                entity.Property(e => e.DataSolicitacao)
                    .HasColumnType("datetime")
                    .HasColumnName("dataSolicitacao");

                entity.Property(e => e.IdHardware).HasColumnName("idHardware");

                entity.Property(e => e.TipoSolicitacao)
                    .HasColumnType("enum('MONITORAMENTO_LUZES','MONITORAMENTO_AR_CONDICIONADO','ATUALIZAR_RESERVAS')")
                    .HasColumnName("tipoSolicitacao")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");


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

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasColumnName("descricao")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Tipousuario>(entity =>
            {
                entity.ToTable("tipousuario");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasColumnName("descricao")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");

                entity.HasIndex(e => e.Cpf, "Cpf_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.TipoUsuario, "fk_Usuario_TipoUsuario1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cpf)
                    .IsRequired()
                    .HasColumnType("varchar(11)")
                    .HasColumnName("cpf")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DataNascimento)
                    .HasColumnType("date")
                    .HasColumnName("dataNascimento");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasColumnName("nome")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasColumnName("senha")
                    .HasCharSet("utf8mb3")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TipoUsuario).HasColumnName("tipoUsuario");

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

                entity.HasIndex(e => e.Organizacao, "fk_Organizacao_has_Usuario_Organizacao1_idx");

                entity.HasIndex(e => e.Usuario, "fk_Organizacao_has_Usuario_Usuario1_idx");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Organizacao).HasColumnName("organizacao");

                entity.Property(e => e.Usuario).HasColumnName("usuario");

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
