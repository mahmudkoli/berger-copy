"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SchemeDetail = exports.SchemeMaster = void 0;
var status_1 = require("../../Enums/status");
var SchemeMaster = /** @class */ (function () {
    function SchemeMaster() {
        this.id = 0;
    }
    return SchemeMaster;
}());
exports.SchemeMaster = SchemeMaster;
var SchemeDetail = /** @class */ (function () {
    function SchemeDetail() {
        this.id = 0;
        this.status = status_1.Status.Pending;
    }
    return SchemeDetail;
}());
exports.SchemeDetail = SchemeDetail;
//# sourceMappingURL=SchemeMaster.js.map