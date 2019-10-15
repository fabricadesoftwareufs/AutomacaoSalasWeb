const context = require('./context')
const Bloco = require('../models/Bloco')
const paginate = require('mongoose-paginate')

const Schema = new context.Schema(Bloco)
Schema.plugin(paginate)

const Persistence = context.model('bloco', Schema)

module.exports = Persistence;