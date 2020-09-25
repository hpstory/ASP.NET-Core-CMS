import { Component, OnInit } from '@angular/core';
import { CategoryListItem } from 'src/app/models/common.types';

@Component({
  selector: 'app-news-category',
  templateUrl: './news-category.component.html',
  styleUrls: ['./news-category.component.scss']
})
export class NewsCategoryComponent implements OnInit {

  categories: CategoryListItem[] = [];
  dataLength = this.categories.length;
  editId: number | null = null;
  loading = true;

  constructor(
    // private newsCategoryService: NewsCategoryService
  ) { }

  ngOnInit(): void {
    this.getCategory();
  }

  startEdit(id: number): void {
    this.editId = id;
  }

  stopEdit(id: number, name: string): void {
    this.editId = null;
    //this.newsCategoryService.editCategory(id, `{"name": "${name}"}`).subscribe();
  }

  getCategory(): void {
    // this.newsCategoryService.getCategory().subscribe(
    //   category => {
    //     this.categories = category;
    //     this.loading = false;
    //   }
    // );
  }
}
