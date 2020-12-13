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
exports.TintingService = void 0;
var core_1 = require("@angular/core");
var TintingService = /** @class */ (function () {
    function TintingService(http, baseUrl) {
        this.http = http;
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }
    TintingService.prototype.getTintingMachineList = function (territory) {
        return this.http.get(this.baseUrl + 'v1/TintingMachine/getTintingMachineList/' + territory);
    };
    TintingService.prototype.getTintingMachineById = function (id) {
        return this.http.get(this.baseUrl + 'v1/Tinting/getTintingMachineById/' + id);
    };
    TintingService = __decorate([
        core_1.Injectable({ providedIn: 'root' }),
        __param(1, core_1.Inject('BASE_URL'))
    ], TintingService);
    return TintingService;
}());
exports.TintingService = TintingService;
//# sourceMappingURL=TintingService.js.map