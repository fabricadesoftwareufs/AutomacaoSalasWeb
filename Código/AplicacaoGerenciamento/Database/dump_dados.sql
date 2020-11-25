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

-- Copiando dados para a tabela str_db.organizacao: ~4 rows (aproximadamente)
/*!40000 ALTER TABLE `organizacao` DISABLE KEYS */;
INSERT INTO `organizacao` (`Id`, `Cnpj`, `RazaoSocial`) VALUES
	(1, '08735240000146', 'FUNDAÇÃO UNIVERSIDADE FEDERAL DE SERGIPE'),
	(2, '57838165000154', 'UNIVERSIDADE TIRADENTES - UNIT'),
	(3, '30056954000187', 'MINISTÉRIO PÚBLICO DE SERGIPE'),
	(4, '50618535000107', 'PREFEITURA MUNICIPAL DE ARACAJU');
/*!40000 ALTER TABLE `organizacao` ENABLE KEYS */;


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
	

-- Copiando dados para a tabela str_db.tipohardware: ~3 rows (aproximadamente)
/*!40000 ALTER TABLE `tipohardware` DISABLE KEYS */;
INSERT INTO `tipohardware` (`Id`, `Descricao`) VALUES
	(1, 'CONTROLADOR DE BLOCO'),
	(2, 'CONTROLADOR DE SALA'),
	(3, 'MODULO DE SENSORIAMENTO');
/*!40000 ALTER TABLE `tipohardware` ENABLE KEYS */;

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

-- Copiando dados para a tabela str_db.tipousuario: ~2 rows (aproximadamente)
/*!40000 ALTER TABLE `tipousuario` DISABLE KEYS */;
INSERT INTO `tipousuario` (`Id`, `Descricao`) VALUES
	(1, 'ADMIN'),
	(2, 'GESTOR'),
	(3, 'CLIENTE');
/*!40000 ALTER TABLE `tipousuario` ENABLE KEYS */;

-- Copiando dados para a tabela str_db.usuario: ~2 rows (aproximadamente)
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` (`Id`, `Cpf`, `Nome`, `DataNascimento`, `Senha`, `TipoUsuario`) VALUES
	(1, '42112664204', 'Lívia Benedita Rebeca Araújo', '1997-08-15', '4796D7022C26F2B5A32B71D5CE1584F885020831D7E47331EC05EDB4FFEBFC59', 2),
	(2, '57377766387', 'Rafael Kevin Teixeira', '1996-07-22', '4796D7022C26F2B5A32B71D5CE1584F885020831D7E47331EC05EDB4FFEBFC59', 2),
	(3, '07852892590', 'Igor bruno dos santos nascimento', '1996-07-22', '4796D7022C26F2B5A32B71D5CE1584F885020831D7E47331EC05EDB4FFEBFC59', 2);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;

-- Copiando dados para a tabela str_db.usuarioorganizacao: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `usuarioorganizacao` DISABLE KEYS */;
INSERT INTO `usuarioorganizacao` (`Id`, `Organizacao`, `Usuario`) VALUES
	(1, 1, 1),
	(3, 1, 3),
	(2, 1, 2);
	
/*!40000 ALTER TABLE `usuarioorganizacao` ENABLE KEYS */;
INSERT INTO `horariosala` (`Id`, `Data`, `HorarioInicio`,`HorarioFim`,`Objetivo`,`Usuario`,`Sala`,`Planejamento`) VALUES
							(1, '2020-08-24', '07:00','09:00','Palestra sobre Engenharia de Software',1,4,null),
							(2, '2020-09-20', '07:00','09:00','Palestra sobre Engenharia de Software',2,4,null);

-- Copiando dados para a tabela str_db.planejamento: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `planejamento` DISABLE KEYS */;
INSERT INTO `planejamento` (`Id`, `DataInicio`, `DataFim`,`HorarioInicio`,`HorarioFim`,`DiaSemana`,`Objetivo`,`Usuario`,`Sala`) VALUES
	(1, '2020-08-24', '2020-12-24','07:00','09:00','SEG','Planejamento de LFT',1,5),
	(2, '2020-08-24', '2020-12-24','07:00','09:00','QUA','Planejamento de LFT',1,5),
	(3, '2020-08-24', '2020-12-24','07:00','09:00','SEX','Planejamento de LFT',1,5);
/*!40000 ALTER TABLE `planejamento` ENABLE KEYS */;

-- Copiando dados para a tabela str_db.salaparticular: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `salaparticular` DISABLE KEYS */;
INSERT INTO `salaparticular` (`Id`, `Usuario`,`Sala`) VALUES 
				(1,1,1),
				(2,2,2),
				(3,2,4),
				(4,2,1),
				(5,1,3);
/*!40000 ALTER TABLE `salaparticular` ENABLE KEYS */;

-- Copiando dados para a tabela str_db.Equipamento: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `salaparticular` DISABLE KEYS */;
INSERT INTO `equipamento` (`Id`, `Modelo`,`Marca`,`Descricao`,`Sala`) VALUES 
				(1,"SAMSUNG","220TT","Condicionador SAMSUNG 110V classe de consumo E",1),
				(2,"ELGIN","SGV330","Condicionador ELGIN 110V classe de consumo G",2),
				(3,"LG","EXTENDER","Condicionador LG 110V classe de consumo G",3);
/*!40000 ALTER TABLE `salaparticular` ENABLE KEYS */;


/*!40000 ALTER TABLE `CodigoInfravermelho` DISABLE KEYS */;
INSERT INTO `codigoInfravermelho` (`Id`, `Equipamento`,`Operacao`,`Codigo`) VALUES 
				(1,2,2,"8412"),
				(2,2,2,"4232"),
				(3,2,2,"496"),
				(4,2,2,"556"),
				(5,2,2,"548"),
				(6,2,2,"1484"),
				(7,2,2,"580"),
				(8,2,2,"1568"),
				(9,2,2,"524"),
				(10,2,2,"536"),
				(11,2,2,"528"),
				(12,2,2,"1572"),
				(13,2,2,"524"),
				(14,2,2,"544"),
				(15,2,2,"528"),
				(16,2,2,"1580"),
				(17,2,2,"520"),
				(18,2,2,"540"),
				(19,2,2,"496"),
				(20,2,2,"556"),
				(21,2,2,"524"),
				(22,2,2,"1568"),
				(23,2,2,"520"),
				(24,2,2,"536"),
				(25,2,2,"500"),
				(26,2,2,"560"),
				(27,2,2,"552"),
				(28,2,2,"1548"),
				(29,2,2,"552"),
				(30,2,2,"1552"),
				(31,2,2,"552"),
				(32,2,2,"1552"),
				(33,2,2,"524"),
				(34,2,2,"540"),
				(35,2,2,"496"),
				(36,2,2,"556"),
				(37,2,2,"500"),
				(38,2,2,"552"),
				(39,2,2,"500"),
				(40,2,2,"560"),
				(41,2,2,"524"),
				(42,2,2,"516"),
				(43,2,2,"520"),
				(44,2,2,"568"),
				(45,2,2,"496"),
				(46,2,2,"572"),
				(47,2,2,"524"),
				(48,2,2,"552"),
				(49,2,2,"524"),
				(50,2,2,"536"),
				(51,2,2,"496"),
				(52,2,2,"556"),
				(53,2,2,"520"),
				(54,2,2,"512"),
				(55,2,2,"520"),
				(56,2,2,"560"),
				(57,2,2,"500"),
				(58,2,2,"564"),
				(59,2,2,"496"),
				(60,2,2,"568"),
				(61,2,2,"500"),
				(62,2,2,"572"),
				(63,2,2,"496"),
				(64,2,2,"576"),
				(65,2,2,"496"),
				(66,2,2,"544"),
				(67,2,2,"516"),
				(68,2,2,"556"),
				(69,2,2,"496"),
				(70,2,2,"560"),
				(71,2,2,"496"),
				(72,2,2,"540"),
				(73,2,2,"516"),
				(74,2,2,"568"),
				(75,2,2,"496"),
				(76,2,2,"568"),
				(77,2,2,"552"),
				(78,2,2,"1528"),
				(79,2,2,"544"),
				(80,2,2,"552"),
				(81,2,2,"500"),
				(82,2,2,"560"),
				(83,2,2,"496"),
				(84,2,2,"556"),
				(85,2,2,"520"),
				(86,2,2,"512"),
				(87,2,2,"524"),
				(88,2,2,"536"),
				(89,2,2,"516"),
				(90,2,2,"568"),
				(91,2,2,"500"),
				(92,2,2,"544"),
				(93,2,2,"516"),
				(94,2,2,"576"),
				(95,2,2,"524"),
				(96,2,2,"1556"),
				(97,2,2,"548"),
				(98,2,2,"1548"),
				(99,2,2,"516"),
				(100,2,2,"560"),
				(101,2,2,"496"),
				(102,2,2,"556"),
				(103,2,2,"524"),
				(104,2,2,"536"),
				(105,2,2,"496"),
				(106,2,2,"544"),
				(107,2,2,"516"),
				(108,2,2,"572"),
				(109,2,2,"500"),
				(110,2,2,"548"),
				(111,2,2,"520"),
				(112,2,2,"576"),
				(113,2,2,"496"),
				(114,2,2,"564"),
				(115,2,2,"492"),
				(116,2,2,"560"),
				(117,2,2,"496"),
				(118,2,2,"556"),
				(119,2,2,"500"),
				(120,2,2,"560"),
				(121,2,2,"496"),
				(122,2,2,"568"),
				(123,2,2,"496"),
				(124,2,2,"544"),
				(125,2,2,"520"),
				(126,2,2,"572"),
				(127,2,2,"500"),
				(128,2,2,"576"),
				(129,2,2,"496"),
				(130,2,2,"564"),
				(131,2,2,"496"),
				(132,2,2,"556"),
				(133,2,2,"496"),
				(134,2,2,"556"),
				(135,2,2,"548"),
				(136,2,2,"1520"),
				(137,2,2,"576"),
				(138,2,2,"1544"),
				(139,2,2,"524"),
				(140,2,2,"544"),
				(141,2,2,"496"),
				(142,2,2,"572"),
				(143,2,2,"496"),
				(144,2,2,"576"),
				(145,2,2,"500"),
				(146,2,2,"560"),
				(147,2,2,"500"),
				(148,2,2,"552"),
				(149,2,2,"500"),
				(150,2,2,"556"),
				(151,2,2,"496"),
				(152,2,2,"564"),
				(153,2,2,"520"),
				(154,2,2,"540"),
				(155,2,2,"496"),
				(156,2,2,"572"),
				(157,2,2,"496"),
				(158,2,2,"572"),
				(159,2,2,"496"),
				(160,2,2,"576"),
				(161,2,2,"496"),
				(162,2,2,"512"),
				(163,2,2,"576"),
				(164,2,2,"508"),
				(165,2,2,"516"),
				(166,2,2,"560"),
				(167,2,2,"524"),
				(168,2,2,"1568"),
				(169,2,2,"552"),
				(170,2,2,"1520"),
				(171,2,2,"524"),
				(172,2,2,"564"),
				(173,2,2,"500"),
				(174,2,2,"548"),
				(175,2,2,"544"),
				(176,2,2,"552"),
				(177,2,2,"520"),
				(178,2,2,"516"),
				(179,2,2,"572"),
				(180,2,2,"1516"),
				(181,2,2,"572"),
				(182,2,2,"1512"),
				(183,2,2,"524"),
				(184,2,2,"536"),
				(185,2,2,"548"),
				(186,2,2,"1548"),
				(187,2,2,"548"),
				(188,2,2,"1576"),
				(189,2,2,"520"),
				(190,2,2,"548"),
				(191,2,2,"496"),
				(192,2,2,"580"),
				(193,2,2,"496"),
				(194,2,2,"564"),
				(195,2,2,"544"),
				(196,2,2,"1516"),
				(197,2,2,"576"),
				(198,2,2,"1512"),
				(199,2,2,"572"),
				(200,2,2,"1520"),
				(201,2,2,"524"),
				(202,2,2,"540"),
				(203,2,2,"520"),
				(204,2,2,"568"),
				(205,2,2,"520"),
				(206,2,2,"548"),
				(207,2,2,"500"),
				(208,2,2,"556"),
				(209,2,2,"540"),
				(210,2,2,"540"),
				(211,2,2,"548"),
				(212,2,2,"1512"),
				(213,2,2,"524"),
				(214,2,2,"532"),
				(215,2,2,"520"),
				(216,2,2,"564"),
				(217,2,2,"524"),
				(218,2,2,"1548"),
				(219,2,2,"544"),
				(220,2,2,"544"),
				(221,2,2,"496"),
				(222,2,2,"572"),
				(223,2,2,"500"),
				(224,2,2,"552"),
				(225,2,2,"520"),
				(226,2,2,"564"),
				(227,2,2,"496"),
				(228,2,2,"528"),
				(229,2,2,"552"),
				(230,2,2,"1560"),
				(231,2,2,"528"),
				(232,2,2,"1544"),
				(233,2,2,"520"),
				(234,2,2,"540"),
				(235,2,2,"548"),
				(236,2,2,"1556"),
				(237,2,2,"520"),
				(238,2,2,"544"),
				(239,2,2,"548"),
				(240,2,2,"1584"),
				(241,2,2,"520"),
				(242,2,2,"508"),
				(243,2,2,"492");
/*!40000 ALTER TABLE `salaparticular` ENABLE KEYS */;


/*!40000 ALTER TABLE `Operacao` DISABLE KEYS */;
INSERT INTO `operacao` (`Id`, `Titulo`,`Descricao`) VALUES 
				(1,"Ligar","Liga Dispositivo"),
				(2,"Desligar","Desliga Dispotivo");
/*!40000 ALTER TABLE `salaparticular` ENABLE KEYS */;
