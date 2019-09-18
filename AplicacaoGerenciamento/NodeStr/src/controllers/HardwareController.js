const PersistenciaHardware = require('../persistences/HardwarePersistence')

module.exports = {
    async GetAll(req, res) {
        const { page = 1 } = req.query;
        await PersistenciaHardware
            .paginate({}, { page, limit: 10 })
            .then((hardware) => {
                return res.json(hardware);
            })
            .catch(err => {
                console.log(err);
                return res.status(400).send('Ocorreu um erro na solicitação!');
            });
           
    },
    
    async GetById(req, res) {
        return res.json(await PersistenciaHardware.findById(req.params.id))
    },

    async Insert(req, res) {
        await PersistenciaHardware
        .create(req.body)
        .then((hardware) => {
            return res.json(hardware)
        }).catch(err => {
            console.log(err)
            return res.status(400).send('Solicitação não Atendida!!')
        });
    },

    async Update(req, res) {
        return res.json(await PersistenciaHardware.findByIdAndUpdate(req.params.id, req.body, { new: true }));
    },

    async Delete(req, res) {
        await PersistenciaHardware.deleteOne(req.params.id)

        // Retornando um OK vazio.
        return res.send()
    }
}