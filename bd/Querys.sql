--Alta producto
INSERT INTO Productos (Descripcion, Precio) 
VALUES ('Mouse Inalámbrico Logitech', 5000);

--Alta presupuesto
INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) 
VALUES ('Carlos Ruiz', '2024-10-25');

-- Agrega 2 unidades del producto con id 3 en el presupuesto 1
INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) 
VALUES (1, 3, 2); 

-- actualiza el nombre de un prudcto
UPDATE Presupuestos 
SET NombreDestinatario = 'Luis Fernández' 
WHERE idPresupuesto = 1;

-- actualiza la descripcion y el precio de un producto
UPDATE Productos 
SET Descripcion = 'Teclado Mecánico Logitech', 
    Precio = 12000
WHERE idProducto = 3;

-- lista 1 presupuesto con su detalle de productos
SELECT 
    P.idPresupuesto,
    P.NombreDestinatario,
    P.FechaCreacion,
    PR.idProducto,
    PR.Descripcion AS Producto,
    PR.Precio,
    PD.Cantidad,
    (PR.Precio * PD.Cantidad) AS Subtotal
FROM 
    Presupuestos P
JOIN 
    PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
JOIN 
    Productos PR ON PD.idProducto = PR.idProducto
WHERE 
    P.idPresupuesto = 1;
    
-- Lista de Presupuestos
SELECT 
    P.idPresupuesto,
    P.NombreDestinatario,
    P.FechaCreacion
FROM 
    Presupuestos P;
    
--borra producto de presupuesto producto
DELETE FROM PresupuestosDetalle
WHERE idPresupuesto = [idPresupuesto] AND idProducto = [idProducto];

