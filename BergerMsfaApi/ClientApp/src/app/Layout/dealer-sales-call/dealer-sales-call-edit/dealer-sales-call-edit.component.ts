import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { finalize } from 'rxjs/operators';
import { APIResponse } from 'src/app/Shared/Entity/Response/api-response';
import { forkJoin, Subscription } from 'rxjs';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { DealerCompetitionSales, DealerSalesCall, DealerSalesIssue } from 'src/app/Shared/Entity/DealerSalesCall/dealer-sales-call';
import { DealerSalesCallService } from 'src/app/Shared/Services/DealerSalesCall/dealer-sales-call.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';
import { EnumDynamicTypeCode } from 'src/app/Shared/Enums/dynamic-type-code';

@Component({
  selector: 'app-dealer-sales-call-edit',
  templateUrl: './dealer-sales-call-edit.component.html',
  styleUrls: ['./dealer-sales-call-edit.component.css']
})
export class DealerSalesCallEditComponent implements OnInit, OnDestroy {

	dealerSalesCall: DealerSalesCall;
	dealerCompetitionSales: DealerCompetitionSales[];
	dealerSalesIssues: DealerSalesIssue[];
  dealerList=[];
  userList=[];
  secondarySalesRatingslst=[];
  premiumProductLiftinglst=[];
  merchendisinglst=[];
  subDealerInfluencelst=[];
  painterInfluencelst=[];
  dealerSatisfactionlst=[];


	
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
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
			console.log(id);
			if (id) {
				this.alertService.fnLoading(true);
				this.dealerSalesCallService.getDealerSalesCall(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.dealerSalesCall = res.data as DealerSalesCall;
							this.dealerCompetitionSales = this.dealerSalesCall.dealerCompetitionSales || [];
							this.dealerSalesIssues = this.dealerSalesCall.dealerSalesIssues || [];
							this.commonService.booleanToText(this.dealerSalesCall);
							this.dealerSalesIssues.forEach(obj => {
								this.commonService.booleanToText(obj);
							});
							console.log(this.dealerSalesCall);
							
						}
					});
			} else {
        
			}
		});
		this.subscriptions.push(routeSubscription);
	
	}

	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

	public backToTheList() {
		this.router.navigate(['/dealer-sales-call']);
	}

	public dcs_ptableSettings: IPTableSetting = {
		tableID: "dealerCompetitionSales-table",
		tableClass: "table table-border ",
		tableName: 'Dealer Competition Sales List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Company Name', width: '40%', internalName: 'companyName', sort: false, type: "" },
			{ headerName: 'Average Monthly Sales', width: '30%', internalName: 'averageMonthlySales', sort: false, type: "text", displayType: 'number-format-color' },
			{ headerName: 'Actual MTD Sales', width: '30%', internalName: 'actualMTDSales', sort: false, type: "text", displayType: 'number-format-color' }
		],
		// enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		// enabledPagination: true,
		// enabledDeleteBtn: true,
		// enabledEditBtn: true,
		// enabledCellClick: true,
		// enabledColumnFilter: false,
		// enabledRecordCreateBtn: true,
		// enabledDataLength: true,
		// newRecordButtonText: 'New ELearning'
	};

	public dsi_ptableSettings: IPTableSetting = {
		tableID: "dealerSalesIssues-table",
		tableClass: "table table-border ",
		tableName: 'Dealer Sales Issue List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Dealer Sales Issue Category', width: '10%', internalName: 'dealerSalesIssueCategoryText', sort: false, type: "" },
			{ headerName: 'Material Name', width: '10%', internalName: 'materialName', sort: false, type: "" },
			{ headerName: 'Material Group', width: '10%', internalName: 'materialGroup', sort: false, type: "" },
			{ headerName: 'Quantity', width: '10%', internalName: 'quantity', sort: false, type: "" },
			{ headerName: 'Batch Number', width: '10%', internalName: 'batchNumber', sort: false, type: "" },
			{ headerName: 'Comments', width: '10%', internalName: 'comments', sort: false, type: "" },
			{ headerName: 'Priority', width: '10%', internalName: 'priorityText', sort: false, type: "" },
			{ headerName: 'Has CB Machine Mantainance', width: '10%', internalName: 'hasCBMachineMantainanceText', sort: false, type: "" },
			{ headerName: 'CB Machine Mantainance', width: '10%', internalName: 'cBMachineMantainanceText', sort: false, type: "" },
			{ headerName: 'CB Machine Mantainance Regular Reason', width: '10%', internalName: 'cBMachineMantainanceRegularReason', sort: false, type: "" }
		],
		// enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		// enabledPagination: true,
		// enabledDeleteBtn: true,
		// enabledEditBtn: true,
		// enabledCellClick: true,
		// enabledColumnFilter: false,
		// enabledRecordCreateBtn: true,
		// enabledDataLength: true,
		// newRecordButtonText: 'New ELearning'
	};

  getUserList(): void {
    this.alertService.fnLoading(true);
    this.commonService.getUserInfoList()
      .subscribe(
        res => {
          this.userList = res.data;
          this.alertService.fnLoading(false);
        })
  }

  getDealerList(): void {
    this.alertService.fnLoading(true);
    this.commonService.getDealerList('',[])
      .subscribe(
        res => {
          this.dealerList = res.data;
          this.alertService.fnLoading(false);
        })
  }

  populateDropdown(): void {
   
      // this.loadDynamicDropdown();
  
          const forkJoinSubscription1 = forkJoin([
              this.commonService.getUserInfoListByCurrentUserWithoutZoUser(),
              this.commonService.getDealerList('',[]),
        this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.Merchendising),
        this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.Ratings),
        this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.ProductLifting),
        this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.SubDealerInfluence),
        this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.PainterInfluence),
        this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.Satisfaction),





        // this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.Customer),
        //       this.commonService.getCreditControlAreaList(),
  
  
          ]).subscribe(([users, dealer,marchendising,Ratings,ProductLifting,SubDealerInfluence,PainterInfluence,Satisfaction]) => {
              this.userList = users.data;
              this.dealerList = dealer.data;
			  this.merchendisinglst = marchendising.data;
			  this.secondarySalesRatingslst = Ratings.data;
			  this.premiumProductLiftinglst = ProductLifting.data;
			  this.subDealerInfluencelst = SubDealerInfluence.data;
			  this.painterInfluencelst = PainterInfluence.data;
			  this.dealerSatisfactionlst = Satisfaction.data;
        
  
          }, (err) => { }, () => { });
  
      this.subscriptions.push(forkJoinSubscription1);
      
  }

}
