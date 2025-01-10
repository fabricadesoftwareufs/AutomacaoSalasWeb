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

    public virtual DbSet<Conexaointernet> Conexaointernets { get; set; }

    public virtual DbSet<Conexaointernetsala> Conexaointernetsalas { get; set; }

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

    public virtual DbSet<Solicitacao> Solicitacaos { get; set; }

    public virtual DbSet<Tipohardware> Tipohardwares { get; set; }

    public virtual DbSet<Tipousuario> Tipousuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Usuarioorganizacao> Usuarioorganizacaos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bloco>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("bloco");

            entity.HasIndex(e => e.IdOrganizacao, "fk_Bloco_Organizacao1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdOrganizacao).HasColumnName("idOrganizacao");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdOrganizacaoNavigation).WithMany(p => p.Blocos)
                .HasForeignKey(d => d.IdOrganizacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Bloco_Organizacao1");
        });

        modelBuilder.Entity<Codigoinfravermelho>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("codigoinfravermelho");

            entity.HasIndex(e => e.IdEquipamento, "fk_CodigoInfravermelho_Equipamento1_idx");

            entity.HasIndex(e => e.IdOperacao, "fk_CodigoInfravermelho_Operacao1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codigo)
                .HasColumnType("mediumtext")
                .HasColumnName("codigo");
            entity.Property(e => e.IdEquipamento).HasColumnName("idEquipamento");
            entity.Property(e => e.IdOperacao).HasColumnName("idOperacao");

            entity.HasOne(d => d.IdEquipamentoNavigation).WithMany(p => p.Codigoinfravermelhos)
                .HasForeignKey(d => d.IdEquipamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_CodigoInfravermelho_Equipamento1");

            entity.HasOne(d => d.IdOperacaoNavigation).WithMany(p => p.Codigoinfravermelhos)
                .HasForeignKey(d => d.IdOperacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_CodigoInfravermelho_Operacao1");
        });

        modelBuilder.Entity<Conexaointernet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("conexaointernet");

            entity.HasIndex(e => e.IdBloco, "fk_ConexaoInternet_Bloco1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdBloco).HasColumnName("idBloco");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Senha)
                .HasMaxLength(100)
                .HasColumnName("senha");

            entity.HasOne(d => d.IdBlocoNavigation).WithMany(p => p.Conexaointernets)
                .HasForeignKey(d => d.IdBloco)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ConexaoInternet_Bloco1");
        });

        modelBuilder.Entity<Conexaointernetsala>(entity =>
        {
            entity.HasKey(e => new { e.IdConexaoInternet, e.IdSala }).HasName("PRIMARY");

            entity.ToTable("conexaointernetsala");

            entity.HasIndex(e => e.IdConexaoInternet, "fk_ConexaoInternet_has_Sala_ConexaoInternet1_idx");

            entity.HasIndex(e => e.IdSala, "fk_ConexaoInternet_has_Sala_Sala1_idx");

            entity.Property(e => e.IdConexaoInternet).HasColumnName("idConexaoInternet");
            entity.Property(e => e.IdSala).HasColumnName("idSala");
            entity.Property(e => e.Prioridade).HasColumnName("prioridade");

            entity.HasOne(d => d.IdConexaoInternetNavigation).WithMany(p => p.Conexaointernetsalas)
                .HasForeignKey(d => d.IdConexaoInternet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ConexaoInternet_has_Sala_ConexaoInternet1");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Conexaointernetsalas)
                .HasForeignKey(d => d.IdSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ConexaoInternet_has_Sala_Sala1");
        });

        modelBuilder.Entity<Equipamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("equipamento");

            entity.HasIndex(e => e.IdHardwareDeSala, "fk_Equipamento_HardwareDeSala1_idx");

            entity.HasIndex(e => e.IdSala, "fk_Equipamento_Sala1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(1000)
                .HasColumnName("descricao");
            entity.Property(e => e.IdHardwareDeSala).HasColumnName("idHardwareDeSala");
            entity.Property(e => e.IdSala).HasColumnName("idSala");
            entity.Property(e => e.Marca)
                .HasMaxLength(100)
                .HasColumnName("marca");
            entity.Property(e => e.Modelo)
                .HasMaxLength(200)
                .HasColumnName("modelo");
            entity.Property(e => e.TipoEquipamento)
                .HasDefaultValueSql("'CONDICIONADOR'")
                .HasColumnType("enum('CONDICIONADOR','LUZES')")
                .HasColumnName("tipoEquipamento");

            entity.HasOne(d => d.IdHardwareDeSalaNavigation).WithMany(p => p.Equipamentos)
                .HasForeignKey(d => d.IdHardwareDeSala)
                .HasConstraintName("fk_Equipamento_HardwareDeSala1");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Equipamentos)
                .HasForeignKey(d => d.IdSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Equipamento_Sala1");
        });

        modelBuilder.Entity<Hardwaredesala>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("hardwaredesala");

            entity.HasIndex(e => e.IdTipoHardware, "fk_HardwareDeSala_TipoHardware1_idx");

            entity.HasIndex(e => e.IdSala, "fk_Hardware_Sala1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdSala).HasColumnName("idSala");
            entity.Property(e => e.IdTipoHardware).HasColumnName("idTipoHardware");
            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .HasColumnName("ip");
            entity.Property(e => e.Mac)
                .HasMaxLength(45)
                .HasColumnName("mac");
            entity.Property(e => e.Registrado).HasColumnName("registrado");
            entity.Property(e => e.Token)
                .HasMaxLength(400)
                .HasColumnName("token");
            entity.Property(e => e.Uuid)
                .HasMaxLength(75)
                .HasColumnName("uuid");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Hardwaredesalas)
                .HasForeignKey(d => d.IdSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Hardware_Sala1");

            entity.HasOne(d => d.IdTipoHardwareNavigation).WithMany(p => p.Hardwaredesalas)
                .HasForeignKey(d => d.IdTipoHardware)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_HardwareDeSala_TipoHardware1");
        });

        modelBuilder.Entity<Horariosala>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("horariosala");

            entity.HasIndex(e => e.IdPlanejamento, "fk_HorarioSala_Planejamento1_idx");

            entity.HasIndex(e => e.IdSala, "fk_HorarioSala_Sala1_idx");

            entity.HasIndex(e => e.IdUsuario, "fk_HorarioSala_Usuario1_idx");

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
            entity.Property(e => e.IdPlanejamento).HasColumnName("idPlanejamento");
            entity.Property(e => e.IdSala).HasColumnName("idSala");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Objetivo)
                .HasMaxLength(500)
                .HasColumnName("objetivo");
            entity.Property(e => e.Situacao)
                .HasDefaultValueSql("'APROVADA'")
                .HasColumnType("enum('PENDENTE','APROVADA','REPROVADA','CANCELADA')")
                .HasColumnName("situacao");

            entity.HasOne(d => d.IdPlanejamentoNavigation).WithMany(p => p.Horariosalas)
                .HasForeignKey(d => d.IdPlanejamento)
                .HasConstraintName("fk_HorarioSala_Planejamento1");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Horariosalas)
                .HasForeignKey(d => d.IdSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_HorarioSala_Sala1");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Horariosalas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_HorarioSala_Usuario1");
        });

        modelBuilder.Entity<Logrequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("logrequest");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Input)
                .HasColumnType("text")
                .HasColumnName("input");
            entity.Property(e => e.Ip)
                .HasMaxLength(150)
                .HasColumnName("ip");
            entity.Property(e => e.Origin)
                .HasDefaultValueSql("'API'")
                .HasColumnType("enum('API','ESP32')")
                .HasColumnName("origin");
            entity.Property(e => e.StatusCode)
                .HasMaxLength(50)
                .HasColumnName("statusCode");
            entity.Property(e => e.Url)
                .HasMaxLength(250)
                .HasColumnName("url");
        });

        modelBuilder.Entity<Monitoramento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("monitoramento");

            entity.HasIndex(e => e.IdEquipamento, "fk_monitoramento_Equipamento1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.IdEquipamento).HasColumnName("idEquipamento");

            entity.HasOne(d => d.IdEquipamentoNavigation).WithMany(p => p.Monitoramentos)
                .HasForeignKey(d => d.IdEquipamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_monitoramento_Equipamento1");
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

            entity.HasIndex(e => e.IdSala, "fk_Planejamento_Sala1_idx");

            entity.HasIndex(e => e.IdUsuario, "fk_Planejamento_Usuario1_idx");

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
            entity.Property(e => e.IdSala).HasColumnName("idSala");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Objetivo)
                .HasMaxLength(500)
                .HasColumnName("objetivo");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Planejamentos)
                .HasForeignKey(d => d.IdSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Planejamento_Sala1");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Planejamentos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Planejamento_Usuario1");
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sala");

            entity.HasIndex(e => e.IdBloco, "fk_Sala_Bloco1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdBlocoNavigation).WithMany(p => p.Salas)
                .HasForeignKey(d => d.IdBloco)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Sala_Bloco1");
        });

        modelBuilder.Entity<Salaparticular>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("salaparticular");

            entity.HasIndex(e => e.IdSala, "fk_MinhaSala_Sala1_idx");

            entity.HasIndex(e => e.IdUsuario, "fk_MinhaSala_Usuario1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdSala).HasColumnName("idSala");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Salaparticulars)
                .HasForeignKey(d => d.IdSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_MinhaSala_Sala1");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Salaparticulars)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_MinhaSala_Usuario1");
        });

        modelBuilder.Entity<Solicitacao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("solicitacao");

            entity.HasIndex(e => e.IdHardware, "fk_solicitacao_HardwareDeSala1_idx");

            entity.HasIndex(e => e.IdHardwareAtuador, "fk_solicitacao_HardwareDeSala2_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataFinalizacao)
                .HasColumnType("datetime")
                .HasColumnName("dataFinalizacao");
            entity.Property(e => e.DataSolicitacao)
                .HasColumnType("datetime")
                .HasColumnName("dataSolicitacao");
            entity.Property(e => e.IdHardware).HasColumnName("idHardware");
            entity.Property(e => e.IdHardwareAtuador).HasColumnName("idHardwareAtuador");
            entity.Property(e => e.Payload)
                .HasColumnType("json")
                .HasColumnName("payload");
            entity.Property(e => e.TipoSolicitacao)
                .HasDefaultValueSql("'ATUALIZAR_RESERVAS'")
                .HasColumnType("enum('MONITORAMENTO_LUZES','MONITORAMENTO_AR_CONDICIONADO','ATUALIZAR_RESERVAS')")
                .HasColumnName("tipoSolicitacao");

            entity.HasOne(d => d.IdHardwareNavigation).WithMany(p => p.SolicitacaoIdHardwareNavigations)
                .HasForeignKey(d => d.IdHardware)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_solicitacao_HardwareDeSala1");

            entity.HasOne(d => d.IdHardwareAtuadorNavigation).WithMany(p => p.SolicitacaoIdHardwareAtuadorNavigations)
                .HasForeignKey(d => d.IdHardwareAtuador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_solicitacao_HardwareDeSala2");
        });

        modelBuilder.Entity<Tipohardware>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tipohardware");

            entity.HasIndex(e => e.IdOrganizacao, "fk_TipoHardware_Organizacao1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(45)
                .HasColumnName("descricao");
            entity.Property(e => e.IdOrganizacao).HasColumnName("idOrganizacao");

            entity.HasOne(d => d.IdOrganizacaoNavigation).WithMany(p => p.Tipohardwares)
                .HasForeignKey(d => d.IdOrganizacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_TipoHardware_Organizacao1");
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
        });

        modelBuilder.Entity<Usuarioorganizacao>(entity =>
        {
            entity.HasKey(e => new { e.IdOrganizacao, e.IdUsuario }).HasName("PRIMARY");

            entity.ToTable("usuarioorganizacao");

            entity.HasIndex(e => e.IdOrganizacao, "fk_Organizacao_has_Usuario_Organizacao1_idx");

            entity.HasIndex(e => e.IdUsuario, "fk_Organizacao_has_Usuario_Usuario1_idx");

            entity.HasIndex(e => e.IdTipoUsuario, "fk_UsuarioOrganizacao_TipoUsuario1_idx");

            entity.Property(e => e.IdOrganizacao).HasColumnName("idOrganizacao");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.DataCadastro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("dataCadastro");
            entity.Property(e => e.IdTipoUsuario).HasColumnName("idTipoUsuario");

            entity.HasOne(d => d.IdOrganizacaoNavigation).WithMany(p => p.Usuarioorganizacaos)
                .HasForeignKey(d => d.IdOrganizacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Organizacao_has_Usuario_Organizacao1");

            entity.HasOne(d => d.IdTipoUsuarioNavigation).WithMany(p => p.Usuarioorganizacaos)
                .HasForeignKey(d => d.IdTipoUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_UsuarioOrganizacao_TipoUsuario1");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Usuarioorganizacaos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Organizacao_has_Usuario_Usuario1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
