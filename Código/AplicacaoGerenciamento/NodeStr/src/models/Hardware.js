const Hardware = {
    mac: {
        type: String,
        required: true
    },

    sala: {
        type: Number,
        required: true
    },

    tipoHardware: {
        type: Number,
        required: true
    },
    createdAt: {
        type: Date,
        default: Date.now
    }
}

module.exports = Hardware;