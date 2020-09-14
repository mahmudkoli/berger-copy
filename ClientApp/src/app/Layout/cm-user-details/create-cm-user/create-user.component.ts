import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/Shared/Entity/Users/user';
import { Router } from '@angular/router';
import { UserService } from 'src/app/Shared/Services/Users';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';



@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
    styleUrls: ['./create-user.component.css'],
    providers: [UserService]
})
export class CreateCmUserComponent implements OnInit {
    public userModel: User = new User();
    // activeStatus: boolean;
    Users: any;
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    passwordTextType = false;
    confirmPasswordTextType = false;

    constructor(private userService: UserService, private router: Router) { }

    ngOnInit() {
        this.getAllUserInfoId();
        // this.userModel.isActive = "1";
  }

    submitUserForm(model) {

        // if (this.userModel.isActive == "0") {
        //     this.activeStatus == false;

        // }
        // else
        // {
        //     this.activeStatus = true;
        // }

        let nameObj = {
            name: this.userModel.name,
            code: this.userModel.code,
            email: this.userModel.email,
            PhoneNumber: this.userModel.phoneNumber,
            address: this.userModel.address,
            familyContactNo: this.userModel.familyContactNo,
            // isActive: this.activeStatus,
            status: this.userModel.status,
            passWord: this.userModel.passWord,
            fmUserId: this.userModel.fmUserId

        }

        var result = this.userService.createUser(nameObj).subscribe(res => {
            this.router.navigate(['/users/users-list/']);
        });
        

    }
    getAllUserInfoId() {
        this.userService.getAllUserInfo().subscribe((res:any) => {
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
