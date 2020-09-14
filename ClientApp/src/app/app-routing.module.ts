import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

const routes: Routes = [
  // {path:"", redirectTo:'login',pathMatch:},
  { path: 'login', loadChildren:() => import('./Outside-Layout/outside-layout.module').then(m => m.OutsideLayoutModule) },
  { path: '', loadChildren: () => import('./Layout/layout.module').then(m => m.LayoutModule) },  
  // { path: 'login', loadChildren: './Outside-Layout/outside-layout.module#OutsideLayoutModule' },
  // { path: '', loadChildren: './Layout/layout.module#LayoutModule' },  
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes,
    {
      scrollPositionRestoration: 'enabled',
      anchorScrolling: 'enabled',
    })],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
