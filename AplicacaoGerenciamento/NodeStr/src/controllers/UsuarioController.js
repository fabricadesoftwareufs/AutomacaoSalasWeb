const PersistenciaUsuario = require('../persistences/UsuarioPersistence')

module.exports = {
    async GetAll(req, res) {
        const { page = 1 } = req.query;
        await PersistenciaUsuario
            .paginate({}, { page, limit: 10 })
            .then((usuarios) => {
                return res.json(usuarios);
            })
            .catch(err => {
                console.log(err);
                return res.status(400).send('Ocorreu um erro na solicitação!');
            });
    },
    async GetById(req, res) {
        return res.json(await PersistenciaUsuario.findById(req.params.id))
    },
    async Insert(req, res) {
        await PersistenciaUsuario
            .create(req.body)
            .then((usuario) => {
                return res.json(usuario);
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
        // O new: true, força o mongoose a retornar o objeto já atualizado
        // caso nao passe o new, ele retorna o objeto antes de ser atualizado.
        return res.json(await PersistenciaUsuario.findByIdAndUpdate(req.params.id, req.body, { new: true }));
    },
    async Delete(req, res) {
        await PersistenciaUsuario.deleteOne(req.params.id)

        // Retornando um OK vazio.
        return res.send()
    }
}