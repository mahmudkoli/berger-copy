import { Component, OnInit } from '@angular/core';
import { User } from '../../../Shared/Entity/Users/user';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
    styleUrls: ['./edit-user.component.css'],
    providers: [UserService]
})
export class EditCmUserComponent implements OnInit {
    user: User = new User();
    Users: number[] = [];
    // activeStatus: boolean;
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    passwordTextType = false;
    confirmPasswordTextType = false;
    
    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private alertService: AlertService,
        private userService: UserService
    ) { }

    ngOnInit() {
        this.getAllUserInfoId();
        if ((this.activatedRoute.snapshot.params).hasOwnProperty('id')) {
            this.getUserById(this.activatedRoute.snapshot.params.id);
        }
    }





    getUserById(id: number) {
        console.log(id);
        this.alertService.fnLoading(true);
        this.userService.getUserById(id).subscribe(
            (res: any) => {
                this.user = res.data || new User();

                if (Object.keys(res.data).length == 0) {
                    this.showError("No such user!Create a new user");
                    this.router.navigate([`/users/users-list/`]);
                }
                else {
                    console.log("User ", this.user);
                    //this.router.navigate([`/users/users-list/`]);
                }
            },
            (error) => {
                console.log(error);
                this.showError(error.message);
                this.router.navigate([`/users/users-list/`]);
            },
            () => {
                this.alertService.fnLoading(false);
            }
        );
    }


    showError(msg: string) {
        this.alertService.fnLoading(false);
        this.alertService.tosterDanger(msg);

    }

    submitUserForm(model) {
        // if (this.user.isActive == "0") {
        //     this.activeStatus == false;

        // }
        // else {
        //     this.activeStatus = true;
        // }

        let userObj = {
            id:this.user.id,
            name: this.user.name,
            code: this.user.code,
            email: this.user.email,
            phoneNumber: this.user.phoneNumber,
            password: this.user.passWord,
            address: this.user.address,
            fmUserId: this.user.fmUserId,
            // isActive: this.activeStatus,
            status: this.user.status,
            familyContactNo: this.user.familyContactNo
        }

        var result = this.userService.updateUser(userObj).subscribe(res => {
            this.router.navigate(['/users/users-list/']);
        });


    }

    getAllUserInfoId() {
        this.userService.getAllUserInfo().subscribe((res: any) => {
            console.log(res);
            this.Users = res.data;
        });


    }

    public backToUserList() {
        this.router.navigate(['/users/users-list']);
    }
    
    togglePasswordTextType(){
        this.passwordTextType = !this.passwordTextType;
    }
    
    toggleConfirmPasswordTextType(){
        this.confirmPasswordTextType = !this.confirmPasswordTextType;
    }

}
