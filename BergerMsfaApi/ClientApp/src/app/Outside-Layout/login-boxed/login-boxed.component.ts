import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgForm } from '@angular/forms';
import { LoginService } from 'src/app/Shared/Services/Users/login.service';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-login-boxed',
  templateUrl: './login-boxed.component.html',
  styles: []
})
export class LoginBoxedComponent implements OnInit {
    invalidLogin: boolean;
     MobileNumber:string;
     Password:string;

   constructor(private router: Router,private loginService:LoginService) {

    }

  ngOnInit() {
    }


    public login = (form: NgForm) => {
        let credentials = JSON.stringify(form.value);
        this.loginService.postLoginData(credentials).subscribe(response=>
            {
                let token = (<any>response).token;
                localStorage.setItem("Fm-App-Token", token);
                this.loginService.getExampleData(token).subscribe(res=>
                    {
                        console.log(res);
                    })
                //this.router.navigate(["/"]);

            });
        // this.http.post("http://localhost:40875/Auth/Login", credentials, {
        //     headers: new HttpHeaders({
        //         "Content-Type": "application/json"
        //     })
        // }).subscribe(response => {
        //     let token = (<any>response).token;
        //     localStorage.setItem("jwt", token);
        //     this.http.post("http://localhost:40785/Auth/Getuser", token, {
        //         headers: new HttpHeaders({
        //             "Content-Type": "application/json"
        //         })
        //     }).subscribe(response => {
        //         let userId = (<any>response);
        //     })

        //     this.invalidLogin = false;
        //     this.router.navigate(["/"]);
        // }, err => {
        //     this.invalidLogin = true;
        // });
    }


    onLogin() {
        let loginModel = '{ "MobileNumber" : "' + this.MobileNumber + '" , "Password": "' + this.Password +'" }';
        let credentials = loginModel;
        this.loginService.postLoginData(credentials).subscribe(response=>
        {
            debugger;
            let tokens =<any>response;        
            let token = tokens.data.token;
            localStorage.setItem("bergermsfa", token);            
            // this.loginService.getExampleData(token).subscribe(res =>
            // {
            //    console.log(res);
            // })
            this.router.navigate(["/"]);
  
        });
      
      }



}
