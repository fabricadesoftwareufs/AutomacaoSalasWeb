-- MySQL Workbench Synchronization
-- Generated: 2024-12-31 10:27
-- Model: New Model
-- Version: 1.0
-- Project: Name of the project
-- Author: gabri

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

ALTER SCHEMA `automacaosalas`  DEFAULT CHARACTER SET utf8  DEFAULT COLLATE utf8_general_ci ;

ALTER TABLE `automacaosalas`.`HorarioSala` 
DROP FOREIGN KEY `fk_HorarioSala_Usuario1`;

ALTER TABLE `automacaosalas`.`HorarioSala` 
DROP COLUMN `planejamento`,
DROP COLUMN `sala`,
DROP COLUMN `usuario`,
ADD COLUMN `idUsuario` INT(10) UNSIGNED NOT NULL AFTER `objetivo`,
ADD COLUMN `idSala` INT(10) UNSIGNED NOT NULL AFTER `idUsuario`,
ADD COLUMN `idPlanejamento` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `idSala`,
DROP INDEX `fk_HorarioSala_Usuario1_idx` ,
ADD INDEX `fk_HorarioSala_Usuario1_idx` (`idUsuario` ASC) VISIBLE,
DROP INDEX `fk_HorarioSala_Sala1_idx` ,
ADD INDEX `fk_HorarioSala_Sala1_idx` (`idSala` ASC) VISIBLE,
DROP INDEX `fk_HorarioSala_Planejamento1_idx` ,
ADD INDEX `fk_HorarioSala_Planejamento1_idx` (`idPlanejamento` ASC) VISIBLE;
;

ALTER TABLE `automacaosalas`.`HardwareDeSala` 
DROP COLUMN `tipoHardware`,
DROP COLUMN `sala`,
ADD COLUMN `idSala` INT(10) UNSIGNED NOT NULL AFTER `mac`,
ADD COLUMN `idTipoHardware` INT(10) UNSIGNED NOT NULL AFTER `idSala`,
CHANGE COLUMN `token` `token` VARCHAR(400) NOT NULL ,
DROP INDEX `fk_Hardware_Sala1_idx` ,
ADD INDEX `fk_Hardware_Sala1_idx` (`idSala` ASC) VISIBLE,
DROP INDEX `fk_HardwareDeSala_TipoHardware1_idx` ,
ADD INDEX `fk_HardwareDeSala_TipoHardware1_idx` (`idTipoHardware` ASC) VISIBLE;
;

ALTER TABLE `automacaosalas`.`TipoHardware` 
ADD COLUMN `idOrganizacao` INT(10) UNSIGNED NOT NULL AFTER `descricao`,
ADD INDEX `fk_TipoHardware_Organizacao1_idx` (`idOrganizacao` ASC) VISIBLE;
;

ALTER TABLE `automacaosalas`.`Sala` 
DROP COLUMN `bloco`,
ADD COLUMN `IdBloco` INT(10) UNSIGNED NOT NULL AFTER `titulo`,
DROP INDEX `fk_Sala_Bloco1_idx` ,
ADD INDEX `fk_Sala_Bloco1_idx` (`IdBloco` ASC) VISIBLE;
;

ALTER TABLE `automacaosalas`.`Usuario` 
DROP COLUMN `tipoUsuario`,
DROP INDEX `fk_Usuario_TipoUsuario1_idx` ;
;

ALTER TABLE `automacaosalas`.`Bloco` 
DROP COLUMN `organizacao`,
ADD COLUMN `idOrganizacao` INT(10) UNSIGNED NOT NULL AFTER `id`,
DROP INDEX `fk_Bloco_Organizacao1_idx` ,
ADD INDEX `fk_Bloco_Organizacao1_idx` (`idOrganizacao` ASC) VISIBLE;
;

ALTER TABLE `automacaosalas`.`UsuarioOrganizacao` 
DROP COLUMN `usuario`,
DROP COLUMN `organizacao`,
DROP COLUMN `id`,
ADD COLUMN `idOrganizacao` INT(10) UNSIGNED NOT NULL FIRST,
ADD COLUMN `idUsuario` INT(10) UNSIGNED NOT NULL AFTER `idOrganizacao`,
ADD COLUMN `idTipoUsuario` INT(10) UNSIGNED NOT NULL AFTER `idUsuario`,
ADD COLUMN `dataCadastro` DATETIME NOT NULL DEFAULT NOW() AFTER `idTipoUsuario`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`idOrganizacao`, `idUsuario`),
DROP INDEX `fk_Organizacao_has_Usuario_Usuario1_idx` ,
ADD INDEX `fk_Organizacao_has_Usuario_Usuario1_idx` (`idUsuario` ASC) VISIBLE,
DROP INDEX `fk_Organizacao_has_Usuario_Organizacao1_idx` ,
ADD INDEX `fk_Organizacao_has_Usuario_Organizacao1_idx` (`idOrganizacao` ASC) VISIBLE,
ADD INDEX `fk_UsuarioOrganizacao_TipoUsuario1_idx` (`idTipoUsuario` ASC) VISIBLE;
;

ALTER TABLE `automacaosalas`.`SalaParticular` 
DROP COLUMN `sala`,
DROP COLUMN `usuario`,
ADD COLUMN `idUsuario` INT(10) UNSIGNED NOT NULL AFTER `id`,
ADD COLUMN `idSala` INT(10) UNSIGNED NOT NULL AFTER `idUsuario`,
DROP INDEX `fk_MinhaSala_Usuario1_idx` ,
ADD INDEX `fk_MinhaSala_Usuario1_idx` (`idUsuario` ASC) VISIBLE,
DROP INDEX `fk_MinhaSala_Sala1_idx` ,
ADD INDEX `fk_MinhaSala_Sala1_idx` (`idSala` ASC) VISIBLE;
;

ALTER TABLE `automacaosalas`.`Planejamento` 
DROP COLUMN `sala`,
DROP COLUMN `usuario`,
ADD COLUMN `idUsuario` INT(10) UNSIGNED NOT NULL AFTER `objetivo`,
ADD COLUMN `idSala` INT(10) UNSIGNED NOT NULL AFTER `idUsuario`,
DROP INDEX `fk_Planejamento_Usuario1_idx` ,
ADD INDEX `fk_Planejamento_Usuario1_idx` (`idUsuario` ASC) VISIBLE,
DROP INDEX `fk_Planejamento_Sala1_idx` ,
ADD INDEX `fk_Planejamento_Sala1_idx` (`idSala` ASC) VISIBLE;
;

ALTER TABLE `automacaosalas`.`Equipamento` 
DROP COLUMN `hardwareDeSala`,
DROP COLUMN `sala`,
ADD COLUMN `idSala` INT(10) UNSIGNED NOT NULL AFTER `descricao`,
ADD COLUMN `idHardwareDeSala` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `tipoEquipamento`,
CHANGE COLUMN `tipoEquipamento` `tipoEquipamento` ENUM('CONDICIONADOR', 'LUZES') NOT NULL DEFAULT 'CONDICIONADOR' ,
DROP INDEX `fk_Equipamento_Sala1_idx` ,
ADD INDEX `fk_Equipamento_Sala1_idx` (`idSala` ASC) VISIBLE,
DROP INDEX `fk_Equipamento_HardwareDeSala1_idx` ,
ADD INDEX `fk_Equipamento_HardwareDeSala1_idx` (`idHardwareDeSala` ASC) VISIBLE;
;

ALTER TABLE `automacaosalas`.`CodigoInfravermelho` 
DROP COLUMN `operacao`,
DROP COLUMN `equipamento`,
ADD COLUMN `idEquipamento` INT(11) NOT NULL AFTER `id`,
ADD COLUMN `idOperacao` INT(11) NOT NULL AFTER `idEquipamento`,
DROP INDEX `fk_CodigoInfravermelho_Equipamento1_idx` ,
ADD INDEX `fk_CodigoInfravermelho_Equipamento1_idx` (`idEquipamento` ASC) VISIBLE,
DROP INDEX `fk_CodigoInfravermelho_Operacao1_idx` ,
ADD INDEX `fk_CodigoInfravermelho_Operacao1_idx` (`idOperacao` ASC) VISIBLE;
;

ALTER TABLE `automacaosalas`.`Solicitacao` 
ADD INDEX `fk_solicitacao_HardwareDeSala1_idx` (`idHardware` ASC) VISIBLE,
ADD INDEX `fk_solicitacao_HardwareDeSala2_idx` (`idHardwareAtuador` ASC) VISIBLE,
DROP INDEX `fk_Solicitacao_Hardware_Atuador1` ,
DROP INDEX `fk_Solicitacao_Hardware1` ;
;

ALTER TABLE `automacaosalas`.`monitoramento` 
DROP COLUMN `equipamento`,
ADD COLUMN `idEquipamento` INT(11) NOT NULL AFTER `estado`,
ADD INDEX `fk_monitoramento_Equipamento1_idx` (`idEquipamento` ASC) VISIBLE,
DROP INDEX `fk_Equipamento_Id` ;
;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`ConexaoInternet` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(100) NOT NULL,
  `senha` VARCHAR(100) NOT NULL,
  `idBloco` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_ConexaoInternet_Bloco1_idx` (`idBloco` ASC) VISIBLE,
  CONSTRAINT `fk_ConexaoInternet_Bloco1`
    FOREIGN KEY (`idBloco`)
    REFERENCES `automacaosalas`.`Bloco` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `automacaosalas`.`ConexaoInternetSala` (
  `idConexaoInternet` INT(10) UNSIGNED NOT NULL,
  `idSala` INT(10) UNSIGNED NOT NULL,
  `prioridade` INT(11) NOT NULL DEFAULT 0,
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
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

ALTER TABLE `automacaosalas`.`HorarioSala` 
ADD CONSTRAINT `fk_HorarioSala_Usuario1`
  FOREIGN KEY (`idUsuario`)
  REFERENCES `automacaosalas`.`Usuario` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_HorarioSala_Sala1`
  FOREIGN KEY (`idSala`)
  REFERENCES `automacaosalas`.`Sala` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_HorarioSala_Planejamento1`
  FOREIGN KEY (`idPlanejamento`)
  REFERENCES `automacaosalas`.`Planejamento` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `automacaosalas`.`HardwareDeSala` 
ADD CONSTRAINT `fk_Hardware_Sala1`
  FOREIGN KEY (`idSala`)
  REFERENCES `automacaosalas`.`Sala` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_HardwareDeSala_TipoHardware1`
  FOREIGN KEY (`idTipoHardware`)
  REFERENCES `automacaosalas`.`TipoHardware` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `automacaosalas`.`TipoHardware` 
ADD CONSTRAINT `fk_TipoHardware_Organizacao1`
  FOREIGN KEY (`idOrganizacao`)
  REFERENCES `automacaosalas`.`Organizacao` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `automacaosalas`.`Sala` 
ADD CONSTRAINT `fk_Sala_Bloco1`
  FOREIGN KEY (`IdBloco`)
  REFERENCES `automacaosalas`.`Bloco` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `automacaosalas`.`Bloco` 
ADD CONSTRAINT `fk_Bloco_Organizacao1`
  FOREIGN KEY (`idOrganizacao`)
  REFERENCES `automacaosalas`.`Organizacao` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `automacaosalas`.`UsuarioOrganizacao` 
ADD CONSTRAINT `fk_Organizacao_has_Usuario_Organizacao1`
  FOREIGN KEY (`idOrganizacao`)
  REFERENCES `automacaosalas`.`Organizacao` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_Organizacao_has_Usuario_Usuario1`
  FOREIGN KEY (`idUsuario`)
  REFERENCES `automacaosalas`.`Usuario` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_UsuarioOrganizacao_TipoUsuario1`
  FOREIGN KEY (`idTipoUsuario`)
  REFERENCES `automacaosalas`.`TipoUsuario` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `automacaosalas`.`SalaParticular` 
ADD CONSTRAINT `fk_MinhaSala_Usuario1`
  FOREIGN KEY (`idUsuario`)
  REFERENCES `automacaosalas`.`Usuario` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_MinhaSala_Sala1`
  FOREIGN KEY (`idSala`)
  REFERENCES `automacaosalas`.`Sala` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `automacaosalas`.`Planejamento` 
ADD CONSTRAINT `fk_Planejamento_Usuario1`
  FOREIGN KEY (`idUsuario`)
  REFERENCES `automacaosalas`.`Usuario` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_Planejamento_Sala1`
  FOREIGN KEY (`idSala`)
  REFERENCES `automacaosalas`.`Sala` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `automacaosalas`.`Equipamento` 
ADD CONSTRAINT `fk_Equipamento_Sala1`
  FOREIGN KEY (`idSala`)
  REFERENCES `automacaosalas`.`Sala` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_Equipamento_HardwareDeSala1`
  FOREIGN KEY (`idHardwareDeSala`)
  REFERENCES `automacaosalas`.`HardwareDeSala` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `automacaosalas`.`CodigoInfravermelho` 
ADD CONSTRAINT `fk_CodigoInfravermelho_Equipamento1`
  FOREIGN KEY (`idEquipamento`)
  REFERENCES `automacaosalas`.`Equipamento` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_CodigoInfravermelho_Operacao1`
  FOREIGN KEY (`idOperacao`)
  REFERENCES `automacaosalas`.`Operacao` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `automacaosalas`.`Solicitacao` 
ADD CONSTRAINT `fk_solicitacao_HardwareDeSala1`
  FOREIGN KEY (`idHardware`)
  REFERENCES `automacaosalas`.`HardwareDeSala` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_solicitacao_HardwareDeSala2`
  FOREIGN KEY (`idHardwareAtuador`)
  REFERENCES `automacaosalas`.`HardwareDeSala` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `automacaosalas`.`monitoramento` 
ADD CONSTRAINT `fk_monitoramento_Equipamento1`
  FOREIGN KEY (`idEquipamento`)
  REFERENCES `automacaosalas`.`Equipamento` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
