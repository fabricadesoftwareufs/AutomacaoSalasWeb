const PersistenciaBloco = require('../persistences/BlocoPersistence')

module.exports = {
    async GetAll(req, res) {
        const { page = 1 } = req.query;
        await PersistenciaBloco
            .paginate({}, { page, limit: 10 })
            .then((bloco) => {
                return res.json(bloco);
            })
            .catch(err => {
                console.log(err);
                return res.status(400).send('Ocorreu um erro na solicitação!');
            });
           
    },
    
    async GetById(req, res) {
        return res.json(await PersistenciaBloco.findById(req.params.id))
    },

    async Insert(req, res) {
        await PersistenciaBloco
        .create(req.body)
        .then((bloco) => {
            return res.json(bloco)
        }).catch(err => {
            console.log(err)
            return res.status(400).send('Solicitação não Atendida!!')
        });
    },

    async Update(req, res) {
        return res.json(await PersistenciaBloco.findByIdAndUpdate(req.params.id, req.body, { new: true }));
    },

    async Delete(req, res) {
        await PersistenciaBloco.deleteOne(req.params.id)

        // Retornando um OK vazio.
        return res.send()
    }
}