import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin, Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import {
  DealerCompetitionSales,
  DealerSalesCall,
  DealerSalesIssue,
} from 'src/app/Shared/Entity/DealerSalesCall/dealer-sales-call';
import { LeadGeneration } from 'src/app/Shared/Entity/DemandGeneration/lead';
import { EnumDynamicTypeCode } from 'src/app/Shared/Enums/dynamic-type-code';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { DealerSalesCallService } from 'src/app/Shared/Services/DealerSalesCall/dealer-sales-call.service';
import { LeadService } from 'src/app/Shared/Services/DemandGeneration/lead.service';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';

@Component({
  selector: 'app-lead-edit',
  templateUrl: './lead-edit.component.html',
  styleUrls: ['./lead-edit.component.css'],
})
export class LeadEditComponent implements OnInit, OnDestroy {
  leadGeneration:LeadGeneration;
  id: number;
  userList: any[] = [];
  depotList: any[] = [];
  territoryList: any[] = [];
  zoneList: any[] = [];
  typeOfClientList: any[] = [];
  paintingStageList:any[]=[];
   visitDate:any;
   nextFollowUpDate:any;
   expectedDateOfPainting:any;





 
  private subscriptions: Subscription[] = [];

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private alertService: AlertService,
    private commonService: CommonService,
    private leadService: LeadService,
    private modalService: NgbModal,
    private dynamicDropdownService: DynamicDropdownService
  ) {
    this.leadGeneration = new LeadGeneration();
    this.leadGeneration.clear();
  }

  ngOnInit() {
    this.populateDropdown();
    // this.alertService.fnLoading(true);
    const routeSubscription = this.activatedRoute.params.subscribe((params) => {
      const id = params['id'];
      this.id = id;
      if (id) {
        this.alertService.fnLoading(true);
        this.leadService
          .getLeadById(id)
          .pipe(finalize(() => this.alertService.fnLoading(false)))
          .subscribe((res) => {
            if (res) {
              this.leadGeneration = res.data as LeadGeneration;
              // this.visitDate=this.leadGeneration.visitDate;
              if(this.leadGeneration.visitDate){
                const dateStr = new Date(this.leadGeneration.visitDate);
                this.visitDate={'year':dateStr.getFullYear(),'month':dateStr.getMonth()+1,'day':dateStr.getDate()};
              }
              if(this.leadGeneration.nextFollowUpDate){
                const dateStr = new Date(this.leadGeneration.nextFollowUpDate);
                this.nextFollowUpDate={'year':dateStr.getFullYear(),'month':dateStr.getMonth()+1,'day':dateStr.getDate()};
              }
              if(this.leadGeneration.expectedDateOfPainting){
                const dateStr = new Date(this.leadGeneration.expectedDateOfPainting);
                this.expectedDateOfPainting={'year':dateStr.getFullYear(),'month':dateStr.getMonth()+1,'day':dateStr.getDate()};
              }
              
            }
          });
      } else {
      }
    });
    this.subscriptions.push(routeSubscription);
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }

  public backToTheList() {
    this.router.navigate(['/lead/list']);
  }

  populateDropdown(): void {
    // this.loadDynamicDropdown();

    const forkJoinSubscription1 = forkJoin([
      this.commonService.getUserInfoListByCurrentUserWithoutZoUser(),
      this.commonService.getDepotList(),
      this.commonService.getTerritoryList(),
      this.commonService.getZoneList(),
      this.dynamicDropdownService.GetDropdownByTypeCd(
        EnumDynamicTypeCode.TypeOfClient
      ),
      this.dynamicDropdownService.GetDropdownByTypeCd(
        EnumDynamicTypeCode.PaintingStage
      )
    ]).subscribe(
      ([
        users,
        depot,
        territory,
        zone,
        typeOfClient,
        paintingStage
        
      ]) => {
        this.userList = users.data;
        this.depotList = depot.data;
        this.territoryList = territory.data;
        this.zoneList = zone.data;
        this.typeOfClientList = typeOfClient.data;
        this.paintingStageList=paintingStage.data
      },
      (err) => {},
      () => {}
    );

    this.subscriptions.push(forkJoinSubscription1);
  }




  save() {
    this.setDateFromDatePicker()
    this.leadService
      .updateLead(this.leadGeneration)
      .pipe(finalize(() => this.alertService.fnLoading(false)))
      .subscribe((res) => {
        if (res.statusCode == 200) {
          this.alertService.tosterSuccess(
            'Lead Generation Update successfully.'
          );
          this.router.navigate(['/lead/list']);
        } else {
          this.alertService.tosterWarning('Lead Generation Update failed.');
        }
      });
  }

  changeTerritoryByDepot() {
    console.log(this.leadGeneration.depot);

    this.commonService
      .getTerritoryListByDepot({'depots':[this.leadGeneration.depot]})
      .subscribe((res) => {
        this.territoryList = res.data;
        // console.log("TerritoryList",this.territoryList);
      });
  }

  changeZoneByDepotTerritory() {
    console.log(this.leadGeneration.depot);
    this.commonService
    .getZoneListByDepotTerritory({'depots':[this.leadGeneration.depot],'territories':[this.leadGeneration.territory]})
    .subscribe((res) => {
      this.zoneList = res.data;
      // console.log("ZoneList",this.zoneList);
    });
  }

  fileChangeEvents(fileInput: any) {
    if (fileInput.target.files && fileInput.target.files[0]) {
      // Size Filter Bytes

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const image = new Image();
        image.src = e.target.result;
        image.onload = (rs) => {
          const imgBase64Path = e.target.result;
          this.leadGeneration.photoCaptureUrlBase64 =
            imgBase64Path;
        };
      };

      reader.readAsDataURL(fileInput.target.files[0]);
    }
  }

  imageDelete($event) {
    this.leadService
      .DeleteLeadImage($event)
      .subscribe((res) => {
        if (res.statusCode == 200) {
          this.leadGeneration.photoCaptureUrl=null;
        } else {
          //this.alertService.tosterWarning('Dealer Sales Call Update failed.');
        }
      });

    console.log($event);
  }

  setDateFromDatePicker() {
    if(this.visitDate && this.visitDate.day && this.visitDate.month && this.visitDate.year){
      this.leadGeneration.visitDate = new Date(this.visitDate.year,this.visitDate.month-1,this.visitDate.day);
    }
    if(this.nextFollowUpDate && this.nextFollowUpDate.day && this.nextFollowUpDate.month && this.nextFollowUpDate.year){
      this.leadGeneration.nextFollowUpDate = new Date(this.nextFollowUpDate.year,this.nextFollowUpDate.month-1,this.nextFollowUpDate.day);

    }
    if(this.expectedDateOfPainting && this.expectedDateOfPainting.day && this.expectedDateOfPainting.month && this.expectedDateOfPainting.year){
      this.leadGeneration.expectedDateOfPainting = new Date(this.expectedDateOfPainting.year,this.expectedDateOfPainting.month-1,this.expectedDateOfPainting.day);

    }
  }
}
