let idCliente = 0;
const confirmaRegistro = "¡Préstamo registrado exitosamente!";

document.addEventListener("DOMContentLoaded", function () {
    // Cargar lista de monedas
    $.LoadingOverlay("show");
    fetch(`https://localhost:7291/api/Moneda/Lista`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    })
        .then(response => response.ok ? response.json() : Promise.reject(response))
        .then(responseJson => {
            $.LoadingOverlay("hide");
            if (responseJson.data.length > 0) {
                responseJson.data.forEach((moneda) => {
                    $("#cboTipoMoneda").append(
                        `<option value="${moneda.idMoneda}" data-simbolo="${moneda.simbolo}">${moneda.nombre}</option>`
                    );
                });
            }
        })
        .catch(() => {
            $.LoadingOverlay("hide");
            Swal.fire({
                title: "Error!",
                text: "No se pudo cargar la lista de monedas.",
                icon: "warning"
            });
        });
});

// Validar campos
function validarCliente(nroDocumento, nombre, apellido, correo, telefono) {
    const docRegex = /^[0-9]{3}-[0-9]{7}-[0-9]{1}$/; // Formato dominicano
    const telRegex = /^[+]?1?[ -]?[0-9]{3}[ -]?[0-9]{3}[ -]?[0-9]{4}$/; // Teléfono dominicano
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!nroDocumento || !docRegex.test(nroDocumento)) {
        Swal.fire({
            title: "Error!",
            text: "El número de documento no tiene un formato válido.",
            icon: "warning"
        });
        return false;
    }

    if (!nombre || !apellido) {
        Swal.fire({
            title: "Error!",
            text: "El nombre y apellido son obligatorios.",
            icon: "warning"
        });
        return false;
    }

    if (!correo || !emailRegex.test(correo)) {
        Swal.fire({
            title: "Error!",
            text: "El correo no tiene un formato válido.",
            icon: "warning"
        });
        return false;
    }

    if (!telefono || !telRegex.test(telefono)) {
        Swal.fire({
            title: "Error!",
            text: "El teléfono no tiene un formato válido.",
            icon: "warning"
        });
        return false;
    }

    return true;
}

// Buscar cliente
$("#btnBuscar").on("click", function () {
    const nroDocumento = $("#txtNroDocumento").val();
    if (!nroDocumento) {
        Swal.fire("Ups!", "Debe ingresar un número de documento.", "warning");
        return;
    }

    $("#cardCliente").LoadingOverlay("show");
    fetch(`https://localhost:7291/api/Prestamo/ObtenerCliente?NroDocumento=${nroDocumento}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    })
        .then(response => response.ok ? response.json() : Promise.reject(response))
        .then(responseJson => {
            $("#cardCliente").LoadingOverlay("hide");
            const cliente = responseJson.data;
            if (cliente.idCliente) {
                idCliente = cliente.idCliente;
                $("#txtNombre").val(cliente.nombre);
                $("#txtApellido").val(cliente.apellido);
                $("#txtCorreo").val(cliente.correo);
                $("#txtTelefono").val(cliente.telefono);
            } else {
                Swal.fire({
                    title: "Cliente no encontrado",
                    text: "¿Desea registrarlo manualmente?",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, registrar",
                    cancelButtonText: "No"
                }).then(result => {
                    if (result.isConfirmed) {
                        idCliente = 0;
                        $("#modalRegistrarCliente").modal("show");
                    }
                });
            }
        })
        .catch(() => {
            $("#cardCliente").LoadingOverlay("hide");
            Swal.fire("Error!", "No se pudo buscar el cliente.", "error");
        });
});

// Guardar cliente
$("#btnGuardarCliente").on("click", function () {
    const nroDocumento = $("#txtNroDocumentoModal").val().trim();
    const nombre = $("#txtNombreModal").val().trim();
    const apellido = $("#txtApellidoModal").val().trim();
    const correo = $("#txtCorreoModal").val().trim();
    const telefono = $("#txtTelefonoModal").val().trim();

    if (!validarCliente(nroDocumento, nombre, apellido, correo, telefono)) return;

    const cliente = { nroDocumento, nombre, apellido, correo, telefono };

    fetch(`https://localhost:7291/api/Cliente/Crear`, {
        method: "POST",
        headers: { 'Content-Type': 'application/json;charset=utf-8' },
        body: JSON.stringify(cliente)
    })
        .then(response => response.ok ? response.json() : Promise.reject(response))
        .then(responseJson => {
            Swal.fire("Éxito!", "Cliente registrado exitosamente.", "success");
            $("#modalRegistrarCliente").modal("hide");
            idCliente = responseJson.data.idCliente;
        })
        .catch(() => {
            Swal.fire("Error!", "No se pudo registrar el cliente.", "error");
        });
});

// Calcular préstamo
$("#btnCalcular").on("click", function () {
    const montoPrestamo = parseFloat($("#txtMontoPrestamo").val());
    const interes = parseFloat($("#txtInteres").val());
    const nroCuotas = parseInt($("#txtNroCuotas").val());

    if (!montoPrestamo || !interes || !nroCuotas) {
        Swal.fire("Error!", "Complete todos los campos para calcular el préstamo.", "warning");
        return;
    }

    const montoInteres = montoPrestamo * (interes / 100);
    const montoTotal = montoPrestamo + montoInteres;
    const montoPorCuota = montoTotal / nroCuotas;

    $("#txtMontoInteres").val(montoInteres.toFixed(2));
    $("#txtMontoPorCuota").val(montoPorCuota.toFixed(2));
    $("#txtMontoTotal").val(montoTotal.toFixed(2));
});

