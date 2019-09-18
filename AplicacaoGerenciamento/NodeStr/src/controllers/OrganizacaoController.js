const PersistenciaOrganizacao = require('../persistences/OrganizacaoPersistence')

module.exports = {
    async GetAll(req, res) {
        const { page = 1 } = req.query;
        await PersistenciaOrganizacao
            .paginate({}, { page, limit: 10 })
            .then((organizacao) => {
                return res.json(organizacao);
            })
            .catch(err => {
                console.log(err);
                return res.status(400).send('Ocorreu um erro na solicitação!');
            });
           
    },
    async GetById(req, res) {
        return res.json(await PersistenciaOrganizacao.findById(req.params.id))
    },
    async Insert(req, res) {
        await PersistenciaOrganizacao
        .create(req.body)
        .then((organizacao) => {
            return res.json(organizacao)
        }).catch(err => {
            console.log(err)
            return res.status(400).send('Solicitação não Atendida!!')
        });
    },
    async Update(req, res) {
        return res.json(await PersistenciaOrganizacao.findByIdAndUpdate(req.params.id, req.body, { new: true }));
    },
    async Delete(req, res) {
        await PersistenciaOrganizacao.deleteOne(req.params.id)

        // Retornando um OK vazio.
        return res.send()
    }
}