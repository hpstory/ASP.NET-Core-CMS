import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NewsDetailComponent } from './components/news-detail/news-detail.component';
import { NewsComponent } from './components/news/news.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { ProfileComponent } from './components/profile/profile.component';
import { RedirectSilentRenewComponent } from './services/oidc/redirect-silent-renew/redirect-silent-renew.component';
import { SigninOidcComponent } from './services/oidc/signin-oidc/signin-oidc.component';


const routes: Routes = [
  { path: "news", component: NewsComponent },
  { path: "news/:id", component: NewsDetailComponent },
  { path: "profile", component: ProfileComponent },
  { path: "signin-oidc", component: SigninOidcComponent },
  { path: "redirect-silent-renew", component: RedirectSilentRenewComponent },
  { path: "", redirectTo: "/news", pathMatch: 'full' },
  { path: "**", component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
