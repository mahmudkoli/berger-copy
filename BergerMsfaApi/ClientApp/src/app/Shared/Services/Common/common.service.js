"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.CommonService = void 0;
var core_1 = require("@angular/core");
var CommonService = /** @class */ (function () {
    function CommonService(http, baseUrl) {
        this.http = http;
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }
    CommonService.prototype.toFormData = function (obj) {
        var formData = new FormData();
        for (var property in obj) {
            var value = obj[property];
            if (value != null && value !== undefined) {
                formData.append(property, value);
            }
        }
        return formData;
    };
    CommonService.prototype.setUserInfoToLocalStorage = function (value) {
        localStorage.setItem('userinfo', JSON.stringify(value));
    };
    CommonService.prototype.getUserInfoFromLocalStorage = function () {
        if (!localStorage.getItem('userinfo'))
            return null;
        return JSON.parse(localStorage.getItem('userinfo'));
    };
    CommonService.prototype.setActivityPermissionToSessionStorage = function (value) {
        localStorage.setItem('activitypermission', JSON.stringify(value));
    };
    CommonService.prototype.getActivityPermissionToSessionStorage = function () {
        if (!localStorage.getItem('activitypermission'))
            return null;
        return JSON.parse(localStorage.getItem('activitypermission'));
    };
    CommonService.prototype.getSaleOfficeList = function () {
        return this.http.get(this.baseUrl + 'v1/Common/getSaleOfficeList');
    };
    CommonService.prototype.getSaleGroupList = function () {
        return this.http.get(this.baseUrl + 'v1/Common/getSaleGroupList');
    };
    CommonService.prototype.getTerritoryList = function () {
        return this.http.get(this.baseUrl + 'v1/Common/getTerritoryList');
    };
    CommonService.prototype.getZoneList = function () {
        return this.http.get(this.baseUrl + 'v1/Common/getZoneList');
    };
    CommonService.prototype.getRoleList = function () {
        return this.http.get(this.baseUrl + 'v1/Common/getRoleList');
    };
    CommonService.prototype.getDepotList = function () {
        return this.http.get(this.baseUrl + 'v1/Common/getDepotList');
    };
    CommonService.prototype.getUserInfoList = function () {
        return this.http.get(this.baseUrl + 'v1/Common/getUserInfoList');
    };
    CommonService.prototype.getDealerList = function (userCategory, userCategoryIds) {
        return this.http.get(this.baseUrl + ("v1/AppDealer/getDealerList?userCategory=" + userCategory + "&userCategoryIds=" + userCategoryIds));
    };
    CommonService = __decorate([
        core_1.Injectable({
            providedIn: 'root'
        }),
        __param(1, core_1.Inject('BASE_URL'))
    ], CommonService);
    return CommonService;
}());
exports.CommonService = CommonService;
//# sourceMappingURL=common.service.js.map