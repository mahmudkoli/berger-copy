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
    // this.populateDropdown();
    // this.alertService.fnLoading(true);
    const routeSubscription = this.activatedRoute.params.subscribe((params) => {
      const id = params['id'];
      this.id = id;
      if (id) {
        this.alertService.fnLoading(true);
        this.leadService
          .getLead(id)
          .pipe(finalize(() => this.alertService.fnLoading(false)))
          .subscribe((res) => {
            if (res) {
              this.leadGeneration = res.data as LeadGeneration;
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
      this.commonService.getTerritoryListByDepot(''),
      this.commonService.getZoneListByDepotTerritory(''),
      this.dynamicDropdownService.GetDropdownByTypeCd(
        EnumDynamicTypeCode.TypeOfClient
      ),
    ]).subscribe(
      ([
        users,
        depot,
        territory,
        zone
        
      ]) => {
        this.userList = users.data;
        this.depotList = depot.data;
        this.territoryList = territory.data;
        this.zoneList = zone.data;


      },
      (err) => {},
      () => {}
    );

    this.subscriptions.push(forkJoinSubscription1);
  }




  save() {
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


}
