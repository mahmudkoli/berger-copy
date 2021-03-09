import { Component, OnInit } from '@angular/core';
import { RoleService } from '../../../Shared/Services/Users/role.service';
import { Role } from '../../../Shared/Entity/Users';
import { MenuActivityPermission } from '../../../Shared/Entity/Menu/menu-activity-permission.model';
import { MenuActivity } from '../../../Shared/Entity/Menu/menu-activity';
import { MenuActivityService } from '../../../Shared/Services/Menu-Details/menu-activity.service';

import { MenuActivityPermissionService } from '../../../Shared/Services/Menu-Details/menu-activity-permission.service';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { Router } from '@angular/router';
import { elementAt } from 'rxjs/operators';
import { MenuActivityPermissionVm } from 'src/app/Shared/Entity/Menu/menu-activity-permission-vm.model';

@Component({
  selector: 'app-menu-activity-permissions',
  templateUrl: './menu-activity-permissions.component.html',
  styleUrls: ['./menu-activity-permissions.component.css']
})
export class MenuActivityPermissionsComponent implements OnInit {
    roleList: Role[] = []; 
    menuActivityList: MenuActivity[] = [];
    menuActivityPermissionVMlist: MenuActivityPermissionVm[] = [];
    menuActivityPermissionModel: MenuActivityPermission[] = [];
    roleId : number;
    selectAllCanView : boolean;
    selectAllCanInsert : boolean;
    selectAllCanUpdate : boolean;
    selectAllCanDelete : boolean;

    constructor(private roleService: RoleService,
        private alertService: AlertService,
        private router: Router,
        private menuActivityService: MenuActivityService,
        private menuActivityPermissionService: MenuActivityPermissionService) { 

            this.selectAllCanView = false;
            this.selectAllCanInsert = false;
            this.selectAllCanUpdate = false;
            this.selectAllCanDelete = false;
        }

    ngOnInit() {
        this.getAllRole();



    }


    getAllRole() {
        this.roleService.getRoleList().subscribe((res: any) => {
            // debugger;
            this.roleList = res.data.model||[];
            
            console.log(this.roleList);
        })
    }
   
    

    submitPermissionForm(model : MenuActivityPermissionVm[]) {
        // debugger;
        this.menuActivityPermissionService.createOrUpdateAllActivityPermission(model).subscribe((res) => {
          //  console.log("resp: ", res);

          this.alertService.titleTosterSuccess("Record has been saved successfully.");
          window.location.reload(true);
          
         
        },
            (error) => {
               // debugger;
               this.displayError(error);
            }, () => this.alertService.fnLoading(false)
  
  
        );
    }



    onRoleChange(id: number) {

        // debugger;

            this.menuActivityService.getAllMenuActivityPermissionByRoleId(id).subscribe(
              (result: any) => {
                
                //this.menuActivityList  = result.data;
                this.menuActivityPermissionVMlist = result.data;
                this.selectAllCanView = false;
                this.selectAllCanInsert = false;
                this.selectAllCanUpdate = false;
                this.selectAllCanDelete = false;

                console.log("data", this.menuActivityList);

                
              },
              (err: any) => console.log(err)
            );
          
    }


    displayError(errorDetails: any) {
        // this.alertService.fnLoading(false);
        console.log("error", errorDetails);
        let errList = errorDetails.error.errors;
        if (errList.length) {
           // console.log("error", errList, errList[0].errorList[0]);
            this.alertService.tosterDanger(errList[0].errorList[0]);
        } else {
            this.alertService.tosterDanger(errorDetails.error.msg);
        }
    }

    onSelectAllCanView(){

        //debugger;

        if(this.selectAllCanView == false)
        {
            this.menuActivityList.forEach((element) =>{

                element.menuActivityPermission.canView = false;

            });
        }
        else
        {
            this.menuActivityList.forEach((element) =>{

                element.menuActivityPermission.canView = true;

            });
        }


    }


    onSelectAllCanInsert(){

       // debugger;

        if(this.selectAllCanInsert == false)
        {
            this.menuActivityList.forEach((element) =>{

                element.menuActivityPermission.canInsert = false;

            });
        }
        else
        {
            this.menuActivityList.forEach((element) =>{

                element.menuActivityPermission.canInsert = true;

            });
        }


    }

    onSelectAllCanUpdate(){

       // debugger;

        if(this.selectAllCanUpdate == false)
        {
            this.menuActivityList.forEach((element) =>{

                element.menuActivityPermission.canUpdate = false;

            });
        }
        else
        {
            this.menuActivityList.forEach((element) =>{

                element.menuActivityPermission.canUpdate = true;

            });
        }


    }

    onSelectAllCanDelete(){

       // debugger;

        if(this.selectAllCanDelete == false)
        {
            this.menuActivityList.forEach((element) =>{

                element.menuActivityPermission.canDelete = false;

            });
        }
        else
        {
            this.menuActivityList.forEach((element) =>{

                element.menuActivityPermission.canDelete = true;

            });
        }


    }

    redirectTo(uri) {
        this.router.navigateByUrl('/', {skipLocationChange: true}).then(() =>
        this.router.navigate([uri]));
        this.alertService.titleTosterSuccess("Record has been saved successfully.");
      }


}
