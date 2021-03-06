import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { finalize } from 'rxjs/operators';
import { APIResponse } from 'src/app/Shared/Entity/Response/api-response';
import { Subscription } from 'rxjs';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { DealerCompetitionSales, DealerSalesCall, DealerSalesIssue } from 'src/app/Shared/Entity/DealerSalesCall/dealer-sales-call';
import { DealerSalesCallService } from 'src/app/Shared/Services/DealerSalesCall/dealer-sales-call.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';

@Component({
	selector: 'app-dealer-sales-call-details',
	templateUrl: './dealer-sales-call-details.component.html',
	styleUrls: ['./dealer-sales-call-details.component.css']
})
export class DealerSalesCallDetailsComponent implements OnInit, OnDestroy {

	dealerSalesCall: DealerSalesCall;
	dealerCompetitionSales: DealerCompetitionSales[];
	dealerSalesIssues: DealerSalesIssue[];
	
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private alertService: AlertService,
		private commonService: CommonService,
		private dealerSalesCallService: DealerSalesCallService,
		private modalService: NgbModal
	) { 
		this.dealerSalesCall = new DealerSalesCall();
		this.dealerSalesCall.clear();
	}

	ngOnInit() {
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
			{ headerName: 'CB Machine Mantainance', width: '10%', internalName: 'cbMachineMantainanceText', sort: false, type: "" },
			{ headerName: 'CB Machine Mantainance Regular Reason', width: '10%', internalName: 'cbMachineMantainanceRegularReason', sort: false, type: "" }
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
}
