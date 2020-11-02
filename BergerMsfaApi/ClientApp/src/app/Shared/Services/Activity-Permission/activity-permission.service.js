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
exports.PermissionGroup = exports.ActivityPermissionService = void 0;
var core_1 = require("@angular/core");
var ActivityPermissionService = /** @class */ (function () {
    function ActivityPermissionService(http, baseUrl, commonService) {
        this.http = http;
        this.commonService = commonService;
        this.url = baseUrl + "api/v1/menuactivitypermission";
    }
    ActivityPermissionService.prototype.getActivityPermissions = function (roleId) {
        return this.http.get(this.url + "/get-activity-permission/" + roleId);
    };
    ActivityPermissionService.prototype.setActivityPermissionToSession = function () {
        var _this = this;
        var roleId = 0;
        var userInfo = this.commonService.getUserInfoFromLocalStorage();
        if (userInfo) {
            roleId = userInfo.roleId;
        }
        this.getActivityPermissions(roleId).subscribe(function (res) {
            console.log(res.data);
            _this.commonService.setActivityPermissionToSessionStorage(res.data);
        }, function (err) {
            console.log(err);
        });
    };
    ActivityPermissionService.prototype.getActivityPermissionFromSession = function () {
        if (!this.commonService.getActivityPermissionToSessionStorage()) {
            this.setActivityPermissionToSession();
        }
        var sessionStorageActPer = this.commonService.getActivityPermissionToSessionStorage();
        var activityPermission = sessionStorageActPer ? sessionStorageActPer : [];
        return activityPermission;
    };
    ActivityPermissionService.prototype.hasPermission = function (permissionType, permissionGroup) {
        var activityPermission = this.getActivityPermissionFromSession();
        var hasPermission = false;
        console.log(activityPermission);
        if (activityPermission && activityPermission.length > 0) {
            var filterActPer = activityPermission.filter(function (per) {
                var actPer = per.url.split('/').filter(function (e) { return e; });
                var perCode = permissionGroup.split('/').filter(function (e) { return e; });
                if (actPer.length != perCode.length) {
                    return false;
                }
                for (var i = 0; i < perCode.length; i++) {
                    if (perCode[i].toLowerCase() != actPer[i].toLowerCase()) {
                        return false;
                    }
                }
                return true;
            });
            console.log(filterActPer);
            if (filterActPer && filterActPer.length > 0) {
                if ((permissionType == "view" && filterActPer[0].canView) ||
                    (permissionType == "create" && filterActPer[0].canInsert) ||
                    (permissionType == "update" && filterActPer[0].canUpdate) ||
                    (permissionType == "delete" && filterActPer[0].canDelete)) {
                    hasPermission = true;
                }
            }
        }
        return hasPermission;
    };
    ActivityPermissionService.prototype.getPermission = function (permissionGroup) {
        var perGroup = { canView: false, canCreate: false, canUpdate: false, canDelete: false };
        var activityPermission = this.getActivityPermissionFromSession();
        console.log(activityPermission);
        if (activityPermission && activityPermission.length > 0) {
            var filterActPer = activityPermission.filter(function (per) {
                var actPer = per.url.split('/').filter(function (e) { return e; });
                var perCode = permissionGroup.split('/').filter(function (e) { return e; });
                if (actPer.length != perCode.length) {
                    return false;
                }
                for (var i = 0; i < perCode.length; i++) {
                    if (perCode[i].toLowerCase() != actPer[i].toLowerCase()) {
                        return false;
                    }
                }
                return true;
            });
            console.log(filterActPer);
            if (filterActPer && filterActPer.length > 0) {
                var grp = filterActPer[0];
                perGroup.canView = grp.canView;
                perGroup.canCreate = grp.canInsert;
                perGroup.canUpdate = grp.canUpdate;
                perGroup.canDelete = grp.canDelete;
            }
        }
        return perGroup;
    };
    ActivityPermissionService = __decorate([
        core_1.Injectable({
            providedIn: 'root'
        }),
        __param(1, core_1.Inject('BASE_URL'))
    ], ActivityPermissionService);
    return ActivityPermissionService;
}());
exports.ActivityPermissionService = ActivityPermissionService;
var PermissionGroup = /** @class */ (function () {
    function PermissionGroup() {
        this.canView = false;
        this.canCreate = false;
        this.canUpdate = false;
        this.canDelete = false;
    }
    return PermissionGroup;
}());
exports.PermissionGroup = PermissionGroup;
//# sourceMappingURL=activity-permission.service.js.map