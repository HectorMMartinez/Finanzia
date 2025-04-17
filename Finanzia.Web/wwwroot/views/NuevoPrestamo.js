let idCliente = 0;
const confirmaRegistro = "¡Préstamo registrado exitosamente!";

// Cargar lista de monedas
$(document).ready(function () {
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
            Swal.fire("Error!", "No se pudo cargar la lista de monedas.", "warning");
        });
});

// Validar cliente
function validarCliente(nroDocumento, nombre, apellido, correo, telefono) {
    const docRegex = /^[0-9]{3}-[0-9]{7}-[0-9]{1}$/;
    const telRegex = /^[+]?1?[ -]?[0-9]{3}[ -]?[0-9]{3}[ -]?[0-9]{4}$/;
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!nroDocumento || !docRegex.test(nroDocumento)) {
        Swal.fire("Error!", "El número de documento no tiene un formato válido.", "warning");
        return false;
    }
    if (!nombre || !apellido) {
        Swal.fire("Error!", "El nombre y apellido son obligatorios.", "warning");
        return false;
    }
    if (!correo || !emailRegex.test(correo)) {
        Swal.fire("Error!", "El correo no tiene un formato válido.", "warning");
        return false;
    }
    if (!telefono || !telRegex.test(telefono)) {
        Swal.fire("Error!", "El teléfono no tiene un formato válido.", "warning");
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

// Registrar préstamo
$("#btnRegistrar").on("click", function () {
    const montoPrestamo = parseFloat($("#txtMontoPrestamo").val());
    const interes = parseFloat($("#txtInteres").val());
    const nroCuotas = parseInt($("#txtNroCuotas").val());
    const formaPago = $("#cboFormaPago").val();
    const idMoneda = parseInt($("#cboTipoMoneda").val());
    const fechaInicio = $("#txtFechaInicio").val();

    const valorInteres = parseFloat($("#txtMontoInteres").val());
    const valorPorCuota = parseFloat($("#txtMontoPorCuota").val());
    const valorTotal = parseFloat($("#txtMontoTotal").val());

    if (!idCliente || !montoPrestamo || !interes || !nroCuotas || !formaPago || !idMoneda || !fechaInicio) {
        Swal.fire("Error!", "Todos los campos deben estar completos para registrar el préstamo.", "warning");
        return;
    }

    if (isNaN(valorInteres) || isNaN(valorPorCuota) || isNaN(valorTotal)) {
        Swal.fire("Error!", "Debe presionar el botón Calcular antes de registrar el préstamo.", "warning");
        return;
    }

    const fechaInicioISO = new Date(fechaInicio).toISOString();
    const fechaCreacion = new Date().toISOString();

    const cliente = {
        idCliente: idCliente,
        nroDocumento: $("#txtNroDocumento").val().trim(),
        nombre: $("#txtNombre").val().trim(),
        apellido: $("#txtApellido").val().trim(),
        correo: $("#txtCorreo").val().trim(),
        telefono: $("#txtTelefono").val().trim(),
        fechaCreacion: fechaCreacion
    };

    const monedaSelect = $("#cboTipoMoneda option:selected");
    const moneda = {
        idMoneda: idMoneda,
        nombre: monedaSelect.text().trim(),
        simbolo: monedaSelect.data("simbolo") || "$",
        fechaCreacion: fechaCreacion
    };

    const prestamo = {
        cliente: cliente,
        moneda: moneda,
        fechaInicioPago: fechaInicioISO,
        montoPrestamo: montoPrestamo,
        interesPorcentaje: interes,
        nroCuotas: nroCuotas,
        formaDePago: formaPago,
        valorPorCuota: valorPorCuota,
        valorInteres: valorInteres,
        valorTotal: valorTotal,
        estado: "Pendiente",
        fechaCreacion: fechaCreacion,
        prestamoDetalle: []
    };

    fetch(`https://localhost:7291/api/Prestamo/Crear`, {
        method: "POST",
        headers: { 'Content-Type': 'application/json;charset=utf-8' },
        body: JSON.stringify(prestamo)
    })
        .then(async response => {
            const contentType = response.headers.get("Content-Type");
            const isJson = contentType && contentType.includes("application/json");
            const data = isJson ? await response.json() : await response.text();

            if (response.ok) {
                Swal.fire("Éxito!", confirmaRegistro, "success");
                setTimeout(() => window.location.href = "/Prestamo", 2000);
            } else {
                let errorMsg = "Error al registrar el préstamo.";
                if (typeof data === "string") {
                    errorMsg = data;
                } else if (data.message) {
                    errorMsg = data.message;
                } else if (data.data) {
                    errorMsg = data.data;
                }
                Swal.fire("Error!", errorMsg, "error");
            }
        })
        .catch(error => {
            console.error("Error en el fetch:", error);
            Swal.fire("Error!", "No se pudo comunicar con el servidor.", "error");
        });
});
