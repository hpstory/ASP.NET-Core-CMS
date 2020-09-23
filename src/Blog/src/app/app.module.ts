import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { zh_CN } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import zh from '@angular/common/locales/zh';
import { NewsComponent } from './components/news/news.component';
import { ProfileComponent } from './components/profile/profile.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { NewsDetailComponent } from './components/news-detail/news-detail.component';
import { ContextSplitPipe } from './pipes/context-split.pipe';
import { IndexComponent } from './components/index/index.component';
import { AuthorizationHeaderInterceptor } from './services/oidc/anthorization-header.interceptor';
import { ServicesModule } from './services/services.module';

registerLocaleData(zh);

@NgModule({
  declarations: [
    AppComponent,
    NewsComponent,
    ProfileComponent,
    PageNotFoundComponent,
    NewsDetailComponent,
    ContextSplitPipe,
    IndexComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    NgZorroAntdModule,
    ServicesModule
  ],
  providers: [
    { provide: NZ_I18N, useValue: zh_CN },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthorizationHeaderInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
