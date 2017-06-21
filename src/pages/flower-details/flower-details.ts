import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';

/*
  Generated class for the FlowerDetails page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-flower-details',
  templateUrl: 'flower-details.html'
})
export class FlowerDetailsPage {

  private flower: any;

  constructor(public navCtrl: NavController, public navParams: NavParams) {
    this.flower = navParams.data;
  }

  getZones() {
    return this.flower.zones.filter(val => { return val; }).join(', ');
  }

  getAltitudeRange() {
    let minAltitude: number = this.flower.minAltitude;
    let maxAltitude: number = this.flower.maxAltitude;

    let altitudeRange: string = '-';
    if (minAltitude > -1)
      altitudeRange = minAltitude.toString();
    if (maxAltitude > -1)
      altitudeRange += ' - ' + maxAltitude.toString();

    if (altitudeRange !== '-')
      altitudeRange += ' ft.';

    return altitudeRange;
  }

  getBloomRange() {
    let startMonth: string = this.getMonth(this.flower.minBloom);
    let endMonth: string = this.getMonth(this.flower.maxBloom);

    let bloomRange: string = '-';
    if (startMonth)
      bloomRange = startMonth;
    if (endMonth)
      bloomRange += ' - ' + endMonth;

    return bloomRange;
  }

  private getMonth(val: number) {
    switch (val) {
      case 0:
        return 'January';
      case 1:
        return 'February';
      case 2:
        return 'March';
      case 3:
        return 'April';
      case 4:
        return 'May';
      case 5:
        return 'June';
      case 6:
        return 'July';
      case 7:
        return 'August';
      case 8:
        return 'September';
      case 9:
        return 'October';
      case 10:
        return 'November';
      case 11:
        return 'December';
      default:
        return null;
    }
  }

}
