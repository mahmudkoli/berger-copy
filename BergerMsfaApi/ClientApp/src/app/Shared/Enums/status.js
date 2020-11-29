"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.PlanStatus = exports.Status = void 0;
var Status;
(function (Status) {
    Status[Status["Inactive"] = 0] = "Inactive";
    Status[Status["Active"] = 1] = "Active";
    Status[Status["Pending"] = 2] = "Pending";
    Status[Status["Revert"] = 3] = "Revert";
    Status[Status["Rejected"] = 4] = "Rejected";
    Status[Status["Completed"] = 5] = "Completed";
    Status[Status["NotCompleted"] = 6] = "NotCompleted";
    Status[Status["InCompleted"] = 7] = "InCompleted";
    Status[Status["InPlace"] = 8] = "InPlace";
    Status[Status["NotInPlace"] = 9] = "NotInPlace";
})(Status = exports.Status || (exports.Status = {}));
var PlanStatus;
(function (PlanStatus) {
    PlanStatus[PlanStatus["Pending"] = 0] = "Pending";
    PlanStatus[PlanStatus["Approved"] = 1] = "Approved";
    PlanStatus[PlanStatus["Edited"] = 2] = "Edited";
})(PlanStatus = exports.PlanStatus || (exports.PlanStatus = {}));
//# sourceMappingURL=status.js.map