import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LocalStorageService } from '../shared/services';

@Injectable({
  providedIn: 'root',
})
export class LanguageService {
  public languages: string[] = ['en', 'es', 'de'];

  constructor(public translate: TranslateService, private storageSevice: LocalStorageService) {
    let browserLang: string = 'en';
    translate.addLangs(this.languages);

    const storedLang = this.storageSevice.get('lang');
    if (typeof storedLang === 'string' && storedLang) {
      browserLang = storedLang;
    }
    translate.use(this.languages.includes(browserLang) ? browserLang : 'en');
  }

  public setLanguage(lang: string) {
    this.translate.use(lang);
    this.storageSevice.set('lang', lang);
  }
}
