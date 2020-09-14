"use strict";
//export enum ApprovalStatus {
//    WaitingForRFMM = 1,
//    WaitingForNFMM = 2,
//    WaitingForFMD = 3,
//    ApprovedPlan = 4,
//    ProgramCompleted = 5
//}
Object.defineProperty(exports, "__esModule", { value: true });
var ApprovalStatuses = /** @class */ (function () {
    function ApprovalStatuses() {
    }
    ApprovalStatuses.approvalStatus = [
        { id: 1, label: "Waiting For RFMM" },
        { id: 2, label: "Waiting For NFMM" },
        { id: 3, label: "Waiting For FMD" },
        { id: 4, label: "Approved Plan" },
        { id: 5, label: "Program Completed" }
    ];
    return ApprovalStatuses;
}());
exports.ApprovalStatuses = ApprovalStatuses;
//# sourceMappingURL=approvalStatus.js.map