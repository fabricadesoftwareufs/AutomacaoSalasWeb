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

-- Copiando dados para a tabela str_db.hardwaredebloco: ~11 rows (aproximadamente)
/*!40000 ALTER TABLE `hardwaredebloco` DISABLE KEYS */;
INSERT INTO `hardwaredebloco` (`id`, `MAC`, `Bloco`, `TipoHardware`) VALUES
	(1, '00:E0:4C:1E:D9:14', 1, 3),
	(2, '00:E0:4C:2F:2D:6C', 2, 3),
	(3, '00:E0:4C:43:B4:48', 3, 3),
	(4, '00:E0:4C:01:9E:76', 4, 3),
	(5, '00:E0:4C:55:6C:99', 5, 3),
	(6, '00:E0:4C:75:B8:15', 6, 3),
	(7, '00:E0:4C:51:34:D1', 7, 3),
	(8, '00:E0:4C:7D:84:19', 8, 3),
	(9, '00:E0:4C:50:86:F5', 9, 3),
	(10, '00:E0:4C:7B:F5:06', 10, 3),
	(11, '00:E0:4C:18:50:68', 11, 3);
/*!40000 ALTER TABLE `hardwaredebloco` ENABLE KEYS */;

-- Copiando dados para a tabela str_db.hardwaredesala: ~39 rows (aproximadamente)
/*!40000 ALTER TABLE `hardwaredesala` DISABLE KEYS */;
INSERT INTO `hardwaredesala` (`Id`, `MAC`, `Sala`, `TipoHardware`) VALUES
	(1, '00:E0:4C:27:56:96', 1, 1),
	(2, '00:E0:4C:5A:D1:DD', 1, 2),
	(3, '00:E0:4C:50:B0:3E', 2, 1),
	(4, '00:E0:4C:30:E3:52', 2, 2),
	(5, '00:E0:4C:66:82:0A', 3, 1),
	(6, '00:E0:4C:59:A4:20', 3, 2),
	(7, '00:E0:4C:15:F6:A0', 4, 1),
	(8, '00:E0:4C:1B:B5:35', 4, 2),
	(9, '00:E0:4C:4E:A9:64', 5, 1),
	(10, '00:E0:4C:04:B6:9E', 5, 2),
	(11, '00:E0:4C:3A:E1:F0', 6, 1),
	(12, '00:E0:4C:46:B6:13', 6, 2),
	(13, '00:E0:4C:15:AA:D1', 7, 1),
	(14, '00:E0:4C:71:5B:0B', 7, 2),
	(15, '00:E0:4C:3B:3F:9F', 8, 1),
	(16, '00:E0:4C:31:19:0B', 8, 2),
	(17, '00:E0:4C:5A:D1:DD', 1, 3),
	(18, '00:E0:4C:32:A1:93', 2, 3),
	(19, '00:E0:4C:04:56:7B', 3, 3),
	(20, '00:E0:4C:32:F0:A6', 4, 3),
	(21, '00:E0:4C:43:7C:3E', 5, 3),
	(22, '00:E0:4C:7B:D9:3E', 6, 3),
	(23, '00:E0:4C:7A:3A:A4', 7, 3),
	(24, '00:E0:4C:5B:DD:23', 8, 3),
	(25, '00:E0:4C:08:72:05', 9, 1),
	(26, '00:E0:4C:33:61:87', 9, 2),
	(27, '00:E0:4C:39:C1:CC', 9, 3),
	(28, '00:E0:4C:44:06:E4', 10, 1),
	(29, '00:E0:4C:1B:38:1F', 10, 2),
	(30, '00:E0:4C:1C:C2:42', 10, 3),
	(31, '00:E0:4C:26:F7:80', 11, 1),
	(32, '00:E0:4C:7B:75:7F', 11, 2),
	(33, '00:E0:4C:41:AA:BD', 11, 3),
	(34, '00:E0:4C:7A:16:3A', 12, 1),
	(35, '00:E0:4C:4B:8A:65', 12, 2),
	(36, '00:E0:4C:43:85:71', 12, 3),
	(37, '00:E0:4C:5F:B4:08', 13, 1),
	(38, '00:E0:4C:21:50:B0', 13, 2),
	(39, '00:E0:4C:28:5F:41', 13, 3);
/*!40000 ALTER TABLE `hardwaredesala` ENABLE KEYS */;

-- Copiando dados para a tabela str_db.horariosala: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `horariosala` DISABLE KEYS */;
/*!40000 ALTER TABLE `horariosala` ENABLE KEYS */;

-- Copiando dados para a tabela str_db.organizacao: ~4 rows (aproximadamente)
/*!40000 ALTER TABLE `organizacao` DISABLE KEYS */;
INSERT INTO `organizacao` (`Id`, `Cnpj`, `RazaoSocial`) VALUES
	(1, '08735240000146', 'Empresa1'),
	(2, '57838165000154', 'Empresa2'),
	(3, '30056954000187', 'Empresa3'),
	(4, '50618535000107', 'Empresa4');
/*!40000 ALTER TABLE `organizacao` ENABLE KEYS */;

-- Copiando dados para a tabela str_db.planejamento: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `planejamento` DISABLE KEYS */;
/*!40000 ALTER TABLE `planejamento` ENABLE KEYS */;

-- Copiando dados para a tabela str_db.sala: ~13 rows (aproximadamente)
/*!40000 ALTER TABLE `sala` DISABLE KEYS */;
INSERT INTO `sala` (`Id`, `Titulo`, `Bloco`) VALUES
	(1, 'Sala 01', 1),
	(2, 'Sala 01', 2),
	(3, 'Sala 01', 3),
	(4, 'Sala 01', 4),
	(5, 'Sala 01', 5),
	(6, 'Sala 01', 6),
	(7, 'Sala 01', 7),
	(8, 'Sala 01', 8),
	(9, 'Sala 01', 9),
	(10, 'Sala 01', 10),
	(11, 'Sala 01', 11),
	(12, 'Sala 01', 12),
	(13, 'Sala 01', 13);
/*!40000 ALTER TABLE `sala` ENABLE KEYS */;

-- Copiando dados para a tabela str_db.salaparticular: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `salaparticular` DISABLE KEYS */;
/*!40000 ALTER TABLE `salaparticular` ENABLE KEYS */;

-- Copiando dados para a tabela str_db.tipohardware: ~3 rows (aproximadamente)
/*!40000 ALTER TABLE `tipohardware` DISABLE KEYS */;
INSERT INTO `tipohardware` (`Id`, `Descricao`) VALUES
	(1, 'Roteador'),
	(2, 'Ar Condicionado'),
	(3, 'Iluminacao');
/*!40000 ALTER TABLE `tipohardware` ENABLE KEYS */;

-- Copiando dados para a tabela str_db.tipousuario: ~2 rows (aproximadamente)
/*!40000 ALTER TABLE `tipousuario` DISABLE KEYS */;
INSERT INTO `tipousuario` (`Id`, `Descricao`) VALUES
	(1, 'ADM'),
	(2, 'Professor');
/*!40000 ALTER TABLE `tipousuario` ENABLE KEYS */;

-- Copiando dados para a tabela str_db.usuario: ~2 rows (aproximadamente)
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` (`Id`, `Cpf`, `Nome`, `DataNascimento`, `Senha`, `TipoUsuario`) VALUES
	(1, '42112664204', 'Lívia Benedita Rebeca Araújo', '1997-08-15', 'teste', 2),
	(2, '57377766387', 'Rafael Kevin Teixeira', '1996-07-22', 'teste', 2);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;

-- Copiando dados para a tabela str_db.usuarioorganizacao: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `usuarioorganizacao` DISABLE KEYS */;
/*!40000 ALTER TABLE `usuarioorganizacao` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
