import { Component, OnInit } from '@angular/core';
import { Role } from 'src/app/Shared/Entity/Users/role';
import { ActivatedRoute, Router } from '@angular/router';
import { UserRoleMapping } from 'src/app/Shared/Entity/Users/userrolemapping';
import { UserInfo } from 'src/app/Shared/Entity/Users/userInfo';
import { UserService } from 'src/app/Shared/Services/Users';
import { RoleService } from 'src/app/Shared/Services/Users/role.service';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';

@Component({
  selector: 'app-role-link-with-user',
  templateUrl: './role-link-with-user.component.html',
  styleUrls: ['./role-link-with-user.component.css']
})
export class RoleLinkWithUserComponent implements OnInit {

    constructor(private userService: UserService, 
        private roleService: RoleService,
        private alertService: AlertService,
         private router: Router) {

    }

    ngOnInit() {
        this.fnGetUserList();
        this.fnGetRoleList();
    }

    public roleMappingModel: UserRoleMapping = new UserRoleMapping();
    
    public userInfoList: UserInfo[] = [];
    private fnGetUserList() {
        this.userService.getAllUserInfo().subscribe((res: any) => {
            console.log("user info list", res.data);
            this.userInfoList = res.data || [];
        });
    }
    //Paged Call
    /*
    private fnGetUserList() {
        this.userService.getAllUserInfo().subscribe((res) => {
            console.log("user info list", res.data.model);
            this.userInfoList = res.data.model || [];
        });
    }
    */


    public roleList: Role[] = [];
    private fnGetRoleList() {
        this.roleService.getRoleList().subscribe((res) => {
            this.roleList = res.data.model || [];
        });
    }

    // public fnSaveRoleMapping(model: UserRoleMapping) {
    //     console.log("RoleMapping model: ", model);
    //     this.userService.postRoleLinkWithUser(model).subscribe((res) => {
    //         console.log("RoleMapping resp: ", res);
    //         this.fnRouteRoleList();
    //     });
    // }


    public fnSaveRoleMapping(model: UserRoleMapping) {
        this.userService.postRoleLinkWithUser(model).subscribe(res => {
           
            this.router.navigate(['/role/role-list']).then(() => {
                this.alertService.tosterSuccess("Role link with user has been saved successfully.");
            });
        },
            (error) => {
               
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private displayError(errorDetails: any) {
        // this.alertService.fnLoading(false);
       
        let errList = errorDetails.error.errors;
        if (errList.length) {
            console.log("error", errList, errList[0].errorList[0]);
            this.alertService.tosterDanger(errList[0].errorList[0]);
        } else {
            this.alertService.tosterDanger(errorDetails.error.msg);
        }
    }





    public fnRouteRoleList() {
        this.router.navigate(['/role/role-list']);
    }

    

}
