const context = require('./context')
const Disciplina = require('../models/Disciplina')
const paginate = require('mongoose-paginate')

const Schema = new context.Schema(Disciplina)
Schema.plugin(paginate)

const Persistence = context.model('disciplinas', Schema)

module.exports = Persistence;