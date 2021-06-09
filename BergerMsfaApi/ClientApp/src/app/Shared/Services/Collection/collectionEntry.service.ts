import { Injectable, Inject } from '@angular/core';
import { APIResponse } from '../../Entity/Response';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class CollectionEntryService {

    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    public getCollectionByType( paymentFrom) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/collectionEntry/getCollectionByType/' + paymentFrom);
    }

    deleteCollection(id) {
        return this.http.delete<APIResponse>(`${this.baseUrl}/v1/collectionEntry/DeleteCollection/${id}`);
    }
}
