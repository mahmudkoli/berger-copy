import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { APIResponse } from '../../Entity';
import { CommonService } from '../Common/common.service';
@Injectable({
  providedIn: 'root',
})
export class ColorBankInstallationTargetService {
  public baseUrl: string;
  public ColorBankInstallationTargetEndPoint: string;
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private commonService: CommonService
  ) {
    this.ColorBankInstallationTargetEndPoint = `${this.baseUrl}/v1/CollectionConfig`;
  }

  getCollectionConfigs() {
    return this.http.get<APIResponse>(
      `${this.ColorBankInstallationTargetEndPoint}`
    );
  }
}
