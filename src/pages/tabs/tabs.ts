import {Component} from '@angular/core';
import {NavParams} from 'ionic-angular';
import {FlowerImagesPage} from '../flower-images/flower-images';
import {FlowerDetailsPage} from '../flower-details/flower-details';

@Component({
  templateUrl: 'tabs.html'
})
export class TabsPage {
  private flower: any;

  // this tells the tabs component which Pages
  // should be each tab's root Page
  tab1Root: any = FlowerImagesPage;
  tab2Root: any = FlowerDetailsPage;

  constructor(private navParams: NavParams) {
    this.flower = navParams.data;
  }
}
