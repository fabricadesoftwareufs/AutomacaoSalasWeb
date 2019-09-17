const TipoUsuario = {
    descricao: {
        type: String,
        required: true
    },
    createdAt: {
        type: Date,
        default: Date.now
    }
}

module.exports = TipoUsuario;