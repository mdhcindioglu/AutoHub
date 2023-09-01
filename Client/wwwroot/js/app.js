function displayMessage(message, title) {
    if (title != null)
        Swal.fire({
            title: title,
            html: message
        });
    else
        Swal.fire({
            html: message
        });
}

function confirmation(message, title) {
    return new Promise((resolve) => {
        Swal.fire({
            title: title,
            html: message,
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'
        }).then((result) => {
            resolve(result.isConfirmed);
        });
    });
}

function setFocus() {
    var elements = document.getElementsByClassName("auto-focus");
    if (elements != null && elements.length > 0)
        elements[elements.length - 1].focus();
}