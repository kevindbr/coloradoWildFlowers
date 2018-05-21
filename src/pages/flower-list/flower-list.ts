import {Component} from '@angular/core';
import {NavController, NavParams, LoadingController, ModalController, AlertController} from 'ionic-angular';
import {Storage} from '@ionic/storage';
import {DataService} from '../../providers/data-service';
import {SearchPage} from '../search/search';
import {SettingsPage} from '../settings/settings';
import {TabsPage} from '../tabs/tabs';

/*
 Generated class for the FlowerList page.

 See http://ionicframework.com/docs/v2/components/#navigation for more info on
 Ionic pages and navigation.
 */
@Component({
  selector: 'page-flower-list',
  templateUrl: 'flower-list.html'
})
export class FlowerListPage {

  private pageSize: number;
  private maxPages: number;
  private page: number;

  private viewMode: string;
  private displayMode: string;
  private data: any;
  private viewData: any;
  private filtered: boolean;
  private searchCriteria: any;

  private loading: any;

  constructor(private navCtrl: NavController, private navParams: NavParams, private storage: Storage,
              private loadingCtrl: LoadingController, private modalCtrl: ModalController,
              private alertCtrl: AlertController, private dataService: DataService) {

    this.pageSize = 30;
    this.maxPages = 0;
    this.page = 0;

    this.filtered = false;
    this.searchCriteria = null;

    this.initializeData();

    if (navParams.get('searchCriteria')) {
      this.searchCriteria = navParams.get('searchCriteria');
      this.search(this.searchCriteria);
    }

    storage.ready().then(() => {
      storage.get('viewMode').then(viewMode => {
        storage.get('displayMode').then(displayMode => {
          this.viewMode = !!viewMode ? viewMode : '0';
          this.displayMode = !!displayMode ? displayMode : '0';
          this.sortData();
        });
      });
    });
  }

  showLoading() {
    this.loading = this.loadingCtrl.create({
      content: 'Loading...'
    });
    this.loading.onDidDismiss(() => {
      this.loading = false;
    });

    this.loading.present();
  }

  hideLoading() {
    if (!!this.loading)
      this.loading.dismiss();
  }

  initializeData() {
    this.filtered = false;
    this.data = this.dataService.getFlowers();
    this.sortData();
  }

  loadPage(page: number) {
    if (this.maxPages === 0 || page > this.maxPages)
      return;
    let start = page * this.pageSize;
    let end = Math.min(start + this.pageSize, this.data.length);
    for (let i = start; i < end; ++i)
      this.viewData.push(this.data[i]);
  }

  loadMore(infiniteScroll) {
    this.page++;
    this.loadPage(this.page);
    infiniteScroll.complete();
  }

  getImageSrc(src: string) {
    return 'assets/img/flowers/' + src;
  }

  getDisplayCommonName(flower: any) {
    switch (this.displayMode) {
      case '0': return flower.commonName;
      case '1': return !!flower.altCommonName ? flower.altCommonName : flower.commonName;
    }
  }

  getDisplayScientificName(flower: any) {
    switch (this.displayMode) {
      case '0': return flower.scientificName;
      case '1': return !!flower.altScientificName ? flower.altScientificName : flower.scientificName;
    }
  }

  getPrimaryTitle(flower: any) {
    switch (this.viewMode) {
      case '0':
        return this.getDisplayScientificName(flower);
      case '1':
        return this.getDisplayCommonName(flower);
      case '2':
        return flower.scientificFamily;
      case '3':
        return flower.commonFamily;
    }
  }

  getSecondaryTitle(flower: any) {
    switch (this.viewMode) {
      case '0':
        return this.getDisplayCommonName(flower);
      case '1':
        return this.getDisplayScientificName(flower);
      case '2':
        return flower.commonFamily;
      case '3':
        return flower.scientificFamily;
    }
  }

  getPrimaryLabel(flower: any) {
    switch (this.viewMode) {
      case '0':
        return flower.scientificFamily;
      case '1':
        return flower.scientificFamily;
      case '2':
        return this.getDisplayScientificName(flower);
      case '3':
        return this.getDisplayScientificName(flower);
    }
  }

  getSecondaryLabel(flower: any) {
    switch (this.viewMode) {
      case '0':
        return flower.commonFamily;
      case '1':
        return flower.commonFamily;
      case '2':
        return this.getDisplayCommonName(flower);
      case '3':
        return this.getDisplayCommonName(flower);
    }
  }

  openSettings() {
    let settingsModal = this.modalCtrl.create(SettingsPage, {
      viewMode: this.viewMode,
      displayMode: this.displayMode
    }, {enableBackdropDismiss: false});
    settingsModal.onDidDismiss(data => {
      if (data.cancel)
        return;
      this.viewMode = data.viewMode;
      this.displayMode = data.displayMode;
      this.sortData();
    });
    settingsModal.present();
  }

  openSearch() {
    let searchModal = this.modalCtrl.create(SearchPage, this.searchCriteria, {
      enableBackdropDismiss: false
    });
    searchModal.onDidDismiss(data => {
      if (data.cancel)
        return;

      this.searchCriteria = data.criteria;
      this.search(data.criteria);
    });
    searchModal.present();
  }

  resetData() {
    this.searchCriteria = null;
    this.initializeData();
  }

  private search(criteria: any) {
    this.data = this.dataService.searchFlowers(criteria);
    this.sortData();
    this.filtered = true;
  }

  private sortData() {
    let key, altKey;

    this.showLoading();

    switch (this.viewMode) {
      case '0':
        switch (this.displayMode) {
          case '0':
            key = 'scientificName';
            break;
          case '1':
            key = 'altScientificName';
            altKey = 'scientificName';
            break;
        }
        break;
      case '1':
        switch (this.displayMode) {
          case '0':
            key = 'commonName';
            break;
          case '1':
            key = 'altCommonName';
            altKey = 'commonName';
            break;
        }
        break;
      case '2':
        key = 'scientificFamily';
        break;
      case '3':
        key = 'commonFamily';
        break;
    }

    let mapped = this.data.map((el, i) => {
      return {index: i, value: !!el[key] ? el[key] : el[altKey]};
    });

    mapped.sort((a, b) => {
      return a.value < b.value ? -1 : a.value > b.value ? 1 : 0;
    });

    let result = mapped.map(el => {
      return this.data[el.index];
    });

    this.viewData = [];
    this.data = result;

    this.page = 0;
    this.maxPages = Math.ceil(this.data.length > 0 ? this.data.length / this.pageSize : 0);
    this.loadPage(this.page);

    this.hideLoading();
  }

  goToView(flower: any) {
    this.navCtrl.push(TabsPage, flower);
  }
}
