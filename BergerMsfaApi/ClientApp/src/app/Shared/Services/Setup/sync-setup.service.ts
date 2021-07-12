import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { APIResponse } from '../../Entity';

@Injectable({
  providedIn: 'root',
})
export class SyncSetupService {
  public baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    console.log('baseUrl: ', baseUrl);
    this.baseUrl = baseUrl + 'api/';
  }

  public getSyncSeetup() {
    return this.http.get<APIResponse>(this.baseUrl + 'v1/syncsetup');
  }

  public getById(id: number) {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/syncsetup/GetById/' + id
    );
  }

  public update(model) {
    return this.http.put<APIResponse>(
      this.baseUrl + 'v1/syncsetup/update',
      model
    );
  }
}
