import {Component} from '@angular/core';
import {NavController, NavParams, ModalController} from 'ionic-angular';
import {DataService} from '../../providers/data-service';
import {ZoneDetailsPage} from '../zone-details/zone-details';


@Component({
  selector: 'page-zones',
  templateUrl: 'zones.html'
})
export class ZonesPage {

  private zones: any;

  constructor(public navCtrl: NavController, public navParams: NavParams, private modalCtrl: ModalController,
              private dataService: DataService) {
    this.zones = dataService.getZones();
  }

  openZoneDetails(zone: any) {
    let zoneModal = this.modalCtrl.create(ZoneDetailsPage, zone);
    zoneModal.present();
  }
}
