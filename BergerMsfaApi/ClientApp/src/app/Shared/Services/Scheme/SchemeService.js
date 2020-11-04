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
exports.SchemeService = void 0;
var core_1 = require("@angular/core");
var SchemeService = /** @class */ (function () {
    function SchemeService(http, baseUrl) {
        this.http = http;
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }
    SchemeService.prototype.getSchemeDetailWithMaster = function () {
        return this.http.get(this.baseUrl + 'v1/SchemeDetail/getSchemeDetailWithMaster');
    };
    SchemeService.prototype.getSchemeMasterList = function () {
        return this.http.get(this.baseUrl + 'v1/SchemeMaster/getSchemeMasterList');
    };
    SchemeService.prototype.createSchemeMaster = function (model) {
        return this.http.post(this.baseUrl + 'v1/SchemeMaster/createSchemeMaster', model);
    };
    SchemeService.prototype.UpdateSchemeMaster = function (model) {
        return this.http.put(this.baseUrl + 'v1/SchemeMaster/updateSchemeMaster', model);
    };
    SchemeService.prototype.DeleteSchemeMaster = function (id) {
        return this.http.delete(this.baseUrl + 'v1/SchemeMaster/DeleteSchemeMaster/' + id);
    };
    SchemeService.prototype.getSchemeMasterById = function (id) {
        return this.http.get(this.baseUrl + 'v1/SchemeMaster/GetSchemeMasterById/' + id);
    };
    //scheme detail
    SchemeService.prototype.getSchemeDetailList = function () {
        return this.http.get(this.baseUrl + 'v1/SchemeDetail/getSchemeDetailList');
    };
    SchemeService.prototype.getSchemeDetailById = function (id) {
        return this.http.get(this.baseUrl + 'v1/SchemeDetail/getSchemeDetailById/' + id);
    };
    SchemeService.prototype.createSchemeDetail = function (model) {
        return this.http.post(this.baseUrl + 'v1/SchemeDetail/createSchemeDetail', model);
    };
    SchemeService.prototype.updateSchemeDetail = function (model) {
        return this.http.put(this.baseUrl + 'v1/SchemeMaster/updateSchemeDetail', model);
    };
    SchemeService.prototype.deleteSchemeDetail = function (id) {
        return this.http.delete(this.baseUrl + 'v1/SchemeMaster/deleteSchemeDetail/' + id);
    };
    SchemeService = __decorate([
        core_1.Injectable({ providedIn: 'root' }),
        __param(1, core_1.Inject('BASE_URL'))
    ], SchemeService);
    return SchemeService;
}());
exports.SchemeService = SchemeService;
//# sourceMappingURL=SchemeService.js.map