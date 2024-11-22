document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.data-format').forEach(element => {
        const dataISO = element.textContent.trim(); 
        const dataBR = new Date(dataISO).toLocaleDateString('pt-BR'); 
        element.textContent = dataBR; 
    });
});

