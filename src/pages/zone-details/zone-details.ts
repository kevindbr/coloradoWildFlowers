import {Component} from '@angular/core';
import {NavController, NavParams, ViewController} from 'ionic-angular';
import {FlowerListPage} from '../flower-list/flower-list';

@Component({
  selector: 'page-zone-details',
  templateUrl: 'zone-details.html'
})
export class ZoneDetailsPage {

  private zone: any;

  constructor(public navCtrl: NavController, public navParams: NavParams, private viewCtrl: ViewController) {
    this.zone = navParams.data;
  }

  close() {
    this.viewCtrl.dismiss();
  }

  goToFlowerList() {
    this.navCtrl.push(FlowerListPage, {searchCriteria: {zone: this.zone.name}});
  }

}
