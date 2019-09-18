const express = require('express')
const routerTipoHardware = express.Router()

// Controllers
const TipoHardwareController = require('../controllers/TipoHardwareController');

// Rotas de Products
routerTipoHardware.get('/', TipoHardwareController.GetAll)
routerTipoHardware.get('/:id', TipoHardwareController.GetById)
routerTipoHardware.post('/', TipoHardwareController.Insert)
routerTipoHardware.put('/:id', TipoHardwareController.Update)
routerTipoHardware.delete('/:id', TipoHardwareController.Delete)

module.exports = routerTipoHardware;