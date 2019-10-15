const Organizacao = {
    cnpj: {
        type: String,
        unique: true,
        required: true,  
    },
    razaoSocial: {
        type: String,
        required: true,  
    },
    createdAt: {
        type: Date,
        default: Date.now
    },
}

module.exports = Organizacao;