import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { BrandService } from 'src/app/Shared/Services/Brand/brand.service';
@Component({
  selector: 'app-brand-info-log-details',
  templateUrl: './brand-info-log-details.component.html',
  styleUrls: ['./brand-info-log-details.component.css']
})
export class BrandInfoLogDetailsComponent implements OnInit {

	constructor(
		private router: Router,
		private brandService: BrandService,
		private alertService: AlertService,
		private activatedRoute: ActivatedRoute,
		private commonService: CommonService,
		private modalService: NgbModal,
	) { }
	id: any;
	ngOnInit() {
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			this.id = params['id'];
			console.log(this.id);
			
		});
	}

}
