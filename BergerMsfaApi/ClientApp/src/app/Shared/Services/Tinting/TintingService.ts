import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
import { CommonService } from '../Common/common.service';

@Injectable({ providedIn: 'root' })

export class TintingService {
    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string,
    private commonService: CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    public getTintingMachinePagingList(index, pageSize, search) {
        return this.http.get<APIResponse>(this.baseUrl + `v1/TintingMachine/getTintingMachinePagingList?index=${index}&pageSize=${pageSize}&search=${search}`);
    }
}
