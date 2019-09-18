const PersistenciaTipoHardware = require('../persistences/TipoHardwarePersistence')

module.exports = {
    async GetAll(req, res) {
        const { page = 1 } = req.query;
        await PersistenciaTipoHardware
            .paginate({}, { page, limit: 10 })
            .then((tipohardware) => {
                return res.json(tipohardware);
            })
            .catch(err => {
                console.log(err);
                return res.status(400).send('Ocorreu um erro na solicitação!');
            });
           
    },
    async GetById(req, res) {
        return res.json(await PersistenciaTipoHardware.findById(req.params.id))
    },
    async Insert(req, res) {
        await PersistenciaTipoHardware
        .create(req.body)
        .then((tipohardware) => {
            return res.json(tipohardware)
        }).catch(err => {
            console.log(err)
            return res.status(400).send('Solicitação não Atendida!!')
        });
    },
    async Update(req, res) {
        return res.json(await PersistenciaTipoHardware.findByIdAndUpdate(req.params.id, req.body, { new: true }));
    },
    async Delete(req, res) {
        await PersistenciaTipoHardware.deleteOne(req.params.id)
        
        // Retornando um OK vazio.
        return res.send()
    }
}