-- --------------------------------------------------------
-- Servidor:                     127.0.0.1
-- Versão do servidor:           8.0.21 - MySQL Community Server - GPL
-- OS do Servidor:               Win64
-- HeidiSQL Versão:              11.0.0.6080
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Copiando estrutura do banco de dados para str_db
CREATE DATABASE IF NOT EXISTS `str_db` /*!40100 DEFAULT CHARACTER SET utf8 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `str_db`;

-- Copiando estrutura para tabela str_db.organizacao
CREATE TABLE IF NOT EXISTS `organizacao` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Cnpj` varchar(15) NOT NULL,
  `RazaoSocial` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=UTF8;

-- Copiando dados para a tabela str_db.organizacao: ~4 rows (aproximadamente)
/*!40000 ALTER TABLE `organizacao` DISABLE KEYS */;
INSERT INTO `organizacao` (`Id`, `Cnpj`, `RazaoSocial`) VALUES
	(1, '08735240000146', 'FUNDAÇÃO UNIVERSIDADE FEDERAL DE SERGIPE'),
	(2, '57838165000154', 'UNIVERSIDADE TIRADENTES - UNIT'),
	(3, '30056954000187', 'MINISTÉRIO PÚBLICO DE SERGIPE'),
	(4, '50618535000107', 'PREFEITURA MUNICIPAL DE ARACAJU');
/*!40000 ALTER TABLE `organizacao` ENABLE KEYS */;


-- Copiando estrutura para tabela str_db.bloco
CREATE TABLE IF NOT EXISTS `bloco` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Organizacao` int unsigned NOT NULL,
  `Titulo` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Bloco_Organizacao1_idx` (`Organizacao`),
  CONSTRAINT `fk_Bloco_Organizacao1` FOREIGN KEY (`Organizacao`) REFERENCES `organizacao` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela str_db.bloco: ~13 rows (aproximadamente)
/*!40000 ALTER TABLE `bloco` DISABLE KEYS */;
INSERT INTO `bloco` (`Id`, `Organizacao`, `Titulo`) VALUES
	(1, 1, 'Bloco A'),
	(2, 1, 'Bloco B'),
	(3, 1, 'Bloco C'),
	(4, 1, 'Bloco D'),
	(5, 1, 'Bloco E'),
	(6, 2, 'Bloco SI'),
	(7, 2, 'Bloco ADM'),
	(8, 2, 'Bloco SAUDE'),
	(9, 2, 'Bloco BIO'),
	(10, 3, 'Bloco X'),
	(11, 3, 'Bloco Y'),
	(12, 3, 'Bloco Z'),
	(13, 4, 'Bloco Unico');
/*!40000 ALTER TABLE `bloco` ENABLE KEYS */;

-- Copiando estrutura para tabela str_db.sala
CREATE TABLE IF NOT EXISTS `sala` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Titulo` varchar(100) NOT NULL,
  `Bloco` int unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Sala_Bloco1_idx` (`Bloco`),
  CONSTRAINT `fk_Sala_Bloco1` FOREIGN KEY (`Bloco`) REFERENCES `bloco` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela str_db.sala: ~13 rows (aproximadamente)
/*!40000 ALTER TABLE `sala` DISABLE KEYS */;
INSERT INTO `sala` (`Id`, `Titulo`, `Bloco`) VALUES
	(1, 'Sala 01', 1),
	(2, 'Sala 02', 2),
	(3, 'Sala 03', 3),
	(4, 'Sala 04', 4),
	(5, 'Sala 05', 5),
	(6, 'Sala 06', 6),
	(7, 'Sala 07', 7),
	(8, 'Sala 08', 8),
	(9, 'Sala 09', 9),
	(10, 'Sala 10', 10),
	(11, 'Sala 11', 11),
	(12, 'Sala 12', 12),
	(13, 'Sala 13', 13),
	(14, 'Sala 106', 4);
/*!40000 ALTER TABLE `sala` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `monitoramento` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `Luzes` TINYINT(4) NOT NULL,
  `ArCondicionado` TINYINT(4) NOT NULL,
  `Sala` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_Sala_Id` FOREIGN KEY (`Sala`) REFERENCES `sala` (`Id`)
) ENGINE = InnoDB DEFAULT CHARACTER SET = utf8;

INSERT INTO `monitoramento` (`Id`, `Luzes`, `ArCondicionado`,`Sala`) VALUES
	(1, 0,0, 1),
	(2, 0,0, 2),
	(3, 0,0, 3),
	(4, 0,0, 4),
	(5, 0,0, 5),
	(6, 0,0, 6),
	(7, 0,0, 7),
	(8, 0,0, 8),
	(9, 0,0, 9),
	(10, 0,0, 10),
	(11, 0,0, 11),
	(12, 0,0, 12),
	(13, 0,0, 13),
	(14, 0,0, 14);
	
-- Copiando estrutura para tabela str_db.tipohardware
CREATE TABLE IF NOT EXISTS `tipohardware` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Descricao` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=UTF8;

-- Copiando dados para a tabela str_db.tipohardware: ~3 rows (aproximadamente)
/*!40000 ALTER TABLE `tipohardware` DISABLE KEYS */;
INSERT INTO `tipohardware` (`Id`, `Descricao`) VALUES
	(1, 'CONTROLADOR DE BLOCO'),
	(2, 'CONTROLADOR DE SALA'),
	(3, 'MODULO DE SENSORIAMENTO');
/*!40000 ALTER TABLE `tipohardware` ENABLE KEYS */;

-- Copiando estrutura para tabela str_db.hardwaredebloco
CREATE TABLE IF NOT EXISTS `hardwaredebloco` (
  `id` int NOT NULL AUTO_INCREMENT,
  `MAC` varchar(45) NOT NULL,
  `Bloco` int unsigned NOT NULL,
  `TipoHardware` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `idHardwareDeBloco_UNIQUE` (`id`),
  KEY `fk_HardwareDeBloco_Bloco1_idx` (`Bloco`),
  KEY `fk_HardwareDeBloco_TipoHardware1_idx` (`TipoHardware`),
  CONSTRAINT `fk_HardwareDeBloco_Bloco1` FOREIGN KEY (`Bloco`) REFERENCES `bloco` (`Id`),
  CONSTRAINT `fk_HardwareDeBloco_TipoHardware1` FOREIGN KEY (`TipoHardware`) REFERENCES `tipohardware` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela str_db.hardwaredebloco: ~11 rows (aproximadamente)
/*!40000 ALTER TABLE `hardwaredebloco` DISABLE KEYS */;
INSERT INTO `hardwaredebloco` (`id`, `MAC`, `Bloco`, `TipoHardware`) VALUES
	(1, '00:E0:4C:1E:D9:14',1, 3),
	(2, '00:E0:4C:2F:2D:6C',2, 3),
	(3, '00:E0:4C:43:B4:48',3, 3),
	(4, '00:E0:4C:01:9E:76',4, 3),
	(5, '00:E0:4C:55:6C:99',5, 3),
	(6, '00:E0:4C:75:B8:15',6, 3),
	(7, '00:E0:4C:51:34:D1',7, 3),
	(8, '00:E0:4C:7D:84:19',8, 3),
	(9, '00:E0:4C:50:86:F5',9, 3),
	(10, '00:E0:4C:7B:F5:06',10, 3),
	(11, '00:E0:4C:18:50:68',11, 3);
/*!40000 ALTER TABLE `hardwaredebloco` ENABLE KEYS */;

-- Copiando estrutura para tabela str_db.hardwaredesala
CREATE TABLE IF NOT EXISTS `hardwaredesala` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `MAC` varchar(45) NOT NULL,
  `Sala` int unsigned NOT NULL,
  `TipoHardware` int unsigned NOT NULL,
  `Ip` VARCHAR(15) NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Hardware_Sala1_idx` (`Sala`),
  KEY `fk_HardwareDeSala_TipoHardware1_idx` (`TipoHardware`),
  CONSTRAINT `fk_Hardware_Sala1` FOREIGN KEY (`Sala`) REFERENCES `sala` (`Id`),
  CONSTRAINT `fk_HardwareDeSala_TipoHardware1` FOREIGN KEY (`TipoHardware`) REFERENCES `tipohardware` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela str_db.hardwaredesala: ~39 rows (aproximadamente)
/*!40000 ALTER TABLE `hardwaredesala` DISABLE KEYS */;
INSERT INTO `hardwaredesala` (`Id`, `MAC`,`Ip`, `Sala`, `TipoHardware`) VALUES
	(1, '00:E0:4C:27:56:96','192.168.15.233',  1, 1),
	(2, '00:E0:4C:5A:D1:DD', '192.168.15.233', 1, 2),
	(3, '00:E0:4C:50:B0:3E','192.168.15.233',  2, 1),
	(4, '00:E0:4C:50:B0:3E','192.168.15.233', 2, 2),
	(5, '00:E0:4C:66:82:0A', '192.168.15.233', 3, 1),
	(6, '00:E0:4C:59:A4:20','192.168.15.233',  3, 2),
	(7, '00:E0:4C:15:F6:A0', '192.168.15.233', 4, 1),
	(8, '00:E0:4C:1B:B5:35', '192.168.15.233', 4, 2),
	(9, '00:E0:4C:4E:A9:64', '192.168.15.233', 5, 1),
	(10, '00:E0:4C:04:B6:9E','192.168.15.233',  5, 2),
	(11, '00:E0:4C:3A:E1:F0','192.168.15.233',  6, 1),
	(12, '00:E0:4C:46:B6:13', '192.168.15.233', 6, 2),
	(13, '00:E0:4C:15:AA:D1', '192.168.15.233', 7, 1),
	(14, '00:E0:4C:71:5B:0B', '192.168.15.233', 7, 2),
	(15, '00:E0:4C:3B:3F:9F','192.168.15.233',  8, 1),
	(16, '00:E0:4C:31:19:0B', '192.168.15.233', 8, 2),
	(17, '00:E0:4C:5A:D1:DD', '192.168.15.233', 1, 3),
	(18, '00:E0:4C:32:A1:93', '192.168.15.233', 2, 3),
	(19, '00:E0:4C:04:56:7B', '192.168.15.233', 3, 3),
	(20, '00:E0:4C:32:F0:A6', '192.168.15.233', 4, 3),
	(21, '00:E0:4C:43:7C:3E', '192.168.15.233', 5, 3),
	(22, '00:E0:4C:7B:D9:3E', '192.168.15.233', 6, 3),
	(23, '00:E0:4C:7A:3A:A4', '192.168.15.233', 7, 3),
	(24, '00:E0:4C:5B:DD:23', '192.168.15.233', 8, 3),
	(25, '00:E0:4C:08:72:05', '192.168.15.233', 9, 1),
	(26, '00:E0:4C:33:61:87', '192.168.15.233', 9, 2),
	(27, '00:E0:4C:39:C1:CC', '192.168.15.233', 9, 3),
	(28, '00:E0:4C:44:06:E4', '192.168.15.233', 10, 1),
	(29, '00:E0:4C:1B:38:1F', '192.168.15.233', 10, 2),
	(30, '00:E0:4C:1C:C2:42', '192.168.15.233', 10, 3),
	(31, '00:E0:4C:26:F7:80', '192.168.15.233', 11, 1),
	(32, '00:E0:4C:7B:75:7F', '192.168.15.233', 11, 2),
	(33, '00:E0:4C:41:AA:BD', '192.168.15.233', 11, 3),
	(34, '00:E0:4C:7A:16:3A', '192.168.15.233', 12, 1),
	(35, '00:E0:4C:4B:8A:65', '192.168.15.233', 12, 2),
	(36, '00:E0:4C:43:85:71', '192.168.15.233', 12, 3),
	(37, '00:E0:4C:5F:B4:08', '192.168.15.233', 13, 1),
	(38, '00:E0:4C:21:50:B0', '192.168.15.233', 13, 2),
	(39, '00:E0:4C:28:5F:41', '192.168.15.233', 13, 3);
/*!40000 ALTER TABLE `hardwaredesala` ENABLE KEYS */;

-- Copiando estrutura para tabela str_db.tipousuario
CREATE TABLE IF NOT EXISTS `tipousuario` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Descricao` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela str_db.tipousuario: ~2 rows (aproximadamente)
/*!40000 ALTER TABLE `tipousuario` DISABLE KEYS */;
INSERT INTO `tipousuario` (`Id`, `Descricao`) VALUES
	(1, 'ADMIN'),
	(2, 'GESTOR'),
	(3, 'CLIENTE');
/*!40000 ALTER TABLE `tipousuario` ENABLE KEYS */;

-- Copiando estrutura para tabela str_db.usuario
CREATE TABLE IF NOT EXISTS `usuario` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Cpf` varchar(11) NOT NULL,
  `Nome` varchar(45) NOT NULL,
  `DataNascimento` date DEFAULT NULL,
  `Senha` varchar(100) NOT NULL,
  `TipoUsuario` int unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Cpf_UNIQUE` (`Cpf`),
  KEY `fk_Usuario_TipoUsuario1_idx` (`TipoUsuario`),
  CONSTRAINT `fk_Usuario_TipoUsuario1` FOREIGN KEY (`TipoUsuario`) REFERENCES `tipousuario` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela str_db.usuario: ~2 rows (aproximadamente)
INSERT INTO `usuario` (`Id`, `Cpf`, `Nome`, `DataNascimento`, `Senha`, `TipoUsuario`) VALUES
	(1, '42112664204', 'Lívia Benedita Rebeca Araújo', '1997-08-15', '60BFAA61E12B4FD3DAD35586B11387689E35645279C6103495F019AAA0C1FCF3', 2),
	(2, '57377766387', 'Rafael Kevin Teixeira', '1996-07-22', '60BFAA61E12B4FD3DAD35586B11387689E35645279C6103495F019AAA0C1FCF3', 2),
	(3, '07852892590', 'Igor bruno dos santos nascimento', '1996-07-22', '60BFAA61E12B4FD3DAD35586B11387689E35645279C6103495F019AAA0C1FCF3', 2);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;

-- Copiando estrutura para tabela str_db.usuarioorganizacao
CREATE TABLE IF NOT EXISTS `usuarioorganizacao` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Organizacao` int unsigned NOT NULL,
  `Usuario` int unsigned NOT NULL,
  PRIMARY KEY (`Id`,`Organizacao`,`Usuario`),
  KEY `fk_Organizacao_has_Usuario_Usuario1_idx` (`Usuario`),
  KEY `fk_Organizacao_has_Usuario_Organizacao1_idx` (`Organizacao`),
  CONSTRAINT `fk_Organizacao_has_Usuario_Organizacao1` FOREIGN KEY (`Organizacao`) REFERENCES `organizacao` (`Id`),
  CONSTRAINT `fk_Organizacao_has_Usuario_Usuario1` FOREIGN KEY (`Usuario`) REFERENCES `usuario` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela str_db.usuarioorganizacao: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `usuarioorganizacao` DISABLE KEYS */;
INSERT INTO `usuarioorganizacao` (`Id`, `Organizacao`, `Usuario`) VALUES
	(1, 1, 1),
	(3, 1, 3),
	(2, 1, 2);
/*!40000 ALTER TABLE `usuarioorganizacao` ENABLE KEYS */;


-- Copiando estrutura para tabela str_db.horariosala
CREATE TABLE IF NOT EXISTS `horariosala` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Data` datetime NOT NULL,
  `HorarioInicio` time NOT NULL,
  `HorarioFim` time NOT NULL,
  `Situacao` ENUM('PENDENTE', 'APROVADA','REPROVADA', 'CANCELADA') NOT NULL DEFAULT 'APROVADA',
  `Objetivo` varchar(500) NOT NULL,
  `Usuario` int unsigned NOT NULL,
  `Sala` int unsigned NOT NULL,
  `Planejamento` int unsigned NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_HorarioSala_Usuario1_idx` (`Usuario`),
  KEY `fk_HorarioSala_Sala1_idx` (`Sala`),
  KEY `fk_HorarioSala_Planejamento1_idx` (`Planejamento`),
  CONSTRAINT `fk_HorarioSala_Sala1` FOREIGN KEY (`Sala`) REFERENCES `sala` (`Id`),
  CONSTRAINT `fk_HorarioSala_Usuario1` FOREIGN KEY (`Usuario`) REFERENCES `usuario` (`Id`),
  CONSTRAINT `fk_HorarioSala_Planejamento1` FOREIGN KEY (`Planejamento`) REFERENCES `Planejamento` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;    

-- Copiando dados para a tabela str_db.horariosala: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `horariosala` DISABLE KEYS */;
INSERT INTO `horariosala` (`Id`, `Data`, `HorarioInicio`,`HorarioFim`,`Objetivo`,`Usuario`,`Sala`,`Planejamento`) VALUES
							(1, '2020-08-24', '07:00','09:00','Palestra sobre Engenharia de Software',1,4,null),
							(2, '2020-09-20', '07:00','09:00','Palestra sobre Engenharia de Software',2,4,null);
			
/*!40000 ALTER TABLE `horariosala` ENABLE KEYS */;

-- Copiando estrutura para tabela str_db.planejamento
CREATE TABLE IF NOT EXISTS `planejamento` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `DataInicio` date NOT NULL,
  `DataFim` date NOT NULL,
  `HorarioInicio` time NOT NULL,
  `HorarioFim` time NOT NULL,
  `DiaSemana` enum('SEG','TER','QUA','QUI','SEX','SAB','DOM') NOT NULL,
  `Objetivo` varchar(500) NOT NULL,
  `Usuario` int unsigned NOT NULL,
  `Sala` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Planejamento_Usuario1_idx` (`Usuario`),
  KEY `fk_Planejamento_Sala1_idx` (`Sala`),
  CONSTRAINT `fk_Planejamento_Sala1` FOREIGN KEY (`Sala`) REFERENCES `sala` (`Id`),
  CONSTRAINT `fk_Planejamento_Usuario1` FOREIGN KEY (`Usuario`) REFERENCES `usuario` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela str_db.planejamento: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `planejamento` DISABLE KEYS */;
INSERT INTO `planejamento` (`Id`, `DataInicio`, `DataFim`,`HorarioInicio`,`HorarioFim`,`DiaSemana`,`Objetivo`,`Usuario`,`Sala`) VALUES
	(1, '2020-08-24', '2020-12-24','07:00','09:00','SEG','Planejamento de LFT',1,5),
	(2, '2020-08-24', '2020-12-24','07:00','09:00','QUA','Planejamento de LFT',1,5),
	(3, '2020-08-24', '2020-12-24','07:00','09:00','SEX','Planejamento de LFT',1,5);
/*!40000 ALTER TABLE `planejamento` ENABLE KEYS */;



-- Copiando estrutura para tabela str_db.salaparticular
CREATE TABLE IF NOT EXISTS `salaparticular` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `Usuario` int unsigned NOT NULL,
  `Sala` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_MinhaSala_Usuario1_idx` (`Usuario`),
  KEY `fk_MinhaSala_Sala1_idx` (`Sala`),
  CONSTRAINT `fk_MinhaSala_Sala1` FOREIGN KEY (`Sala`) REFERENCES `sala` (`Id`),
  CONSTRAINT `fk_MinhaSala_Usuario1` FOREIGN KEY (`Usuario`) REFERENCES `usuario` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela str_db.salaparticular: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `salaparticular` DISABLE KEYS */;
INSERT INTO `salaparticular` (`Id`, `Usuario`,`Sala`) VALUES 
				(1,1,1),
				(2,2,2),
				(3,2,4),
				(4,2,1),
				(5,1,3);
/*!40000 ALTER TABLE `salaparticular` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `equipamento` (
  `id` INT(11) NOT NULL,
  `Modelo` VARCHAR(200) NOT NULL,
  `Marca` VARCHAR(100) NOT NULL,
  `Descricao` VARCHAR(1000) NULL DEFAULT NULL,
  `Sala` INT(10) UNSIGNED NOT NULL,
  `TipoEquipamento` ENUM('CONDICIONADOR', 'LUZES') DEFAULT ('CONDICIONADOR') NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Equipamento_Sala1_idx` (`Sala`),
  CONSTRAINT `fk_Equipamento_Sala1` FOREIGN KEY (`Sala`) REFERENCES `str_db`.`Sala` (`Id`)
) ENGINE = InnoDB DEFAULT CHARACTER SET = utf8;

/*!40000 ALTER TABLE `salaparticular` DISABLE KEYS */;
INSERT INTO `equipamento` (`Id`, `Modelo`,`Marca`,`Descricao`,`Sala`) VALUES 
				(1,"SAMSUNG","220TT","Condicionador SAMSUNG 110V classe de consumo E",1),
				(2,"ELGIN","SGV330","Condicionador ELGIN 110V classe de consumo G",2),
				(3,"LG","EXTENDER","Condicionador LG 110V classe de consumo G",3);
/*!40000 ALTER TABLE `salaparticular` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `codigoInfravermelho` (
  `id` INT(11) NOT NULL,
  `Equipamento` INT(11) NOT NULL,
  `Operacao` INT(11) NOT NULL,
  `Codigo` TEXT(65535) NOT NULL COLLATE 'utf8_unicode_ci',
  PRIMARY KEY (`id`),
  KEY `fk_CodigoInfravermelho_Equipamento1_idx` (`Equipamento` ASC) VISIBLE,
  KEY `fk_CodigoInfravermelho_Operacao1_idx` (`Operacao` ASC) VISIBLE,
  CONSTRAINT `fk_CodigoInfravermelho_Equipamento1` FOREIGN KEY (`Equipamento`) REFERENCES `str_db`.`Equipamento` (`id`),
  CONSTRAINT `fk_CodigoInfravermelho_Operacao1` FOREIGN KEY (`Operacao`) REFERENCES `str_db`.`Operacao` (`id`)
)ENGINE = InnoDB DEFAULT CHARACTER SET = utf8;


/*!40000 ALTER TABLE `CodigoInfravermelho` DISABLE KEYS */;
INSERT INTO `codigoInfravermelho` (`Id`, `Equipamento`,`Operacao`,`Codigo`) VALUES 
-- --------------------------------------------------------
-- Servidor:                     127.0.0.1
-- Versão do servidor:           8.0.22 - MySQL Community Server - GPL
-- OS do Servidor:               Win64
-- HeidiSQL Versão:              11.0.0.5919
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Copiando dados para a tabela str_db.codigoinfravermelho: ~245 rows (aproximadamente)
/*!40000 ALTER TABLE `codigoinfravermelho` DISABLE KEYS */;
INSERT INTO `codigoinfravermelho` (`id`, `Equipamento`, `Operacao`, `Codigo`) VALUES
	(0, 1, 2, '460,264,184,256,182,584,184,422,184,256,182,584,186,258,182,256,182,256,184,586,184,584,186,420,184,584,186,280,158,282,160,748,186,264,188'),
	(1, 2, 2, '8412,4232,496,556,548,1484,580,1568,524,536,528,1572,524,544,528,1580,520,540,496,556,524,1568,520,536,500,560,552,1548,552,1552,552,1552,524,540,496,556,500,552,500,560,524,516,520,568,496,572,524,552,524,536,496,556,520,512,520,560,500,564,496,568,500,572,496,576,496,544,516,556,496,560,496,540,516,568,496,568,552,1528,544,552,500,560,496,556,520,512,524,536,516,568,500,544,516,576,524,1556,548,1548,516,560,496,556,524,536,496,544,516,572,500,548,520,576,496,564,492,560,496,556,500,560,496,568,496,544,520,572,500,576,496,564,496,556,496,556,548,1520,576,1544,524,544,496,572,496,576,500,560,500,552,500,556,496,564,520,540,496,572,496,572,496,576,496,512,576,508,516,560,524,1568,552,1520,524,564,500,548,544,552,520,516,572,1516,572,1512,524,536,548,1548,548,1576,520,548,496,580,496,564,544,1516,576,1512,572,1520,524,540,520,568,520,548,500,556,540,540,548,1512,524,532,520,564,524,1548,544,544,496,572,500,552,520,564,496,528,552,1560,528,1544,520,540,548,1556,520,544,548,1584,520,508,492'),
	(244, 1, 1, '9076,4438,612,522,610,522,610,1640,610,524,610,522,610,522,608,524,584,548,584,1666,586,1666,586,548,584,1666,586,1666,586,1666,586,1668,584,1668,586,548,584,548,584,548,584,1666,586,548,584,548,586,548,586,546,586,1666,588,1664,586,1666,588,546,586,1666,586,1666,586,1666,588,1664,586,39900,9028,2226,584');
/*!40000 ALTER TABLE `codigoinfravermelho` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

/*!40000 ALTER TABLE `salaparticular` ENABLE KEYS */;


CREATE TABLE IF NOT EXISTS `operacao` (
  `id` INT(11) NOT NULL,
  `Titulo` VARCHAR(50) NOT NULL,
  `Descricao` VARCHAR(200) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = UTF8;

/*!40000 ALTER TABLE `Operacao` DISABLE KEYS */;
INSERT INTO `operacao` (`Id`, `Titulo`,`Descricao`) VALUES 
				(1,"Ligar","Liga Dispositivo"),
				(2,"Desligar","Desliga Dispotivo");
/*!40000 ALTER TABLE `salaparticular` ENABLE KEYS */;


/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
