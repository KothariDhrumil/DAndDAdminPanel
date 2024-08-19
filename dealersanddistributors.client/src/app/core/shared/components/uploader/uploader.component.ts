import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Upload } from '../../../models/uploads/upload';
import { UploadType } from '../../../models/uploads/upload-type';

@Component({
  selector: 'app-uploader',
  templateUrl: './uploader.component.html',
  styleUrls: ['./uploader.component.scss']
})
export class UploaderComponent implements OnInit {
  @Input() url: any;
  @Output() onLoadFile = new EventEmitter<Upload>();

  upload = new Upload();

  constructor() { }

  ngOnInit(): void {
  }

  onSelectFile(event: any) {
    if (event.target.files && event.target.files[0]) {
      var reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]); // read file as data url

      this.upload.fileName = event.target.files[0].name.split('.').shift()
      this.upload.extension = event.target.files[0].name.split('.').pop();
      this.upload.uploadType = UploadType.Product;

      reader.onloadend = (event) => { // called once readAsDataURL is completed
        if (event.target) {
          this.url = event.target.result;
          this.upload.data = event.target.result as string | ArrayBuffer;
        }
      }

      this.onLoadFile.emit(this.upload);
    }
  }

}
