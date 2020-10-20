import { Component, OnInit, Directive, Input, ViewChild } from '@angular/core';
import { UserInfo } from '../../../Shared/Entity/Users/userInfo';
import { Router } from '@angular/router';
import { UserService } from 'src/app/Shared/Services/Users/user.service';
import { SalesPoint } from '../../../Shared/Entity';
import { Node } from '../../../Shared/Entity';
import { SalesPointService } from '../../../Shared/Services/Sales';
import { NodeService } from "../../../Shared/Services/Sales/node.service";

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
    nodes: Node[] = [];
    regions: Node[] = [];
    areas: Node[] = [];
    territories: Node[] = [];


    constructor(private userService: UserService,
        private router: Router,
        private salesPointService: SalesPointService,
        private nodeService: NodeService) { }

    ngOnInit() {
        this.findAllAdUser();
        //this.getAllSalesPoint();
        this.getAllNodes();
    }

    submitUserForm(model) {

        let userInfoObj = {

            name: this.userInfoModel.name,
            designation: this.userInfoModel.designation,
            phoneNumber: this.userInfoModel.phoneNumber,
            code: this.userInfoModel.code,
            salesPointId: this.userInfoModel.salesPointId,           
            territoryNodeIds: this.userInfoModel.territoryNodeIds,
            employeeId: this.userInfoModel.employeeId,
            email: this.userInfoModel.email,
            adGuid: this.userInfoModel.adGuid,
            groups: this.userInfoModel.groups
        }

        var result = this.userService.createUserInfo(userInfoObj).subscribe(res => {
            this.router.navigate(['/users-info']);
        });

    }


    findAllAdUser() {

       

    }


    onUserInfoSearch() {
        this.userInfoModel.name = this.adUser.displayName;
        this.userInfoModel.phoneNumber = this.adUser.mobilePhone;
        this.userInfoModel.designation = this.adUser.jobTitle;
        this.userInfoModel.email = this.adUser.mail;
        this.userInfoModel.adGuid = this.adUser.id;

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
        this.nodeService.getNodeList().subscribe(res => {
            this.nodes = res.data;
            console.log('getAllNodes this.nodes', this.nodes);
            this.regions = this.nodes.filter(s => s.code.startsWith('R'));
            this.areas = this.nodes.filter(s => s.code.startsWith('A'));
            this.territories = this.nodes.filter(s => s.code.startsWith('T'));
            console.log('regions console', this.regions);
        });
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

    private fnRouteUserInfoList() {
        this.router.navigate(['/users-info/users-infolist']);
    }
}