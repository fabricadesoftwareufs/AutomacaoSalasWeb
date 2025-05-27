-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema automacaosalas
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `automacaosalas` ;

-- -----------------------------------------------------
-- Schema automacaosalas
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `automacaosalas` DEFAULT CHARACTER SET utf8 ;
USE `automacaosalas` ;

-- -----------------------------------------------------
-- Table `automacaosalas`.`Usuario`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`Usuario` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`Usuario` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `cpf` VARCHAR(11) NOT NULL,
  `nome` VARCHAR(45) NOT NULL,
  `dataNascimento` DATE NULL,
  `senha` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `Cpf_UNIQUE` (`cpf` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`Organizacao`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`Organizacao` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`Organizacao` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `cnpj` VARCHAR(15) NOT NULL,
  `razaoSocial` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`Bloco`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`Bloco` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`Bloco` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idOrganizacao` INT UNSIGNED NOT NULL,
  `titulo` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Bloco_Organizacao1_idx` (`idOrganizacao` ASC) VISIBLE,
  CONSTRAINT `fk_Bloco_Organizacao1`
    FOREIGN KEY (`idOrganizacao`)
    REFERENCES `automacaosalas`.`Organizacao` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`Sala`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`Sala` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`Sala` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `titulo` VARCHAR(100) NOT NULL,
  `IdBloco` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Sala_Bloco1_idx` (`IdBloco` ASC) VISIBLE,
  CONSTRAINT `fk_Sala_Bloco1`
    FOREIGN KEY (`IdBloco`)
    REFERENCES `automacaosalas`.`Bloco` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`Planejamento`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`Planejamento` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`Planejamento` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `dataInicio` DATE NOT NULL,
  `dataFim` DATE NOT NULL,
  `horarioInicio` TIME NOT NULL,
  `horarioFim` TIME NOT NULL,
  `diaSemana` ENUM('SEG', 'TER', 'QUA', 'QUI', 'SEX', 'SAB', 'DOM') NOT NULL,
  `objetivo` VARCHAR(500) NOT NULL,
  `idUsuario` INT UNSIGNED NOT NULL,
  `idSala` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Planejamento_Usuario1_idx` (`idUsuario` ASC) VISIBLE,
  INDEX `fk_Planejamento_Sala1_idx` (`idSala` ASC) VISIBLE,
  CONSTRAINT `fk_Planejamento_Usuario1`
    FOREIGN KEY (`idUsuario`)
    REFERENCES `automacaosalas`.`Usuario` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Planejamento_Sala1`
    FOREIGN KEY (`idSala`)
    REFERENCES `automacaosalas`.`Sala` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`HorarioSala`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`HorarioSala` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`HorarioSala` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `data` DATETIME NOT NULL,
  `horarioInicio` TIME NOT NULL,
  `horarioFim` TIME NOT NULL,
  `situacao` ENUM('PENDENTE', 'APROVADA', 'REPROVADA', 'CANCELADA') NOT NULL DEFAULT 'APROVADA',
  `objetivo` VARCHAR(500) NOT NULL,
  `idUsuario` INT UNSIGNED NOT NULL,
  `idSala` INT UNSIGNED NOT NULL,
  `idPlanejamento` INT UNSIGNED NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_HorarioSala_Usuario1_idx` (`idUsuario` ASC) VISIBLE,
  INDEX `fk_HorarioSala_Sala1_idx` (`idSala` ASC) VISIBLE,
  INDEX `fk_HorarioSala_Planejamento1_idx` (`idPlanejamento` ASC) VISIBLE,
  CONSTRAINT `fk_HorarioSala_Usuario1`
    FOREIGN KEY (`idUsuario`)
    REFERENCES `automacaosalas`.`Usuario` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_HorarioSala_Sala1`
    FOREIGN KEY (`idSala`)
    REFERENCES `automacaosalas`.`Sala` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_HorarioSala_Planejamento1`
    FOREIGN KEY (`idPlanejamento`)
    REFERENCES `automacaosalas`.`Planejamento` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`TipoHardware`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`TipoHardware` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`TipoHardware` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `descricao` VARCHAR(45) NOT NULL,
  `idOrganizacao` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_TipoHardware_Organizacao1_idx` (`idOrganizacao` ASC) VISIBLE,
  CONSTRAINT `fk_TipoHardware_Organizacao1`
    FOREIGN KEY (`idOrganizacao`)
    REFERENCES `automacaosalas`.`Organizacao` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`HardwareDeSala`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`HardwareDeSala` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`HardwareDeSala` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `mac` VARCHAR(45) NOT NULL,
  `idSala` INT UNSIGNED NOT NULL,
  `idTipoHardware` INT UNSIGNED NOT NULL,
  `ip` VARCHAR(15) NULL,
  `uuid` VARCHAR(75) NULL,
  `token` VARCHAR(400) NOT NULL,
  `registrado` TINYINT NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  INDEX `fk_Hardware_Sala1_idx` (`idSala` ASC) VISIBLE,
  INDEX `fk_HardwareDeSala_TipoHardware1_idx` (`idTipoHardware` ASC) VISIBLE,
  CONSTRAINT `fk_Hardware_Sala1`
    FOREIGN KEY (`idSala`)
    REFERENCES `automacaosalas`.`Sala` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_HardwareDeSala_TipoHardware1`
    FOREIGN KEY (`idTipoHardware`)
    REFERENCES `automacaosalas`.`TipoHardware` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`TipoUsuario`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`TipoUsuario` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`TipoUsuario` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `descricao` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`UsuarioOrganizacao`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`UsuarioOrganizacao` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`UsuarioOrganizacao` (
  `idOrganizacao` INT UNSIGNED NOT NULL,
  `idUsuario` INT UNSIGNED NOT NULL,
  `idTipoUsuario` INT UNSIGNED NOT NULL,
  `dataCadastro` DATETIME NOT NULL DEFAULT NOW(),
  PRIMARY KEY (`idOrganizacao`, `idUsuario`),
  INDEX `fk_Organizacao_has_Usuario_Usuario1_idx` (`idUsuario` ASC) VISIBLE,
  INDEX `fk_Organizacao_has_Usuario_Organizacao1_idx` (`idOrganizacao` ASC) VISIBLE,
  INDEX `fk_UsuarioOrganizacao_TipoUsuario1_idx` (`idTipoUsuario` ASC) VISIBLE,
  CONSTRAINT `fk_Organizacao_has_Usuario_Organizacao1`
    FOREIGN KEY (`idOrganizacao`)
    REFERENCES `automacaosalas`.`Organizacao` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Organizacao_has_Usuario_Usuario1`
    FOREIGN KEY (`idUsuario`)
    REFERENCES `automacaosalas`.`Usuario` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_UsuarioOrganizacao_TipoUsuario1`
    FOREIGN KEY (`idTipoUsuario`)
    REFERENCES `automacaosalas`.`TipoUsuario` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`SalaParticular`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`SalaParticular` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`SalaParticular` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idUsuario` INT UNSIGNED NOT NULL,
  `idSala` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_MinhaSala_Usuario1_idx` (`idUsuario` ASC) VISIBLE,
  INDEX `fk_MinhaSala_Sala1_idx` (`idSala` ASC) VISIBLE,
  CONSTRAINT `fk_MinhaSala_Usuario1`
    FOREIGN KEY (`idUsuario`)
    REFERENCES `automacaosalas`.`Usuario` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_MinhaSala_Sala1`
    FOREIGN KEY (`idSala`)
    REFERENCES `automacaosalas`.`Sala` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`MarcaEquipamento`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`MarcaEquipamento` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`MarcaEquipamento` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`ModeloEquipamento`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`ModeloEquipamento` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`ModeloEquipamento` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(300) NOT NULL,
  `idMarcaEquipamento` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_ModeloEquipamento_MarcaEquipamento1_idx` (`idMarcaEquipamento` ASC) VISIBLE,
  CONSTRAINT `fk_ModeloEquipamento_MarcaEquipamento1`
    FOREIGN KEY (`idMarcaEquipamento`)
    REFERENCES `automacaosalas`.`MarcaEquipamento` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`Equipamento`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`Equipamento` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`Equipamento` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `idModeloEquipamento` INT UNSIGNED NULL,
  `idSala` INT UNSIGNED NOT NULL,
  `descricao` VARCHAR(1000) NULL,
  `tipoEquipamento` ENUM('CONDICIONADOR', 'LUZES') NOT NULL DEFAULT 'CONDICIONADOR',
  `idHardwareDeSala` INT UNSIGNED NULL,
  `status` ENUM('L', 'D') NOT NULL DEFAULT 'D' COMMENT 'L - LIGADO\nD - DESLIGADO',
  `temperatura` TINYINT NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  INDEX `fk_Equipamento_Sala1_idx` (`idSala` ASC) VISIBLE,
  INDEX `fk_Equipamento_HardwareDeSala1_idx` (`idHardwareDeSala` ASC) VISIBLE,
  INDEX `fk_Equipamento_ModeloEquipamento1_idx` (`idModeloEquipamento` ASC) VISIBLE,
  CONSTRAINT `fk_Equipamento_Sala1`
    FOREIGN KEY (`idSala`)
    REFERENCES `automacaosalas`.`Sala` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Equipamento_HardwareDeSala1`
    FOREIGN KEY (`idHardwareDeSala`)
    REFERENCES `automacaosalas`.`HardwareDeSala` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Equipamento_ModeloEquipamento1`
    FOREIGN KEY (`idModeloEquipamento`)
    REFERENCES `automacaosalas`.`ModeloEquipamento` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`Operacao`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`Operacao` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`Operacao` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `titulo` VARCHAR(50) NOT NULL,
  `descricao` VARCHAR(200) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`CodigoInfravermelho`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`CodigoInfravermelho` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`CodigoInfravermelho` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `idOperacao` INT NOT NULL,
  `idModeloEquipamento` INT UNSIGNED NOT NULL,
  `codigo` MEDIUMTEXT NOT NULL,
  INDEX `fk_CodigoInfravermelho_Operacao1_idx` (`idOperacao` ASC) VISIBLE,
  PRIMARY KEY (`id`),
  INDEX `fk_CodigoInfravermelho_ModeloEquipamento1_idx` (`idModeloEquipamento` ASC) VISIBLE,
  CONSTRAINT `fk_CodigoInfravermelho_Operacao1`
    FOREIGN KEY (`idOperacao`)
    REFERENCES `automacaosalas`.`Operacao` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_CodigoInfravermelho_ModeloEquipamento1`
    FOREIGN KEY (`idModeloEquipamento`)
    REFERENCES `automacaosalas`.`ModeloEquipamento` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`Solicitacao`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`Solicitacao` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`Solicitacao` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `payload` JSON NOT NULL,
  `idHardware` INT UNSIGNED NOT NULL,
  `idHardwareAtuador` INT UNSIGNED NOT NULL,
  `dataSolicitacao` DATETIME NOT NULL,
  `dataFinalizacao` DATETIME NULL DEFAULT NULL,
  `tipoSolicitacao` ENUM('MONITORAMENTO_LUZES', 'MONITORAMENTO_AR_CONDICIONADO', 'ATUALIZAR_RESERVAS') CHARACTER SET 'utf8mb3' NOT NULL DEFAULT 'ATUALIZAR_RESERVAS',
  PRIMARY KEY (`id`),
  INDEX `fk_solicitacao_HardwareDeSala1_idx` (`idHardware` ASC) VISIBLE,
  INDEX `fk_solicitacao_HardwareDeSala2_idx` (`idHardwareAtuador` ASC) VISIBLE,
  CONSTRAINT `fk_solicitacao_HardwareDeSala1`
    FOREIGN KEY (`idHardware`)
    REFERENCES `automacaosalas`.`HardwareDeSala` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_solicitacao_HardwareDeSala2`
    FOREIGN KEY (`idHardwareAtuador`)
    REFERENCES `automacaosalas`.`HardwareDeSala` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 58
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `automacaosalas`.`LogRequest`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`LogRequest` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`LogRequest` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `ip` VARCHAR(150) CHARACTER SET 'utf8mb3' NOT NULL,
  `url` VARCHAR(250) NOT NULL,
  `date` DATETIME NOT NULL,
  `input` TEXT NOT NULL,
  `statusCode` VARCHAR(50) CHARACTER SET 'utf8mb3' NOT NULL,
  `origin` ENUM('API', 'ESP32') NOT NULL DEFAULT 'API',
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 16839
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `automacaosalas`.`monitoramento`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`monitoramento` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`monitoramento` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `idEquipamento` INT NOT NULL,
  `idOperacao` INT NOT NULL,
  `dataHora` DATETIME NOT NULL DEFAULT NOW(),
  `idUsuario` INT UNSIGNED NOT NULL,
  `temperatura` TINYINT NULL DEFAULT 0,
  `estado` TINYINT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_monitoramento_Equipamento1_idx` (`idEquipamento` ASC) VISIBLE,
  INDEX `fk_monitoramento_Operacao1_idx` (`idOperacao` ASC) VISIBLE,
  INDEX `fk_monitoramento_Usuario1_idx` (`idUsuario` ASC) VISIBLE,
  CONSTRAINT `fk_monitoramento_Equipamento1`
    FOREIGN KEY (`idEquipamento`)
    REFERENCES `automacaosalas`.`Equipamento` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_monitoramento_Operacao1`
    FOREIGN KEY (`idOperacao`)
    REFERENCES `automacaosalas`.`Operacao` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_monitoramento_Usuario1`
    FOREIGN KEY (`idUsuario`)
    REFERENCES `automacaosalas`.`Usuario` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 13
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `automacaosalas`.`ConexaoInternet`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`ConexaoInternet` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`ConexaoInternet` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(100) NOT NULL,
  `senha` VARCHAR(100) NOT NULL,
  `idBloco` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_ConexaoInternet_Bloco1_idx` (`idBloco` ASC) VISIBLE,
  CONSTRAINT `fk_ConexaoInternet_Bloco1`
    FOREIGN KEY (`idBloco`)
    REFERENCES `automacaosalas`.`Bloco` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `automacaosalas`.`ConexaoInternetSala`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `automacaosalas`.`ConexaoInternetSala` ;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`ConexaoInternetSala` (
  `idConexaoInternet` INT UNSIGNED NOT NULL,
  `idSala` INT UNSIGNED NOT NULL,
  `prioridade` INT NOT NULL DEFAULT 0,
  PRIMARY KEY (`idConexaoInternet`, `idSala`),
  INDEX `fk_ConexaoInternet_has_Sala_Sala1_idx` (`idSala` ASC) VISIBLE,
  INDEX `fk_ConexaoInternet_has_Sala_ConexaoInternet1_idx` (`idConexaoInternet` ASC) VISIBLE,
  CONSTRAINT `fk_ConexaoInternet_has_Sala_ConexaoInternet1`
    FOREIGN KEY (`idConexaoInternet`)
    REFERENCES `automacaosalas`.`ConexaoInternet` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_ConexaoInternet_has_Sala_Sala1`
    FOREIGN KEY (`idSala`)
    REFERENCES `automacaosalas`.`Sala` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

UPDATE monitoramento
SET estado = 1,idOperacao = 1
WHERE id > 0