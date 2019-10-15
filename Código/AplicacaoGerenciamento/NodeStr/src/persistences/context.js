const moongoose = require('mongoose')

moongoose.connect(
    'mongodb://localhost/nodeApiSTR',
    {
        useNewUrlParser: true,
        useCreateIndex: true,
        useUnifiedTopology: true,
        useCreateIndex: true,
        useFindAndModify: false
    });
moongoose.Promise = global.Promise;

module.exports = moongoose;