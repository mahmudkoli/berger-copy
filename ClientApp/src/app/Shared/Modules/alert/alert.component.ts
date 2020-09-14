import { Component, OnInit } from '@angular/core';
import { AlertService } from "./alert.service";
import { slideInOutAnimation } from './slideInOutAnimation';

@Component({
  selector: 'alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css'],
  animations:[slideInOutAnimation]
})
export class AlertComponent implements OnInit {
  loading: boolean;
  message: any;
  tosterCollection:any[]=[];
  titleTosterCollection:any[]=[];
  constructor(private alertService: AlertService) { }

  ngOnInit() {
    //this function waits for a message from alert service, it gets 
    this.alertService.getMessage().subscribe(message => {
      if ((message!=null?message.type:'undefine') == 'loading') {
        if (message.text == "true") {
          this.loading = true;
        } else {
          this.loading = false;
        }

      }
      else if((message!=null) && (message.type=="toster") ){
        let tosterId= Math.floor(1000 + Math.random() * 9000);
        this.tosterCollection.push({
            type:message.alertType,
            text:message.text,
            id:tosterId
        });

        setTimeout(() => {
          console.log("tosterId: ", tosterId,this.tosterCollection);
          this.tosterCollection= this.tosterCollection.filter(res=>res.id!=tosterId)
        }, 5000);
      }
      else if((message!=null) && (message.type).includes("title-toster") ){ // for title toster
        let tosterId= Math.floor(1000 + Math.random() * 9000);
        this.titleTosterCollection.push({
            type:message.alertType,
            text:message.text,
            id:tosterId
        });

        setTimeout(() => {
          console.log("tosterId: ", tosterId,this.titleTosterCollection);
          this.titleTosterCollection= this.titleTosterCollection.filter(res=>res.id!=tosterId)
        }, 300000);
      }
      this.message = message;
    },err=>{
      console.log("Error in Alert Service: ", err);
    });

  }

  fnRemoveToster(tosterId){
    this.tosterCollection= this.tosterCollection.filter(res=>res.id!=tosterId)
  }
  fnRemoveTitleToster(tosterId){
    this.titleTosterCollection= this.titleTosterCollection.filter(res=>res.id!=tosterId)
  }

  

}
