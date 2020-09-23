import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  public baseUrl: string;
  public testBaseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl + "api/";
    this.testBaseUrl = baseUrl;
  }

  public postLoginData(data) {
    const body = JSON.parse(data);
    return this.http.post(this.testBaseUrl + 'api/v1/Login/login', body, { responseType: 'json' });
  }

  public Login(data) {

    // const headers = { 'Authorization': 'Bearer ' }
    const body = JSON.parse(data);
    console.log(body);

    return this.http.post(this.baseUrl + 'v1/Login/aduserlogin', body, { responseType: 'json' });
  }


  public getExampleData(token) {
    return this.http.get<any>(this.baseUrl + 'v1/Login/getuser');
  }

  public getAdUser() {
    return this.http.get<any>(this.baseUrl + 'v1/Login/getaduser');
  }

  public IsLoggedIn()
  {
     return !!localStorage.getItem('bergermsfa');
  }
}




