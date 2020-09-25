import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'welcome', loadChildren: () => import('./pages/welcome/welcome.module').then(m => m.WelcomeModule) },
  { path: 'content', loadChildren: () => import('./pages/content-management/content-management.module').then(m => m.ContentManagementModule) },
  { path: 'staff', loadChildren: () => import('./pages/staff-management/staff-management.module').then(m => m.StaffManagementModule) },
  { path: 'data', loadChildren: () => import('./pages/data-management/data-management.module').then(m => m.DataManagementModule) },
  { path: '', redirectTo: '/login', pathMatch: "full" },
  { path: "**", component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
