import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { UserInfo } from "../../../Shared/Entity/Users/userInfo";
import { UserService } from "src/app/Shared/Services/Users";
import { AlertService } from "../../../Shared/Modules/alert/alert.service";
import { NodeService } from "src/app/Shared/Services/Sales/node.service";
import { Node } from "../../../Shared/Entity";
import { RoleService } from 'src/app/Shared/Services/Users/role.service';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { Hierarchy } from 'src/app/Shared/Entity/Sales/hierarchy';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
@Component({
  selector: "app-user-info-edit",
  templateUrl: "./user-info-edit.component.html",
  styleUrls: ["./user-info-edit.component.css"],
  providers: [UserService],
})
export class UserInfoEditComponent implements OnInit {
  public form: FormGroup;
  userInfo: UserInfo = new UserInfo();
  nodes: Node[] = [];
  regions: Node[] = [];
  areas: Node[] = [];
  territories: Node[] = [];
  selectedRole = null;
  roleList: any[] = [];

  selectedNode: Node = new Node();
  enumStatusTypes: MapObject[] = StatusTypes.statusType;
  hierarchyType: Hierarchy[] = [];

  public val: any;
  public isNational: boolean;
  public isRegion: boolean;
  public isArea: boolean;
  public isTerritory: boolean;
  //public isSalesPoint: boolean;
  public isAdmin = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private alertService: AlertService,
    private nodeService: NodeService,
    private userService: UserService,
    private roleService: RoleService,
    private commonService: CommonService
  ) {}

  ngOnInit() {
    if (this.activatedRoute.snapshot.params.hasOwnProperty("id")) {
      this.getUserById(this.activatedRoute.snapshot.params.id);
    }
    this.getHierarchyType();
    this.getRoles();
    this.getAllNodes();

    const userInfo = this.commonService.getUserInfoFromLocalStorage();
    this.isAdmin = userInfo != null ? userInfo.roleName == 'Admin' ? true : false : false;
  }

  getHierarchyType() {
    this.userService.getAllHierarchy().subscribe((result: any) => {
      this.hierarchyType = result.data;

    });
  }

  getRoles() {
    this.roleService.getRoleList().subscribe(
        (res: any) => {
            console.log("Roles: ", res.data);
            this.roleList = res.data.model;
            this.selectedRole = this.roleList.length ? this.roleList[0].id : 0;
         //   this.getMenus();
        },
        (err: any) => {
            console.log(err);
        }
    );
}
  getUserById(id: number) {
    console.log(id);
    this.alertService.fnLoading(true);
    this.userService.getUserInfoById(id).subscribe(
      (res: any) => {
        this.userInfo = res.data || new UserInfo();

        if (Object.keys(res.data).length == 0) {
          this.showError("No such user!Create a new user");
          this.router.navigate([`/users-info/users-infolist/`]);
        } else {
          console.log("User Info", this.userInfo);
          this.userService
            .getDesignationCodeById(this.userInfo.id)
            .subscribe((result: any) => {
              debugger;
              this.selectedNode = result.data;
              if (this.selectedNode != null) {
                  if (this.selectedNode.code.startsWith("N")) {
                      this.userInfo.nationalNodeIds = this.selectedNode.nodeIdList;
     
                  
                }

                  if (this.selectedNode.code.startsWith("R")) {
                      this.userInfo.regionNodeIds = this.selectedNode.nodeIdList;
                  
                }

                  if (this.selectedNode.code.startsWith("A")) {
                      this.userInfo.areaNodeIds = this.selectedNode.nodeIdList;
                 
                }

                  if (this.selectedNode.code.startsWith("T")) {
                      this.userInfo.territoryNodeIds = this.selectedNode.nodeIdList;
                
                }

                this.changeFnOptType(this.userInfo.hierarchyId);  


              }
            });
        }
      },
      (error) => {
        console.log(error);
        this.showError(error.message);
        this.router.navigate([`/users-info/users-infolist/`]);
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

  submitUserForm(userInfoPostModel : UserInfo) {

    userInfoPostModel.hierarchyId

    if(userInfoPostModel.hierarchyId == 1){
      userInfoPostModel.regionNodeIds = [];
      userInfoPostModel.areaNodeIds = [];
      userInfoPostModel.territoryNodeIds = [];
    }
    else if(userInfoPostModel.hierarchyId == 2)
    {
      userInfoPostModel.nationalNodeIds = [];
      userInfoPostModel.areaNodeIds = [];
      userInfoPostModel.territoryNodeIds = [];
    }
    else if(userInfoPostModel.hierarchyId == 3)
    {
      userInfoPostModel.nationalNodeIds = [];
      userInfoPostModel.regionNodeIds = [];
      userInfoPostModel.territoryNodeIds = [];
    }
    else if(userInfoPostModel.hierarchyId == 4)
    {
      userInfoPostModel.nationalNodeIds = [];
      userInfoPostModel.regionNodeIds = [];
      userInfoPostModel.areaNodeIds = [];
    }
    console.log(userInfoPostModel);
    debugger;

    var result = this.userService
      .updateUserInfo(userInfoPostModel)
      .subscribe((res) => {

        this.router.navigate(["/users-info"]).then(() => {
          this.alertService.titleTosterSuccess("Record has been updated successfully.");
        });
      },
      (error) => {
        // debugger;
        this.displayError(error);
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

  changeFnOptType(val) {
    console.log("Dropdown selection:", val);
    this.isNational = val === 1;
    this.isRegion = val === 2;
    this.isArea = val === 3;
    this.isTerritory = val === 4;
    //this.isSalesPoint = val.some(el => el === 5);
  }

  getAllNodes() {
    this.nodeService.getNodeList().subscribe((res) => {
      this.nodes = res.data;
      console.log("getAllNodes this.nodes", this.nodes);
      this.regions = this.nodes.filter((s) => s.code.startsWith("R"));
      this.areas = this.nodes.filter((s) => s.code.startsWith("A"));
      this.territories = this.nodes.filter((s) => s.code.startsWith("T"));
      console.log("regions console", this.regions);
    });
  }
}
