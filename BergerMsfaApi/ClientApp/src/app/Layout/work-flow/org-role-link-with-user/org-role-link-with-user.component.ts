import { Component, OnInit } from '@angular/core';
import { OrganizationRoleService } from "../../../Shared/Services/Workflow/organizationrole.service"
import { OrganizationRole } from "../../../Shared/Entity/Organizations/orgrole";
import { ActivatedRoute, Router } from '@angular/router';
import { UserRoleMapping } from 'src/app/Shared/Entity/Users/userrolemapping';
import { UserInfo } from 'src/app/Shared/Entity/Users/userInfo';
import { UserService } from 'src/app/Shared/Services/Users/user.service';

@Component({
  selector: 'app-org-role-link-with-user',
  templateUrl: './org-role-link-with-user.component.html',
  styleUrls: ['./org-role-link-with-user.component.css']
})
export class OrgRoleLinkWithUserComponent implements OnInit {

    constructor(private userService: UserService, private roleService: OrganizationRoleService, private router: Router) {

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

    public roleList: OrganizationRole[] = [];
    private fnGetRoleList() {
        this.roleService.getOrganizationRoleList().subscribe((res) => {
            this.roleList = res.data.model || [];
        });
    }

    public fnSaveRoleMapping(model: UserRoleMapping) {
        console.log("RoleMapping model: ", model);
        this.roleService.postOrganizationRoleLinkWithUser(model).subscribe((res) => {
            console.log("RoleMapping resp: ", res);
            this.fnRouteRoleList();
        });
    }

    public fnRouteRoleList() {
        this.router.navigate(['/work-flow/organization-role-list']);
    }

}
