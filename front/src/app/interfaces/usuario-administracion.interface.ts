export interface UsuarioAdministracion {
    id: number,
    nombre: string,
    email: string,
    rol: string,
    esCliente: boolean,
    esRepartidor: boolean,
    repartidorVerificado: boolean
}
