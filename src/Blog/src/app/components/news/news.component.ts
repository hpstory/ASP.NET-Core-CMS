import { HttpClient } from '@angular/common/http';
import { Component, OnInit, SkipSelf } from '@angular/core';
import { ArticleListItem } from 'src/app/models/common.types';
import { ArticleListService } from 'src/app/services/article-list.service';
import { BannerListService } from 'src/app/services/banner-list.service';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.scss']
})
export class NewsComponent implements OnInit {
  public bannerArray: any[];
  data: any[] = [];
  loadingMore = false;
  loading: boolean = false;
  pageSize = 5;
  pageNumber = 1;
  total = 1;
  listOfAriticle: ArticleListItem[] = [];
  hotAricles: ArticleListItem[];
  loadMoreUrl: string = "";

  constructor(
    @SkipSelf()
    private bannerListService: BannerListService,
    private articleListService: ArticleListService,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    this.getBannersTable();
    this.loadArticlesFromServer(this.pageNumber, this.pageSize, []);
    this.getHotArticles(this.pageNumber, 3, [ { key: "isHot", value: true } ]);
  }

  getBannersTable(): void {
    this.bannerListService.getBanners().subscribe(banners => {
      this.bannerArray = banners;
    });
  }

  loadArticlesFromServer(
    pageNumber: number,
    pageSize: number,
    filter: Array<{ key: string; value: string }>,
  ): void {
    this.loading = true;
    this.articleListService.getArticles(pageNumber, pageSize, filter).subscribe(resp => {
      this.loading = false;
      var pagination = JSON.parse(resp.headers.get("x-pagination"));
      this.total = pagination["totalCount"];
      this.loadMoreUrl = pagination["nextPageLink"]
      this.listOfAriticle = resp.body;
    })
  }

  getHotArticles(
    pageNumber: number,
    pageSize: number,
    filter: Array<{ key: string; value: any }>,
  ): void {
    this.articleListService.getArticles(pageNumber, pageSize, filter).subscribe(resp => {
      this.loading = false;
      var pagination = JSON.parse(resp.headers.get("x-pagination"));
      this.total = pagination["totalCount"];
      this.hotAricles = resp.body;
    })
  }

  onLoadMore(): void {
    if (this.loadMoreUrl === null){
      this.loadingMore = true;
    }else{
      this.loadingMore = true;
      this.http.get(this.loadMoreUrl, {observe: "response"}).subscribe((res: any) => {
        this.listOfAriticle = this.listOfAriticle.concat(res.body);
        this.loadMoreUrl = JSON.parse(res.headers.get("x-pagination"))["nextPageLink"];
        this.loadingMore = false;
      });
    }
  }

  countIncrease(id: number, type: string){
    // return clickCount++;
  }

}
