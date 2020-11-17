﻿import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';



@Injectable({ providedIn: 'root' })

export class TintingService {

    public baseUrl: string;

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') baseUrl: string
    ) {

        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }
    public getTintingMachineList(territory: string) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/TintingMachine/getTintingMachineList/' + territory);
    }

    public getTintingMachineById(id) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/Tinting/getTintingMachineById/' + id);
    }

 


   


}