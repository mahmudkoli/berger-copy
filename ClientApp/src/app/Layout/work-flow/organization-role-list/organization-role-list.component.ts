import { Component, OnInit } from '@angular/core';
import { OrganizationRoleService } from "src/app/Shared/Services/Workflow/organizationrole.service";
import { OrganizationRole } from "src/app/Shared/Entity/Organizations/orgrole";
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from 'src/app/Shared/Enums/status';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { PermissionGroup, ActivityPermissionService } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';

@Component({
    selector: 'app-organization-role-list',
    templateUrl: './organization-role-list.component.html',
    styleUrls: ['./organization-role-list.component.css']
})
export class OrganizationRoleListComponent implements OnInit {

    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    tosterMsgError: string = "Something went wrong!";
    orgUserList: OrganizationRole[] = [];

    permissionGroup: PermissionGroup = new PermissionGroup();
    constructor(private roleService: OrganizationRoleService,
        private activityPermissionService: ActivityPermissionService,
        private activatedRoute: ActivatedRoute,
        private alertService: AlertService,
        private router: Router) {
            this.initPermissionGroup();

    }
    //Treeview
    ngOnInit() {
        //this.fnGetOrganizationRoleList();
        this.getOrgUser();
    }


    private initPermissionGroup() {
        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
    }

    // Tree view

    getOrgUser() {
        this.alertService.fnLoading(true);
        this.roleService.getOrgUserList().subscribe(
            (res: any) => {
                this.orgUserList = res.data;
                console.log(this.orgUserList);
                this.orgUserList.forEach(s => s.checked = true);


            },
            (error) => {
                console.log(error);
                this.alertService.fnLoading(false);
                this.alertService.tosterDanger(this.tosterMsgError);
            },
            () => this.alertService.fnLoading(false));
    }



    public fnRouteAddRole() {
        this.router.navigate(['/work-flow/organization-role-add']);
    }

    public fnRouteLinkRoleWithUser() {
        this.router.navigate(['/work-flow/organization-role-link-with-user']);
    }

    public edit(id: number) {
        this.router.navigate(['/work-flow/organization-role-edit/' + id]);
    }

}
