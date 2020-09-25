import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzMessageService, NzUploadFile } from 'ng-zorro-antd';
import { Observable, Observer } from 'rxjs';
import { BannerListItem } from 'src/app/models/common.types';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-banners',
  templateUrl: './banners.component.html',
  styleUrls: ['./banners.component.scss']
})
export class BannersComponent implements OnInit {
  loading = true;
  banners: BannerListItem[] = [];
  dataLength = this.banners.length;
  newBannerForm!: FormGroup;
  uploadUrl = environment.apiBase + "/upload";
  fileList: NzUploadFile[] = [];
  imageUrl: string;
  previewVisible = false;
  editId: number | null = null;

  constructor(
    private msg: NzMessageService,
    // private bannerListService: BannerListService,
    private fb: FormBuilder,
  ) { }

 // 获取轮播列表
 getBannersTable(): void {
  // this.bannerListService.getBanners().subscribe(banners => {
  //   this.banners = banners;
  //   this.loading = false;
  // });
}
  // 删除轮播图
  deleteRow(id: number, len): void {
    // if(len <= 1){
    //   this.msg.info("请至少保留1张轮播图！")
    // }else{
    //   this.banners = this.banners.filter(d => d.id !== id);
    //   this.bannerListService.deleteBanner(id).subscribe();
    // }
  }
  // 新增轮播图
  addRow(len): void {
    if (len >= 6){
      this.msg.info("最多添加6张轮播图！");
    }else{
      this.showModal();
    }
  }
  // 封面上传
  handlePreview = async () => {
    this.previewVisible = true;
  };

  isVisible = false;
  isOkLoading = false;
  // 显示对话框
  showModal(id?: number): void {
    if(id){
      var editBanner = this.banners.filter(d => d.id == id);
      this.imageUrl = editBanner[0].imageUrl;
      this.newBannerForm = this.fb.group({
        title: [editBanner[0].title],
        position: [editBanner[0].position],
        link: [editBanner[0].link],
        imageUrl: [editBanner[0].imageUrl]
      });
      this.editId = id;
    }else{
      this.newBannerForm = this.fb.group({
        title: [null],
        position: [null],
        link: [null],
        imageUrl: [null]
      });
    }
    this.isVisible = true;
  }
  // 对话框取消按钮
  handleCancel(): void {
    this.isVisible = false;
  }
  // 对话框确认按钮
  handleOk(value: BannerListItem, id?: number): void {
    for (const i in this.newBannerForm.controls) {
      this.newBannerForm.controls[i].markAsDirty();
      this.newBannerForm.controls[i].updateValueAndValidity();
    }
    if(id){
      // value.imageUrl = this.imageUrl;
      // this.bannerListService.editBanner(id, value).subscribe(resp => {
      //   if(resp.status == 201 || resp.status == 204){
      //     this.isVisible = false;
      //     location.reload();
      //   }
      // }, error => {
      //   let errMsg = this.handleError(error);
      //   let err = Object.values(errMsg)
      //   for(let i = 0; i < err.length; i++){
      //     let errList = err[i];
      //     this.msg.error(errList[0])
      //   }
      // });
      // this.editId = null;
    }else{
      // this.isOkLoading = true;
      // value.imageUrl = this.imageUrl;
      // this.bannerListService.addBanner(value).subscribe(resp => {
      //   if(resp.status == 201 || resp.status == 204){
      //     this.isVisible = false;
      //     location.reload();
      //   }
      // }, error => {
      //   let errMsg = this.handleError(error);
      //   let err = Object.values(errMsg)
      //   for(let i = 0; i < err.length; i++){
      //     let errList = err[i];
      //     this.msg.error(errList[0])
      //   }
      // });
    }
    this.isOkLoading = false;
  }

  private handleError(responseErrors: HttpErrorResponse | any) {
    if (responseErrors instanceof HttpErrorResponse) {
        const data = responseErrors.error || '';
        const errorArray = data.errors;
        return errorArray;
    }
  }

  getImageUrl(event: { file: any; type: string; }){
    let file = event ? event.file : null;
    let datas = file ? file : file.response && file.response.rlt == 0 && file.response.datas;
    if (datas) {
      if (event.type == 'success') {
          console.log(datas)
          this.imageUrl = file.response["filePath"];
      } else if (event.type == 'removed') {
        this.imageUrl = "";
      }
    }
  }

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

  ngOnInit(): void {
    this.getBannersTable();
    this.newBannerForm = this.fb.group({
      title: [null, [Validators.required]],
      position: [null, [Validators.required]],
      link: [null],
      imageUrl: [null, [Validators.required]]
    });
  }

}
