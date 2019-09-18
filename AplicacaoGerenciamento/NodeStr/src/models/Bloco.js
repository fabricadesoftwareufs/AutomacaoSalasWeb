const Bloco = {
    // nome/descricao do bloco - adicionei esse campo
    titulo: {
        type: String,
    },

    organizacao: {
        type: Number,
        required: true
    },
    
    createdAt: {
        type: Date,
        default: Date.now
    }
}

module.exports = Bloco;