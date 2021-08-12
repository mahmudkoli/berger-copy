import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { forkJoin, Subscription } from 'rxjs';
import { NewDealerDevelopmentQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { EnumMonthLabel } from 'src/app/Shared/Enums/employee-role';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { NewDealerDevelopmentService } from 'src/app/Shared/Services/KPI/NewDealerDevelopmentService';

@Component({
  selector: 'app-new-dealer-development',
  templateUrl: './new-dealer-development.component.html',
  styleUrls: ['./new-dealer-development.component.css']
})
export class NewDealerDevelopmentComponent implements OnInit {
  query: NewDealerDevelopmentQuery;
	data: any[];
  month: MapObject[] = EnumMonthLabel.EnumMonth;
  depotList=[];
  territoryList=[];
	searchForm: FormGroup;
	private subscriptions: Subscription[] = [];

  constructor(
    private newDealerDevelopmentService:NewDealerDevelopmentService,
    private commonService: CommonService,
    private formBuilder: FormBuilder,
    private alertService: AlertService
  ) { }

  ngOnInit() {
    this.populateDropdown();
    this.initCollectionForm();
  }

  
  prepareNewDealerEntry(): NewDealerDevelopmentQuery {
		const controls = this.searchForm.controls;

		const _query = new NewDealerDevelopmentQuery();
		_query.depot = controls['depot'].value;
		_query.territory = controls['territory'].value;
    _query.year=new Date().getFullYear();
		return _query;
	}

  initCollectionForm() {
		this.createForm();
	}

	createForm() {
		this.searchForm = this.formBuilder.group({
			depot: [''],
			territory: [''],
			
		});

	}
	get formControls() { return this.searchForm.controls; }

  loadData() {
    const query=this.prepareNewDealerEntry();
    this.data = [];
    this.newDealerDevelopmentService.getNewDealerDevelopment(query).subscribe(
      res => this.data = res.data,
      error => console.log(error),
      () => console.log('done')
    );
  }

  SaveOrUpdateData() {

this.newDealerDevelopmentService.SaveOrUpdateNewDealerDevelopment(this.data).subscribe(
      res =>{

        if(res){}
        this.alertService.tosterSuccess("New Dealer Development Save Successfully")
      },
      error => console.log(error),
      () => console.log('done')
    );
}


populateDropdown(): void {
   
  // this.loadDynamicDropdown();

      const forkJoinSubscription1 = forkJoin([
          this.commonService.getDepotList(),
          this.commonService.getTerritoryList(),
      ]).subscribe(([depot, territory]) => {
        
          this.depotList = depot.data;
          this.territoryList = territory.data;
      }, (err) => { }, () => { });

  this.subscriptions.push(forkJoinSubscription1);
  
}



}
