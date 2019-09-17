const express = require('express')
const routerDisciplina = express.Router()

// Controllers
const DisciplinaController = require('../controllers/DisciplinaController');

// Rotas de Products
routerDisciplina.get('/', DisciplinaController.GetAll)
routerDisciplina.get('/:id', DisciplinaController.GetById)
routerDisciplina.post('/', DisciplinaController.Insert)
routerDisciplina.put('/:id', DisciplinaController.Update)
routerDisciplina.delete('/:id', DisciplinaController.Delete)

module.exports = routerDisciplina;