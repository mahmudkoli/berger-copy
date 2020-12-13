"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.StatusPipe = void 0;
var core_1 = require("@angular/core");
var PlanStatus_1 = require("../Enums/PlanStatus");
var StatusPipe = /** @class */ (function () {
    function StatusPipe() {
    }
    StatusPipe.prototype.transform = function (value) {
        debugger;
        var val = Number(value) ? PlanStatus_1.PlanStatus[value] : undefined;
        return val;
    };
    StatusPipe = __decorate([
        core_1.Pipe({ name: 'msfaStatus' })
    ], StatusPipe);
    return StatusPipe;
}());
exports.StatusPipe = StatusPipe;
//@Pipe({ name: 'msfaPlanStatus' })
//export class PlanStatusPipe implements PipeTransform {
//    transform(value: number): string {
//        debu
//        return Number(value) ? PlanStatus[value] : undefined;
//    }
//}
//# sourceMappingURL=status-filter.pipe.js.map