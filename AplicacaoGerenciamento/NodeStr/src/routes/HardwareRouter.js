const express = require('express')
const routerHardware = express.Router()

// Controllers
const HardwareController = require('../controllers/HardwareController');

// Rotas de Products
routerHardware.get('/', HardwareController.GetAll)
routerHardware.get('/:id', HardwareController.GetById)
routerHardware.post('/', HardwareController.Insert)
routerHardware.put('/:id', HardwareController.Update)
routerHardware.delete('/:id', HardwareController.Delete)

module.exports = routerHardware;