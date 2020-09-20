"use strict";
//export enum RejectedStatus {
//    ReturnFromAFMM = 1,
//    ReturnFromRFMM = 2,
//    ReturnFromNFMM = 3,
//    ReturnFromFMD = 4
//}
Object.defineProperty(exports, "__esModule", { value: true });
exports.RejectedStatuses = void 0;
var RejectedStatuses = /** @class */ (function () {
    function RejectedStatuses() {
    }
    RejectedStatuses.rejectedStatus = [
        { id: 1, label: "Return From AFMM" },
        { id: 2, label: "Return From RFMM" },
        { id: 3, label: "Return From NFMM" },
        { id: 4, label: "Return From FMD" }
    ];
    return RejectedStatuses;
}());
exports.RejectedStatuses = RejectedStatuses;
//# sourceMappingURL=rejectedStatus.js.map