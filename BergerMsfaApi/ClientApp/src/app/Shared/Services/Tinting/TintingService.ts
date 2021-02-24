import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
import { CommonService } from '../Common/common.service';

@Injectable({ providedIn: 'root' })

export class TintingService {
    public baseUrl: string;
    public TintingMachinesEndpoint: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string,
    private commonService: CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
        this.TintingMachinesEndpoint = this.baseUrl + 'v1/TintingMachine';
    }

    public getTintingMachinePagingList(index, pageSize, search) {
        return this.http.get<APIResponse>(`${this.TintingMachinesEndpoint}/getTintingMachinePagingList?index=${index}&pageSize=${pageSize}&search=${search}`);
    }

    getTintingMachines(filter?) {
        return this.http.get<APIResponse>(`${this.TintingMachinesEndpoint}?${this.commonService.toQueryString(filter)}`);
    }
}
