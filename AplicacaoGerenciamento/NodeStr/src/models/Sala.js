const Sala = {
    nome: {
        ttype: String,
        required: true
    },

    bloco: {
        type: Number,
        required: true
    },
    
    createdAt: {
        type: Date,
        default: Date.now
    }

}

module.exports = Sala;