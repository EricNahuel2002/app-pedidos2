export interface Orden{
    idOrden: number,
    idUsuario : number,
    idMenu : number,
    nombreMenu : string,
    nombreCliente: string,
    emailCliente: string,
    precioAPagar : number,
    estado :string
    direccion : string,
    idRepartidor : number,
    nombreRepartidor : string,
    dniRepartidor : string,
    fechaOrden : string
}