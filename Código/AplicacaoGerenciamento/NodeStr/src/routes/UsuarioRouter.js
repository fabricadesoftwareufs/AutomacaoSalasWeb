// Imports
const express = require('express');
const routerUsuario = express.Router();

// Controllers
const UsuarioController = require('../controllers/UsuarioController');

// Rotas de Products
routerUsuario.get('/', UsuarioController.GetAll)
routerUsuario.get('/:id', UsuarioController.GetById)
routerUsuario.post('/', UsuarioController.Insert)
routerUsuario.put('/:id', UsuarioController.Update)
routerUsuario.delete('/:id', UsuarioController.Delete)

module.exports = routerUsuario;