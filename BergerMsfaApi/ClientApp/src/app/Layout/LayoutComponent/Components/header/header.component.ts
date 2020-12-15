import {Component, HostBinding, OnInit} from '@angular/core';
import {select} from '@angular-redux/store';
import {Observable} from 'rxjs';
import {ThemeOptions} from '../../../../theme-options';
import { CommonService } from '../../../../Shared/Services/Common/common.service';
import { UserInfo } from '../../../../Shared/Entity';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
})
export class HeaderComponent implements OnInit {
    userName: string;
    constructor(public globals: ThemeOptions, private commonSvc: CommonService) {
    }
    ngOnInit() {
        let user = this.commonSvc.getUserInfoFromLocalStorage();
        if (user) this.userName = `${user.fullName}_${user.employeeId}_${user.managerId}`
    }
  @HostBinding('class.isActive')
  get isActiveAsGetter() {
    return this.isActive;
  }

  isActive: boolean;

  @select('config') public config$: Observable<any>;

  toggleSidebarMobile() {
    this.globals.toggleSidebarMobile = !this.globals.toggleSidebarMobile;
  }

  toggleHeaderMobile() {
    this.globals.toggleHeaderMobile = !this.globals.toggleHeaderMobile;
  }

}
