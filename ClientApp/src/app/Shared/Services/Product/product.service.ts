import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity/Response/api-response';
@Injectable({
    providedIn: 'root'
})
export class ProductService {
    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    public getProductList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/product');
    }

    public getProduct(id: number) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/product/'+id);
    }

    public postProduct(model) {

        return this.http.post<APIResponse>(this.baseUrl + 'v1/product/save', model);
    }

    public putProduct(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/product/update', model);
    }

    public deleteProduct(id: number) {
        return this.http.delete<any>(`${this.baseUrl}v1/product/delete/${id}`);
    }
}
