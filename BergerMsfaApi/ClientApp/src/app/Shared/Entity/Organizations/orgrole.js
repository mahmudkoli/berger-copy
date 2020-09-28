"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.OrganizationRole = void 0;
var status_1 = require("../../Enums/status");
var OrganizationRole = /** @class */ (function () {
    function OrganizationRole() {
        this.id = 0;
        this.name = '';
        this.status = status_1.Status.Active;
        this.configList = [];
    }
    return OrganizationRole;
}());
exports.OrganizationRole = OrganizationRole;
//# sourceMappingURL=orgrole.js.map