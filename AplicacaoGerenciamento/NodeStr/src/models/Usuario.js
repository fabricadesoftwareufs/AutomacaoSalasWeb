const Usuario = {
    cpf: {
        type: String,
        unique: true,
        required: true
    },
    nome: {
        type: String,
        required: true
    },
    email: {
        type: String,
        unique: true,
        required: true,
        lowercase: true
    },
    senha: {
        type: String,
        required: true,
        select: true
    },
    createdAt: {
        type: Date,
        default: Date.now
    },
    tipoUsuario: {
        type: Number,
        required: true
    }
}

// Exportando a model
module.exports = Usuario;