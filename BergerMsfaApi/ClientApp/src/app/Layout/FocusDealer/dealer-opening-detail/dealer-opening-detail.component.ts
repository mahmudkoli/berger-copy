import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DealerOpeningStatus } from 'src/app/Shared/Entity/DealerOpening/DealerOpeningStatusChange';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { DealeropeningService } from 'src/app/Shared/Services/FocusDealer/dealeropening.service';


@Component({
  selector: 'app-dealer-opening-detail',
  templateUrl: './dealer-opening-detail.component.html',
  styleUrls: ['./dealer-opening-detail.component.css']
})
export class DealerOpeningDetailComponent implements OnInit {
  dealerOpen:any;
  public baseUrl: string;
  painter: any;
  dealeropening:DealerOpeningStatus=new DealerOpeningStatus();
  constructor(

    private alertService: AlertService,
    private route: ActivatedRoute,
    private dealerOpeningSvc: DealeropeningService,
    private router: Router,
    @Inject('BASE_URL') baseUrl: string) { this.baseUrl = baseUrl; }

  ngOnInit() {
    if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
      console.log("id", this.route.snapshot.params.id);
      let id = this.route.snapshot.params.id;
      this.getDealerOpenDetailById(id);
    }
  }

  private getDealerOpenDetailById(id) {
    this.dealerOpeningSvc.GetDealerOpeningDetailById(id).subscribe(
      (result: any) => {
        if(result){
          this.dealerOpen = result.data;

        }
        // debugger;
        // console.log("dealeropeningLog",result.data)
      },
      (err: any) => console.log(err)
    );
  }

  onStatusChange(mySelect, dealer) {

    // debugger;
    console.log(dealer)
    this.dealeropening.dealerOpeningId = dealer.id;
    this.dealeropening.status = Number(mySelect);
  
    if (this.dealeropening.status == 3) {
        if (!this.dealeropening.comment) {
            this.alertService.alert("Please leave a comment");
            return;
        }
    }


    this.alertService.confirm(`Are you sure to change status?`, () => {

        //if (PlanStatus.Rejected == Number(mySelect)) {
        //    alert("Rejected");
        //}
        //else if (PlanStatus.Approved == Number(mySelect)) {
        //    alert("Approved");
        //}
        //else return;

        this.alertService.fnLoading(true);
        this.dealerOpeningSvc.DealerOpeningStatusChange(this.dealeropening).subscribe(
            (res) => {
              console.log(res)
            
                this.router.navigate(["/dealer/openingList"]).then(
                    () => {
                      if(res.data){
                        this.alertService.tosterSuccess(`New Dealer status approved successfully.`);

                      }
                      else {
                        this.alertService.tosterInfo(`New Dealer status rejected successfully.`);

                      }
                    });
              
            },
            (error) => {
                console.log(error);
                this.displayError(error);
            }

        ).add(() => this.alertService.fnLoading(false));
    }, () => {

    });
}

  back() {
    this.router.navigate(['dealer/openingList']);
  }

  private displayError(errorDetails: any) {

    console.log("error", errorDetails);
    let errList = errorDetails.error.errors;
    if (errList.length) {
        console.log("error", errList, errList[0].errorList[0]);
        this.alertService.tosterDanger(errList[0].errorList[0]);
    } else {
        this.alertService.tosterDanger(errorDetails.error.msg);
    }
}
}
