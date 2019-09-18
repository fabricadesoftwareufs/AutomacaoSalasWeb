const HorarioSala = {
    data: {
        type: Date,
        required: true,    
    },
    
    horaInicio: {
        type: String,
        required: true
    },
    
    horaFim: {
        type: String,
        required: true
    },
    
    turno: {
        type: String,
        required: true
    },

    qtdAlunos: {
        type: Number,
        required: true
    },

    usuario: {
        type: Number,
        required: true
    },

    disciplina:{
        type: Number
    },

    sala:{
        type: Number,
        required: true
    },
    createdAt: {
        type: Date,
        default: Date.now
    }
}

module.exports = HorarioSala;