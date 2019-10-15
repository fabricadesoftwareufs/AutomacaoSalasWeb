const express = require('express')
const routerUsuarioOrganizacoes = express.Router()

// Controllers
const UsuarioOrganizacoesController = require('../controllers/UsuarioOrganizacaoController');

// Rotas de Products
routerUsuarioOrganizacoes.get('/', UsuarioOrganizacoesController.GetAll)
routerUsuarioOrganizacoes.get('/:id', UsuarioOrganizacoesController.GetById)
routerUsuarioOrganizacoes.post('/', UsuarioOrganizacoesController.Insert)
routerUsuarioOrganizacoes.put('/:id', UsuarioOrganizacoesController.Update)
routerUsuarioOrganizacoes.delete('/:id', UsuarioOrganizacoesController.Delete)

module.exports = routerUsuarioOrganizacoes;