function swtAlert(type, title, message) {
    Swal.fire({
        icon: type,
        title: title,
        text: message,
    });
}

function swtAlertRedirectIndex(type, title, message, url) {
    Swal.fire({
        icon: type,
        title: title,
        text: message,
        footer:
            '<a href="' + url + '" class="btn btn-success">OK</a>',
        showConfirmButton: false,
    });
}