let tablaData;
let idEditar = 0;
const controlador = "Moneda";
const modal = "mdData";
const preguntaEliminar = "Desea eliminar la moneda";
const confirmaEliminar = "La moneda fue eliminada.";
const confirmaRegistro = "Moneda registrada!";

document.addEventListener("DOMContentLoaded", function () {
    // Inicializar DataTable
    tablaData = $('#tbData').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": `https://localhost:7291/api/Moneda/Lista`,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title: "", "data": "idMoneda", visible: false },
            { title: "Nombre", "data": "nombre" },
            { title: "Símbolo", "data": "simbolo" },
            {
                title: "Fecha Creación", "data": "fechaCreacion", render: function (data) {
                    return new Date(data).toLocaleDateString();
                }
            },
            {
                title: "Acciones", "data": "idMoneda", width: "100px", render: function (data) {
                    return `<button class="btn btn-primary me-2 btn-editar"><i class="fa-solid fa-pen"></i></button>` +
                        `<button class="btn btn-danger btn-eliminar"><i class="fa-solid fa-trash"></i></button>`;
                }
            }
        ],
        "order": [[0, 'desc']],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});

// Validar campos obligatorios y formato
function validarDatos(nombre, simbolo) {
    if (!nombre.trim() || !simbolo.trim()) {
        Swal.fire({
            title: "Error!",
            text: "Todos los campos son obligatorios.",
            icon: "warning"
        });
        return false;
    }
    if (!/^[A-Z]{1,3}$/.test(simbolo)) { // Símbolos como USD, EUR
        Swal.fire({
            title: "Error!",
            text: "El símbolo debe ser de 1 a 3 letras en mayúscula.",
            icon: "warning"
        });
        return false;
    }
    return true;
}

// Botón nuevo
$("#btnNuevo").on("click", function () {
    idEditar = 0;
    $("#txtNombre").val("");
    $("#txtSimbolo").val("");
    $(`#${modal}`).modal('show');
});

// Botón editar
$("#tbData tbody").on("click", ".btn-editar", function () {
    const filaSeleccionada = $(this).closest('tr');
    const data = tablaData.row(filaSeleccionada).data();

    idEditar = data.idMoneda;
    $("#txtNombre").val(data.nombre);
    $("#txtSimbolo").val(data.simbolo);
    $(`#${modal}`).modal('show');
});

// Botón guardar
$("#btnGuardar").on("click", function () {
    const nombre = $("#txtNombre").val().trim();
    const simbolo = $("#txtSimbolo").val().trim();

    if (!validarDatos(nombre, simbolo)) {
        return;
    }

    const objeto = {
        idMoneda: idEditar,
        nombre: nombre,
        simbolo: simbolo,
        fechaCreacion: idEditar === 0 ? new Date().toISOString() : $("#txtFechaCreacion").val()
    };

    const url = idEditar !== 0
        ? `https://localhost:7291/api/Moneda/Editar`
        : `https://localhost:7291/api/Moneda/Crear`;

    const metodo = idEditar !== 0 ? "PUT" : "POST";

    fetch(url, {
        method: metodo,
        headers: { 'Content-Type': 'application/json;charset=utf-8' },
        body: JSON.stringify(objeto)
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(err => Promise.reject(err));
            }
            return response.json();
        })
        .then(() => {
            Swal.fire({
                text: confirmaRegistro,
                icon: "success"
            });
            $(`#${modal}`).modal('hide');
            tablaData.ajax.reload();
        })
        .catch(error => {
            Swal.fire({
                title: "Error!",
                text: error.message || "No se pudo registrar la moneda.",
                icon: "error"
            });
        });
});

// Botón eliminar
$("#tbData tbody").on("click", ".btn-eliminar", function () {
    const filaSeleccionada = $(this).closest('tr');
    const data = tablaData.row(filaSeleccionada).data();

    Swal.fire({
        text: `${preguntaEliminar} ${data.nombre}?`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`https://localhost:7291/api/Moneda/${data.idMoneda}`, {
                method: "DELETE",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            })
                .then(async response => {
                    if (!response.ok) {
                        const errorData = await response.json();
                        throw new Error(errorData.message || "Error inesperado.");
                    }
                    return response.json();
                })
                .then(() => {
                    Swal.fire({
                        title: "Listo!",
                        text: confirmaEliminar,
                        icon: "success"
                    });
                    tablaData.ajax.reload();
                })
                .catch(error => {
                    Swal.fire({
                        title: "Error!",
                        text: error.message || "No se pudo eliminar la moneda.",
                        icon: "error"
                    });
                });
        }
    });
});

