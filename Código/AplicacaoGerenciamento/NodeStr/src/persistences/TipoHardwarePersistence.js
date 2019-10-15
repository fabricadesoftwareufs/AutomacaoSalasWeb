const context = require('./context')
const TipoHardware = require('../models/TipoHardware')
const paginate = require('mongoose-paginate')

const Schema = new context.Schema(TipoHardware)
Schema.plugin(paginate)

const Persistence = context.model('tipohardware', Schema)

module.exports = Persistence;