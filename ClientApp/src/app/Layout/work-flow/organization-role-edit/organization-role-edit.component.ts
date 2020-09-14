import { Component, OnInit } from '@angular/core';
import { OrganizationRoleService } from "../../../Shared/Services/Workflow/organizationrole.service"
import { OrganizationRole } from "../../../Shared/Entity/Organizations/orgrole";
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from "../../../Shared/Enums/status";
import { AlertService } from "../../../Shared/Modules/alert/alert.service";
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { Enums } from 'src/app/Shared/Enums/enums';
import { Hierarchy } from 'src/app/Shared/Entity/Sales/hierarchy';
import { UserService } from 'src/app/Shared/Services/Users';
import { UserInfo } from '../../../Shared/Entity/Users/userInfo';

import { UserRoleMapping } from 'src/app/Shared/Entity/Users/userrolemapping';
@Component({
  selector: 'app-organization-role-edit',
  templateUrl: './organization-role-edit.component.html',
  styleUrls: ['./organization-role-edit.component.css']
})
export class OrganizationRoleEditComponent implements OnInit {

    public roleModel: OrganizationRole = new OrganizationRole();

  
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    enumHierarchyType: MapObject[] = Enums.hierarchyType;
    hierarchyType: Hierarchy[] = [];
    adUserList: any[] = [];


    constructor(private alertService: AlertService,
        private route: ActivatedRoute,
        private roleService: OrganizationRoleService,
        private roleUserService: OrganizationRoleService,
        private router: Router,
         private userService: UserService) { }
    ngOnInit() {
        this.createForm();
        this.getHierarchyType();
        this.getUserList();
        //console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
           
            let roleId = this.route.snapshot.params.id;
            this.getOrganizationRole(roleId);
        }
    }

    public roleMappingModel: UserRoleMapping = new UserRoleMapping();

    public userInfoList: UserInfo[] = [];
    getUserList() {
    
        this.userService.getAllUserInfo().subscribe((result: any) => {
          //  console.log("user info list", res.data);
            this.userInfoList = result.data;
        });
    }
    getHierarchyType() {
        this.userService.getAllHierarchy().subscribe((result: any) => {
          this.hierarchyType = result.data;
    
        });
      }

    createForm() {
    }

    public fnRouteOrganizationRoleList() {
        this.router.navigate(['/work-flow/organization-role-list']);
    }

    private getOrganizationRole(roleId) {
        this.roleService.getOrganizationRole(roleId).subscribe(
            (result: any) => {
                console.log("Organization role data", result.data);
                this.editForm(result.data);
            },
            (err: any) => console.log(err)
        );
    };

    private editForm(role: OrganizationRole) {
        this.roleService.getOrgnazationuserByRole(role.id).subscribe(res => {
            this.roleModel.id = role.id;
            this.roleModel.name = role.name;
            this.roleModel.status = role.status;
            this.roleModel.designationId = role.designationId;
            this.roleModel.userList = res.data;
        }
        , err => console.log(err))
        
        
    }

    public fnSaveRole() {
        //debugger;
        //this.roleMappingModel.userList = [];
       // this.roleModel.userList = this.roleMappingModel.userIds;

         this.updateRole(this.roleModel);
    }

    private insertRole(model: OrganizationRole) {
        this.roleService.postOrganizationRole(model).subscribe(res => {
               
            this.router.navigate(['/work-flow/organization-role-list']).then(() => {
                this.alertService.tosterSuccess("Organization Role has been created successfully.");
                });
            },
            (error) => {
               
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private updateRole(model: OrganizationRole) {
        this.roleService.putOrganizationRole(model).subscribe(res => {
           
            this.router.navigate(['/work-flow/organization-role-list']).then(() => {
                    this.alertService.tosterSuccess("Organization Role has been edited successfully.");
                });
            },
            (error) => {
                
                this.displayError(error.message);
            }, () => this.alertService.fnLoading(false)
        );
    }
 
    private displayError(errorDetails: any) {
        // this.alertService.fnLoading(false);
       
        let errList = errorDetails.error.errors;
        if (errList.length) {
            
            this.alertService.tosterDanger(errList[0].errorList[0]);
        } else {
            this.alertService.tosterDanger(errorDetails.error.msg);
        }
    }



}
