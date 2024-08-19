import { Component, OnInit } from '@angular/core';
import { MultilingualService } from '../../../core/service/multilingual.service';


@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {
  constructor(private translationService : MultilingualService) {

  }
  selected = 'en';
  ngOnInit(): void {
    this.selected = this.translationService.currentLanguage();
  }
  changeLanguage()
  {
    this.translationService.changeLanguage(this.selected);
  }
}
