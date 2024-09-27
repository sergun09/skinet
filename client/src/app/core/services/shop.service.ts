import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/Pagination';
import { Product } from '../../shared/models/Product';
import { Observable } from 'rxjs';
import { ShopParams } from '../../shared/models/shopParams';
import { environment } from '../../../environments/environment';
import { UpsertProductDto } from '../../shared/models/upsertProductDto';

@Injectable({
  providedIn: 'root'
})
export class ShopService {

  baseUrl = environment.apiUrl;
  types: string[] = [];
  brands: string[] = [];

  constructor(private http: HttpClient){}

  getProducts(shopParams: ShopParams) : Observable<Pagination<Product>>
  {
    let params = new HttpParams();

    if(shopParams.brands.length > 0)
      params = params.append("brands", shopParams.brands.join(","))
    
    if(shopParams.types.length > 0)
      params = params.append("types", shopParams.types.join(","))

    if(shopParams.sort)
      params = params.append("sort", shopParams.sort)

    if(shopParams.search)
      params = params.append("search", shopParams.search);

    params = params.append("pageSize", shopParams.pageSize);
    params = params.append("pageIndex", shopParams.pageIndex);

    return this.http.get<Pagination<Product>>(this.baseUrl + "products",{params});
  }

  getProduct(id:number)
  {
    return this.http.get<Product>(this.baseUrl + "products/" + id);
  }

  createProduct(productDto : any){
    return this.http.post<Product>(this.baseUrl + "products", productDto)
  }

  updateProduct(productDto : Product, productId : number){
    let params = new HttpParams();

    if(!productId && productId == 0)
      throw new Error("Impossible de mettre un jour un produit qui n'existe pas")

    params.append("id", productId);
    return this.http.put<Product>(this.baseUrl + "products", productDto, {params})
  }

  deleteProduct(id:number)
  {
    return this.http.delete(this.baseUrl + "products/" + id);
  }

  getBrands() 
  {
    if(this.brands.length != 0) 
      return this.brands;
    return this.http.get<string[]>(this.baseUrl + "products/brands").subscribe({
      next : response => this.brands = response
    });
  }

  getTypes()
  {
    if(this.types.length != 0) 
      return this.types;
    return this.http.get<string[]>(this.baseUrl + "products/types").subscribe({
      next : response => this.types = response
    });
  }
}
