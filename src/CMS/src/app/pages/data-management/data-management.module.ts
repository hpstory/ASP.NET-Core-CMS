import { NgModule } from '@angular/core';
import { DataManagementRoutingModule } from './data-management-routing.module';
import { StatisticsComponent } from './statistics/statistics.component';


@NgModule({
  declarations: [StatisticsComponent],
  imports: [
    DataManagementRoutingModule
  ]
})
export class DataManagementModule { }
