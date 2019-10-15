const context = require('./context')
const Hardware = require('../models/Hardware')
const paginate = require('mongoose-paginate')

const Schema = new context.Schema(Hardware)
Schema.plugin(paginate)

const Persistence = context.model('hardware', Schema)

module.exports = Persistence;