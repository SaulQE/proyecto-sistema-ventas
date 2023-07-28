USE db_sistemaventa

select * from USUARIO


/*  CREACIÓN DE PROCEDIMIENTOS ALMACENADOS  */

-----PROCEDIMIENTOS PARA USUARIO------
/* PROCEDIMIENTO ALMACENADO PARA REGISTRAR USUARIO */

CREATE PROC SP_REGISTRARUSUARIO(
@Documento varchar(50),
@Nom_Completo varchar(100),
@Correo varchar(50),
@Clave varchar(50),
@id_Rol int,
@Estado bit,
@id_UsuarioResultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @id_UsuarioResultado = 0
	set @Mensaje = ''


	if not exists(select * from USUARIO	where Documento = @Documento)
	begin
		
		insert into usuario(Documento,Nom_Completo,Correo,Clave,id_Rol,Estado) values
		(@Documento,@Nom_Completo,@Correo,@Clave,@id_Rol,@Estado)

		set @id_UsuarioResultado = SCOPE_IDENTITY() 
		
	end
	else 
		set @Mensaje = 'No se puede repetir el documento de otro usuario' 
end

go


/* PROCEDMIENTO ALMACENADO PARA EDITAR USAURIO */

CREATE PROC SP_EDITARUSUARIO(
@id_Usuario int,
@Documento varchar(50),
@Nom_Completo varchar(100),
@Correo varchar(50),
@Clave varchar(50),
@id_Rol int,
@Estado bit,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = '' 


	if not exists(select * from USUARIO	where Documento = @Documento and id_Usuario != @id_Usuario)
	begin
		update usuario set
		Documento = @Documento,
		Nom_Completo = @Nom_Completo,
		Correo = @Correo,
		Clave = @Clave,
		id_Rol = @id_Rol,
		Estado =@Estado
		where id_Usuario = @id_Usuario

		set @Respuesta = 1 
		
	end
	else 
		set @Mensaje = 'No se puede repetir el documento de otro usuario'
end

go


/* PROCEDIMIENTO ALMACENADO PARA ELIMINAR USUARIO */

CREATE PROC SP_ELIMINARUSUARIO(
@id_Usuario int,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = '' 
	declare @pasoregla bit = 1 

	IF EXISTS (SELECT * FROM COMPRA C
	INNER JOIN USUARIO U ON U.id_Usuario = C.id_Usuario 
	WHERE U.id_Usuario = @id_Usuario
	)
	BEGIN
		set @pasoregla = 0 
		set @Respuesta = 0 
		set @Mensaje = @Mensaje + 'Un usuario relacionado a una compra no se puede eliminar\n'
	END

	IF EXISTS (SELECT * FROM VENTA V
	INNER JOIN USUARIO U ON U.id_Usuario = V.id_Usuario 
	WHERE U.id_Usuario = @id_Usuario
	)
	BEGIN
		set @pasoregla = 0 
		set @Respuesta = 0 
		set @Mensaje = @Mensaje + 'Un usuario relacionado a una venta no se puede eliminar\n'
	END

	if(@pasoregla = 1)
	BEGIN
		delete from USUARIO where id_Usuario = @id_Usuario
		set @Respuesta = 1 
	END
end


go

/* ---- PROCEDIMIENTO ALMACENADO PARA CATEGORIA ----*/
--PROCEDIMIENTO PARA GUARDAR CATEGORIA

CREATE PROC SP_RegistrarCategoria(
@Descripcion varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
begin
	set @Resultado = 0
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion)
	begin
		insert into CATEGORIA(Descripcion,Estado) values (@Descripcion,@Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	ELSE
		set @Mensaje = 'No se puede repetir la descripcion de una categoria'
end

go

--PROCEDIMIENTO PARA MODIFICAR CATEGORIA
CREATE PROC SP_EditarCategoria(
@id_Categoria int,
@Descripcion varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)as
begin
	set @Resultado = 1 
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion and id_Categoria != @id_Categoria)
		update CATEGORIA set
		Descripcion = @Descripcion,
		Estado = @Estado
		where id_Categoria = @id_Categoria
	ELSE
	begin
		set @Resultado = 0 
		set @Mensaje = 'No se puede repetir la descripcion de una categoria'
	end
end


go


--PROCEDIMIENTO PARA ELIMINAR CATEGORIA
CREATE PROC SP_EliminarCategoria(
@id_Categoria int,
@Resultado bit output,
@Mensaje varchar(500) output
)as
begin
	set @Resultado = 1 
	IF NOT EXISTS (
	 select * from CATEGORIA c
	 inner join PRODUCTO p on p.id_Categoria = c.id_Categoria
	 where c.id_Categoria = @id_Categoria
	)
	begin
		delete top(1) from CATEGORIA where id_Categoria = @id_Categoria
	end
	ELSE
	begin
		set @Resultado = 0 
		set @Mensaje = 'La categoria se encuentra relacionada a un producto'
	end
end

go

/* PROCEDIMIENTO ALMACENADO PARA PRODUCTO */
--PROCEDIMIENTO PARA REGISTRAR PRODUCTO
CREATE PROC SP_RegistrarProducto(
@Codigo varchar(50),
@Nombre varchar(50),
@Descripcion varchar(50),
@id_Categoria int,
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
begin
	set @Resultado = 0 
	IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE Codigo = @Codigo)
	begin
		insert into PRODUCTO(Codigo,Nombre,Descripcion,id_Categoria,Estado) values (@Codigo,@Nombre,@Descripcion,@id_Categoria,@Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	ELSE
		set @Mensaje = 'Ya existe un producto con el mismo codigo'
end

go

--PROCEDIMIENTO EDITAR PRODUCTO
CREATE PROC SP_EditarProducto(
@id_Producto int,
@Codigo varchar(50),
@Nombre varchar(50),
@Descripcion varchar(50),
@id_Categoria int,
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
begin
	set @Resultado = 1
	IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE Codigo = @Codigo and id_Producto != @id_Producto)
		update PRODUCTO set
		Codigo = @Codigo,
		Nombre = @Nombre,
		Descripcion = @Descripcion,
		id_Categoria = @id_Categoria,
		Estado = @Estado
		where id_Producto = @id_Producto
	ELSE
	begin
		set @Resultado = 0
		set @Mensaje = 'Ya existe un producto con el mismo codigo'
	end
end


go


--PROCEDIMIENTO ELIMINAR PRODUCTO
CREATE PROC SP_EliminarProducto(
@id_Producto int,
@Respuesta bit output,
@Mensaje varchar(500) output
)as
begin
	set @Respuesta = 0 
	set @Mensaje = '' 
	declare @pasoreglas bit = 1 

	IF EXISTS (SELECT * FROM DETALLE_COMPRA dc
	INNER JOIN PRODUCTO p on p.id_Producto = dc.id_Producto
	WHERE p.id_Producto = @id_Producto
	)
	BEGIN
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar porque se encuentra relacionado a una COMPRA\n'
	END

	IF EXISTS (SELECT * FROM DETALLE_VENTA dv
	INNER JOIN PRODUCTO p on p.id_Producto = dv.id_Producto
	WHERE p.id_Producto = @id_Producto
	)
	BEGIN
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar porque se encuentra relacionado a una VENTA\n'
	END

	if(@pasoreglas = 1)
	begin
		delete from PRODUCTO where id_Producto = @id_Producto
		set @Respuesta = 1
	end
end


go


/* PROCEDIMIENTO ALMACENADO PARA CLIENTE */
--PROCEDIMIENTO PARA REGISTRAR CLIENTE

CREATE PROC SP_RegistrarCliente(
@Documento varchar(50),
@Nom_Completo varchar(100),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
BEGIN
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Documento = @Documento)
	BEGIN
		INSERT INTO CLIENTE (Documento,Nom_Completo,Correo,Telefono,Estado) VALUES(
		@Documento,@Nom_Completo,@Correo,@Telefono,@Estado)

		SET @Resultado = SCOPE_IDENTITY()
	END
	ELSE
		SET @Mensaje = 'El número de documento ya existe' 
END


go


--PROCEDIMIENTO EDITAR CLIENTE
CREATE PROC SP_EditarCliente(
@id_Cliente int,
@Documento varchar(50),
@Nom_Completo varchar(100),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
BEGIN
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Documento = @Documento and id_Cliente != @id_Cliente)
	BEGIN
		UPDATE CLIENTE SET
		Documento = @Documento,
		Nom_Completo = @Nom_Completo,
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		WHERE id_Cliente = @id_Cliente
	END
	ELSE
	BEGIN
		SET @Resultado = 0
		SET @Mensaje = 'El número de documento ya existe'
	END
END


go

/* PROCEDIMIENTO ALMACENADO PARA PROVEEDORES */
--PROCEDIMIENTO PARA REGISTRAR PROVEEDORES

CREATE PROC SP_RegistrarProveedor(
@Documento varchar(50),
@Razon_Social varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
BEGIN
	SET @Resultado = 0 
	IF NOT EXISTS (SELECT * FROM PROVEEDOR WHERE Documento = @Documento)
	BEGIN
		INSERT INTO PROVEEDOR (Documento,Razon_Social,Correo,Telefono,Estado) VALUES(
		@Documento,@Razon_Social,@Correo,@Telefono,@Estado)

		SET @Resultado = SCOPE_IDENTITY()
	END
	ELSE
		SET @Mensaje = 'El número de documento ya existe'
END

GO

--PROCEDIMIENTO PARA EDITAR PROVEEDOR
CREATE PROC SP_EditarProveedor(
@id_Proveedor int,
@Documento varchar(50),
@Razon_Social varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
BEGIN
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM PROVEEDOR WHERE Documento = @Documento and id_Proveedor != @id_Proveedor)
	BEGIN 
		UPDATE PROVEEDOR SET
		Documento = @Documento,
		Razon_Social = @Razon_Social,
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		WHERE id_Proveedor = @id_Proveedor
	END
	ELSE
	BEGIN
		SET @Resultado = 0
		SET @Mensaje = 'El número de documento ya existe'
	END
END

go

--PROCEDIMIENTO PARA ELIMINAR PROVEEDOR
CREATE PROC SP_EliminarProveedore(
@id_Proveedor int,
@Resultado bit output,
@Mensaje varchar(500) output
)as
BEGIN
	SET @Resultado = 1
	IF NOT EXISTS (
	SELECT * FROM PROVEEDOR p
	INNER JOIN COMPRA c on p.id_Proveedor = c.id_Proveedor
	WHERE p.id_Proveedor = @id_Proveedor
	)
	BEGIN
		DELETE TOP(1) FROM PROVEEDOR WHERE id_Proveedor = @id_Proveedor
	END
	ELSE
	BEGIN
		SET @Resultado = 0
		SET @Mensaje = 'El Proveedor se encuentra relacionado a una compra'
	END
END

go




/* PROCESOS PARA REGISTRAR UNA COMPRA */ 

CREATE TYPE [dbo].[EDetalle_Compra] AS TABLE(
	[id_Producto] int NULL,
	[Precio_Compra] decimal(18,2) NULL,
	[Precio_Venta] decimal(18,2) NULL,
	[Cantidad] int NULL,
	[Monto_Total] decimal(18,2) NULL
)


go


/* PROCEDIMIENTO ALMACENADO REGISTRAR COMPRAS */
CREATE PROC SP_RegistrarCompra(
@id_Usuario int,
@id_Proveedor int,
@Tipo_Documento varchar(50),
@Nro_Documento varchar(50),
@Monto_Total decimal(18,2),
@Detalle_Compra [EDetalle_Compra] READONLY, 
@Resultado bit output,
@Mensaje varchar(500) output
)as
BEGIN
	BEGIN try
		declare @id_Compra int = 0
		set @Resultado = 1 
		set @Mensaje = '' 

		begin transaction registro

		insert into COMPRA(id_Usuario,id_Proveedor,Tipo_Documento,Nro_Documento,Monto_Total)
		values(@id_Usuario,@id_Proveedor,@Tipo_Documento,@Nro_Documento,@Monto_Total)

		set @id_Compra = SCOPE_IDENTITY() 

		insert into DETALLE_COMPRA(id_Compra,id_Producto,Precio_Compra,Precio_Venta,Cantidad,Monto_Total)
		select @id_Compra,id_Producto,Precio_Compra,Precio_Venta,Cantidad,Monto_Total from @Detalle_Compra

		update p set  p.Stock = p.Stock + dc.Cantidad,
		p.Precio_Compra = dc.Precio_Compra,
		p.Precio_Venta = dc.Precio_Venta
		from PRODUCTO p
		INNER JOIN @Detalle_Compra dc on dc.id_Producto = p.id_Producto

		commit transaction registro
	END try

	BEGIN catch
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro 
	END catch
END

go


/* PROCESO PARA REGISTRAR UNA VENTA */

CREATE TYPE [dbo].[EDetalle_Venta] AS TABLE(
	[id_Producto] int NULL,
	[Precio_Venta] decimal(18,2) NULL,
	[Cantidad] int NULL,
	[SubTotal] decimal(18,2) NULL
)

go

/* PROCEDIMIENTO ALMACENADO REGISTRAR VENTAS */
CREATE PROC SP_RegistrarVenta(
@id_Usuario int,
@Tipo_Documento varchar(50),
@Nro_Documento varchar(50),
@Documento_Cliente varchar(50),
@Nom_Cliente varchar(100),
@Monto_Pago decimal(18,2),
@Monto_Cambio decimal(18,2),
@Monto_Total decimal(18,2),
@Detalle_Venta [EDetalle_Venta] READONLY,
@Resultado bit output,
@Mensaje varchar(500) output
)as
BEGIN
	BEGIN TRY
		DECLARE @idventa int = 0
		SET @Resultado = 1
		SET @Mensaje = ''

		BEGIN TRANSACTION registro

		INSERT INTO VENTA(id_Usuario,Tipo_Documento,Nro_Documento,Documento_Cliente,Nom_Cliente,Monto_Pago,Monto_Cambio,Monto_Total)
		VALUES(@id_Usuario,@Tipo_Documento,@Nro_Documento,@Documento_Cliente,@Nom_Cliente,@Monto_Pago,@Monto_Cambio,@Monto_Total)
		SET @idventa = SCOPE_IDENTITY()

		INSERT INTO DETALLE_VENTA(id_Venta,id_Producto,Precio_Venta,Cantidad,SubTotal)
		SELECT @idventa,id_Producto,Precio_Venta,Cantidad,SubTotal FROM @Detalle_Venta

		COMMIT TRANSACTION registro 
	
	END TRY
	BEGIN CATCH
		SET @Resultado = 0 
		SET @Mensaje = ERROR_MESSAGE()
		ROLLBACK TRANSACTION registro 
	END CATCH
END

go

/* PROCEDIMIENTO ALMACENADO PARA REPORTE DE COMPRAS */
CREATE PROC SP_ReporteCompras(
@fechaInicio varchar(10),
@fechaFin varchar(10),
@id_Proveedor int
)as
BEGIN

 SET DATEFORMAT dmy;
 select 
CONVERT(char(10),c.F_Registro,103)[F_Registro],c.Tipo_Documento,c.Nro_Documento,c.Monto_Total,
u.Nom_Completo[UsuarioRegistro],
pr.Documento[DocumentoProveedor],pr.Razon_Social,
p.Codigo[CodigoProducto],p.Nombre[NombreProducto],ca.Descripcion[Categoria],dc.Precio_Compra,dc.Precio_Venta,dc.Cantidad,dc.Monto_Total[SubTotal]
from COMPRA c
INNER JOIN USUARIO u on u.id_Usuario = c.id_Usuario
INNER JOIN PROVEEDOR pr on pr.id_Proveedor = c.id_Proveedor
INNER JOIN DETALLE_COMPRA dc on dc.id_Compra = c.id_Compra
INNER JOIN PRODUCTO p on p.id_Producto = dc.id_Producto
INNER JOIN CATEGORIA ca on ca.id_Categoria = p.id_Categoria
WHERE CONVERT(date,c.F_Registro) BETWEEN @fechaInicio and @fechaFin
and pr.id_Proveedor = iif (@id_Proveedor = 0,pr.id_Proveedor,@id_Proveedor)
END

exec SP_ReporteCompras '05/05/2023','30/05/2023',0

go

/* PROCEDIMIENTO ALMACENADO PARA REPORTE DE VENTAS */
CREATE PROC SP_ReporteVentas(
@fechaInicio varchar(10),
@fechaFin varchar(10)
)as
BEGIN

 SET DATEFORMAT dmy;
 select 
CONVERT(char(10),v.F_Registro,103)[F_Registro],v.Tipo_Documento,v.Nro_Documento,v.Monto_Total,
u.Nom_Completo[UsuarioRegistro],
v.Documento_Cliente,v.Nom_Cliente,
p.Codigo[CodigoProducto],p.Nombre[NombreProducto],ca.Descripcion[Categoria],dv.Precio_Venta,dv.Cantidad,dv.SubTotal
from VENTA v
INNER JOIN USUARIO u on u.id_Usuario = v.id_Usuario
INNER JOIN DETALLE_VENTA dv on dv.id_Venta = v.id_Venta
INNER JOIN PRODUCTO p on p.id_Producto = dv.id_Producto
INNER JOIN CATEGORIA ca on ca.id_Categoria = p.id_Categoria
WHERE CONVERT(date,v.F_Registro) BETWEEN @fechaInicio and @fechaFin
END


exec SP_ReporteVentas '05/05/2023','30/05/2023'