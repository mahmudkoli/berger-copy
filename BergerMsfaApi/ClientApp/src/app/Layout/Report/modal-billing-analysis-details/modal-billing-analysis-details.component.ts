import { Component, OnInit, OnDestroy, ViewChild, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { QuestionOption } from 'src/app/Shared/Entity/ELearning/questionOption';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { LeadFollowUp } from 'src/app/Shared/Entity/DemandGeneration/lead';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';

@Component({
	selector: 'app-modal-billing-analysis-details',
	templateUrl: './modal-billing-analysis-details.component.html',
	styleUrls: ['./modal-billing-analysis-details.component.css']
})
export class ModalBillingAnalysisDetailsComponent implements OnInit, OnDestroy {

	@Input() billingAnalysis: any;
	billingAnalysisCustomers: any[];
	// @ViewChild('fileInput', {static:false}) fileInput: FileUpload; 
	
	private subscriptions: Subscription[] = [];

	constructor(private activatedRoute: ActivatedRoute,
		private router: Router,
		private alertService: AlertService,
		private commonService: CommonService,
		private modalService: NgbModal,
		public activeModal: NgbActiveModal) { }

	ngOnInit() {
		console.log('Billing Analysis Details: ', this.billingAnalysis);
		this.billingAnalysisCustomers = this.billingAnalysis.details || [];
	}

	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

	public ptableSettings: IPTableSetting = {
		tableID: "billingAnalysis-table",
		tableClass: "table table-border ",
		tableName: 'Customer List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Customer No', width: '20%', internalName: 'customerNo', sort: true, type: "" },
			{ headerName: 'Customer Name', width: '40%', internalName: 'customerName', sort: true, type: "" },
			{ headerName: 'Is Billing', width: '40%', internalName: 'isBillingText', sort: true, type: "" },
		],
		enabledSearch: true,
		enabledSerialNo: true,
	};

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
