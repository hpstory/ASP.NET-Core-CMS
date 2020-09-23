import { Component, OnInit } from '@angular/core';
import { OpenIdConnectService } from 'src/app/services/oidc/open-id-connect.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {

  constructor(public oidc: OpenIdConnectService) { }

  ngOnInit(): void {
  }

}
