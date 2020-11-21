import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/Shared/Services/Users/user.service';
import { UserInfo } from '../../../Shared/Entity/Users/userInfo';
import { SalesPointService } from '../../../Shared/Services/Sales';
import { NodeService } from "../../../Shared/Services/Sales/node.service";
import { DynamicDropdownService } from '../../../Shared/Services/Setup/dynamic-dropdown.service';
import { forkJoin } from 'rxjs';
import { Status } from '../../../Shared/Enums/status';
import { CommonService } from '../../../Shared/Services/Common/common.service';

@Component({
    selector: 'app-user-info-insert',
    templateUrl: './user-info-insert.component.html',
    styleUrls: ['./user-info-insert.component.css'],
    providers: [UserService, SalesPointService]
})
export class UserInfoInsertComponent implements OnInit {

    public userInfoModel: UserInfo = new UserInfo();

    adUser: any;
    azureUser: any[] = [];
    activeStatus: boolean;
    adUserList: any[] = [];
    //salesPoints: SalesPoint[] = [];
    //nodes: Node[] = [];
    //regions: Node[] = [];
    //areas: Node[] = [];
    //territories: Node[] = [];

    plants: any[] = [];
    saleOffices: any[] = [];
    areaGroups: any[] = [];
    // territorys: any[] = [];
    zones: any[] = [];
    roles: any[] = [];
    territories:any[]=[]
    changeStatus = Status;
    statusKeys: any[] = [];
    adError: string;


    constructor(
        private userService: UserService,
        private router: Router,
        private commonSvc: CommonService,
        private salesPointService: SalesPointService,
        private dropdownService: DynamicDropdownService,
        private nodeService: NodeService) { }

    ngOnInit() {
        this.statusKeys = Object.keys(this.changeStatus).filter(k => !isNaN(Number(k)));
        this.populateDropdwonDataList();
        this.findAllAdUser();
        //this.getAllSalesPoint();
        this.getAllNodes();
    }

    submitUserForm() {

        let userInfoObj = {

           // name: this.userInfoModel.name,
            firstName: this.userInfoModel.firstName,
            middleName: this.userInfoModel.middleName,
            lastName: this.userInfoModel.lastName,
            designation: this.userInfoModel.designation,
            phoneNumber: this.userInfoModel.phoneNumber,
            code: this.userInfoModel.code,
            employeeId: this.userInfoModel.employeeId,
            email: this.userInfoModel.email,
            adGuid: this.userInfoModel.adGuid,
            //manager: this.userInfoModel.manager,
            managerName: this.userInfoModel.managerName,
            managerId: this.userInfoModel.managerId,
            loginName: this.userInfoModel.loginName,
            userName: this.userInfoModel.userName,
            loginNameWithDomain: this.userInfoModel.loginNameWithDomain,
            postalCode: this.userInfoModel.postalCode,
            //salesPointId: this.userInfoModel.salesPointId,
            //territoryNodeIds: this.userInfoModel.territoryNodeIds,
         
            //groups: this.userInfoModel.groups,
   
            city: this.userInfoModel.city,
            department: this.userInfoModel.department,
            country: this.userInfoModel.country,
            streetAddress: this.userInfoModel.streetAddress,
            extension: this.userInfoModel.extension,
            fax: this.userInfoModel.fax,
            statusText: this.userInfoModel.statusText,
            status: this.userInfoModel.status,
            state: this.userInfoModel.state,
            plantIds: this.userInfoModel.plantIds,
            territoryIds: this.userInfoModel.territoryIds,
            areaIds: this.userInfoModel.areaIds,
            zoneIds: this.userInfoModel.zoneIds,
            saleOfficeIds: this.userInfoModel.saleOfficeIds,
            roleIds:this.userInfoModel.roleIds
           
           
        }

        var result = this.userService.createUserInfo(userInfoObj).subscribe(res => {
            this.router.navigate(['/users-info']);
        });

    }


    findAllAdUser() {



    }


    onUserInfoSearch(adUserName:any) {
        this.userService.getAdUserInfo(adUserName).subscribe((res:any) => {
            console.log(res);
            this.adUser = res.data;
            this.mapToUserInfo(this.adUser);
        });
    }

    mapToUserInfo(data:any) {
        this.userInfoModel.firstName= data.firstName;
        this.userInfoModel.middleName= data.middleName;
        this.userInfoModel.lastName= data.lastName;
        this.userInfoModel.designation= data.designation;
        this.userInfoModel.department= data.department;
        this.userInfoModel.phoneNumber= data.mobile;
        this.userInfoModel.code= data.code;
        this.userInfoModel.city= data.city;
        this.userInfoModel.company= data.company;
        this.userInfoModel.employeeId= data.employeeId;
        this.userInfoModel.email= data.emailAddress;
        this.userInfoModel.adGuid= data.adGuid;
        this.userInfoModel.managerName= data.managerName;
        this.userInfoModel.managerId= data.managerId;
        this.userInfoModel.loginName= data.loginName;
        this.userInfoModel.userName= data.loginName;
        this.userInfoModel.loginNameWithDomain= data.loginNameWithDomain;
        this.userInfoModel.postalCode= data.postalCode;
        this.userInfoModel.streetAddress= data.streetAddress;
        this.userInfoModel.state= data.state;
        this.userInfoModel.title= data.title;
    }

    adUserTableSearch(searchVal: string) {

        for (var i = 0; i < this.azureUser.length; i++) {
            if (this.azureUser[i].userPrincipalName.toLowerCase() == searchVal.toLowerCase()) {
                this.adUser = this.azureUser[i];
                console.log(this.azureUser[i]);
            }

        }
        //this.adUserList = this.azureUser.filter(function (hero) {
        //    if (hero.displayName.toLowerCase().includes(searchVal.toLowerCase())) {
        //        return hero;
        //    } else { return "Not Found" }
        //}) || [];
        // this.adUserList = this.azureUser.filter((record: any) => { if (record.headerName.toLowerCase().includes(searchVal.toLowerCase())) { return true } else { return false } }) || [];
    }


    //getAllSalesPoint() {
    //    this.salesPointService.getAllSalesPoint().subscribe(res => {
    //        this.salesPoints = res.data;
    //    });
    //}

    getAllNodes() {
        //this.nodeService.getNodeList().subscribe(res => {
        //    this.nodes = res.data;
        //    console.log('getAllNodes this.nodes', this.nodes);
        //    this.regions = this.nodes.filter(s => s.code.startsWith('R'));
        //    this.areas = this.nodes.filter(s => s.code.startsWith('A'));
        //    this.territories = this.nodes.filter(s => s.code.startsWith('T'));
        //    console.log('regions console', this.regions);
        //});
    }

    populateDropdwonDataList() {
       
        forkJoin(
            this.commonSvc.getDepotList(),
            this.commonSvc.getSaleOfficeList(),
            this.commonSvc.getSaleGroupList(),
            this.commonSvc.getTerritoryList(),
            this.commonSvc.getZoneList(),
            this.commonSvc.getRoleList(),
            //this.dropdownService.GetDropdownByTypeCd('Z01'),
        
         //   this.dropdownService.GetDropdownByTypeCd('SO01'),
          
            //this.dropdownService.GetDropdownByTypeCd('PA01'),
        /*this.dropdownService.GetDropdownByTypeCd('T01'),*/
         
        
        ).subscribe(res => {

            this.plants = res[0].data;
            this.saleOffices = res[1].data;
            this.areaGroups = res[2].data;
            this.territories = res[3].data;
            this.zones = res[4].data;
            this.roles = res[5].data;

        }, (err) => { }, () => { });

    }

    public val: any;
    public isNational: boolean;
    public isRegion: boolean;
    public isArea: boolean;
    public isTerritory: boolean;
    //public isSalesPoint: boolean;

    changeFnOptType(val) {
        console.log("Dropdown selection:", val);
        this.isNational = val === 1;
        this.isRegion = val === 2;
        this.isArea = val === 3;
        this.isTerritory = val === 4;
        //this.isSalesPoint = val.some(el => el === 5);
    }

     fnRouteUserInfoList() {
        this.router.navigate(['/users-info/users-infolist']);
    }
}
