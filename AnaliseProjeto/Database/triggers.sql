USE `automacaosalas`;

-- Trigger para inserção
DELIMITER //

CREATE TRIGGER after_insert_organizacao
AFTER INSERT ON organizacao
FOR EACH ROW
BEGIN
    -- Insere o ID da organização recém criada na tabela tipohardware
    INSERT INTO `tipohardware` (`descricao`,`idOrganizacao`) VALUES
    ('CONTROLADOR DE SALA',NEW.id),
    ('MODULO DE SENSORIAMENTO',NEW.id),
    ('MODULO DE DISPOSITIVO',NEW.id);
END //

-- Trigger para deleção
CREATE TRIGGER before_delete_organizacao
BEFORE DELETE ON organizacao
FOR EACH ROW
BEGIN
    -- Remove todos os tipos de hardware associados à organização
    DELETE FROM tipohardware WHERE idOrganizacao = OLD.id;
END //

DELIMITER ;


