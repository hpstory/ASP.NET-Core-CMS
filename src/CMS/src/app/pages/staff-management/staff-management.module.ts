import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StaffManagementRoutingModule } from './staff-management-routing.module';
import { AuthorityComponent } from './authority/authority.component';


@NgModule({
  declarations: [AuthorityComponent],
  imports: [
    CommonModule,
    StaffManagementRoutingModule
  ]
})
export class StaffManagementModule { }
