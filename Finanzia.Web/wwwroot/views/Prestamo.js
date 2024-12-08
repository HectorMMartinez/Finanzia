let tablaData;
let idPrestamo = 0;
let nroDocumento = ""; // Inicialmente vacío para buscar todos los préstamos

const controlador = "Prestamo";
const modal = "mdData";

document.addEventListener("DOMContentLoaded", function () {
    // Inicializar DataTable
    tablaData = $('#tbData').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": `https://localhost:7291/api/${controlador}/ObtenerPrestamos?idPrestamo=0&nroDocumento=${nroDocumento}`,
            "type": "GET",
            "datatype": "json",
            "dataSrc": "", // La API devuelve un array directamente
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
            { title: "Nro. Préstamo", "data": "idPrestamo" },
            {
                title: "Cliente", "data": "cliente", render: function (data) {
                    return `${data.nombre} ${data.apellido}`;
                }
            },
            { title: "Monto Préstamo", "data": "montoPrestamo", render: $.fn.dataTable.render.number(',', '.', 2, '$') },
            { title: "Monto Interés", "data": "valorInteres", render: $.fn.dataTable.render.number(',', '.', 2, '$') },
            { title: "Monto Total", "data": "valorTotal", render: $.fn.dataTable.render.number(',', '.', 2, '$') },
            {
                title: "Moneda", "data": "moneda", render: function (data) {
                    return `${data.nombre}`;
                }
            },
            {
                title: "Estado", "data": "estado", render: function (data) {
                    return data === "Pendiente"
                        ? '<span class="badge bg-danger p-2">Pendiente</span>'
                        : '<span class="badge bg-success p-2">Cancelado</span>';
                }
            },
            {
                title: "Acciones", "data": "idPrestamo", width: "120px", render: function (data) {
                    return `<button class="btn btn-primary me-2 btn-detalle"><i class="fa-solid fa-list-ol"></i> Ver detalle</button>`;
                }
            }
        ],
        "order": [[0, 'desc']],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
});

// Ver detalle del préstamo
$("#tbData tbody").on("click", ".btn-detalle", function () {
    const filaSeleccionada = $(this).closest('tr');
    const prestamo = tablaData.row(filaSeleccionada).data();
    const detalle = prestamo.prestamoDetalle || [];
    idPrestamo = prestamo.idPrestamo;

    // Mostrar detalles en el modal
    $("#txtIdPrestamo").text(`Nro. Préstamo: ${prestamo.idPrestamo}`);
    $("#txtMontoPrestamo").val(prestamo.montoPrestamo.toFixed(2));
    $("#txtInteres").val(prestamo.interesPorcentaje.toFixed(2));
    $("#txtNroCuotas").val(prestamo.nroCuotas);
    $("#txtFormaPago").val(prestamo.formaDePago);
    $("#txtTipoMoneda").val(prestamo.moneda.nombre);
    $("#txtMontoTotal").val(prestamo.valorTotal.toFixed(2));

    // Limpiar y mostrar detalles
    $("#tbDetalle tbody").html("");
    if (detalle.length === 0) {
        $("#tbDetalle tbody").append('<tr><td colspan="6" class="text-center">No hay detalles disponibles.</td></tr>');
    } else {
        detalle.forEach(function (e) {
            $("#tbDetalle tbody").append(`
                <tr>
                    <td>${e.nroCuota}</td>
                    <td>${e.fechaPago || 'N/A'}</td>
                    <td>${parseFloat(e.montoCuota).toFixed(2)}</td>
                    <td>${e.estado}</td>
                </tr>
            `);
        });
    }

    $(`#${modal}`).modal('show');
});

// Imprimir el préstamo seleccionado
$("#btnImprimir").on("click", function () {
    if (!idPrestamo) {
        Swal.fire({
            title: "Error!",
            text: "Debe seleccionar un préstamo antes de imprimir.",
            icon: "warning"
        });
        return;
    }
    window.open(`https://localhost:7291/api/${controlador}/${idPrestamo}/GenerarPDF`, "_blank");
});

