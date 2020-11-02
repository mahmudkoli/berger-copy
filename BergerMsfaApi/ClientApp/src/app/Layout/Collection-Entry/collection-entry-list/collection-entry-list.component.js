"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.CollectionEntryListComponent = exports.CustomerTypeEnum = void 0;
var core_1 = require("@angular/core");
var activity_permission_service_1 = require("../../../Shared/Services/Activity-Permission/activity-permission.service");
var CustomerTypeEnum;
(function (CustomerTypeEnum) {
    CustomerTypeEnum[CustomerTypeEnum["Dealer"] = 1] = "Dealer";
    CustomerTypeEnum[CustomerTypeEnum["SubDealer"] = 2] = "SubDealer";
    CustomerTypeEnum[CustomerTypeEnum["Project"] = 3] = "Project";
    CustomerTypeEnum[CustomerTypeEnum["Customer"] = 4] = "Customer";
})(CustomerTypeEnum = exports.CustomerTypeEnum || (exports.CustomerTypeEnum = {}));
var CollectionEntryListComponent = /** @class */ (function () {
    function CollectionEntryListComponent(collectionEntryService, alertService, activityPermissionService, activatedRoute, router) {
        this.collectionEntryService = collectionEntryService;
        this.alertService = alertService;
        this.activityPermissionService = activityPermissionService;
        this.activatedRoute = activatedRoute;
        this.router = router;
        this.permissionGroup = new activity_permission_service_1.PermissionGroup();
        this.customerTypeList = [
            { key: 1, type: 'Dealer' },
            { key: 2, type: 'Sub Dealer' },
            { key: 3, type: 'Project' },
            { key: 4, type: 'Customer' },
        ];
        this.ptableSettings = null;
        this.gridDataSource = [];
        this._initPermissionGroup();
    }
    CollectionEntryListComponent.prototype.ngOnInit = function () {
        this.selectedType = CustomerTypeEnum.Dealer;
        this.onChange(this.selectedType);
    };
    CollectionEntryListComponent.prototype._initPermissionGroup = function () {
        //this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        //console.log(this.permissionGroup);
        //this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
        //this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        //this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
    };
    CollectionEntryListComponent.prototype.onChange = function (selected) {
        this.customerWiseTableConfig(selected);
    };
    CollectionEntryListComponent.prototype.getCustomerDetails = function (type) {
        var _this = this;
        this.collectionEntryService.getCollectionByType(type).subscribe(function (res) {
            _this.gridDataSource = res.data;
        });
    };
    CollectionEntryListComponent.prototype.customerWiseTableConfig = function (selected) {
        var tableColDef;
        this.gridDataSource = [];
        var tableName = "";
        if (CustomerTypeEnum.Dealer == selected) {
            tableName = "Dealer Collection";
            tableColDef = [
                { headerName: 'Code  ', width: '10%', internalName: 'code', sort: true, type: "" },
                { headerName: 'Dealer ', width: '10%', internalName: 'name', sort: true, type: "" },
                { headerName: 'Mobile Number ', width: '10%', internalName: 'mobileNumber', sort: true, type: "" },
                { headerName: 'Payment Method', width: '5%', internalName: 'paymentMethodName', sort: true, type: "" },
                { headerName: 'Area', width: '15%', internalName: 'creditControllAreaName', sort: true, type: "" },
                { headerName: 'Bank Name', width: '15%', internalName: 'bankName', sort: true, type: "" },
                { headerName: 'Number(Account)', width: '15%', internalName: 'number', sort: true, type: "" },
                { headerName: 'Amount', width: '10%', internalName: 'number', sort: true, type: "" },
                { headerName: 'Number(Manual)', width: '10%', internalName: 'manualNumber', sort: true, type: "" },
            ];
        }
        else if (CustomerTypeEnum.SubDealer == selected) {
            tableName = "Sub Dealer Collection";
            tableColDef = [
                { headerName: 'Code  ', width: '10%', internalName: 'code', sort: true, type: "" },
                { headerName: 'Sub Dealer ', width: '10%', internalName: 'name', sort: true, type: "" },
                { headerName: 'Mobile Number ', width: '10%', internalName: 'mobileNumber', sort: true, type: "" },
                { headerName: 'Payment Method', width: '5%', internalName: 'paymentMethodName', sort: true, type: "" },
                { headerName: 'Area', width: '15%', internalName: 'creditControllAreaName', sort: true, type: "" },
                { headerName: 'Bank Name', width: '15%', internalName: 'bankName', sort: true, type: "" },
                { headerName: 'Number(Account)', width: '15%', internalName: 'number', sort: true, type: "" },
                { headerName: 'Amount', width: '10%', internalName: 'number', sort: true, type: "" },
                { headerName: 'Number(Manual)', width: '10%', internalName: 'manualNumber', sort: true, type: "" },
            ];
        }
        else if (CustomerTypeEnum.Project == selected) {
            tableName = "Project Collection";
            tableColDef = [
                { headerName: 'Code  ', width: '10%', internalName: 'code', sort: true, type: "" },
                { headerName: 'Project ', width: '10%', internalName: 'name', sort: true, type: "" },
                { headerName: 'Mobile Number ', width: '10%', internalName: 'mobileNumber', sort: true, type: "" },
                { headerName: 'Payment Method', width: '5%', internalName: 'paymentMethodName', sort: true, type: "" },
                { headerName: 'Area', width: '15%', internalName: 'creditControllAreaName', sort: true, type: "" },
                { headerName: 'Bank Name', width: '15%', internalName: 'bankName', sort: true, type: "" },
                { headerName: 'Number(Account)', width: '15%', internalName: 'number', sort: true, type: "" },
                { headerName: 'Amount', width: '10%', internalName: 'number', sort: true, type: "" },
                { headerName: 'Number(Manual)', width: '10%', internalName: 'manualNumber', sort: true, type: "" },
            ];
        }
        else if (CustomerTypeEnum.Customer == selected) {
            tableName = "Cutomer Collection";
            tableColDef = [
                { headerName: 'Code  ', width: '10%', internalName: 'code', sort: true, type: "" },
                { headerName: 'Customer ', width: '10%', internalName: 'name', sort: true, type: "" },
                { headerName: 'Mobile Number ', width: '10%', internalName: 'mobileNumber', sort: true, type: "" },
                { headerName: 'Payment Method', width: '5%', internalName: 'paymentMethodName', sort: true, type: "" },
                { headerName: 'Area', width: '15%', internalName: 'creditControllAreaName', sort: true, type: "" },
                { headerName: 'Bank Name', width: '15%', internalName: 'bankName', sort: true, type: "" },
                { headerName: 'Number(Account)', width: '15%', internalName: 'number', sort: true, type: "" },
                { headerName: 'Amount', width: '10%', internalName: 'number', sort: true, type: "" },
                { headerName: 'Number(Manual)', width: '10%', internalName: 'manualNumber', sort: true, type: "" },
            ];
        }
        this.getCustomerDetails(CustomerTypeEnum[selected]);
        this.configureTable(tableName, tableColDef);
    };
    CollectionEntryListComponent.prototype.configureTable = function (tableName, tableCol) {
        this.ptableSettings = {
            tableID: "Setup-table",
            tableClass: "table table-border ",
            tableName: tableName,
            tableRowIDInternalName: "Id",
            tableColDef: tableCol,
            enabledSearch: true,
            enabledSerialNo: true,
            pageSize: 10,
            enabledPagination: true,
            //enabledAutoScrolled:true,
            enabledDeleteBtn: true,
            enabledEditBtn: true,
            // enabledCellClick: true,
            enabledColumnFilter: true,
            // enabledDataLength:true,
            // enabledColumnResize:true,
            // enabledReflow:true,
            // enabledPdfDownload:true,
            // enabledExcelDownload:true,
            // enabledPrint:true,
            // enabledColumnSetting:true,
            enabledRecordCreateBtn: true,
        };
    };
    CollectionEntryListComponent = __decorate([
        core_1.Component({
            selector: 'app-collection-entry-list',
            templateUrl: './collection-entry-list.component.html',
            styleUrls: ['./collection-entry-list.component.css']
        })
    ], CollectionEntryListComponent);
    return CollectionEntryListComponent;
}());
exports.CollectionEntryListComponent = CollectionEntryListComponent;
//# sourceMappingURL=collection-entry-list.component.js.map