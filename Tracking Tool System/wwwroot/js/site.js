
document.addEventListener("DOMContentLoaded", function () {

    document.querySelectorAll(".table-scroll-container").forEach(function (container) {

        const topScroll = container.querySelector(".horizontal-scroll");
        const tableScroll = container.querySelector(".table-scroll");
        const table = tableScroll.querySelector("table");
        const topContent = container.querySelector(".horizontal-scroll-content");

        if (!topScroll || !tableScroll || !table || !topContent) {
            return;
        }

        topContent.style.width = table.offsetWidth + "px";

        topScroll.addEventListener("scroll", function () {
            tableScroll.scrollLeft = topScroll.scrollLeft;
        });

        tableScroll.addEventListener("scroll", function () {
            topScroll.scrollLeft = tableScroll.scrollLeft;
        });

    });

});

function mostrarExito(mensaje, titulo = "Operación exitosa") {
    return Swal.fire({
        icon: "success",
        title: titulo,
        text: mensaje,
        confirmButtonText: "Aceptar"
    });
}

function mostrarError(mensaje, titulo = "Ocurrió un error") {
    return Swal.fire({
        icon: "error",
        title: titulo,
        text: mensaje,
        confirmButtonText: "Aceptar"
    });
}

function mostrarAdvertencia(mensaje, titulo = "Advertencia") {
    return Swal.fire({
        icon: "warning",
        title: titulo,
        text: mensaje,
        confirmButtonText: "Aceptar"
    });
}

function mostrarInformacion(mensaje, titulo = "Información") {
    return Swal.fire({
        icon: "info",
        title: titulo,
        text: mensaje,
        confirmButtonText: "Aceptar"
    });
}

function confirmarEliminacion(callback, mensaje) {
    Swal.fire({
        icon: "warning",
        title: "¿Confirmar eliminación?",
        text: mensaje || "Esta acción no se puede deshacer.",
        showCancelButton: true,
        confirmButtonText: "Sí, eliminar",
        cancelButtonText: "Cancelar",
        reverseButtons: true,
        focusCancel: true
    }).then(function (result) {
        if (result.isConfirmed) {
            callback();
        }
    });
}