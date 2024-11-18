const input = document.querySelector('#Cnpj');

input.addEventListener('input', function () {
    let value = input.value.replace(/\D/g, ''); // Remove tudo o que não for número
    let formattedValue = '';

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

    input.value = formattedValue;
});