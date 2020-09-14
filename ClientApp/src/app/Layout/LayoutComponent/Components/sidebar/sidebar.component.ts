import { Component, HostListener, OnInit } from '@angular/core';
import { ThemeOptions } from '../../../../theme-options';
import { select } from '@angular-redux/store';
import { Observable } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { MenuService } from 'src/app/Shared/Services/Menu-Details/menu.service';
import { Menu } from 'src/app/Shared/Entity/Menu/menu.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent implements OnInit {
  public extraParameter: any;
  menuList: Menu[] = [];

  constructor(public globals: ThemeOptions, private activatedRoute: ActivatedRoute,
    private menuService: MenuService ) {

  }

  @select('config') public config$: Observable<any>;

  private newInnerWidth: number;
  private innerWidth: number;
  activeId = 'dashboardsMenu';
  prod: any;
  toggleSidebar() {
    this.globals.toggleSidebar = !this.globals.toggleSidebar;
  }

  sidebarHover() {
    this.globals.sidebarHover = !this.globals.sidebarHover;
  }

  ngOnInit() {
    this.prod = !environment.production;
    setTimeout(() => {

      this.innerWidth = window.innerWidth;
      if (this.innerWidth < 1200) {
        this.globals.toggleSidebar = true;
      }
    });

    this.extraParameter = this.activatedRoute.snapshot.firstChild.data.extraParameter || "";
    
    this.getMenus();

  }

  getMenus() { 
    let roleId = 1;
    if(localStorage.getItem('userinfo')) {
        const userInfo = JSON.parse(localStorage.getItem('userinfo'));
        roleId = userInfo.roleId;
    }
    this.menuService.getPermissionMenus(roleId).subscribe(
      (res: any) => {
        console.log("Menus:................",res.data);
        this.menuList = res.data;        
      },
      (err: any) => {
        console.log(err);
      }
    );
  }

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.newInnerWidth = event.target.innerWidth;

    if (this.newInnerWidth < 1200) {
      this.globals.toggleSidebar = true;
    } else {
      this.globals.toggleSidebar = false;
    }

  }
}
