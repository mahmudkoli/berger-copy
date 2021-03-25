import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { IPTableSetting } from '../../../Shared/Modules/p-table';
import { CommonService } from '../../../Shared/Services/Common/common.service';

@Component({
  selector: 'app-modal-painter-call-details',
  templateUrl: './modal-painter-call-details.component.html',
  styleUrls: ['./modal-painter-call-details.component.css']
})
export class ModalPainterCallDetailsComponent implements OnInit {

	@Input() painterCompanyMTDValue;

	private subscriptions: Subscription[] = [];

	constructor(
		private activatedRoute: ActivatedRoute,
		private router: Router,
		private alertService: AlertService,
		private commonService: CommonService,
		private modalService: NgbModal,
		public activeModal: NgbActiveModal
	) { }

	 ngOnInit() {
	 }
	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

	public ptableSettings: IPTableSetting = {
		tableID: "painterCompanyMTDValue-table",
		tableClass: "table table-border ",
		tableName: 'painter Company MTD Value',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'CompanyId', width: '20%', internalName: 'companyId', sort: false, type: "" },
			{ headerName: 'CompanyName', width: '20%', internalName: 'companyName', sort: false, type: "" },
			{ headerName: 'Value', width: '20%', internalName: 'value', sort: false, type: "" },
			{ headerName: 'Count In Percent', width: '20%', internalName: 'countInPercent', sort: false, type: "" },
			{ headerName: 'Cumelative In Percent', width: '20%', internalName: 'cumelativeInPercent', sort: false, type: "" }
			


		],
		//enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		enabledPagination: true,
		// enabledDeleteBtn: true,
		// enabledEditBtn: true,
		//enabledCellClick: true,
		//enabledColumnFilter: false,
		// enabledRecordCreateBtn: true,
		enabledDataLength: true,
		// newRecordButtonText: 'New ELearning'
	};
		
}
