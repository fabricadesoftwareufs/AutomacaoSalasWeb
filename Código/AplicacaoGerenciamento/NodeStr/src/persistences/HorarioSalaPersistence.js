const context = require('./context')
const HorarioSala = require('../models/HorarioSala')
const paginate = require('mongoose-paginate')

// Schema que será criado no banco efetivamente
const Schema = new context.Schema(HorarioSala);
Schema.plugin(paginate);

const Persistence = context.model('horariosala', Schema);

module.exports = Persistence;