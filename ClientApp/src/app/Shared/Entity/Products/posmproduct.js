"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var status_1 = require("../../Enums/status");
var PosmProduct = /** @class */ (function () {
    function PosmProduct() {
        this.id = 0;
        this.name = '';
        //this.isJTIProduct = YesNo.Yes;
        //this.isDigitalSignatureEnable = YesNo.Yes;
        this.status = status_1.Status.Active;
    }
    return PosmProduct;
}());
exports.PosmProduct = PosmProduct;
//# sourceMappingURL=posmproduct.js.map