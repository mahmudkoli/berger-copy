import { Component, OnInit } from '@angular/core';
import { Role } from 'src/app/Shared/Entity/Users/role';
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from "../../../Shared/Enums/status";
import { AlertService } from "../../../Shared/Modules/alert/alert.service";
import { RoleService } from '../../../Shared/Services/Users/role.service';
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";

@Component({
    selector: 'app-role-add',
    templateUrl: './role-add.component.html',
    styleUrls: ['./role-add.component.css']
})
export class RoleAddComponent implements OnInit {

    public roleModel: Role = new Role();
    enumStatusTypes: MapObject[] = StatusTypes.statusType;


    constructor(private alertService: AlertService, private route: ActivatedRoute, private roleService: RoleService, private router: Router) { }
    ngOnInit() {
        this.createForm();
        console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let roleId = this.route.snapshot.params.id;
            this.getRole(roleId);
        }
    }

    createForm() {
    }

    public fnRouteRoleList() {
        this.router.navigate(['/role/role-list']);
    }

    private getRole(roleId) {
        this.roleService.getRole(roleId).subscribe(
            (result: any) => {
                console.log("role data", result.data);
                this.editForm(result.data);
            },
            (err: any) => console.log(err)
        );
    };

    private editForm(role: Role) {
        this.roleModel.id = role.id;
        this.roleModel.name = role.name;
        this.roleModel.status = role.status;
        console.log("role data edit after", this.roleModel);
    }

    public fnSaveRole() {
        this.roleModel.id == 0 ? this.insertRole(this.roleModel) : this.updateRole(this.roleModel);
    }

    private insertRole(model: Role) {
        this.roleService.postRole(model).subscribe(res => {
            console.log("Role res: ", res);
            this.router.navigate(['/role/role-list']).then(() => {
                this.alertService.tosterSuccess("Role has been created successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private updateRole(model: Role) {
        this.roleService.putRole(model).subscribe(res => {
            console.log("Role upd res: ", res);
            this.router.navigate(['/role/role-list']).then(() => {
                this.alertService.tosterSuccess("Role has been edited successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error.message);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private displayError(errorDetails: any) {
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

    validateInput(){

        let letters = /^[A-Za-z]+$/;

        if(!this.roleModel.name.match(letters)){
            this.roleModel.name = null;
        }
        


    }
}