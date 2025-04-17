let tablaData;
let idEditar = 0;
const controlador = "Cliente"; // Nombre del controlador en la API
const modal = "mdData"; // ID del modal para agregar/editar
const preguntaEliminar = "Desea eliminar el cliente";
const confirmaEliminar = "El cliente fue eliminado.";
const confirmaRegistro = "Cliente registrado!";

document.addEventListener("DOMContentLoaded", function () {
    // Inicialización de DataTable
    tablaData = $('#tbData').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": `https://localhost:7291/api/Cliente/Lista`, // Endpoint para listar clientes
            "type": "GET",
            "datatype": "json",
            "error": function (xhr, error, code) {
                console.error("Error al cargar los datos:", xhr.responseText);
                Swal.fire({
                    title: "Error!",
                    text: "No se pudieron cargar los datos.",
                    icon: "error"
                });
            }
        },
        "columns": [
            { title: "", "data": "idCliente", visible: false },
            { title: "Nro. Documento", "data": "nroDocumento" },
            { title: "Nombre", "data": "nombre" },
            { title: "Apellido", "data": "apellido" },
            { title: "Correo", "data": "correo" },
            { title: "Teléfono", "data": "telefono" },
            {
                title: "Fecha Creación",
                "data": "fechaCreacion",
                render: function (data) {
                    return new Date(data).toLocaleDateString(); // Formato de fecha legible
                }
            },
            {
                title: "Acciones", "data": "idCliente", width: "150px", render: function (data, type, row) {
                    return `
                        <button class="btn btn-primary me-2 btn-editar"><i class="fa-solid fa-pen"></i></button>
                        <button class="btn btn-danger btn-eliminar"><i class="fa-solid fa-trash"></i></button>
                    `;
                }
            }
        ],
        "order": [[0, 'desc']],
        fixedColumns: {
            start: 0,
            end: 1
        },
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
});

// Acción al hacer clic en el botón de editar
$("#tbData tbody").on("click", ".btn-editar", function () {
    const filaSeleccionada = $(this).closest('tr');
    const data = tablaData.row(filaSeleccionada).data();

    idEditar = data.idCliente;
    $("#txtNroDocumento").val(data.nroDocumento);
    $("#txtNombre").val(data.nombre);
    $("#txtApellido").val(data.apellido);
    $("#txtCorreo").val(data.correo);
    $("#txtTelefono").val(data.telefono);
    $(`#${modal}`).modal('show');
});

// Acción para abrir el modal de nuevo cliente
$("#btnNuevo").on("click", function () {
    idEditar = 0;
    $("#txtNroDocumento").val("");
    $("#txtNombre").val("");
    $("#txtApellido").val("");
    $("#txtCorreo").val("");
    $("#txtTelefono").val("");
    $(`#${modal}`).modal('show');
});

// Validaciones de formato
function validarFormulario(cliente) {
    // Validar formato de número de documento
    if (!/^\d{3}-\d{7}-\d{1}$/.test(cliente.nroDocumento)) {
        Swal.fire({
            title: "Error!",
            text: "El número de documento debe tener el formato ###-#######-#.",
            icon: "warning"
        });
        return false;
    }

    // Validar formato de correo electrónico
    if (!/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/.test(cliente.correo)) {
        Swal.fire({
            title: "Error!",
            text: "El correo electrónico no tiene un formato válido.",
            icon: "warning"
        });
        return false;
    }

    // Validar formato de teléfono
    if (!/^\d{3}-\d{3}-\d{4}$/.test(cliente.telefono)) {
        Swal.fire({
            title: "Error!",
            text: "El teléfono debe tener el formato ###-###-####.",
            icon: "warning"
        });
        return false;
    }

    return true;
}

// Acción para guardar
$("#btnGuardar").on("click", function () {
    const objeto = {
        idCliente: idEditar,
        nroDocumento: $("#txtNroDocumento").val().trim(),
        nombre: $("#txtNombre").val().trim(),
        apellido: $("#txtApellido").val().trim(),
        correo: $("#txtCorreo").val().trim(),
        telefono: $("#txtTelefono").val().trim(),
        fechaCreacion: new Date().toISOString(),
    };

    // Validar el formulario antes de enviar
    if (!validarFormulario(objeto)) {
        return;
    }

    const metodo = idEditar !== 0 ? "PUT" : "POST";
    const url = `https://localhost:7291/api/Cliente/${idEditar !== 0 ? "Editar" : "Crear"}`;

    fetch(url, {
        method: metodo,
        headers: { 'Content-Type': 'application/json;charset=utf-8' },
        body: JSON.stringify(objeto)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(responseJson => {
            if (responseJson.data === "") {
                Swal.fire({
                    text: idEditar !== 0 ? "Se guardaron los cambios!" : confirmaRegistro,
                    icon: "success"
                });
                $(`#${modal}`).modal('hide');
                tablaData.ajax.reload();
            } else {
                Swal.fire({
                    title: "Error!",
                    text: responseJson.data,
                    icon: "error"
                });
            }
        })
        .catch(error => {
            Swal.fire({
                title: "Error!",
                text: idEditar !== 0 ? "No se pudo editar." : "No se pudo registrar.",
                icon: "error"
            });
            console.error("Error al realizar la solicitud:", error);
        });
});

// Acción para eliminar un cliente
$("#tbData tbody").on("click", ".btn-eliminar", function () {
    const filaSeleccionada = $(this).closest('tr');
    const data = tablaData.row(filaSeleccionada).data();

    Swal.fire({
        text: `${preguntaEliminar} ${data.nombre} ${data.apellido}?`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`https://localhost:7291/api/Cliente/Eliminar?id=${data.idCliente}`, {
                method: "DELETE",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => response.json())
                .then(responseJson => {
                    if (responseJson.data === "") {
                        Swal.fire({
                            title: "Listo!",
                            text: confirmaEliminar,
                            icon: "success"
                        });
                        tablaData.ajax.reload();
                    } else {
                        Swal.fire({
                            title: "Error!",
                            text: responseJson.data, // ✅ muestra el mensaje real
                            icon: "error"
                        });
                    }

                })
                .catch(() => {
                    Swal.fire({
                        title: "Error!",
                        text: responseJson.data || "No se pudo eliminar.",
                        icon: "error"
                    });
                });
        }
    });
});
