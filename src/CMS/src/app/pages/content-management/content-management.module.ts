import { NgModule } from '@angular/core';
import { ContentRoutingModule } from './content-routing.module';
import { BannersComponent } from './banners/banners.component';
import { NewsCategoryComponent } from './news-category/news-category.component';
import { ContentManagementComponent } from './content-management.component';
import { ArticleListComponent } from './article-list/article-list.component';
import { AddArticleComponent } from './add-article/add-article.component';
import { EditArticleComponent } from './edit-article/edit-article.component';
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';



@NgModule({
  imports: [
    CommonModule,
    ContentRoutingModule,
    NgZorroAntdModule,
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [
    ContentManagementComponent,
    BannersComponent,
    NewsCategoryComponent,
    ArticleListComponent,
    AddArticleComponent,
    EditArticleComponent
  ],
  exports: [
  ]
})
export class ContentManagementModule { }
