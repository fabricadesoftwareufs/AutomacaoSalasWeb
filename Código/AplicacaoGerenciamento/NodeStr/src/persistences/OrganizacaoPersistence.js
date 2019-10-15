const context = require('./context')
const Organizacao = require('../models/Organizacao')
const paginate = require('mongoose-paginate')

const Schema = new context.Schema(Organizacao)
Schema.plugin(paginate)

const Persistence = context.model('organizacao', Schema)

module.exports = Persistence;