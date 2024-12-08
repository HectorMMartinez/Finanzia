-- Table: Cliente
INSERT INTO Cliente (NroDocumento, Nombre, Apellido, Correo, Telefono, FechaCreacion) VALUES
('111-22-3333', 'Jay', 'Bryant', 'rushashley@gmail.com', '809-555-1234', '2021-11-15 17:28:12'),
('222-33-4444', 'Dillon', 'Carlson', 'foxscott@gmail.com', '829-555-5678', '2020-11-26 01:26:50'),
('333-44-5555', 'Marissa', 'Taylor', 'jamesshaw@davis.com', '849-555-9101', '2022-03-18 06:14:23'),
('444-55-6666', 'James', 'Brown', 'wjohns@barry.com', '849-555-1212', '2022-05-08 23:19:49'),
('555-66-7777', 'Daniel', 'Schneider', 'sullivankristen@morgan.com', '829-555-1313', '2020-07-11 23:17:40');

-- Table: Moneda
INSERT INTO Moneda (Nombre, Simbolo, FechaCreacion) VALUES
('Dólar', '$', '2021-01-01 10:00:00'),
('Euro', '€', '2022-01-01 10:00:00'),
('Peso Dominicano', 'RD$', '2023-01-01 10:00:00');
-- Table: Prestamo
INSERT INTO Prestamo (IdCliente, IdMoneda, FechaInicioPago, MontoPrestamo, InteresPorcentaje, NroCuotas, FormaDePago, ValorPorCuota, ValorInteres, ValorTotal, Estado, FechaCreacion) VALUES
(1, 1, '2023-07-01', 10000.00, 3.5, 12, 'Mensual', 850.00, 1200.00, 11200.00, 'Pendiente', '2023-06-01 10:00:00'),
(2, 2, '2023-06-15', 15000.00, 4.0, 24, 'Quincenal', 700.00, 1600.00, 16600.00, 'Cancelado', '2023-06-02 14:00:00'),
(3, 3, '2023-05-01', 20000.00, 3.0, 36, 'Semanal', 550.00, 1800.00, 21800.00, 'Pendiente', '2023-04-01 11:00:00'),
(4, 1, '2023-04-01', 5000.00, 2.5, 6, 'Diario', 850.00, 300.00, 5300.00, 'Pendiente', '2023-03-01 12:00:00'),
(5, 3, '2023-03-15', 12000.00, 3.7, 18, 'Mensual', 850.00, 1200.00, 13200.00, 'Pendiente', '2023-02-01 14:00:00');

-- Table: PrestamoDetalle
INSERT INTO PrestamoDetalle (IdPrestamo, FechaPago, NroCuota, MontoCuota, Estado, FechaPagado, FechaCreacion) VALUES
(1, '2023-07-15', 1, 850.00, 'Pendiente', '2023-07-14 15:30:00', '2023-06-01 10:30:00'),
(1, '2023-08-15', 2, 850.00, 'Pendiente', '2023-08-14 15:30:00', '2023-06-01 10:30:00'),
(2, '2023-06-30', 1, 700.00, 'Cancelado', '2023-06-20 10:00:00', '2023-06-02 14:30:00'),
(2, '2023-07-15', 2, 700.00, 'Pendiente', '2023-07-14 10:00:00', '2023-06-02 14:30:00'),
(3, '2023-05-10', 1, 550.00, 'Pendiente', '2023-05-09 14:30:00', '2023-04-01 11:30:00'),
(3, '2023-05-17', 2, 550.00, 'Pendiente', '2023-05-16 14:30:00', '2023-04-01 11:30:00'),
(4, '2023-04-02', 1, 850.00, 'Pendiente', '2023-04-01 10:30:00', '2023-03-01 12:30:00'),
(5, '2023-03-16', 1, 850.00, 'Pendiente', '2023-03-15 10:30:00', '2023-02-01 14:30:00');

-- Table: Usuario
INSERT INTO Usuario (NombreCompleto, Correo, Clave, FechaCreacion) VALUES
('Jay Bryant', 'rushashley@gmail.com', '@N2UQhUP*z', '2021-11-15 17:28:12'),
('Dillon Carlson', 'foxscott@gmail.com', '2%6rW#whR%', '2020-11-26 01:26:50'),
('Marissa Taylor', 'jamesshaw@davis.com', '0O_@2!FlC5', '2022-03-18 06:14:23'),
('James Brown', 'wjohns@barry.com', 'Xy&4AfCv!u', '2022-05-08 23:19:49'),
('Daniel Schneider', 'sullivankristen@morgan.com', '!dJY^^uo4w', '2020-07-11 23:17:40');
