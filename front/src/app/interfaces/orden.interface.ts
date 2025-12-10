export interface Orden{
    idOrden: number,
    idUsuario : number,
    idMenu : number,
    nombreCliente: string,
    emailCliente: string,
    precioAPagar : number,
    estado :string
    direccion : string,
    fechaOrden : string
}