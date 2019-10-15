const express = require('express')
const routerBloco = express.Router()

// Controllers
const BlocoController = require('../controllers/BlocoController');

// Rotas de Products
routerBloco.get('/', BlocoController.GetAll)
routerBloco.get('/:id', BlocoController.GetById)
routerBloco.post('/', BlocoController.Insert)
routerBloco.put('/:id', BlocoController.Update)
routerBloco.delete('/:id', BlocoController.Delete)

module.exports = routerBloco;