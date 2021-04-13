import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { finalize } from 'rxjs/operators';
import { APIResponse } from 'src/app/Shared/Entity/Response/api-response';
import { LeadFollowUp, LeadGeneration } from 'src/app/Shared/Entity/DemandGeneration/lead';
import { Subscription } from 'rxjs';
import { LeadService } from 'src/app/Shared/Services/DemandGeneration/lead.service';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ModalLeadFollowUpDetailsComponent } from '../modal-lead-followup-details/modal-lead-followup-details.component';
import { AuthService } from '../../../Shared/Services/Users';

@Component({
  selector: 'app-lead-details',
  templateUrl: './lead-details.component.html',
  styleUrls: ['./lead-details.component.css']
})
export class LeadDetailsComponent implements OnInit, OnDestroy {

  lead: LeadGeneration;
	leadFollowUps: LeadFollowUp[];
	userRole: string;
	public baseUrl: string;
	private subscriptions: Subscription[] = [];

  constructor(
	private router: Router,
	private activatedRoute: ActivatedRoute,
	private alertService: AlertService,
	private leadService: LeadService,
	  private modalService: NgbModal,
	  private authService: AuthService,
	  @Inject('BASE_URL') baseUrl: string
  ) { 
	this.lead = new LeadGeneration();
	  this.lead.clear();
	  this.baseUrl = baseUrl;
  }

	ngOnInit() {
		
		// this.alertService.fnLoading(true);
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
			console.log(id);
			if (id) {
				this.alertService.fnLoading(true);
				this.leadService.getLead(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.lead = res.data as LeadGeneration;
							this.lead.requirementOfColorSchemeText = this.lead.requirementOfColorScheme ? "Yes" : "No";
							this.lead.productSamplingRequiredText = this.lead.productSamplingRequired ? "Yes" : "No";
							this.leadFollowUps = this.lead.leadFollowUps || [];
							console.log(this.lead);
							this.leadFollowUps.forEach((x) => {
								x.detailsBtnText = "View FollowUp";
								//x.deleteBtnText = "Delete";
								//x.deleteBtnClass = 'btn-transition btn btn-sm btn-outline-danger d-flex align-items-center';
								//x.deleteBtnIcon = 'fa fa-trash';
							});
						}
					});
			} else {
		
			}
		});
		this.subscriptions.push(routeSubscription);

		this.addLeadFollowUpDeleteBtn();
	}

	

	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

  public backToTheList() {
	this.router.navigate(['/lead']);
  }
  
	public ptableSettings: IPTableSetting = {
		tableID: "leadFollowUps-table",
		tableClass: "table table-border ",
		tableName: 'Lead FollowUp List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Last Visited Date', width: '10%', internalName: 'lastVisitedDateText', sort: false, type: "" },
			{ headerName: 'Next Visit Date Plan', width: '10%', internalName: 'nextVisitDatePlanText', sort: false, type: "" },
			{ headerName: 'Actual Visit Date', width: '10%', internalName: 'actualVisitDateText', sort: false, type: "" },
			{ headerName: 'Type Of Client', width: '10%', internalName: 'typeOfClientText', sort: false, type: "" },
			{ headerName: 'Key Contact Person Name', width: '10%', internalName: 'keyContactPersonName', sort: false, type: "" },
			{ headerName: 'Key Contact Person Mobile', width: '10%', internalName: 'keyContactPersonMobile', sort: false, type: "" },
			{ headerName: 'Paint Contractor Name', width: '10%', internalName: 'paintContractorName', sort: false, type: "" },
			{ headerName: 'Paint Contractor Mobile', width: '10%', internalName: 'paintContractorMobile', sort: false, type: "" },
			{ headerName: 'Expected Value', width: '10%', internalName: 'expectedValue', sort: false, type: "" },
			{ headerName: 'Details', width: '10%', internalName: 'detailsBtnText', sort: false, type: "button", onClick: 'true', innerBtnIcon: "" },
			
		],
		// enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		// enabledPagination: true,
		 //enabledDeleteBtn: true,
		// enabledEditBtn: true,
		enabledCellClick: true,
		// enabledColumnFilter: false,
		// enabledRecordCreateBtn: true,
		// enabledDataLength: true,
		// newRecordButtonText: 'New ELearning'
		
		
	};
	public fnCustomTrigger(event) {
		console.log("custom  click: ", event);

		
		if (event.action == "delete-item") {
			this.deleteLeadFollowUp(event.record.id);
			
		}
	}
	deleteLeadFollowUp(id) {
		this.alertService.confirm("Are you sure want to delete this lead follow up?",
			() => {
				this.alertService.fnLoading(true);
				const deleteSubscription = this.leadService.deleteLeadFollowUp(id)
					.pipe(finalize(() => { this.alertService.fnLoading(false); }))
					.subscribe((res: any) => {
						console.log('res from del func', res);
						this.alertService.tosterSuccess("Lead follow up has been deleted successfully.");
							this.leadFollowUps.forEach((value, index) => {
								if (value.id == id) this.leadFollowUps.splice(index, 1);
							});
						},
						(error) => {
							console.log(error);
						});
				this.subscriptions.push(deleteSubscription);
			},
			() => { });
	}
	public cellClickCallbackFn(event: any) {
		console.log(event);
		
		let leadFollowUp = event.record;
		let cellName = event.cellName;

		if (cellName == "detailsBtnText") {
			this.detailsLeadFollowUp(leadFollowUp);
		}
		if (cellName == "deleteBtnText") {
			this.deleteLeadFollowUp(leadFollowUp);
		}
	}
	//private deleteLeadFollowUp(leadFollowUp: LeadFollowUp) {
	//	//alert('delete' + leadFollowUp.id);
	//	this.leadService.deleteLeadFollowUp(leadFollowUp.id).subscribe(res => {
	//		this.leadFollowUps.forEach((value, index) => {
	//			if (value.id == leadFollowUp.id) this.leadFollowUps.splice(index, 1);
	//		});
	//	});
	//}
	
	public detailsLeadFollowUp(leadFollowUp: LeadFollowUp) {
		console.log(leadFollowUp);
		this.openLeadFollowUpDetailsModal(leadFollowUp);
	}
	
	openLeadFollowUpDetailsModal(leadFollowUp: LeadFollowUp) {
		let ngbModalOptions: NgbModalOptions = {
		  backdrop: "static",
		  keyboard: false,
		  size: "lg",
		};
		const modalRef = this.modalService.open(
			ModalLeadFollowUpDetailsComponent,
		  ngbModalOptions
		);
		modalRef.componentInstance.leadFollowUp = leadFollowUp;
	
		modalRef.result.then(
			(result) => {
				console.log(result);
			},
			(reason) => {
				console.log(reason);
			}
		);
	}

	private addLeadFollowUpDeleteBtn() {
		this.userRole = this.authService.currentUserValue.roleName;
		//console.log("Role: " + userRole);
		if (this.userRole == "Admin") {
			this.ptableSettings.enabledDeleteBtn = true;
			//this.ptableSettings.tableColDef.push(
			//	{ headerName: '', width: '10%', internalName: 'deleteBtnText', className: 'deleteBtnClass', sort: false, type: "dynamic-button", onClick: 'true', innerBtnIcon: "deleteBtnIcon" }
			//);
		}
	}
}
