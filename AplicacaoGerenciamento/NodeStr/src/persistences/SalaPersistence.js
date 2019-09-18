const context = require('./context')
const Sala = require('../models/Sala')
const paginate = require('mongoose-paginate')

const Schema = new context.Schema(Sala)
Schema.plugin(paginate)

const Persistence = context.model('sala', Schema)

module.exports = Persistence;