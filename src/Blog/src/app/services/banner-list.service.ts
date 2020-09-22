import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_CONFIG } from './services.module';
import { map } from 'rxjs/internal/operators';
import { BannerListItem } from '../models/common.types';

@Injectable({
  providedIn: 'root'
})
export class BannerListService {

  constructor(
    private http: HttpClient,
    @Inject(API_CONFIG) private uri: string
  ) { }

  getBanners(): Observable<BannerListItem []> {
    return this.http.get(this.uri + "api/banners").pipe(
      map((banners: BannerListItem[]) => banners));
  }
}
