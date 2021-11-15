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
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';

@Component({
  selector: 'app-lead-edit',
  templateUrl: './lead-edit.component.html',
  styleUrls: ['./lead-edit.component.css'],
})
export class LeadEditComponent implements OnInit, OnDestroy {
  leadGeneration:LeadGeneration;
  id: number;
 
  private subscriptions: Subscription[] = [];

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private alertService: AlertService,
    private commonService: CommonService,
    private dealerSalesCallService: DealerSalesCallService,
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
        this.dealerSalesCallService
          .getDealerSalesCall(id)
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
    this.router.navigate(['/dealer-sales-call']);
  }

  // populateDropdown(): void {
  //   // this.loadDynamicDropdown();

  //   const forkJoinSubscription1 = forkJoin([
  //     this.commonService.getUserInfoListByCurrentUserWithoutZoUser(),
  //     this.commonService.getDealerList('', []),
  //     this.dynamicDropdownService.GetDropdownByTypeCd(
  //       EnumDynamicTypeCode.Merchendising
  //     ),
  //     this.dynamicDropdownService.GetDropdownByTypeCd(
  //       EnumDynamicTypeCode.Ratings
  //     ),
  //     this.dynamicDropdownService.GetDropdownByTypeCd(
  //       EnumDynamicTypeCode.ProductLifting
  //     ),
  //     this.dynamicDropdownService.GetDropdownByTypeCd(
  //       EnumDynamicTypeCode.SubDealerInfluence
  //     ),
  //     this.dynamicDropdownService.GetDropdownByTypeCd(
  //       EnumDynamicTypeCode.PainterInfluence
  //     ),
  //     this.dynamicDropdownService.GetDropdownByTypeCd(
  //       EnumDynamicTypeCode.Satisfaction
  //     ),
  //     this.dynamicDropdownService.GetDropdownByTypeCd(
  //       EnumDynamicTypeCode.SwappingCompetitionCompany
  //     ),
  //     this.dynamicDropdownService.GetDropdownByTypeCd(
  //       EnumDynamicTypeCode.Priority
  //     ),
  //     this.dynamicDropdownService.GetDropdownByTypeCd(
  //       EnumDynamicTypeCode.CBMachineMantainance
  //     ),
  //     this.dynamicDropdownService.GetDropdownByTypeCd(
  //       EnumDynamicTypeCode.ISSUES_01
  //     ),
  //   ]).subscribe(
  //     ([
  //       users,
  //       dealer,
  //       marchendising,
  //       Ratings,
  //       ProductLifting,
  //       SubDealerInfluence,
  //       PainterInfluence,
  //       Satisfaction,
  //       company,
  //       priority,
  //       CBMachineMantainance,
  //       issue,
  //     ]) => {
  //       this.userList = users.data;
  //       this.dealerList = dealer.data;
  //       this.merchendisinglst = marchendising.data;
  //       this.secondarySalesRatingslst = Ratings.data;
  //       this.premiumProductLiftinglst = ProductLifting.data;
  //       this.subDealerInfluencelst = SubDealerInfluence.data;
  //       this.painterInfluencelst = PainterInfluence.data;
  //       this.dealerSatisfactionlst = Satisfaction.data;
  //       this.companylst = company.data;
  //       this.prioritylst = priority.data;
  //       this.cBMachineMantainancelst = CBMachineMantainance.data;
  //       this.dealerSalesIssueCategorylst = issue.data;
  //     },
  //     (err) => {},
  //     () => {}
  //   );

  //   this.subscriptions.push(forkJoinSubscription1);
  // }




  save() {
    this.dealerSalesCallService
      .updateDealerSalesCall(new Object())
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
