const input = document.querySelector('#Cnpj');
input.addEventListener('mouseover', function () {
    if (input.value.trim() === '') {
        input.value = '__.___.___/____-__';
        input.dataset.empty = true;
    }
});
input.addEventListener('focus', function () {
    if (input.dataset.empty === 'true') {
        input.value = '';
        delete input.dataset.empty;
    }
});
input.addEventListener('input', function () {
    let value = input.value.replace(/\D/g, ''); 
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
input.addEventListener('blur', function () {
    if (input.value.replace(/\D/g, '').length === 0) {
        input.value = '__.___.___/____-__'; 
        input.dataset.empty = true; 
    }
});
