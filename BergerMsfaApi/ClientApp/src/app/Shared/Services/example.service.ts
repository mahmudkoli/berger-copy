import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class ExampleService {
  public baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    console.log("baseUrl: ", baseUrl);
    this.baseUrl = baseUrl + 'api/';
  }

  public getExampleData() {
    console.log("service of example data: ");
    return this.http.get<any[]>(this.baseUrl + 'v1/Example');
  }
  public getDailyCMData() {
    return this.http.get<any[]>(this.baseUrl + 'v1/DailyCMActivity');
  }

  public postExample(model) {
    this.http.post(this.baseUrl + 'v1/Example', model);

  }
}
