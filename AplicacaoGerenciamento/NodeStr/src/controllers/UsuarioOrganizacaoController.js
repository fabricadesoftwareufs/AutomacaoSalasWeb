const PersistenciaUsuarioOrganizacao = require('../persistences/UsuarioOrganizacoesPersistence')

module.exports = {
    async GetAll(req, res) {
        const { page = 1 } = req.query;
        await PersistenciaUsuarioOrganizacao
            .paginate({}, { page, limit: 10 })
            .then((usuarioorganizacao) => {
                return res.json(usuarioorganizacao);
            })
            .catch(err => {
                console.log(err);
                return res.status(400).send('Ocorreu um erro na solicitação!');
            });
           
    },
    async GetById(req, res) {
        return res.json(await PersistenciaUsuarioOrganizacao.findById(req.params.id))
    },
    async Insert(req, res) {
        await PersistenciaUsuarioOrganizacao
        .create(req.body)
        .then((usuarioorganizacao) => {
            return res.json(usuarioorganizacao)
        }).catch(err => {
            console.log(err)
            return res.status(400).send('Solicitação não Atendida!!')
        });
    },
    async Update(req, res) {
        return res.json(await PersistenciaUsuarioOrganizacao.findByIdAndUpdate(req.params.id, req.body, { new: true }));
    },
    async Delete(req, res) {
        await PersistenciaUsuarioOrganizacao.deleteOne(req.params.id)
        
        // Retornando um OK vazio.
        return res.send()
    }
}