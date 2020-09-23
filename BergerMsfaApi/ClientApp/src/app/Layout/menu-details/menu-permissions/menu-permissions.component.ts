import { Component, OnInit } from '@angular/core';
import { Menu } from 'src/app/Shared/Entity/Menu/menu.model';
import { MenuService } from 'src/app/Shared/Services/Menu-Details/menu.service';
import { RoleService } from 'src/app/Shared/Services/Users/role.service';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { MenuPermission } from 'src/app/Shared/Entity/Menu/menuPermission.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu-permissions',
  templateUrl: './menu-permissions.component.html',
  styleUrls: ['./menu-permissions.component.css']
})
export class MenuPermissionsComponent implements OnInit {

  selectedRole = null;
  menuList: Menu[] = [];
  roleList: any[] = [];
  selectedMenuIds: any[] = [];
  selectedMenuPermissions: any[] = [];
  updatedMenuPermissions: any[] = [];

  constructor(
    private router: Router,
    private menuService: MenuService,
    private roleService: RoleService,
    private alertService: AlertService
  ) { }

  ngOnInit() {
    //this.getMenus();
    this.getRoles();
  }

  getMenus() {
    this.selectedMenuPermissions = [];
    this.updatedMenuPermissions = [];

    this.menuService.getAll().subscribe(
      (res: any) => {
        console.log(res.data);
        this.menuList = res.data;
        this.setCheckedToMenuNestedChildren(this.menuList);
      },
      (err: any) => {
        console.log(err);
      }
    );
  }

  setCheckedToMenuNestedChildren(arr) {
    //debugger;  
    // console.log("arr: ", arr);
    for (var i in arr) {
      let hasPermission = arr[i].menuPermissions.length != 0 ? arr[i].menuPermissions.find(p => p.roleId == this.selectedRole) : null;
      let menuChecked = hasPermission != null ? "checked" : (arr[i].isParent ? "checked" : null);
      arr[i].menuChecked = menuChecked;
      // console.log("childArray: ", i, arr[i]);

      if (!arr[i].isParent && hasPermission != null) {
        let menuPermission = new MenuPermission();
        menuPermission.id = hasPermission.id;
        menuPermission.menuId = hasPermission.menuId,
          menuPermission.roleId = hasPermission.roleId;

        this.selectedMenuPermissions.push(menuPermission);
        this.updatedMenuPermissions.push(menuPermission);
      }

      if (arr[i].children.length) {
        this.setCheckedToMenuNestedChildren(arr[i].children);
      }
    }
  }

  getRoles() {
    this.roleService.getRoleList().subscribe(
      (res: any) => {
        console.log("Roles: ", res.data);
        this.roleList = res.data.model;
        this.selectedRole = this.roleList.length ? this.roleList[0].id : 0;
        this.getMenus();
      },
      (err: any) => {
        console.log(err);
      }
    );
  }

  showMenuPermission(event) {
    console.log("event: ", event);
    this.getMenus();
  }

  selectMenu(event, menuObj) {
    // console.log("menuObj: ", menuObj, this.selectedMenuPermissions);
    if (menuObj.isParent) {
      return;
    }

    if (event.target.checked == false) {
      // remove permissions
      this.updatedMenuPermissions = this.updatedMenuPermissions.filter(obj => obj.menuId != menuObj.id);
      // console.log("Menu permissions array after delete: ", this.updatedMenuPermissions);      
    }
    else {
      let hasPermission = this.selectedMenuPermissions.filter(obj => obj.menuId == menuObj.id);
      if (hasPermission.length == 0) {
        let menuPermission = new MenuPermission();
        menuPermission.id = 0;
        menuPermission.menuId = menuObj.id,
          menuPermission.roleId = this.selectedRole;
        this.updatedMenuPermissions.push(menuPermission);
      }
      else {
        this.updatedMenuPermissions.push(hasPermission[0]);
      }
      // console.log("Menu permissions array after add: ", this.updatedMenuPermissions);
    }
  }

  assignRoleToMenu() {
    this.menuService.assignRoleToMenu(this.updatedMenuPermissions, this.selectedRole).subscribe(
      (res: any) => {
        this.alertService.tosterSuccess("Assigned role successfully.");
      },
      (err: any) => {
        console.log(err);
        this.displayError(err);
      }, () => this.alertService.fnLoading(false)
    );
  }

  displayError(errorDetails: any) {
    // this.alertService.fnLoading(false);
    console.log("error", errorDetails);
    let errList = errorDetails.error.errors;
    if (errList.length) {
      console.log("error", errList, errList[0].errorList[0]);
      this.alertService.tosterDanger(errList[0].errorList[0]);
    } else {
      this.alertService.tosterDanger(errorDetails.error.msg);
    }
  }

  backToMenuList() {
    this.router.navigate(['/menu/menu-list']);
  }
}
