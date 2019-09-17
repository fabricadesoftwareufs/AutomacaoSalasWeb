const express = require('express');
const bodyParser = require('body-parser');

// Instanciando o servidor
const app = express();

// Configurações do Servidor
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

// Removendo o header que informa o driver utilizado no sistema
app.disable('x-powered-by');

// Rotas
app.use('/api/usuario', require('./src/routes/UsuarioRouter'))
app.use('/api/tipoUsuario', require('./src/routes/TipoUsuarioRouter'))
app.use('/api/disciplina', require('./src/routes/DisciplinaRouter'))

// Porta
app.listen(20074);