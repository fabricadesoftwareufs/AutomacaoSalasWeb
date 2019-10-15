const PersistenciaSala = require('../persistences/SalaPersistence')

module.exports = {
    async GetAll(req, res) {
        const { page = 1 } = req.query;
        await PersistenciaSala
            .paginate({}, { page, limit: 10 })
            .then((sala) => {
                return res.json(sala);
            })
            .catch(err => {
                console.log(err);
                return res.status(400).send('Ocorreu um erro na solicitação!');
            });
           
    },
    async GetById(req, res) {
        return res.json(await PersistenciaSala.findById(req.params.id))
    },
    async Insert(req, res) {
        await PersistenciaSala
        .create(req.body)
        .then((sala) => {
            return res.json(sala)
        }).catch(err => {
            console.log(err)
            return res.status(400).send('Solicitação não Atendida!!')
        });
    },
    async Update(req, res) {
        return res.json(await PersistenciaSala.findByIdAndUpdate(req.params.id, req.body, { new: true }));
    },
    async Delete(req, res) {
        await PersistenciaSala.deleteOne(req.params.id)
        
        // Retornando um OK vazio.
        return res.send()
    }
}