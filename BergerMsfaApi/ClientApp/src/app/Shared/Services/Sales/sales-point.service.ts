import { HttpClient } from '@angular/common/http';
import { Inject } from '@angular/core';
import { APIResponse } from '../../Entity';



export class SalesPointService {
    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }


    getAllSalesPoint() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/salespoint');
    }

}