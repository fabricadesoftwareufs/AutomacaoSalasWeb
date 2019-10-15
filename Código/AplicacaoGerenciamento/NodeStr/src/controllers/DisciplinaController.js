const PersistenciaDisciplina = require('../persistences/DisciplinaPersistence')

module.exports = {
    async GetAll(req, res) {
        const { page = 1 } = req.query;
        return res.json(await PersistenciaDisciplina.paginate({}, { page, limit: 10 }))
    },
    async GetById(req, res) {
        return res.json(await PersistenciaDisciplina.findById(req.params.id))
    },
    async Insert(req, res) {
        await PersistenciaDisciplina
            .create(req.body)
            .then((disciplina) => {
                return res.json(disciplina)
            }).catch(err => {
                console.log(err)
                return res.status(400).send('Solicitação não Atendida!!')
            });
    },
    async Update(req, res) {
        return res.send(await PersistenciaDisciplina.findByIdAndUpdate(req.params.id, req.body, { new: true }))
    },
    async Delete(req, res) {
        await PersistenciaDisciplina.findByIdAndDelete(req.params.id)
        return res.send()
    }
}