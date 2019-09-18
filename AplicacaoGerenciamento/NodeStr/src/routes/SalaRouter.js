const express = require('express')
const routerSala = express.Router()

// Controllers
const SalaController = require('../controllers/SalaController');

// Rotas de Products
routerSala.get('/', SalaController.GetAll)
routerSala.get('/:id', SalaController.GetById)
routerSala.post('/', SalaController.Insert)
routerSala.put('/:id', SalaController.Update)
routerSala.delete('/:id', SalaController.Delete)

module.exports = routerSala;