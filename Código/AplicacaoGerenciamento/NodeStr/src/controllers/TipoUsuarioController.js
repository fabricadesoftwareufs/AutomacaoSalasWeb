const PersistenciaTipoUsuario = require('../persistences/TipoUsuarioPersistence')

module.exports = {
    async GetAll(req, res) {
        const { page = 1 } = req.query;
        await PersistenciaTipoUsuario
            .paginate({}, { page, limit: 10 })
            .then((tipoUsuarios) => {
                return res.json(tipoUsuarios);
            })
            .catch(err => {
                console.log(err);
                return res.status(400).send('Ocorreu um erro na solicitação!');
            });
    },
    async GetById(req, res) {
        return res.json(await PersistenciaTipoUsuario.findById(req.params.id))
    },
    async Insert(req, res) {
        await PersistenciaTipoUsuario
            .create(req.body)
            .then((tipoUsuario) => {
                return res.json(tipoUsuario);
            })
            .catch(err => {
                // Capturando erro de campos duplicados!
                let duplicateInputs = [];
                if (err.errmsg.includes('duplicate key error'))
                    for (let key in err.keyValue)
                        duplicateInputs.push(key)

                // Retorno com o campo duplicado
                return res.status(400).send(`Campo ${duplicateInputs[0]} duplicado!`);
            });
    },
    async Update(req, res) {
        return res.json(await PersistenciaTipoUsuario.updateOne(req.params.id, req.body, { new: true }));
    },
    async Delete(req, res) {
        await PersistenciaTipoUsuario.deleteOne(req.params.id)

        // Retornando um OK vazio.
        return res.send()
    }
}