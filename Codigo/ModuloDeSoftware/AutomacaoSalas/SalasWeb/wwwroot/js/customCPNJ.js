document.addEventListener('DOMContentLoaded', function () {
    const input = document.querySelector('#Cnpj');
    const errorSpan = document.querySelector('#span_cnpj');

    // Máscara ao perder o foco
    input.addEventListener('blur', function () {
        if (input.value.replace(/\D/g, '').length === 0) {
            input.value = '__.___.___/____-__';
            input.dataset.empty = true;
        } else {
            if (!validaCnpjLocal(input.value)) {
                errorSpan.textContent = "CNPJ inválido!";
                errorSpan.style.display = "inline";
            } else {
                errorSpan.style.display = "none";
            }
        }
    });

    // Limpa o valor ao focar no campo
    input.addEventListener('focus', function () {
        if (input.dataset.empty === 'true') {
            input.value = '';
            delete input.dataset.empty;
        }
    });

    // Aplica a formatação conforme o usuário digita
    input.addEventListener('input', function () {
        let value = input.value.replace(/\D/g, ''); // Remove caracteres não numéricos
        let formattedValue = '';

        if (value.length > 0) {
            if (value.length <= 2) {
                formattedValue = value;
            } else if (value.length <= 5) {
                formattedValue = value.substring(0, 2) + '.' + value.substring(2);
            } else if (value.length <= 8) {
                formattedValue = value.substring(0, 2) + '.' + value.substring(2, 5) + '.' + value.substring(5);
            } else if (value.length <= 12) {
                formattedValue = value.substring(0, 2) + '.' + value.substring(2, 5) + '.' + value.substring(5, 8) + '/' + value.substring(8);
            } else {
                formattedValue = value.substring(0, 2) + '.' + value.substring(2, 5) + '.' + value.substring(5, 8) + '/' + value.substring(8, 12) + '-' + value.substring(12, 14);
            }
        }

        input.value = formattedValue;
    });

    // Função para validar o CNPJ
    function validaCnpjLocal(cnpj) {
        cnpj = cnpj.replace(/[^\d]+/g, ''); // Remove caracteres não numéricos

        if (cnpj.length !== 14) return false; // Verifica se tem 14 dígitos
        if (/^(\d)\1+$/.test(cnpj)) return false; // Elimina CNPJs inválidos conhecidos

        let tamanho = cnpj.length - 2;
        let numeros = cnpj.substring(0, tamanho);
        let digitos = cnpj.substring(tamanho);
        let soma = 0;
        let pos = tamanho - 7;

        for (let i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2) pos = 9;
        }

        let resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado !== parseInt(digitos.charAt(0))) return false;

        tamanho++;
        numeros = cnpj.substring(0, tamanho);
        soma = 0;
        pos = tamanho - 7;

        for (let i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2) pos = 9;
        }

        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        return resultado === parseInt(digitos.charAt(1));
    }
});
