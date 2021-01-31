import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { AuthService } from 'src/app/Shared/Services/Users';
import { Login } from 'src/app/Shared/Entity/Users/auth';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-login-boxed',
  templateUrl: './login-boxed.component.html',
  styles: []
})
export class LoginBoxedComponent implements OnInit, OnDestroy {
	// Public properties
	// login: LoginUser;
    loginForm: FormGroup;
    errorMessage: string;
    passwordTextType: boolean = false;
    isLoading: boolean = false;
// 	hasFormErrors: boolean = false;
// 	errors: any[];
// 	// Private properties
	private subscriptions: Subscription[] = [];


	constructor(private activatedRoute: ActivatedRoute,
		private router: Router,
		private loginFB: FormBuilder,
		private alertService: AlertService,
		private authService: AuthService) { }

	ngOnInit() {
        this.initLogin();
	}

 	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

	initLogin() {
		this.createForm();
	}

// 	resetErrors() {
// 		this.hasFormErrors = false;
// 		this.errors = [];
// 	}

	createForm() {
		this.loginForm = this.loginFB.group({
			userName: ['', [Validators.required]],
			password: ['', Validators.required],
			rememberMe: [false],
		});
	} 
	
	get ufControls() { return this.loginForm.controls; }

// 	resetAll() {
// 		this.user = Object.assign({}, this.oldUser);
// 		this.createForm();
// 		this.resetErrors();
// 		this.userForm.markAsPristine();
// 		this.userForm.markAsUntouched();
// 		this.userForm.updateValueAndValidity();
// 	}

	onSubmit() {
		// this.resetErrors();
		const controls = this.loginForm.controls;
		
		if (this.loginForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);

			// this.hasFormErrors = true;
			// this.selectedTab = 0;
			return;
		}

		const login = this.prepareLogin();
        this.login(login);
	}

	prepareLogin(): Login {
		const controls = this.loginForm.controls;
		const _login = new Login();
		_login.clear();
		_login.userName = controls['userName'].value;
		_login.password = controls['password'].value;
		_login.rememberMe = controls['rememberMe'].value;
		return _login;
	}

	login(_login: Login) {
        this.alertService.fnLoading(true);
        this.isLoading = true;
		const createSubscription = this.authService.login(_login)
			.pipe(finalize(() => {this.alertService.fnLoading(false); this.isLoading = false;}))
			.subscribe(res => {
				// this.alertService.tosterSuccess(`New user successfully has been added.`);
				this.goHome();
			},
			error => {
				this.throwError(error);
			});
		this.subscriptions.push(createSubscription);
	}
	
	goHome() {
		this.router.navigate([`/`], { relativeTo: this.activatedRoute });
	}
    
    togglePasswordTextType(){
        this.passwordTextType = !this.passwordTextType;
    }

// 	onAlertClose($event) {
// 		this.resetErrors();
// 	}

    private throwError(errorDetails: any) {
        // this.alertService.fnLoading(false);
        console.log("error", errorDetails);
        let errList = errorDetails.error.errors;
        if (errList.length) {
            console.log("error", errList, errList[0].errorList[0]);
            // this.alertService.tosterDanger(errList[0].errorList[0]);
            this.errorMessage = errList[0].errorList[0];
        } else {
            // this.alertService.tosterDanger(errorDetails.error.message);
            this.errorMessage = errorDetails.error.message;
        }
    }
}
