export interface Resena {
    id: number;
    idOrden: number;
    idCliente: number;
    idRepartidor: number;
    nombreCliente: string;
    nombreRepartidor: string;
    puntaje: number;
    comentario: string | null;
    fechaCreacion: string;
}

export interface ResenasRepartidor {
    idRepartidor: number;
    nombreRepartidor: string;
    promedio: number;
    cantidad: number;
    resenas: Resena[];
}
