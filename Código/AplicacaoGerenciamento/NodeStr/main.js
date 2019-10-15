const express = require('express');
const bodyParser = require('body-parser');
const cors = require('cors');

// Instanciando o servidor
const app = express();

// Configurações do Servidor
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.use(cors());

// Removendo o header que informa o driver utilizado no sistema
app.disable('x-powered-by');

app.get('/', (req, res) => { return res.send() });

// Rotas
app.use('/api/bloco', require('./src/routes/BlocoRouter'))
app.use('/api/disciplina', require('./src/routes/DisciplinaRouter'))
app.use('/api/hardware', require('./src/routes/HardwareRouter'))
app.use('/api/horarioSala', require('./src/routes/HorarioSalaRouter'))
app.use('/api/organizacao', require('./src/routes/OrganizacaoRouter'))
app.use('/api/sala', require('./src/routes/SalaRouter'))
app.use('/api/tipoHardware', require('./src/routes/TipoHardware'))
app.use('/api/tipoUsuario', require('./src/routes/TipoUsuarioRouter'))
app.use('/api/usuarioOrganizacao', require('./src/routes/UsuarioOrganizacoesRouter'))
app.use('/api/usuario', require('./src/routes/UsuarioRouter'))

// Porta
app.listen(20074);