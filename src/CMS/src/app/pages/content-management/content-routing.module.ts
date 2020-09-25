import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddArticleComponent } from './add-article/add-article.component';
import { ArticleListComponent } from './article-list/article-list.component';
import { BannersComponent } from './banners/banners.component';
import { EditArticleComponent } from './edit-article/edit-article.component';
import { NewsCategoryComponent } from './news-category/news-category.component';

const routes: Routes = [
  { path: 'category', component: NewsCategoryComponent },
  { path: 'banner', component: BannersComponent },
  { path: 'article', component: ArticleListComponent },
  { path: 'addArticle', component: AddArticleComponent },
  { path: 'editArticle', component: EditArticleComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContentRoutingModule { }
