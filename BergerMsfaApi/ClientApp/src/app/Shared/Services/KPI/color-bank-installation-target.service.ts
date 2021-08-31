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
    this.baseUrl = baseUrl + 'api';
    this.ColorBankInstallationTargetEndPoint = `${this.baseUrl}/v1/ColorBankInstallationTarget`;
  }

  getCollectionConfigs(filter?) {
    return this.http.get<APIResponse>(
      `${
        this.ColorBankInstallationTargetEndPoint
      }/getTarget?${this.commonService.toQueryString(filter)}`
    );
  }

  saveOrUpdateInstallTarget(filter) {
    return this.http.post<APIResponse>(
      `${this.ColorBankInstallationTargetEndPoint}`,
      filter
    );
  }
}
