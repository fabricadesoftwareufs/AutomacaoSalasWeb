const PersistenciaHorarioSala = require('../persistences/HorarioSalaPersistence')

module.exports = {
    async GetAll(req, res) {
        const { page = 1 } = req.query;
        await PersistenciaHorarioSala
            .paginate({}, { page, limit: 10 })
            .then((horariosala) => {
                return res.json(horariosala);
            })
            .catch(err => {
                console.log(err);
                return res.status(400).send('Ocorreu um erro na solicitação!');
            });
           
    },
    async GetById(req, res) {
        return res.json(await PersistenciaHorarioSala.findById(req.params.id))
    },
    async Insert(req, res) {
        await PersistenciaHorarioSala
        .create(req.body)
        .then((horariosala) => {
            return res.json(horariosala)
        }).catch(err => {
            console.log(err)
            return res.status(400).send('Solicitação não Atendida!!')
        });
    },
    async Update(req, res) {
        return res.json(await PersistenciaHorarioSala.findByIdAndUpdate(req.params.id, req.body, { new: true }));
    },
    async Delete(req, res) {
        await PersistenciaHorarioSala.deleteOne(req.params.id)

        // Retornando um OK vazio.
        return res.send()
    }
}