import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { IAuthUser, Login, ResetPassword } from '../../Entity/Users/auth';
import { JwtHelperService } from '@auth0/angular-jwt';
import * as _ from 'lodash';
import { map } from 'rxjs/operators';
import { APIResponse } from '../../Entity/Response/api-response';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public baseUrl: string;
  public authEndpoint: string;
  private currentUserSubject: BehaviorSubject<IAuthUser>;
  public currentUser: Observable<IAuthUser>;

  constructor(private http: HttpClient,
    private jwtHelper: JwtHelperService, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl + "api/";
    this.authEndpoint = this.baseUrl + 'v1/auth';
    this.currentUserSubject = new BehaviorSubject<IAuthUser>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  
  public get currentUserValue(): IAuthUser {
    return this.currentUserSubject.value;
  }

  public get isLoggedIn(): boolean {
      return this.currentUserSubject.value !== null;
  }

  public get isTokenExpired(): boolean {
    if(!this.isLoggedIn) return true;
    const token = this.currentUserSubject.value.token;
    return this.jwtHelper.isTokenExpired(token);
  }

  public get currentUserToken(): string {
    if(!this.isLoggedIn) return "";
    const token = this.currentUserSubject.value.token;
    return token;
  }

  private get getPermissions(): any[] {
    if(!this.isLoggedIn) return [];
    const token = this.currentUserSubject.value.token;
    const decodedToken = this.jwtHelper.decodeToken(token);
    const permissions = decodedToken.Permission as any[];
    if(Array.isArray(permissions))  return permissions; 
    else  return [permissions];
  }

  hasPermission(per: string | string[]) : boolean {
    var permission = [];
    if(Array.isArray(per)) permission = per;
    else permission = [per];
    return !_.isEmpty(_.intersection(this.getPermissions, permission));
  }

  login(login: Login) {
    return this.http.post<APIResponse>(`${this.authEndpoint}/login`, login)
        .pipe(map((res: APIResponse) => {
            const authUser = res && res.data;
            if (authUser && authUser.token) {
                localStorage.setItem('currentUser', JSON.stringify(authUser));
                this.currentUserSubject.next(authUser);
                // this.ngxPermissionsService.loadPermissions(this.getPermissions);
            }
            return authUser;
        }));
  }

  forgotPassword(email: string) {
    return this.http.get<APIResponse>(`${this.authEndpoint}/forgot-password/${email}`);
  }

  resetPassword(resetPass: ResetPassword) {
    return this.http.post<APIResponse>(`${this.authEndpoint}/reset-password`, resetPass);
  }

  logout() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    // this.ngxPermissionsService.flushPermissions();
  }

}




