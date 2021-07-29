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
	selector: 'app-modal-lead-followup-details',
	templateUrl: './modal-lead-followup-details.component.html',
	styleUrls: ['./modal-lead-followup-details.component.css']
})
export class ModalLeadFollowUpDetailsComponent implements OnInit, OnDestroy {

	@Input() leadFollowUp: LeadFollowUp;
	// @ViewChild('fileInput', {static:false}) fileInput: FileUpload; 
	
	private subscriptions: Subscription[] = [];

	constructor(private activatedRoute: ActivatedRoute,
		private router: Router,
		private alertService: AlertService,
		private commonService: CommonService,
		private modalService: NgbModal,
		public activeModal: NgbActiveModal) { }

	ngOnInit() {
		console.log('Lead FollowUp Details: ', this.leadFollowUp);
	}

	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

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
  
	public ptableSettingsInterior: IPTableSetting = {
		tableID: "leadFollowUps-table",
		tableClass: "table table-border ",
		tableName: 'Actual Volume Sold (Interior)',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Brand Description', width: '10%', internalName: 'brandInfoText', sort: false, type: "" },
			{ headerName: 'Quantity', width: '10%', internalName: 'quantity', sort: false, type: "" },
			{ headerName: 'Total Amount', width: '10%', internalName: 'totalAmount', sort: false, type: "" },
		],
		// enabledSearch: true,
		enabledSerialNo: true,
	};
  
	public ptableSettingsExterior: IPTableSetting = {
		tableID: "leadFollowUps-table",
		tableClass: "table table-border ",
		tableName: 'Actual Volume Sold (Exterior)',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Brand Description', width: '10%', internalName: 'brandInfoText', sort: false, type: "" },
			{ headerName: 'Quantity', width: '10%', internalName: 'quantity', sort: false, type: "" },
			{ headerName: 'Total Amount', width: '10%', internalName: 'totalAmount', sort: false, type: "" },
		],
		// enabledSearch: true,
		enabledSerialNo: true,
	};
  
	public ptableSettingsUnderCoat: IPTableSetting = {
		tableID: "leadFollowUps-table",
		tableClass: "table table-border ",
		tableName: 'Actual Volume Sold (Under Coat)',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Brand Description', width: '10%', internalName: 'brandInfoText', sort: false, type: "" },
			{ headerName: 'Quantity', width: '10%', internalName: 'quantity', sort: false, type: "" },
			{ headerName: 'Total Amount', width: '10%', internalName: 'totalAmount', sort: false, type: "" },
		],
		// enabledSearch: true,
		enabledSerialNo: true,
	};
  
	public ptableSettingsTopCoat: IPTableSetting = {
		tableID: "leadFollowUps-table",
		tableClass: "table table-border ",
		tableName: 'Actual Volume Sold (Top Coat)',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Brand Description', width: '10%', internalName: 'brandInfoText', sort: false, type: "" },
			{ headerName: 'Quantity', width: '10%', internalName: 'quantity', sort: false, type: "" },
			{ headerName: 'Total Amount', width: '10%', internalName: 'totalAmount', sort: false, type: "" },
		],
		// enabledSearch: true,
		enabledSerialNo: true,
	};
}
