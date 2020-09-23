import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { NzCascaderOption, NzDateMode, NzMessageService, NzUploadFile } from 'ng-zorro-antd';
import { Observable, Observer } from 'rxjs';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  uploadUrl = environment.apiBase + "/upload";
  profileForm!: FormGroup;
  previewVisible = false;
  avatarUrl: string | undefined = window.localStorage.getItem('userinfo')? window.localStorage.getItem('userinfo')["avatar"]: null;
  dateFormat = 'yyyy-MM-dd';
  gender = "male"


  submitForm(): void {
    for (const i in this.profileForm.controls) {
      this.profileForm.controls[i].markAsDirty();
      this.profileForm.controls[i].updateValueAndValidity();
    }
  }

  // 时间选择
  dateMode: NzDateMode = 'time';

  handleDateOpenChange(open: boolean): void {
    if (open) {
      this.dateMode = 'time';
    }
  }

  beforeUpload = (file: NzUploadFile, _fileList: NzUploadFile[]) => {
    return new Observable((observer: Observer<boolean>) => {
      if (!(file.type === 'image/jpg' || file.type === 'image/jpeg')) {
        this.msg.error('You can only upload JPG file!');
        observer.complete();
        return;
      }
      const isLt2M = file.size! / 1024 / 1024 < 1;
      if (!isLt2M) {
        this.msg.error('Image must smaller than 1MB!');
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
          this.avatarUrl = file.response["filePath"];
      } else if (event.type == 'removed') {
        this.avatarUrl = "";
      }
    }
  }

  handlePreview = async () => {
    this.previewVisible = true;
  };

  constructor(
    private fb: FormBuilder,
    private msg: NzMessageService,
    ) {}

  ngOnInit(): void {
    this.profileForm = this.fb.group({
      nickName: [null, [Validators.required]],
      birthDay: [null],
      city: [null],
      gender: [null],
    });
  }




  values: string[] | null = null;

  onChanges(values: string[]): void {
    console.log(values);
  }

  /** load data async execute by `nzLoadData` method */
  loadData(node: NzCascaderOption, index: number): PromiseLike<void> {
    return new Promise(resolve => {
      setTimeout(() => {
        if (index < 0) {
          // if index less than 0 it is root node
          node.children = provinces;
        } else if (index === 0) {
          node.children = cities[node.value];
        }
        resolve();
      }, 1000);
    });
  }
}

const provinces = [
  {
    value: 'zhejiang',
    label: 'Zhejiang'
  },
  {
    value: 'jiangsu',
    label: 'Jiangsu'
  }
];

const cities: { [key: string]: Array<{ value: string; label: string; isLeaf?: boolean }> } = {
  zhejiang: [
    {
      value: 'hangzhou',
      label: 'Hangzhou',
      isLeaf: true
    },
    {
      value: 'ningbo',
      label: 'Ningbo',
      isLeaf: true
    }
  ],
  jiangsu: [
    {
      value: 'nanjing',
      label: 'Nanjing',
      isLeaf: true
    }
  ]
}
