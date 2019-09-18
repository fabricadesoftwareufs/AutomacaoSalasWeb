const context = require('./context')
const UsuarioOrganizacoes = require('../models/UsuarioOrganizacoes')
const paginate = require('mongoose-paginate')

const Schema = new context.Schema(UsuarioOrganizacoes);
Schema.plugin(paginate);

const Persistence = context.model('usuarioorganizacoes', Schema);

module.exports = Persistence;