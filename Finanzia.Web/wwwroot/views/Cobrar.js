let idPrestamo = 0;
let totalPagar = 0;
let prestamosEncontrados = [];

document.addEventListener("DOMContentLoaded", function (event) { });

// Validación del formato del número de documento
function validarNumeroDocumento(nroDocumento) {
    return /^\d{3}-\d{7}-\d{1}$/.test(nroDocumento); // Formato dominicano
}

// Buscar préstamos por número de documento
$("#btnBuscar").on("click", function () {
    const nroDocumento = $("#txtNroDocumento").val().trim();

    if (nroDocumento === "") {
        Swal.fire({
            title: "Ups!",
            text: "Debe ingresar un número de documento.",
            icon: "warning"
        });
        return;
    }

    if (!validarNumeroDocumento(nroDocumento)) {
        Swal.fire({
            title: "Error!",
            text: "El número de documento debe tener el formato ###-#######-#.",
            icon: "warning"
        });
        return;
    }

    $.LoadingOverlay("show");

    fetch(`https://localhost:7291/api/Prestamo/ObtenerPrestamos?idPrestamo=0&nroDocumento=${nroDocumento}`, {
        method: "GET",
        headers: { "Content-Type": "application/json;charset=utf-8" }
    })
        .then(async (response) => {
            if (!response.ok) {
                const errorData = await response.json().catch(() => ({})); // Manejar respuesta malformada
                throw new Error(errorData.message || "Error inesperado.");
            }
            return response.json(); // Procesar la respuesta
        })
        .then(responseJson => {
            $.LoadingOverlay("hide");
            prestamosEncontrados = [];

            if (!responseJson.length) {
                Limpiar(false);
                Swal.fire({
                    title: "Ups!",
                    text: "No se encontró un cliente.",
                    icon: "warning"
                });
                return;
            }

            if (responseJson.length === 1) {
                const prestamo = responseJson[0];
                mostrarPrestamo(prestamo); // Actualiza los datos del préstamo actual
            } else {
                Limpiar(false);
                prestamosEncontrados = responseJson;

                $("#tbPrestamosEncontrados tbody").html("");
                responseJson.forEach(e => {
                    $("#tbPrestamosEncontrados tbody").append(`
                        <tr>
                            <td><button class="btn btn-primary btn-sm btn-prestamo-encontrado" data-idprestamo="${e.idPrestamo}"><i class="fa-solid fa-check"></i></button></td>
                            <td>${e.idPrestamo}</td>
                            <td>${e.montoPrestamo}</td>
                            <td>${e.estado === "Pendiente" ? '<span class="badge bg-danger p-2">Pendiente</span>' : '<span class="badge bg-success p-2">Cancelado</span>'}</td>
                            <td>${e.fechaCreacion}</td>
                        </tr>
                    `);
                });
                $(`#mdData`).modal("show");
            }
        })
        .catch(error => {
            $.LoadingOverlay("hide");
            Swal.fire({
                title: "Error!",
                text: error.message,
                icon: "warning"
            });
        });
});

// Limpiar los campos
function Limpiar(limpiarNroDocumento) {
    if (limpiarNroDocumento) $("#txtNroDocumento").val("");

    idPrestamo = 0;
    totalPagar = 0;
    $("#txtNroPrestamo").val("");
    $("#txtNombreCliente").val("");
    $("#txtMontoPrestamo").val("");
    $("#txtInteres").val("");
    $("#txtNroCuotas").val("");
    $("#txtMontoTotal").val("");
    $("#txtFormadePago").val("");
    $("#txtTipoMoneda").val("");
    $("#txtTotalaPagar").val("");
    $("#tbDetalle tbody").html("");
}

// Mostrar detalles del préstamo seleccionado
function mostrarPrestamo(prestamo) {
    idPrestamo = prestamo.idPrestamo;

    $("#txtNroPrestamo").val(prestamo.idPrestamo);
    $("#txtNombreCliente").val(`${prestamo.cliente.nombre} ${prestamo.cliente.apellido}`);
    $("#txtMontoPrestamo").val(prestamo.montoPrestamo.toFixed(2));
    $("#txtInteres").val(prestamo.interesPorcentaje.toFixed(2));
    $("#txtNroCuotas").val(prestamo.nroCuotas);
    $("#txtMontoTotal").val(prestamo.valorTotal.toFixed(2));
    $("#txtFormadePago").val(prestamo.formaDePago);
    $("#txtTipoMoneda").val(prestamo.moneda.nombre);

    $("#tbDetalle tbody").html("");
    prestamo.prestamoDetalle.forEach(e => {
        const activar = e.estado === "Cancelado" ? "disabled checked" : "";
        const clase = e.estado === "Cancelado" ? "" : "checkPagado";

        $("#tbDetalle tbody").append(`
            <tr>
                <td><input class="form-check-input ${clase}" type="checkbox" name="${e.nroCuota}" data-monto="${e.montoCuota}" ${activar}/></td>
                <td>${e.nroCuota}</td>
                <td>${e.fechaPago}</td>
                <td>${e.montoCuota}</td>
                <td>${e.estado === "Pendiente" ? '<span class="badge bg-danger p-2">Pendiente</span>' : '<span class="badge bg-success p-2">Cancelado</span>'}</td>
                <td>${e.fechaPagado}</td>
            </tr>
        `);
    });
}

// Actualizar total a pagar al seleccionar cuotas
$(document).on("click", ".checkPagado", function () {
    const montoCuota = parseFloat($(this).data("monto"));
    if (!montoCuota || isNaN(montoCuota)) {
        Swal.fire({
            title: "Error!",
            text: "El monto de la cuota no es válido.",
            icon: "warning"
        });
        return;
    }

    if ($(this).is(":checked")) {
        totalPagar += montoCuota;
    } else {
        totalPagar -= montoCuota;
    }
    $("#txtTotalaPagar").val(totalPagar.toFixed(2));
});

// Seleccionar préstamo desde la lista de préstamos encontrados
$(document).on("click", ".btn-prestamo-encontrado", function () {
    const idPrestamoSeleccionado = parseInt($(this).data("idprestamo"));
    const prestamo = prestamosEncontrados.find(e => e.idPrestamo === idPrestamoSeleccionado);
    mostrarPrestamo(prestamo);
    $(`#mdData`).modal("hide");
});

// Registrar el pago de cuotas seleccionadas
$("#btnRegistrarPago").on("click", function () {
    if (idPrestamo === 0) {
        Swal.fire({
            title: "Error!",
            text: `No hay préstamo encontrado.`,
            icon: "warning"
        });
        return;
    }

    if (totalPagar === 0) {
        Swal.fire({
            title: "Error!",
            text: `No hay cuotas seleccionadas.`,
            icon: "warning"
        });
        return;
    }

    const cuotasSeleccionadas = $(".checkPagado:checked").map(function () {
        return $(this).attr("name");
    }).get().join(",");

    fetch(`https://localhost:7291/api/Prestamo/PagarCuotas?idPrestamo=${idPrestamo}&nroCuotasPagadas=${cuotasSeleccionadas}`, {
        method: "POST",
        headers: { "Content-Type": "application/json;charset=utf-8" }
    })
        .then(async (response) => {
            if (!response.ok) {
                const errorData = await response.json().catch(() => ({}));
                throw new Error(errorData.message || "Error inesperado.");
            }
            return response.json();
        })
        .then(responseJson => {
            if (responseJson.success) {
                Swal.fire({
                    title: "Listo!",
                    text: responseJson.message,
                    icon: "success"
                });
                Limpiar(true);
            } else {
                Swal.fire({
                    title: "Error!",
                    text: responseJson.message,
                    icon: "warning"
                });
            }
        })
        .catch(error => {
            Swal.fire({
                title: "Error!",
                text: error.message,
                icon: "error"
            });
        });
});
