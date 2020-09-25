import { Component, OnInit } from '@angular/core';
import { NzMessageService, NzTableQueryParams } from 'ng-zorro-antd';
import { ArticleListItem } from 'src/app/models/common.types';

@Component({
  selector: 'app-article-list',
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.scss']
})
export class ArticleListComponent implements OnInit {
  total = 1;
  listOfAriticle: ArticleListItem[] = [];
  loading = true;
  pageSize = 10;
  pageNumber = 1;
  filterCategory = [
    { text: '财经', value: "财经" },
    { text: '理财', value: "理财" },
    { text: '股票', value: "股票" },
    { text: '大盘', value: "大盘" },
  ];
  loadArticlesFromServer(
    pageNumber: number,
    pageSize: number,
    filter: Array<{ key: string; value: string }>,
  ): void {
    this.loading = true;
    // this.articleListService.getArticles(pageNumber, pageSize, filter).subscribe(resp => {
    //   this.loading = false;
    //   var pagination = JSON.parse(resp.headers.get("x-pagination"));
    //   this.total = pagination["totalCount"];
    //   this.listOfAriticle = resp.body;
    // })
  }

  onQueryParamsChange(params: NzTableQueryParams): void {
    const { pageIndex, pageSize, filter } = params;
  //   const currentSort = sort.find(item => item.value !== null);
  //   const sortField = (currentSort && currentSort.key) || null;
  //   const sortOrder = (currentSort && currentSort.value) || null;
    this.loadArticlesFromServer(pageIndex, pageSize, filter);
  }

  constructor(
    // private articleListService: ArticleListService,
    public msg: NzMessageService
    ) {}

  ngOnInit(): void {
    this.loadArticlesFromServer(this.pageNumber, this.pageSize, []);
  }
  deleteRow(id: number): void {
    // this.articleListService.deleteArticles(id).subscribe();
    this.listOfAriticle = this.listOfAriticle.filter(d => d.id !== id);
    location.reload();
  }
  searchValue = '';
  visible = false;
  // reset(): void {
  //   this.searchValue = '';
  //   this.search();
  // }
  listOfDisplayData = [...this.listOfAriticle];
  // search(): void {
  //   this.visible = false;
  //   this.listOfDisplayData = this.listOfAriticle.filter((item: ArticleListItem) => item.title.indexOf(this.searchValue) !== -1);
  // }

}
