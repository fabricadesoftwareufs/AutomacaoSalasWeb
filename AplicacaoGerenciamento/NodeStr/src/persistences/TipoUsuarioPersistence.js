const context = require('./context')
const TipoUsuario = require('../models/TipoUsuario')
const paginate = require('mongoose-paginate')

// Schema que será criado no banco efetivamente
const Schema = new context.Schema(TipoUsuario);
Schema.plugin(paginate);

const Persistence = context.model('tipousuario', Schema);

module.exports = Persistence;