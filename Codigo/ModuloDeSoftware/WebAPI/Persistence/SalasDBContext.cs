using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public partial class SalasDBContext : DbContext
{
    public SalasDBContext()
    {
    }

    public SalasDBContext(DbContextOptions<SalasDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bloco> Blocos { get; set; }

    public virtual DbSet<Codigoinfravermelho> Codigoinfravermelhos { get; set; }

    public virtual DbSet<Equipamento> Equipamentos { get; set; }

    public virtual DbSet<Hardwaredesala> Hardwaredesalas { get; set; }

    public virtual DbSet<Horariosala> Horariosalas { get; set; }

    public virtual DbSet<Logrequest> Logrequests { get; set; }

    public virtual DbSet<Monitoramento> Monitoramentos { get; set; }

    public virtual DbSet<Operacao> Operacaos { get; set; }

    public virtual DbSet<Organizacao> Organizacaos { get; set; }

    public virtual DbSet<Planejamento> Planejamentos { get; set; }

    public virtual DbSet<Sala> Salas { get; set; }

    public virtual DbSet<Salaparticular> Salaparticulars { get; set; }

    public virtual DbSet<Tipohardware> Tipohardwares { get; set; }

    public virtual DbSet<Tipousuario> Tipousuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Usuarioorganizacao> Usuarioorganizacaos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=123456;database=automacaosalas");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bloco>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("bloco");

            entity.HasIndex(e => e.Organizacao, "fk_Bloco_Organizacao1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Organizacao).HasColumnName("organizacao");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .HasColumnName("titulo");

            entity.HasOne(d => d.OrganizacaoNavigation).WithMany(p => p.Blocos)
                .HasForeignKey(d => d.Organizacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Bloco_Organizacao1");
        });

        modelBuilder.Entity<Codigoinfravermelho>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("codigoinfravermelho");

            entity.HasIndex(e => e.Equipamento, "fk_CodigoInfravermelho_Equipamento1_idx");

            entity.HasIndex(e => e.Operacao, "fk_CodigoInfravermelho_Operacao1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codigo)
                .HasColumnType("mediumtext")
                .HasColumnName("codigo");
            entity.Property(e => e.Equipamento).HasColumnName("equipamento");
            entity.Property(e => e.Operacao).HasColumnName("operacao");

            entity.HasOne(d => d.EquipamentoNavigation).WithMany(p => p.Codigoinfravermelhos)
                .HasForeignKey(d => d.Equipamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_CodigoInfravermelho_Equipamento1");

            entity.HasOne(d => d.OperacaoNavigation).WithMany(p => p.Codigoinfravermelhos)
                .HasForeignKey(d => d.Operacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_CodigoInfravermelho_Operacao1");
        });

        modelBuilder.Entity<Equipamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("equipamento");

            entity.HasIndex(e => e.HardwareDeSala, "fk_Equipamento_HardwareDeSala1_idx");

            entity.HasIndex(e => e.Sala, "fk_Equipamento_Sala1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(1000)
                .HasColumnName("descricao");
            entity.Property(e => e.HardwareDeSala).HasColumnName("hardwareDeSala");
            entity.Property(e => e.Marca)
                .HasMaxLength(100)
                .HasColumnName("marca");
            entity.Property(e => e.Modelo)
                .HasMaxLength(200)
                .HasColumnName("modelo");
            entity.Property(e => e.Sala).HasColumnName("sala");
            entity.Property(e => e.TipoEquipamento)
                .HasDefaultValueSql("'CONDICIONADOR'")
                .HasColumnType("enum('CONDICIONADOR','LUZES')")
                .HasColumnName("tipoEquipamento");

            entity.HasOne(d => d.HardwareDeSalaNavigation).WithMany(p => p.Equipamentos)
                .HasForeignKey(d => d.HardwareDeSala)
                .HasConstraintName("fk_Equipamento_HardwareDeSala1");

            entity.HasOne(d => d.SalaNavigation).WithMany(p => p.Equipamentos)
                .HasForeignKey(d => d.Sala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Equipamento_Sala1");
        });

        modelBuilder.Entity<Hardwaredesala>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("hardwaredesala");

            entity.HasIndex(e => e.TipoHardware, "fk_HardwareDeSala_TipoHardware1_idx");

            entity.HasIndex(e => e.Sala, "fk_Hardware_Sala1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .HasColumnName("ip");
            entity.Property(e => e.Mac)
                .HasMaxLength(45)
                .HasColumnName("mac");
            entity.Property(e => e.Registrado).HasColumnName("registrado");
            entity.Property(e => e.Sala).HasColumnName("sala");
            entity.Property(e => e.TipoHardware).HasColumnName("tipoHardware");
            entity.Property(e => e.Token)
                .HasMaxLength(400)
                .HasColumnName("token");
            entity.Property(e => e.Uuid)
                .HasMaxLength(75)
                .HasColumnName("uuid");

            entity.HasOne(d => d.SalaNavigation).WithMany(p => p.Hardwaredesalas)
                .HasForeignKey(d => d.Sala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Hardware_Sala1");

            entity.HasOne(d => d.TipoHardwareNavigation).WithMany(p => p.Hardwaredesalas)
                .HasForeignKey(d => d.TipoHardware)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_HardwareDeSala_TipoHardware1");
        });

        modelBuilder.Entity<Horariosala>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

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
                .HasMaxLength(500)
                .HasColumnName("objetivo");
            entity.Property(e => e.Planejamento).HasColumnName("planejamento");
            entity.Property(e => e.Sala).HasColumnName("sala");
            entity.Property(e => e.Situacao)
                .HasDefaultValueSql("'APROVADA'")
                .HasColumnType("enum('PENDENTE','APROVADA','REPROVADA','CANCELADA')")
                .HasColumnName("situacao");
            entity.Property(e => e.Usuario).HasColumnName("usuario");

            entity.HasOne(d => d.PlanejamentoNavigation).WithMany(p => p.Horariosalas)
                .HasForeignKey(d => d.Planejamento)
                .HasConstraintName("fk_HorarioSala_Planejamento1");

            entity.HasOne(d => d.SalaNavigation).WithMany(p => p.Horariosalas)
                .HasForeignKey(d => d.Sala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_HorarioSala_Sala1");

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Horariosalas)
                .HasForeignKey(d => d.Usuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_HorarioSala_Usuario1");
        });

        modelBuilder.Entity<Logrequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("logrequest");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.Input)
                .HasColumnType("text")
                .HasColumnName("input");
            entity.Property(e => e.Ip)
                .HasMaxLength(150)
                .HasColumnName("ip");
            entity.Property(e => e.StatusCode)
                .HasMaxLength(45)
                .HasColumnName("statusCode");
            entity.Property(e => e.Url)
                .HasMaxLength(250)
                .HasColumnName("url");
        });

        modelBuilder.Entity<Monitoramento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("monitoramento");

            entity.HasIndex(e => e.Sala, "fk_Monitoramento_Sala2_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArCondicionado).HasColumnName("arCondicionado");
            entity.Property(e => e.Luzes).HasColumnName("luzes");
            entity.Property(e => e.Sala).HasColumnName("sala");

            entity.HasOne(d => d.SalaNavigation).WithMany(p => p.Monitoramentos)
                .HasForeignKey(d => d.Sala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Monitoramento_Sala2");
        });

        modelBuilder.Entity<Operacao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("operacao");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(200)
                .HasColumnName("descricao");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .HasColumnName("titulo");
        });

        modelBuilder.Entity<Organizacao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("organizacao");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cnpj)
                .HasMaxLength(15)
                .HasColumnName("cnpj");
            entity.Property(e => e.RazaoSocial)
                .HasMaxLength(45)
                .HasColumnName("razaoSocial");
        });

        modelBuilder.Entity<Planejamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

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
                .HasColumnType("enum('SEG','TER','QUA','QUI','SEX','SAB','DOM')")
                .HasColumnName("diaSemana");
            entity.Property(e => e.HorarioFim)
                .HasColumnType("time")
                .HasColumnName("horarioFim");
            entity.Property(e => e.HorarioInicio)
                .HasColumnType("time")
                .HasColumnName("horarioInicio");
            entity.Property(e => e.Objetivo)
                .HasMaxLength(500)
                .HasColumnName("objetivo");
            entity.Property(e => e.Sala).HasColumnName("sala");
            entity.Property(e => e.Usuario).HasColumnName("usuario");

            entity.HasOne(d => d.SalaNavigation).WithMany(p => p.Planejamentos)
                .HasForeignKey(d => d.Sala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Planejamento_Sala1");

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Planejamentos)
                .HasForeignKey(d => d.Usuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Planejamento_Usuario1");
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sala");

            entity.HasIndex(e => e.Bloco, "fk_Sala_Bloco1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bloco).HasColumnName("bloco");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .HasColumnName("titulo");

            entity.HasOne(d => d.BlocoNavigation).WithMany(p => p.Salas)
                .HasForeignKey(d => d.Bloco)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Sala_Bloco1");
        });

        modelBuilder.Entity<Salaparticular>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("salaparticular");

            entity.HasIndex(e => e.Sala, "fk_MinhaSala_Sala1_idx");

            entity.HasIndex(e => e.Usuario, "fk_MinhaSala_Usuario1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Sala).HasColumnName("sala");
            entity.Property(e => e.Usuario).HasColumnName("usuario");

            entity.HasOne(d => d.SalaNavigation).WithMany(p => p.Salaparticulars)
                .HasForeignKey(d => d.Sala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_MinhaSala_Sala1");

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Salaparticulars)
                .HasForeignKey(d => d.Usuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_MinhaSala_Usuario1");
        });

        modelBuilder.Entity<Tipohardware>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tipohardware");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(45)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<Tipousuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tipousuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(45)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Cpf, "Cpf_UNIQUE").IsUnique();

            entity.HasIndex(e => e.TipoUsuario, "fk_Usuario_TipoUsuario1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .HasColumnName("cpf");
            entity.Property(e => e.DataNascimento)
                .HasColumnType("date")
                .HasColumnName("dataNascimento");
            entity.Property(e => e.Nome)
                .HasMaxLength(45)
                .HasColumnName("nome");
            entity.Property(e => e.Senha)
                .HasMaxLength(100)
                .HasColumnName("senha");
            entity.Property(e => e.TipoUsuario).HasColumnName("tipoUsuario");

            entity.HasOne(d => d.TipoUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.TipoUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Usuario_TipoUsuario1");
        });

        modelBuilder.Entity<Usuarioorganizacao>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Organizacao, e.Usuario }).HasName("PRIMARY");

            entity.ToTable("usuarioorganizacao");

            entity.HasIndex(e => e.Organizacao, "fk_Organizacao_has_Usuario_Organizacao1_idx");

            entity.HasIndex(e => e.Usuario, "fk_Organizacao_has_Usuario_Usuario1_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Organizacao).HasColumnName("organizacao");
            entity.Property(e => e.Usuario).HasColumnName("usuario");

            entity.HasOne(d => d.OrganizacaoNavigation).WithMany(p => p.Usuarioorganizacaos)
                .HasForeignKey(d => d.Organizacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Organizacao_has_Usuario_Organizacao1");

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Usuarioorganizacaos)
                .HasForeignKey(d => d.Usuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Organizacao_has_Usuario_Usuario1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
