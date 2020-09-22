import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ArticleListItem } from '../models/common.types';
import { API_CONFIG } from './services.module';

@Injectable({
  providedIn: 'root'
})
export class ArticleListService {

  constructor(
    private http: HttpClient,
    @Inject(API_CONFIG) private uri: string
    ) { }

    getArticles(
      pageNumber: number,
      pageSize: number,
      filters: Array<{ key: string; value: string }>,
    ): Observable<HttpResponse<ArticleListItem[]>>{
      let params = new HttpParams()
      .append("pageNumber", `${pageNumber}`)
      .append("pageSize", `${pageSize}`);
      if (filters[0] == null){

      }else if (filters[0].value == null){
      }else{
        params = params.append(filters[0].key, filters[0].value)
      }

      return this.http.get<ArticleListItem[]>(
        this.uri + "api/articles",
        {observe: "response", params});
    };
}

