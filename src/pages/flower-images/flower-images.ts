import {Component} from '@angular/core';
import {NavController, NavParams} from 'ionic-angular';

/*
 Generated class for the FlowerImages page.

 See http://ionicframework.com/docs/v2/components/#navigation for more info on
 Ionic pages and navigation.
 */
@Component({
  selector: 'page-flower-images',
  templateUrl: 'flower-images.html'
})
export class FlowerImagesPage {

  private flower: any;
  private options: any;

  constructor(public navCtrl: NavController, public navParams: NavParams) {
    this.flower = navParams.data;
    this.options = {
      pager: true,
      loop: true
    };
  }

  getImageSrc(src: string) {
    return 'assets/img/flowers/' + src;
  }

}
