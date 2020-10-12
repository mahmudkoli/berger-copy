import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity/Response';

@Injectable({
    providedIn: 'root'
})
export class DynamicDropdownService {

    public baseUrl: string;

    constructor( private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    public getDropdownList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/dynamicDropdown/getDropdownList');
    }
    public GetDropdownById(id) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/dynamicDropdown/getDropdownById/' + id);
    }
    public GetDropdownByTypeCd(typeCd) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/dynamicDropdown/GetDropdownByTypeCd/' + typeCd);
    }
    public getDropdownTypeList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/dynamicDropdown/getDropdownTypeList');
    }
    public getLastSquence(id, typeId) {
        return this.http.get<any>(this.baseUrl + `v1/dynamicDropdown/getLastSquence/${id}/${typeId}`);
    }
    public create(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/dynamicDropdown/create', model);

    }
    public update(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/dynamicDropdown/update', model);
    }

    public delete(id: number) {
        return this.http.delete<any>(this.baseUrl + 'v1/dynamicDropdown/' + id);
    }

}
