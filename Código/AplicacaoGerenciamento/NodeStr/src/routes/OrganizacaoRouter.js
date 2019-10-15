const express = require('express')
const routerOrganizacao = express.Router()

// Controllers
const OrganizacaoController = require('../controllers/OrganizacaoController');

// Rotas de Products
routerOrganizacao.get('/', OrganizacaoController.GetAll)
routerOrganizacao.get('/:id', OrganizacaoController.GetById)
routerOrganizacao.post('/', OrganizacaoController.Insert)
routerOrganizacao.put('/:id', OrganizacaoController.Update)
routerOrganizacao.delete('/:id', OrganizacaoController.Delete)

module.exports = routerOrganizacao;