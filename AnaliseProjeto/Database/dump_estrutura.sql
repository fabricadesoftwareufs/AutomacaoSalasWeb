CREATE DATABASE IF NOT EXISTS `automacaosalas` /*!40100 DEFAULT CHARACTER SET utf8mb3 COLLATE utf8_unicode_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `automacaosalas`;

-- Copiando estrutura para tabela str_db.organizacao
CREATE TABLE IF NOT EXISTS `organizacao` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `cnpj` varchar(15) NOT NULL,
  `razaoSocial` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.bloco
CREATE TABLE IF NOT EXISTS `bloco` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `organizacao` int unsigned NOT NULL,
  `titulo` varchar(100) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Bloco_Organizacao1_idx` (`organizacao`),
  CONSTRAINT `fk_Bloco_Organizacao1` FOREIGN KEY (`organizacao`) REFERENCES `organizacao` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.sala
CREATE TABLE IF NOT EXISTS `sala` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `titulo` varchar(100) NOT NULL,
  `bloco` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Sala_Bloco1_idx` (`bloco`),
  CONSTRAINT `fk_Sala_Bloco1` FOREIGN KEY (`bloco`) REFERENCES `bloco` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.tipohardware
CREATE TABLE IF NOT EXISTS `tipohardware` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `descricao` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.hardwaredesala
CREATE TABLE IF NOT EXISTS `hardwaredesala` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `mac` varchar(45) NOT NULL,
  `sala` int unsigned NOT NULL,
  `tipoHardware` int unsigned NOT NULL,
  `ip` varchar(15) DEFAULT NULL,
  `uuid` varchar(75) DEFAULT NULL,
  `token` varchar(400) DEFAULT NULL,
  `registrado` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `fk_Hardware_Sala1_idx` (`sala`),
  KEY `fk_HardwareDeSala_TipoHardware1_idx` (`tipoHardware`),
  CONSTRAINT `fk_Hardware_Sala1` FOREIGN KEY (`sala`) REFERENCES `sala` (`id`),
  CONSTRAINT `fk_HardwareDeSala_TipoHardware1` FOREIGN KEY (`tipoHardware`) REFERENCES `tipohardware` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.equipamento
CREATE TABLE IF NOT EXISTS `equipamento` (
  `id` int NOT NULL AUTO_INCREMENT,
  `modelo` varchar(200) NOT NULL,
  `marca` varchar(100) NOT NULL,
  `descricao` varchar(1000) DEFAULT NULL,
  `sala` int unsigned NOT NULL,
  `tipoEquipamento` enum('CONDICIONADOR','LUZES') NOT NULL DEFAULT 'CONDICIONADOR',
  `hardwareDeSala` int unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Equipamento_Sala1_idx` (`sala`),
  KEY `fk_Equipamento_HardwareDeSala1_idx` (`hardwareDeSala`),
  CONSTRAINT `fk_Equipamento_HardwareDeSala1` FOREIGN KEY (`hardwareDeSala`) REFERENCES `hardwaredesala` (`id`),
  CONSTRAINT `fk_Equipamento_Sala1` FOREIGN KEY (`sala`) REFERENCES `sala` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.operacao
CREATE TABLE IF NOT EXISTS `operacao` (
  `id` int NOT NULL AUTO_INCREMENT,
  `titulo` varchar(50) NOT NULL,
  `descricao` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.codigoinfravermelho
CREATE TABLE IF NOT EXISTS `codigoinfravermelho` (
  `id` int NOT NULL AUTO_INCREMENT,
  `equipamento` int NOT NULL,
  `operacao` int NOT NULL,
  `codigo` mediumtext NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_CodigoInfravermelho_Equipamento1_idx` (`equipamento`),
  KEY `fk_CodigoInfravermelho_Operacao1_idx` (`operacao`),
  CONSTRAINT `fk_CodigoInfravermelho_Equipamento1` FOREIGN KEY (`equipamento`) REFERENCES `equipamento` (`id`),
  CONSTRAINT `fk_CodigoInfravermelho_Operacao1` FOREIGN KEY (`operacao`) REFERENCES `operacao` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.tipousuario
CREATE TABLE IF NOT EXISTS `tipousuario` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `descricao` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
	
-- Copiando estrutura para tabela str_db.usuario
CREATE TABLE IF NOT EXISTS `usuario` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `cpf` varchar(11) NOT NULL,
  `nome` varchar(45) NOT NULL,
  `dataNascimento` date DEFAULT NULL,
  `senha` varchar(100) NOT NULL,
  `tipoUsuario` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `Cpf_UNIQUE` (`cpf`),
  KEY `fk_Usuario_TipoUsuario1_idx` (`tipoUsuario`),
  CONSTRAINT `fk_Usuario_TipoUsuario1` FOREIGN KEY (`tipoUsuario`) REFERENCES `tipousuario` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.planejamento
CREATE TABLE IF NOT EXISTS `planejamento` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `dataInicio` date NOT NULL,
  `dataFim` date NOT NULL,
  `horarioInicio` time NOT NULL,
  `horarioFim` time NOT NULL,
  `diaSemana` enum('SEG','TER','QUA','QUI','SEX','SAB','DOM') NOT NULL,
  `objetivo` varchar(500) NOT NULL,
  `usuario` int unsigned NOT NULL,
  `sala` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Planejamento_Usuario1_idx` (`usuario`),
  KEY `fk_Planejamento_Sala1_idx` (`sala`),
  CONSTRAINT `fk_Planejamento_Sala1` FOREIGN KEY (`sala`) REFERENCES `sala` (`id`),
  CONSTRAINT `fk_Planejamento_Usuario1` FOREIGN KEY (`usuario`) REFERENCES `usuario` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.horariosala
CREATE TABLE IF NOT EXISTS `horariosala` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `data` datetime NOT NULL,
  `horarioInicio` time NOT NULL,
  `horarioFim` time NOT NULL,
  `situacao` enum('PENDENTE','APROVADA','REPROVADA','CANCELADA') NOT NULL DEFAULT 'APROVADA',
  `objetivo` varchar(500) NOT NULL,
  `usuario` int unsigned NOT NULL,
  `sala` int unsigned NOT NULL,
  `planejamento` int unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_HorarioSala_Usuario1_idx` (`usuario`),
  KEY `fk_HorarioSala_Sala1_idx` (`sala`),
  KEY `fk_HorarioSala_Planejamento1_idx` (`planejamento`),
  CONSTRAINT `fk_HorarioSala_Planejamento1` FOREIGN KEY (`planejamento`) REFERENCES `planejamento` (`id`),
  CONSTRAINT `fk_HorarioSala_Sala1` FOREIGN KEY (`sala`) REFERENCES `sala` (`id`),
  CONSTRAINT `fk_HorarioSala_Usuario1` FOREIGN KEY (`usuario`) REFERENCES `usuario` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=66 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.logrequest
CREATE TABLE IF NOT EXISTS `logrequest` (
  `id` int NOT NULL AUTO_INCREMENT,
  `ip` varchar(150) CHARACTER SET utf8mb3 COLLATE utf8_general_ci NOT NULL,
  `url` varchar(250) NOT NULL,
  `date` datetime NOT NULL,
  `input` text NOT NULL,
  `statusCode` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8_general_ci NOT NULL,
  `origin` enum('API','ESP32') NOT NULL DEFAULT 'API',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=16839 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.monitoramento
CREATE TABLE IF NOT EXISTS `monitoramento` (
  `id` int NOT NULL AUTO_INCREMENT,
  `estado` tinyint NOT NULL,
  `equipamento` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Equipamento_Id` (`equipamento`),
  CONSTRAINT `fk_Equipamento_Id` FOREIGN KEY (`equipamento`) REFERENCES `equipamento` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.salaparticular
CREATE TABLE IF NOT EXISTS `salaparticular` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `usuario` int unsigned NOT NULL,
  `sala` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_MinhaSala_Usuario1_idx` (`usuario`),
  KEY `fk_MinhaSala_Sala1_idx` (`sala`),
  CONSTRAINT `fk_MinhaSala_Sala1` FOREIGN KEY (`sala`) REFERENCES `sala` (`id`),
  CONSTRAINT `fk_MinhaSala_Usuario1` FOREIGN KEY (`usuario`) REFERENCES `usuario` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.solicitacao
CREATE TABLE IF NOT EXISTS `solicitacao` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `payload` json NOT NULL,
  `idHardware` int unsigned NOT NULL,
  `idHardwareAtuador` int unsigned NOT NULL,
  `dataSolicitacao` datetime NOT NULL,
  `dataFinalizacao` datetime DEFAULT NULL,
  `tipoSolicitacao` enum('MONITORAMENTO_LUZES','MONITORAMENTO_AR_CONDICIONADO','ATUALIZAR_RESERVAS') CHARACTER SET utf8mb3 COLLATE utf8_general_ci NOT NULL DEFAULT 'ATUALIZAR_RESERVAS',
  PRIMARY KEY (`id`),
  KEY `fk_Solicitacao_Hardware1` (`idHardware`),
  CONSTRAINT `fk_Solicitacao_Hardware1` FOREIGN KEY (`idHardware`) REFERENCES `hardwaredesala` (`id`),
   CONSTRAINT `fk_Solicitacao_Hardware_Atuador1` FOREIGN KEY (`idHardwareAtuador`) REFERENCES `hardwaredesala` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=58 DEFAULT CHARSET=utf8mb3;

-- Copiando estrutura para tabela str_db.usuarioorganizacao
CREATE TABLE IF NOT EXISTS `usuarioorganizacao` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `organizacao` int unsigned NOT NULL,
  `usuario` int unsigned NOT NULL,
  PRIMARY KEY (`id`,`organizacao`,`usuario`),
  KEY `fk_Organizacao_has_Usuario_Usuario1_idx` (`usuario`),
  KEY `fk_Organizacao_has_Usuario_Organizacao1_idx` (`organizacao`),
  CONSTRAINT `fk_Organizacao_has_Usuario_Organizacao1` FOREIGN KEY (`organizacao`) REFERENCES `organizacao` (`id`),
  CONSTRAINT `fk_Organizacao_has_Usuario_Usuario1` FOREIGN KEY (`usuario`) REFERENCES `usuario` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb3;
