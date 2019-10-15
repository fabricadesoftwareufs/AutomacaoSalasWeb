const context = require('./context')
const Usuario = require('../models/Usuario')
const paginate = require('mongoose-paginate')

// Schema que será criado no banco efetivamente
const Schema = new context.Schema(Usuario);
Schema.plugin(paginate);

const Persistence = context.model('usuarios', Schema);

module.exports = Persistence;