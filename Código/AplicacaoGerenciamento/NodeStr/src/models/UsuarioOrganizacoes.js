const UsuarioOrganizacoes = {
    organizacao: {
        type: Number,
        required: true
    },
    usuario: {
        type: Number,
        required: true
    },
    createdAt: {
        type: Date,
        default: Date.now
    }
}

module.exports = UsuarioOrganizacoes;