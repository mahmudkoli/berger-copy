import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MenuPermission } from '../../Entity/Menu/menuPermission.model';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  public url: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = `${baseUrl}api/v1/menu`;
  }

  public getAll() {
    return this.http.get(`${this.url}`);
  }

  public getAllActive() {
    return this.http.get(`${this.url}/get-active`);
  }

  public getAllChild() {
    return this.http.get(`${this.url}/get-child`);
  }

  public getAllById(id: number) {
    return this.http.get(`${this.url}/${id}`);
  }

  public delete(id: number) {
    return this.http.delete(`${this.url}/delete/${id}`);
  }

  public create(model: any) {
    return this.http.post<any>(`${this.url}/create/`, model);
  }

  public update(model: any) {
    return this.http.put(`${this.url}/update/`, model);
  }

  public assignRoleToMenu(model: MenuPermission[], roleId) {    
    return this.http.post<any>(`${this.url}/assignRoleToMenu/${roleId}`, model);
  } 

  public getPermissionMenus(roleId) {    
    return this.http.get<any>(`${this.url}/get-permission-menu/${roleId}`);
  }   
}
