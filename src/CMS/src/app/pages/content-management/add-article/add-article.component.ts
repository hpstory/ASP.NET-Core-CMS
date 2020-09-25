import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NzDateMode, NzMessageService, NzUploadFile } from 'ng-zorro-antd';
import { Observable, Observer } from 'rxjs';
import { CategoryListItem } from 'src/app/models/common.types';
import { environment } from 'src/environments/environment';



@Component({
  selector: 'app-add-article',
  templateUrl: './add-article.component.html',
  styleUrls: ['./add-article.component.scss']
})
export class AddArticleComponent implements OnInit {
  editor: any;
  defaultMessage: string = "请输入内容";
  uploadUrl = environment.apiBase + "/upload";
  fileList: NzUploadFile[] = [];
  imageUrl: string | undefined = null;
  previewVisible = false;
  articleForm: FormGroup;
  categories: CategoryListItem[] = [];
  isOkLoading = false;
  dateFormat = 'yyyy-MM-dd HH:mm:ss';

  constructor(
    private fb: FormBuilder,
    private msg: NzMessageService,
    // private service: FileUploadService,
    // private newsCategoryService: NewsCategoryService,
    // private articleListService: ArticleListService,
    private router: Router
  ) { }
  ngOnInit(): void {
    this.articleForm = this.fb.group({
      title: [null, [Validators.required]],
      author: [null],
      categoryId: [null, [Validators.required]],
      publishDate: [null, [Validators.required]],
      isHot: [false],
      cover: [null],
      content: [null],
      publisherName: [null],
    });
    // this.editor = new wangEditor("#editorMenu", "#editor");
    this.setEditorConfig();
    this.editor.create();
    // this.newsCategoryService.getCategory().subscribe(
    //   category => {
    //     this.categories = category;
    //   })
  }

  // 时间选择
  dateMode: NzDateMode = 'time';

  handleDateOpenChange(open: boolean): void {
    if (open) {
      this.dateMode = 'time';
    }
  }

  setEditorConfig(){
    this.editor.customConfig.menus = this.getMenuConfig();
    this.editor.customConfig.colors = this.getColorConfig();
    this.editor.customConfig.uploadImgServer = '/upload';
    this.editor.customConfig.uploadImgMaxSize = 2 * 1014 *1024;
    this.editor.customConfig.zIndex = 100;

    this.editor.customConfig.customUploadImg = (files, insert) => {
      if(files.length !== 1){
        this.msg.info("单次只能上传一个图片");
        return;
      }
      let formData = new FormData();
      formData.append("formFile", files[0] as any);

      // this.service.uploadFile(formData).subscribe(res => {
      //   if(res.ok){
      //     insert(res.body["filePath"]);
      //   }else{

      //   }
      // })
    }
  }

  getColorConfig(): string[]{
    return [
      '#ffffff',
      '#000000',
      '#eeece0',
      '#1c487f',
      '#4d80bf',
      '#c24f4a',
      '#8baa4a',
      '#7b5ba1',
      '#46acc8',
      '#f9963b',
      '#0076B8',
      '#E2C08D',
      '#8EE153',
      '#B6001F'
    ];
  }

  getMenuConfig(): string[] {
    return [
      'bold',  // 粗体
      'italic',  // 斜体
      'underline',  // 下划线
      'head',  // 标题
      'fontName',  // 字体
      'fontSize',  // 字号
      'strikeThrough',  // 删除线
      'foreColor',  // 文字颜色
      'backColor',  // 背景颜色
      'link',  // 插入链接
      'list',  // 列表
      'justify',  // 对齐方式
      'quote',  // 引用
      'table',  // 表格
      'image',  // 插入图片
      'code',  // 插入代码
    ];
  }

  // 封面上传
  handlePreview = async () => {
    this.previewVisible = true;
  };

  beforeUpload = (file: NzUploadFile, _fileList: NzUploadFile[]) => {
    return new Observable((observer: Observer<boolean>) => {
      if (!(file.type === 'image/jpg' || file.type === 'image/jpeg')) {
        this.msg.error('You can only upload JPG file!');
        observer.complete();
        return;
      }
      const isLt2M = file.size! / 1024 / 1024 < 2;
      if (!isLt2M) {
        this.msg.error('Image must smaller than 2MB!');
        observer.complete();
        return;
      }
      observer.next((file.type === 'image/jpg' || file.type === 'image/jpeg') && isLt2M);
      observer.complete();
    });
  };
  getImageUrl(event: { file: any; type: string; }){
    let file = event ? event.file : null;
    let datas = file ? file : file.response && file.response.rlt == 0 && file.response.datas;
    if (datas) {
      if (event.type == 'success') {
          this.imageUrl = file.response["filePath"];
      } else if (event.type == 'removed') {
        this.imageUrl = "";
      }
    }
  }
  // 提交
  submitForm(
    // value: ArticleCreateItem
    ){
    // for (const i in this.articleForm.controls) {
    //   this.articleForm.controls[i].markAsDirty();
    //   this.articleForm.controls[i].updateValueAndValidity();
    // }
    // this.isOkLoading = true;
    // value.cover = this.imageUrl;
    // value.publisherName = JSON.parse(window.localStorage.getItem('userinfo'))["userName"];
    // value.content = this.editor.txt.html();
    // this.articleListService.addArticle(value).subscribe(resp => {
    //   if(resp.status == 201 || resp.status == 204){
    //     this.router.navigateByUrl("/cms/articleList");
    //   }
    // }, error => {
    //   let errMsg = this.handleError(error);
    //   let err = Object.values(errMsg)
    //   for(let i = 0; i < err.length; i++){
    //     let errList = err[i];
    //     this.msg.error(errList[0])
    //   }
    // });
    // this.isOkLoading = false;
  }
  private handleError(responseErrors: HttpErrorResponse | any) {
    if (responseErrors instanceof HttpErrorResponse) {
        const data = responseErrors.error || '';
        const errorArray = data.errors;
        return errorArray;
    }
  }

}
