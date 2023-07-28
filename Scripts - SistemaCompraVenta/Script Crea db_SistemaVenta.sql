
CREATE DATABASE db_sistemaventa

GO

USE db_sistemaventa

GO

create table ROL(
id_Rol int primary key identity,
Descripcion varchar(50),
F_Registro datetime default getdate()
)

go

create table PERMISO(
id_Permiso int primary key identity,
id_Rol int references ROL(id_Rol),
Nom_Menu varchar(100),
F_Registro datetime default getdate()
)

go

create table PROVEEDOR(
id_Proveedor int primary key identity,
Documento varchar(50),
Razon_Social varchar(50),
Correo varchar(50),
Telefono varchar(50),
Estado bit,
F_Registro datetime default getdate()
)

go

create table CLIENTE(
id_Cliente int primary key identity,
Documento varchar(50),
Nom_Completo varchar(100),
Correo varchar(50),
Telefono varchar(50),
Estado bit,
F_Registro datetime default getdate()
)

go

create table USUARIO(
id_Usuario int primary key identity,
Documento varchar(50),
Nom_Completo varchar(100),
Correo varchar(50),
Clave varchar(50),
id_Rol int references ROL(id_Rol),
Estado bit,
F_Registro datetime default getdate()
)

go

create table CATEGORIA(
id_Categoria int primary key identity,
Descripcion varchar(50),
Estado bit,
F_Registro datetime default getdate()
)

go

create table PRODUCTO(
id_Producto int primary key identity,
Codigo varchar(50),
Nombre varchar(50),
Descripcion varchar(50),
id_Categoria int references CATEGORIA(id_Categoria),
Stock int not null default 0,
Precio_Compra decimal(10,2) default 0,
Precio_Venta decimal(10,2) default 0,
Estado bit,
F_Registro datetime default getdate()
)

go

create table COMPRA(
id_Compra int primary key identity,
id_Usuario int references USUARIO(id_Usuario),
id_Proveedor int references PROVEEDOR(id_Proveedor),
Tipo_Documento varchar(50),
Nro_Documento varchar(50),
Monto_Total decimal(10,2),
F_Registro datetime default getdate()
)

go

create table DETALLE_COMPRA(
id_DetalleCompra int primary key identity,
id_Compra int references COMPRA(id_Compra),
id_Producto int references PRODUCTO(id_Producto),
Precio_Compra decimal(10,2) default 0,
Precio_Venta decimal(10,2) default 0,
Cantidad int,
Monto_Total decimal(10,2),
F_Registro datetime default getdate()
)

go

create table VENTA(
id_Venta int primary key identity,
id_Usuario int references USUARIO(id_Usuario),
Tipo_Documento varchar(50),
Nro_Documento varchar(50),
Documento_Cliente varchar(50),
Nom_Cliente varchar(100),
Monto_Pago decimal(10,2),
Monto_Cambio decimal(10,2),
Monto_Total decimal(10,2),
F_Registro datetime default getdate()
)

go

create table DETALLE_VENTA(
id_DetalleVenta int primary key identity,
id_Venta int references VENTA(id_Venta),
id_Producto int references PRODUCTO(id_Producto),
Precio_Venta decimal(10,2),
Cantidad int,
SubTotal decimal(10,2),
F_Registro datetime default getdate()
)

go

create table NEGOCIO(
id_Negocio int primary key,
Nombre varchar(50),
RUC varchar(50),
Direccion varchar(50),
Logo varbinary(max) null
)

---------------------------------------

select * from ROL

select * from PERMISO

select * from PROVEEDOR

select * from CLIENTE

select * from USUARIO

select * from CATEGORIA

select * from PRODUCTO

select * from COMPRA

select * from DETALLE_COMPRA

select * from VENTA

select * from DETALLE_VENTA

select * from NEGOCIO

------------------------------------------
