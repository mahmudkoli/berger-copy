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
import { EnumDynamicTypeCode } from 'src/app/Shared/Enums/dynamic-type-code';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { DealerSalesCallService } from 'src/app/Shared/Services/DealerSalesCall/dealer-sales-call.service';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';

@Component({
  selector: 'app-dealer-sales-call-edit',
  templateUrl: './dealer-sales-call-edit.component.html',
  styleUrls: ['./dealer-sales-call-edit.component.css'],
})
export class DealerSalesCallEditComponent implements OnInit, OnDestroy {
  dealerSalesCall: DealerSalesCall;
  dealerCompetitionSales: DealerCompetitionSales[];
  dealerSalesIssues: DealerSalesIssue[];

  dealerList = [];
  userList = [];
  secondarySalesRatingslst = [];
  premiumProductLiftinglst = [];
  merchendisinglst = [];
  subDealerInfluencelst = [];
  painterInfluencelst = [];
  dealerSatisfactionlst = [];

  companylst = [];

  dealerSalesIssueCategorylst = [];
  prioritylst = [];
  cBMachineMantainancelst = [];
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
    this.dealerSalesCall = new DealerSalesCall();
    this.dealerSalesCall.clear();
  }

  ngOnInit() {
    this.populateDropdown();
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
              this.dealerSalesCall = res.data as DealerSalesCall;
              this.dealerCompetitionSales =
                this.dealerSalesCall.dealerCompetitionSales || [];
              this.dealerSalesIssues =
                this.dealerSalesCall.dealerSalesIssues || [];
              this.commonService.booleanToText(this.dealerSalesCall);
              this.dealerSalesIssues.forEach((obj) => {
                obj.id = 0;
                this.commonService.booleanToText(obj);
              });
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

  populateDropdown(): void {
    // this.loadDynamicDropdown();

    const forkJoinSubscription1 = forkJoin([
      this.commonService.getUserInfoListByCurrentUserWithoutZoUser(),
      this.commonService.getDealerList('', []),
      this.dynamicDropdownService.GetDropdownByTypeCd(
        EnumDynamicTypeCode.Merchendising
      ),
      this.dynamicDropdownService.GetDropdownByTypeCd(
        EnumDynamicTypeCode.Ratings
      ),
      this.dynamicDropdownService.GetDropdownByTypeCd(
        EnumDynamicTypeCode.ProductLifting
      ),
      this.dynamicDropdownService.GetDropdownByTypeCd(
        EnumDynamicTypeCode.SubDealerInfluence
      ),
      this.dynamicDropdownService.GetDropdownByTypeCd(
        EnumDynamicTypeCode.PainterInfluence
      ),
      this.dynamicDropdownService.GetDropdownByTypeCd(
        EnumDynamicTypeCode.Satisfaction
      ),
      this.dynamicDropdownService.GetDropdownByTypeCd(
        EnumDynamicTypeCode.SwappingCompetitionCompany
      ),
      this.dynamicDropdownService.GetDropdownByTypeCd(
        EnumDynamicTypeCode.Priority
      ),
      this.dynamicDropdownService.GetDropdownByTypeCd(
        EnumDynamicTypeCode.CBMachineMantainance
      ),
      this.dynamicDropdownService.GetDropdownByTypeCd(
        EnumDynamicTypeCode.ISSUES_01
      ),
    ]).subscribe(
      ([
        users,
        dealer,
        marchendising,
        Ratings,
        ProductLifting,
        SubDealerInfluence,
        PainterInfluence,
        Satisfaction,
        company,
        priority,
        CBMachineMantainance,
        issue,
      ]) => {
        this.userList = users.data;
        this.dealerList = dealer.data;
        this.merchendisinglst = marchendising.data;
        this.secondarySalesRatingslst = Ratings.data;
        this.premiumProductLiftinglst = ProductLifting.data;
        this.subDealerInfluencelst = SubDealerInfluence.data;
        this.painterInfluencelst = PainterInfluence.data;
        this.dealerSatisfactionlst = Satisfaction.data;
        this.companylst = company.data;
        this.prioritylst = priority.data;
        this.cBMachineMantainancelst = CBMachineMantainance.data;
        this.dealerSalesIssueCategorylst = issue.data;
      },
      (err) => {},
      () => {}
    );

    this.subscriptions.push(forkJoinSubscription1);
  }
  //   deleteDealerCompetitionSalesRow(x){
  // 	var delBtn = confirm(" Do you want to delete ?");
  // 	if ( delBtn == true ) {
  // 	  this.dealerCompetitionSales.splice(x, 1 );
  // 	}
  //   }

  //   addDealerCompetitionSalesTable() {
  // 	const obj = new DealerCompetitionSales();
  // 	this.dealerCompetitionSales.push(obj)
  //   }

  deleteDealerSalesIssueRow(x) {
    var delBtn = confirm(' Do you want to delete ?');
    if (delBtn == true) {
      this.dealerSalesIssues.splice(x, 1);
    }
  }

  addDealerSalesIssueTable() {
    const obj = new DealerSalesIssue();
    // if(obj.dealerSalesCallId==0 || obj.priorityId==0 || obj.quantity==null)
    // return;
    obj.dealerSalesCallId = this.id;
    obj.comments = '';
    //obj.priorityId = 61;
    //obj.dealerSalesIssueCategoryId = 23;
    obj.quantity = 0;
    this.dealerSalesIssues.push(obj);
  }

  save() {
    this.dealerSalesCall.dealerCompetitionSales = this.dealerCompetitionSales;
    this.dealerSalesCall.dealerSalesIssues = this.dealerSalesIssues;
    this.dealerSalesCallService
      .updateDealerSalesCall(this.dealerSalesCall)
      .pipe(finalize(() => this.alertService.fnLoading(false)))
      .subscribe((res) => {
        if (res.statusCode == 200) {
          this.alertService.tosterSuccess(
            'Dealer Sales Call Update successfully.'
          );
          this.router.navigate(['/dealer-sales-call']);
        } else {
          this.alertService.tosterWarning('Dealer Sales Call Update failed.');
        }
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
          this.dealerSalesCall.competitionProductDisplayImageBase64 =
            imgBase64Path;
        };
      };

      reader.readAsDataURL(fileInput.target.files[0]);
    }
  }

  fileChangeEvent(fileInput: any) {
    if (fileInput.target.files && fileInput.target.files[0]) {
      // Size Filter Bytes

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const image = new Image();
        image.src = e.target.result;
        image.onload = (rs) => {
          const imgBase64Path = e.target.result;
          this.dealerSalesCall.competitionSchemeModalityImageBase64 =
            imgBase64Path;
        };
      };

      reader.readAsDataURL(fileInput.target.files[0]);
    }
  }

  changeDealer() {}

  imageDelete($event) {
    this.dealerSalesCallService
      .DeleteIDealerSalesImage($event)
      .subscribe((res) => {
        if (res.statusCode == 200) {
          if ($event.type == 'competitionProductDisplayImageUrl') {
            this.dealerSalesCall.competitionProductDisplayImageUrl = null;
          } else {
            this.dealerSalesCall.competitionSchemeModalityImageUrl = null;
          }
        } else {
          //this.alertService.tosterWarning('Dealer Sales Call Update failed.');
        }
      });

    console.log($event);
  }
}
