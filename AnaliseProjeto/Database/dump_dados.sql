USE `automacaosalas`;

-- Copiando dados para a tabela automacaosalas.organizacao: ~4 rows (aproximadamente)
INSERT INTO `organizacao` (`cnpj`, `razaoSocial`) VALUES
	('08735240000146', 'UNIVERSIDADE FEDERAL DE SERGIPE - UFS'),
	('57838165000154', 'UNIVERSIDADE TIRADENTES - UNIT'),
	('30056954000187', 'MINISTÉRIO PÚBLICO DE SERGIPE - MPSE'),
	('50618535000107', 'PREFEITURA MUNICIPAL DE ARACAJU');

-- Copiando dados para a tabela automacaosalas.bloco: ~13 rows (aproximadamente)
INSERT INTO `bloco` (`idOrganizacao`, `titulo`) VALUES
	(1, 'Bloco A'),
	(1, 'Bloco B'),
	(1, 'Bloco C'),
	(1, 'Bloco D'),
	(1, 'Bloco E'),
	(2, 'Bloco SI'),
	(2, 'Bloco ADM'),
	(2, 'Bloco SAUDE'),
	(2, 'Bloco BIO'),
	(3, 'Bloco X'),
	(3, 'Bloco Y'),
	(3, 'Bloco Z'),
	(4, 'Bloco Unico');

-- Copiando dados para a tabela automacaosalas.sala: ~13 rows (aproximadamente)
INSERT INTO `sala` (`titulo`, `idBloco`) VALUES
	('Sala 01', 1),
	('Sala 02', 2),
	('Sala 03', 3),
	('Sala 04', 4),
	('Sala 05', 5),
	('Sala 06', 6),
	('Sala 07', 7),
	('Sala 08', 8),
	('Sala 09', 9),
	('Sala 10', 10),
	('Sala 11', 11),
	('Sala 12', 12),
	('Sala 13', 13),
	('Sala 106', 4);


-- Copiando dados para a tabela automacaosalas.tipohardware: ~3 rows (aproximadamente)
INSERT INTO `tipohardware` (`descricao`,`idOrganizacao`) VALUES
	('CONTROLADOR DE SALA',1),
	('MODULO DE SENSORIAMENTO',1),
	('MODULO DE DISPOSITIVO',1),
	('CONTROLADOR DE SALA',2),
	('MODULO DE SENSORIAMENTO',2),
	('MODULO DE DISPOSITIVO',2),
	('CONTROLADOR DE SALA',3),
	('MODULO DE SENSORIAMENTO',3),
	('MODULO DE DISPOSITIVO',3),
	('CONTROLADOR DE SALA',4),
	('MODULO DE SENSORIAMENTO',4),
	('MODULO DE DISPOSITIVO',4);

    -- (1, 'MODULO DE DISPOSITIVO'),
	-- (2, 'CONTROLADOR DE SALA'),
	-- (3, 'MODULO DE SENSORIAMENTO');





-- Copiando dados para a tabela automacaosalas.tipousuario: ~2 rows (aproximadamente)
INSERT INTO `tipousuario` (`descricao`) VALUES
	('ADMIN'),
	('GESTOR'),
	('COLABORADOR'),
	('PENDENTE');


-- Copiando dados para a tabela automacaosalas.usuario: ~2 rows (aproximadamente)
INSERT INTO `usuario` (`cpf`, `nome`, `dataNascimento`) VALUES
	('42112664204', 'Lívia Benedita Rebeca Araújo', '1997-08-15'),
	('57377766387', 'Rafael Kevin Teixeira', '1996-07-22'),
	('07852892590', 'Igor bruno dos santos nascimento', '1996-07-22'),
	('07334824571', 'Abraao Alves', '1998-06-06'),
    ('12345678909',	'ADM Genérico',	'2001-01-01');

-- Copiando dados para a tabela automacaosalas.usuarioorganizacao: ~0 rows (aproximadamente)
INSERT INTO `usuarioorganizacao` (`idOrganizacao`, `idUsuario`,`idTipoUsuario`) VALUES
	(1, 1, 2),
	(1, 3, 2),
	(1, 2, 2),
	(1, 4, 1),
	(1, 5, 1);


-- Copiando dados para a tabela automacaosalas.horariosala: ~0 rows (aproximadamente)
INSERT INTO `horariosala` (`data`, `horarioInicio`,`horarioFim`,`objetivo`,`idUsuario`,`idSala`,`idPlanejamento`) VALUES
							('2020-08-24', '07:00','09:00','Palestra sobre Engenharia de Software',1,4,null),
							('2020-09-20', '07:00','09:00','Palestra sobre Engenharia de Software',2,4,null);

-- Copiando dados para a tabela automacaosalas.planejamento: ~0 rows (aproximadamente)
INSERT INTO `planejamento` (`dataInicio`, `dataFim`,`horarioInicio`,`horarioFim`,`diaSemana`,`objetivo`,`idUsuario`,`idSala`) VALUES
	('2020-08-24', '2020-12-24','07:00','09:00','SEG','Planejamento de LFT',1,5),
	('2020-08-24', '2020-12-24','07:00','09:00','QUA','Planejamento de LFT',1,5),
	('2020-08-24', '2020-12-24','07:00','09:00','SEX','Planejamento de LFT',1,5);

-- Copiando dados para a tabela automacaosalas.salaparticular: ~0 rows (aproximadamente)
INSERT INTO `salaparticular` (`idUsuario`,`idSala`) VALUES
				(5,1),
				(2,2),
				(2,4),
				(2,1),
				(1,3);

	-- Copiando dados para a tabela automacaosalas.hardwaredesala: ~39 rows (aproximadamente)
INSERT INTO `hardwaredesala` (`mac`,`ip`, `idSala`, `idTipoHardware`,`uuid`, `token`) VALUES
	('00:E0:4C:27:56:96','192.168.15.233',  1, 1,NULL, '594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:5A:D1:DD', '192.168.15.233', 1, 2,NULL, '594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:50:B0:3E','192.168.15.233',  2, 1,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:50:B0:3E','192.168.15.233', 2, 2,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:66:82:0A', '192.168.15.233', 3, 1,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:59:A4:20','192.168.15.233',  3, 2,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:15:F6:A0', '192.168.15.233', 4, 1,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:1B:B5:35', '192.168.15.233', 4, 2,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:4E:A9:64', '192.168.15.233', 5, 1,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:04:B6:9E','192.168.15.233',  5, 2,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:3A:E1:F0','192.168.15.233',  6, 4,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:46:B6:13', '192.168.15.233', 6, 5,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:15:AA:D1', '192.168.15.233', 7, 4,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:71:5B:0B', '192.168.15.233', 7, 5,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:3B:3F:9F','192.168.15.233',  8, 4,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:31:19:0B', '192.168.15.233', 8, 5,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:5A:D1:DD', '192.168.15.233', 1, 3,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:32:A1:93', '192.168.15.233', 2, 3,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:04:56:7B', '192.168.15.233', 3, 3,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:32:F0:A6', '192.168.15.233', 4, 3,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:43:7C:3E', '192.168.15.233', 5, 3,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:7B:D9:3E', '192.168.15.233', 6, 6,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:7A:3A:A4', '192.168.15.233', 7, 6,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:5B:DD:23', '192.168.15.233', 8, 6,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:08:72:05', '192.168.15.233', 9, 4,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:33:61:87', '192.168.15.233', 9, 5,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:39:C1:CC', '192.168.15.233', 9, 6,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:44:06:E4', '192.168.15.233', 10, 7,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:1B:38:1F', '192.168.15.233', 10, 8,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:1C:C2:42', '192.168.15.233', 10, 9,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:26:F7:80', '192.168.15.233', 11, 7,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:7B:75:7F', '192.168.15.233', 11, 8,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:41:AA:BD', '192.168.15.233', 11, 9,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:7A:16:3A', '192.168.15.233', 12, 7,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:4B:8A:65', '192.168.15.233', 12, 8,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:43:85:71', '192.168.15.233', 12, 9,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:5F:B4:08', '192.168.15.233', 13, 10,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:21:50:B0', '192.168.15.233', 13, 11,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe'),
	('00:E0:4C:28:5F:41', '192.168.15.233', 13, 12,NULL,'594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe');

INSERT INTO marcaequipamento(nome) VALUES
     ('SAMSUNG'),
     ('ELGIN'),
     ('LG');

INSERT INTO `modeloequipamento`(nome, idMarcaEquipamento) VALUES
    ('220TT',1),
    ('SGV330',2),
    ('EXTENDER',3);

INSERT INTO `equipamento` (`descricao`,`idSala`,`idHardwareDeSala`,`idModeloEquipamento`) VALUES
	('Condicionador SAMSUNG 110V classe de consumo E',1,17,1),
	('Condicionador ELGIN 110V classe de consumo G',2,18,2),
	('Condicionador LG 110V classe de consumo G',3,19,3);

INSERT INTO `equipamento` (idSala, descricao, tipoEquipamento, idHardwareDeSala) VALUES
    (1, 'Luminum', 'LUZES',1);

INSERT INTO `operacao` (`id`, `titulo`,`descricao`) VALUES
				(1,'Ligar','Liga Dispositivo'),
				(2,'Desligar','Desliga Dispotivo');

INSERT INTO `codigoinfravermelho` (`idModeloEquipamento`, `idOperacao`, `codigo`) VALUES
	(1, 2, '460,264,184,256,182,584,184,422,184,256,182,584,186,258,182,256,182,256,184,586,184,584,186,420,184,584,186,280,158,282,160,748,186,264,188'),
	(2, 2, '8412,4232,496,556,548,1484,580,1568,524,536,528,1572,524,544,528,1580,520,540,496,556,524,1568,520,536,500,560,552,1548,552,1552,552,1552,524,540,496,556,500,552,500,560,524,516,520,568,496,572,524,552,524,536,496,556,520,512,520,560,500,564,496,568,500,572,496,576,496,544,516,556,496,560,496,540,516,568,496,568,552,1528,544,552,500,560,496,556,520,512,524,536,516,568,500,544,516,576,524,1556,548,1548,516,560,496,556,524,536,496,544,516,572,500,548,520,576,496,564,492,560,496,556,500,560,496,568,496,544,520,572,500,576,496,564,496,556,496,556,548,1520,576,1544,524,544,496,572,496,576,500,560,500,552,500,556,496,564,520,540,496,572,496,572,496,576,496,512,576,508,516,560,524,1568,552,1520,524,564,500,548,544,552,520,516,572,1516,572,1512,524,536,548,1548,548,1576,520,548,496,580,496,564,544,1516,576,1512,572,1520,524,540,520,568,520,548,500,556,540,540,548,1512,524,532,520,564,524,1548,544,544,496,572,500,552,520,564,496,528,552,1560,528,1544,520,540,548,1556,520,544,548,1584,520,508,492'),
	(1, 1, '9077,4438,612,522,610,522,610,1640,610,524,610,522,610,522,608,524,584,548,584,1666,586,1666,586,548,584,1666,586,1666,586,1666,586,1668,584,1668,586,548,584,548,584,548,584,1666,586,548,584,548,586,548,586,546,586,1666,588,1664,586,1666,588,546,586,1666,586,1666,586,1666,588,1664,586,39900,9028,2226,584'),
    (3, 1, '461,264,184,256,182,584,184,422,184,256,182,584,186,258,182,256,182,256,184,586,184,584,186,420,184,584,186,280,158,282,160,748,186,264,188'),
	(3, 2, '8412,4232,496,556,548,1484,580,1568,524,536,528,1572,524,544,528,1580,520,540,496,556,524,1568,520,536,500,560,552,1548,552,1552,552,1552,524,540,496,556,500,552,500,560,524,516,520,568,496,572,524,552,524,536,496,556,520,512,520,560,500,564,496,568,500,572,496,576,496,544,516,556,496,560,496,540,516,568,496,568,552,1528,544,552,500,560,496,556,520,512,524,536,516,568,500,544,516,576,524,1556,548,1548,516,560,496,556,524,536,496,544,516,572,500,548,520,576,496,564,492,560,496,556,500,560,496,568,496,544,520,572,500,576,496,564,496,556,496,556,548,1520,576,1544,524,544,496,572,496,576,500,560,500,552,500,556,496,564,520,540,496,572,496,572,496,576,496,512,576,508,516,560,524,1568,552,1520,524,564,500,548,544,552,520,516,572,1516,572,1512,524,536,548,1548,548,1576,520,548,496,580,496,564,544,1516,576,1512,572,1520,524,540,520,568,520,548,500,556,540,540,548,1512,524,532,520,564,524,1548,544,544,496,572,500,552,520,564,496,528,552,1560,528,1544,520,540,548,1556,520,544,548,1584,520,508,492'),
    (2, 1, '8469,4232,496,556,548,1484,580,1568,524,536,528,1572,524,544,528,1580,520,540,496,556,524,1568,520,536,500,560,552,1548,552,1552,552,1552,524,540,496,556,500,552,500,560,524,516,520,568,496,572,524,552,524,536,496,556,520,512,520,560,500,564,496,568,500,572,496,576,496,544,516,556,496,560,496,540,516,568,496,568,552,1528,544,552,500,560,496,556,520,512,524,536,516,568,500,544,516,576,524,1556,548,1548,516,560,496,556,524,536,496,544,516,572,500,548,520,576,496,564,492,560,496,556,500,560,496,568,496,544,520,572,500,576,496,564,496,556,496,556,548,1520,576,1544,524,544,496,572,496,576,500,560,500,552,500,556,496,564,520,540,496,572,496,572,496,576,496,512,576,508,516,560,524,1568,552,1520,524,564,500,548,544,552,520,516,572,1516,572,1512,524,536,548,1548,548,1576,520,548,496,580,496,564,544,1516,576,1512,572,1520,524,540,520,568,520,548,500,556,540,540,548,1512,524,532,520,564,524,1548,544,544,496,572,500,552,520,564,496,528,552,1560,528,1544,520,540,548,1556,520,544,548,1584,520,508,492');

INSERT INTO `monitoramento` (idEquipamento, idOperacao, idUsuario,estado)
VALUES
    (1, 1,5,1),  -- Condicionador SAMSUNG no estado 'ligado'
    (2, 2,5,0),  -- Condicionador ELGIN no estado 'desligado'
    (3, 1,5,1),  -- Condicionador LG no estado 'ligado'
    (4, 2,5,0);  -- Luzes estão no estado 'desligado'

INSERT INTO `conexaointernet`(nome, senha, idBloco)
VALUES
    ('Lab2','g522W28MTRFCpFo3pD/7rw==',1),
    ('Lab1','/aTrZryt+D2WsDt82on5KA==',1),
    ('Lab3','wjCM/P1LGWR9b007RJUnsQ==',1),
    ('Lab4','g522W28MTRFCpFo3pD/7rw==',6);