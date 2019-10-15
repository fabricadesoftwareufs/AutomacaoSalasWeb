// Imports
const express = require('express');
const routerTipoUsuario = express.Router();

// Controllers
const TipoUsuarioController = require('../controllers/TipoUsuarioController');

// Rotas de Products
routerTipoUsuario.get('/', TipoUsuarioController.GetAll)
routerTipoUsuario.get('/:id', TipoUsuarioController.GetById)
routerTipoUsuario.post('/', TipoUsuarioController.Insert)
routerTipoUsuario.put('/:id', TipoUsuarioController.Update)
routerTipoUsuario.delete('/:id', TipoUsuarioController.Delete)

module.exports = routerTipoUsuario;