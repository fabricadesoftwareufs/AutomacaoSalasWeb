const express = require('express')
const routerHoarioSala = express.Router()

// Controllers
const HorarioSalaController = require('../controllers/HorarioSalaController');

// Rotas de Products
routerHoarioSala.get('/', HorarioSalaController.GetAll)
routerHoarioSala.get('/:id', HorarioSalaController.GetById)
routerHoarioSala.post('/', HorarioSalaController.Insert)
routerHoarioSala.put('/:id', HorarioSalaController.Update)
routerHoarioSala.delete('/:id', HorarioSalaController.Delete)

module.exports = routerHoarioSala;