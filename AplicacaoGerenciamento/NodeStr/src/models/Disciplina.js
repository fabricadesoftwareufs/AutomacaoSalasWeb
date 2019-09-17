const Disciplina = {
    nome: {
        type: String,
        required: true
    },
    codigo: {
        type: String,
        required: true
    },
    createdAt: {
        type: Date,
        default: Date.now
    }
}

module.exports = Disciplina;