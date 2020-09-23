import { InjectionToken, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SigninOidcComponent } from './oidc/signin-oidc/signin-oidc.component';
import { RedirectSilentRenewComponent } from './oidc/redirect-silent-renew/redirect-silent-renew.component';

export const API_CONFIG = new InjectionToken('ApiConfigToken');

@NgModule({
  declarations: [SigninOidcComponent, RedirectSilentRenewComponent],
  imports: [
    CommonModule
  ],
  providers: [
    { provide: API_CONFIG, useValue: 'https://localhost:6001/' }
  ]
})
export class ServicesModule { }
