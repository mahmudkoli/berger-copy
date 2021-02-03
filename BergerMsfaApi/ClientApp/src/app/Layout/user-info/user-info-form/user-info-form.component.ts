import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from 'src/app/Shared/Services/Users/user.service';
import { SaveUserInfo, UserInfo } from '../../../Shared/Entity/Users/userInfo';
import { SalesPointService } from '../../../Shared/Services/Sales';
import { NodeService } from "../../../Shared/Services/Sales/node.service";
import { DynamicDropdownService } from '../../../Shared/Services/Setup/dynamic-dropdown.service';
import { forkJoin, Subscription } from 'rxjs';
import { Status } from '../../../Shared/Enums/status';
import { CommonService } from '../../../Shared/Services/Common/common.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { finalize } from 'rxjs/operators';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';

@Component({
    selector: 'app-user-info-form',
    templateUrl: './user-info-form.component.html',
    styleUrls: ['./user-info-form.component.css']
})
export class UserInfoFormComponent implements OnInit {

    public user: UserInfo;
	userForm: FormGroup;

    adUser: any;
    adError: string;

    plants: any[] = [];
    saleOffices: any[] = [];
    areaGroups: any[] = [];
    zones: any[] = [];
    roles: any[] = [];
    territories:any[]=[]

	actInStatusTypes: MapObject[] = StatusTypes.actInStatusType;

// 	// Private properties
    private subscriptions: Subscription[] = [];

    constructor(
        private activatedRoute: ActivatedRoute,
		private router: Router,
		private userFB: FormBuilder,
		private alertService: AlertService,
		private userService: UserService,
        private commonSvc: CommonService) { }

    ngOnInit() {
        this.populateDropdwonDataList();
		
		// this.alertService.fnLoading(true);
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
			if (id && !isNaN(Number(id))) {

				this.alertService.fnLoading(true);
				this.userService.getUserInfoById(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.user = res.data as UserInfo;
							this.initUser();
						}
					});
			} else {
				this.user = new UserInfo();
				this.user.clear();
				this.initUser();
			}
		});
		this.subscriptions.push(routeSubscription);
    }

    populateDropdwonDataList() {
        forkJoin([
            this.commonSvc.getDepotList(),
            this.commonSvc.getSaleOfficeList(),
            this.commonSvc.getSaleGroupList(),
            this.commonSvc.getTerritoryList(),
            this.commonSvc.getZoneList(),
            this.commonSvc.getRoleList()
        ]).subscribe(([plants, salesOffices, areaGroups, territories, zones, roles]) => {
            this.plants = plants.data;
            this.saleOffices = salesOffices.data;
            this.areaGroups = areaGroups.data;
            this.territories = territories.data;
            this.zones = zones.data;
            this.roles = roles.data;
        }, (err) => { }, () => { });
    }

    ngOnDestroy() {
        this.subscriptions.forEach(sb => sb.unsubscribe());
    }

    initUser() {
        this.createForm();
    }

   onADUserSearch(adUserName:any) {
        if(adUserName==null || adUserName=='') return;
        
        this.userService.getAdUserInfo(adUserName).subscribe((res:any) => {
            console.log(res);
            this.adUser = res.data;
            this.mapToUser(this.adUser);
        });
   }

   mapToUser(data:any) {
        const phoneNumber= data.mobile;
        const email= data.emailAddress;
        const userName= data.loginName;
        const employeeId= data.employeeId;
        const department= data.department;
        const title= data.title;
        const managerName= data.managerName;
        const managerId= data.managerId;
        let fullName = data.firstName + data.middleName + data.lastName;
        fullName = fullName.replace('  ', ' ');

        this.userForm.controls.userName.setValue(userName);
        this.userForm.controls.fullName.setValue(fullName);
        this.userForm.controls.email.setValue(email);
        this.userForm.controls.phoneNumber.setValue(phoneNumber);
        this.userForm.controls.employeeId.setValue(employeeId);
        this.userForm.controls.department.setValue(department);
        this.userForm.controls.designation.setValue(title);
        this.userForm.controls.managerName.setValue(managerName);
        this.userForm.controls.managerId.setValue(managerId);
   }

// 	resetErrors() {
// 		this.hasFormErrors = false;
// 		this.errors = [];
// 	}

   createForm() {
       this.userForm = this.userFB.group({
           userName: [this.user.userName, [Validators.required, Validators.pattern(/^(?!\s+$).+/)]],
           fullName: [this.user.fullName, [Validators.required, Validators.pattern(/^(?!\s+$).+/)]],
           email: [this.user.email, [Validators.required, Validators.email]],
           // phoneNumber: [this.user.phoneNumber, [Validators.required, Validators.pattern(/^(01[0-9]{9})$/)]],
           phoneNumber: [this.user.phoneNumber, [Validators.required, Validators.pattern(/^(?!\s+$).+/)]],
           address: [this.user.address],
           gender: [this.user.gender],
           dateOfBirth: [],
           code: [this.user.code, Validators.required],
           employeeId: [this.user.employeeId],
           designation: [this.user.designation],
           department: [this.user.department],
           managerId: [this.user.managerId],
           managerName: [this.user.managerName],
           status: [this.user.status],
           roleIds: [this.user.roleIds],
           plantIds: [this.user.plantIds],
           saleOfficeIds: [this.user.saleOfficeIds],
           areaIds: [this.user.areaIds],
           territoryIds: [this.user.territoryIds],
           zoneIds: [this.user.zoneIds],
       });

       if(this.user.dateOfBirth) {
           const dateStr = new Date(this.user.dateOfBirth);
           this.userForm.controls.dateOfBirth.setValue({
               year: dateStr.getFullYear(),
               month: dateStr.getMonth()+1,
               day: dateStr.getDate()
           });
       }
   } 
   
   get ufControls() { return this.userForm.controls; }

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
       const controls = this.userForm.controls;
       
       if (this.userForm.invalid) {
           Object.keys(controls).forEach(controlName =>
               controls[controlName].markAsTouched()
           );

           // this.hasFormErrors = true;
           // this.selectedTab = 0;
           return;
       }

       const editedUser = this.prepareUser();

       if (editedUser.id && !isNaN(Number(editedUser.id))) {
           this.updateUser(editedUser);
       }
       else {
           this.createUser(editedUser);
       }
   }

   prepareUser(): SaveUserInfo {
       const controls = this.userForm.controls;
       const _user = new SaveUserInfo();
       _user.clear();
       _user.id = this.user.id;
       _user.userName = controls['userName'].value;
       _user.email = controls['email'].value;
       _user.fullName = controls['fullName'].value;
       _user.gender = controls['gender'].value;
       _user.address = controls['address'].value;
       _user.phoneNumber = controls['phoneNumber'].value;
       _user.code = controls['code'].value;
       _user.employeeId = controls['employeeId'].value;
       _user.designation = controls['designation'].value;
       _user.department = controls['department'].value;
       _user.managerId = controls['managerId'].value;
       _user.managerName = controls['managerName'].value;
       _user.status = controls['status'].value;
       _user.roleIds = controls['roleIds'].value;
       _user.plantIds = controls['plantIds'].value;
       _user.saleOfficeIds = controls['saleOfficeIds'].value;
       _user.areaIds = controls['areaIds'].value;
       _user.territoryIds = controls['territoryIds'].value;
       _user.zoneIds = controls['zoneIds'].value;
       const date = controls['dateOfBirth'].value;
       if(date && date.year && date.month && date.day) {
           _user.dateOfBirth = new Date(date.year,date.month-1,date.day);
       }
       return _user;
   }

   createUser(_user: SaveUserInfo) {
       this.alertService.fnLoading(true);
       const createSubscription = this.userService.createUserInfo(_user)
           .pipe(finalize(() => this.alertService.fnLoading(false)))
           .subscribe(res => {
               this.alertService.tosterSuccess(`New user successfully has been added.`);
               this.goBack();
           },
           error => {
               this.throwError(error);
           });
       this.subscriptions.push(createSubscription);
   }

   updateUser(_user: SaveUserInfo) {
       this.alertService.fnLoading(true);
       const updateSubscription = this.userService.updateUserInfo(_user)
           .pipe(finalize(() => this.alertService.fnLoading(false)))
           .subscribe(res => {
               this.alertService.tosterSuccess(`User successfully has been saved.`);
               this.goBack();
           },
           error => {
               this.throwError(error);
           });
       this.subscriptions.push(updateSubscription);
   }

   getComponentTitle() {
       let result = 'Create user';
       if (!this.user || !this.user.id) {
           return result;
       }

       result = `Edit user - ${this.user.fullName}`;
       return result;
   }
   
   goBack() {
       this.router.navigate([`/users-info/users-infolist`], { relativeTo: this.activatedRoute });
   }

   stringToInt(value): number {
       return Number.parseInt(value);
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
       } else {
           // this.alertService.tosterDanger(errorDetails.error.message);
       }
   }
}
