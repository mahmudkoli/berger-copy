import { Component, OnInit } from '@angular/core';
import { Role } from 'src/app/Shared/Entity/Users/role';
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from 'src/app/Shared/Enums/status';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { RoleService } from 'src/app/Shared/Services/Users/role.service';
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { PermissionGroup, ActivityPermissionService } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.css']
})
export class RoleListComponent implements OnInit {

    //heading = 'Product List';
    //subheading = 'Get all products information';
    //icon = 'pe-7s-drawer icon-gradient bg-happy-itmeo';
    permissionGroup: PermissionGroup = new PermissionGroup();

    enumStatusTypes: MapObject[] = StatusTypes.statusType;

    constructor(private roleService: RoleService,
         private alertService: AlertService,
         private activityPermissionService: ActivityPermissionService,
         private activatedRoute: ActivatedRoute,
          private router: Router) {
            this.initPermissionGroup();

    }

    ngOnInit() {
        this.fnGetRoleList();
    }

    private initPermissionGroup() {
        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
    }

    public roleList: Role[] = [];
    private fnGetRoleList() {
        this.alertService.fnLoading(true);
        this.roleService.getRoleList().subscribe(
            (res) => {
                let rolesData = res.data.model || [];
                rolesData.forEach(obj => {
                    obj.statusText = this.enumStatusTypes.filter(k => k.id == obj.status)[0].label;
                });
                this.roleList = rolesData;
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }

    public fnRouteAddRole() {
        this.router.navigate(['/role/role-add']);
    }
    public fnRouteLinkRoleWithUser() {
        this.router.navigate(['/role/role-link-with-user']);
    }

    private edit(id: number) {
        this.router.navigate(['/role/role-add/' + id]);
    }

    private delete(id: number) {
        // console.log("Id:", id);
        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.roleService.deleteRole(id).subscribe(
                (res: any) => {
                    console.log('res from del func', res);
                    this.alertService.tosterSuccess("Role has been deleted successfully.");
                    this.fnGetRoleList();
                },
                (error) => {
                    console.log(error);
                }
            );
        }, () => {

        });
    }

    public fnPtableCellClick(event) {
        console.log("cell click: ");
    }

    public ptableSettings = {
        tableID: "Roles-table",
        tableClass: "table table-border ",
        tableName: '',
        tableRowIDInternalName: "Id",
        tableColDef: [
            { headerName: 'Role Name ', width: '70%', internalName: 'name', sort: true, type: "" },
            { headerName: 'Status', width: '30%', internalName: 'statusText', sort: true, type: "" }
        ],
        enabledSearch: true,
        enabledSerialNo: true,
        pageSize: 10,
        enabledPagination: true,
        // enabledAutoScrolled:true,
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
        // enabledRecordCreateBtn: true,
        // enabledTotal:true,
        //tableHeaderFooterVisibility: false
    };

    public fnCustomTrigger(event) {
        console.log("custom  click: ", event);

        if (event.action == "new-record") {
            this.fnRouteAddRole();
        }
        else if (event.action == "edit-item") {
            this.edit(event.record.id);
        }
        else if (event.action == "delete-item") {
            this.delete(event.record.id);
        }
    }
}


