import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/Shared/Services/Users';
import { User } from '../../../Shared/Entity/Users/user';



@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
    styleUrls: ['./user-info.component.css'],
    providers: [UserService]
})
export class UserInfoComponent implements OnInit {
  public userModel: User = new User();

    constructor(private userService: UserService) { }

  ngOnInit() {
    }
   

    submitUserForm(model: User) {
        console.log( model);
        
        this.userService.createUser(model);
    }


}
