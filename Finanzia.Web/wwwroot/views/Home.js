document.addEventListener("DOMContentLoaded", function () {
    $.LoadingOverlay("show");

    fetch("https://localhost:7291/api/Resumen", {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            $.LoadingOverlay("hide");
            const resumen = data?.data;

            if (resumen) {
                console.log(resumen); // Depuración

                // Actualizar texto de resumen
                $("#spTotalClientes").text(resumen.totalClientes || "0");
                $("#spPrestamosPendientes").text(resumen.prestamosPendientes || "0");
                $("#spPrestamosCancelados").text(resumen.prestamosCancelados || "0");
                $("#spInteresAcumulado").text(`$${parseFloat(resumen.interesAcumulado || 0).toFixed(2)}`);
                $("#spMontoTotalPrestado").text(`$${parseFloat(resumen.montoTotalPrestado || 0).toFixed(2)}`);
                $("#spTotalPagosRecibidos").text(resumen.totalPagosRecibidos || "0");
                $("#spTasaInteresesPromedio").text(`${parseFloat(resumen.tasaInteresesPromedio || 0).toFixed(2)}%`);
                $("#spClientesActivos").text(resumen.clientesActivos || "0");

                // Inicializar gráficos con datos reales
                inicializarGraficoPrestamos(resumen);
                inicializarGraficoIngresos(resumen.ingresosPorMes || []);

                // Mostrar pagos próximos y atrasados
                mostrarPagosProximos(resumen.pagosProximos);
                mostrarPagosAtrasados(resumen.pagosAtrasados);
            } else {
                Swal.fire("Advertencia", "No se encontraron datos para el resumen.", "warning");
            }
        })
        .catch(error => {
            $.LoadingOverlay("hide");
            Swal.fire("Error", `No se pudo obtener el resumen: ${error.message}`, "error");
        });

    function inicializarGraficoPrestamos(resumen) {
        const ctx = document.getElementById("chartPrestamos").getContext("2d");
        new Chart(ctx, {
            type: "pie",
            data: {
                labels: ["Pendientes", "Cancelados"],
                datasets: [{
                    data: [
                        parseInt(resumen.prestamosPendientes || 0),
                        parseInt(resumen.prestamosCancelados || 0)
                    ],
                    backgroundColor: ["#FFD700", "#00BFFF"],
                    borderColor: ["#FFD700", "#00BFFF"],
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        position: "bottom"
                    }
                }
            }
        });
    }

    function inicializarGraficoIngresos(ingresosPorMes) {
        if (!ingresosPorMes || ingresosPorMes.length === 0) {
            Swal.fire("Advertencia", "No se encontraron datos para los ingresos del mes actual.", "warning");
            return;
        }

        const labels = ingresosPorMes.map(item => item.mes); // Meses
        const data = ingresosPorMes.map(item => parseFloat(item.ingreso)); // Ingresos

        const ctx = document.getElementById("chartIngresos").getContext("2d");
        new Chart(ctx, {
            type: "bar",
            data: {
                labels: labels,
                datasets: [{
                    label: "Ingresos ($)",
                    data: data,
                    backgroundColor: "rgba(54, 162, 235, 0.5)",
                    borderColor: "rgba(54, 162, 235, 1)",
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }

    function mostrarPagosProximos(pagos) {
        const tbody = document.querySelector("#tablaPagosProximos tbody");
        tbody.innerHTML = "";
        pagos.forEach(pago => {
            tbody.innerHTML += `
                <tr>
                    <td>${pago.cliente}</td>
                    <td>$${pago.montoCuota.toFixed(2)}</td>
                    <td>${new Date(pago.fechaPago).toLocaleDateString()}</td>
                    <td>${pago.cuota}</td>
                </tr>
            `;
        });
    }

    function mostrarPagosAtrasados(pagos) {
        const tbody = document.querySelector("#tablaPagosAtrasados tbody");
        tbody.innerHTML = "";
        pagos.forEach(pago => {
            tbody.innerHTML += `
                <tr>
                    <td>${pago.cliente}</td>
                    <td>$${pago.montoCuota.toFixed(2)}</td>
                    <td>${new Date(pago.fechaPago).toLocaleDateString()}</td>
                    <td>${pago.cuota}</td>
                </tr>
            `;
        });
    }
});




