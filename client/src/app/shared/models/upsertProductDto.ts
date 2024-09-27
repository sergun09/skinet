export type UpsertProductDto = 
{
    id? : number
    name: string,
    description: string,
    price: number,
    file: File,
    pictureUrl?: string
    brand: string,
    type: string,
    quantityInStock: number
}