import {Component} from '@angular/core';
import {NavController, NavParams, ViewController} from 'ionic-angular';
import {Storage} from '@ionic/storage';


@Component({
  selector: 'page-settings',
  templateUrl: 'settings.html'
})
export class SettingsPage {

  private viewMode: string;
  private displayMode: string;

  constructor(public navCtrl: NavController, public navParams: NavParams, public viewCtrl: ViewController,
              private storage: Storage) {
    this.viewMode = navParams.get('viewMode');
    this.displayMode = navParams.get('displayMode');
  }

  ionViewDidLoad() {
    //console.log('ionViewDidLoad SettingsPage');
  }

  public updateOptions() {
    this.storage.ready().then(() => {
      this.storage.set('viewMode', this.viewMode).then(() => {
        this.storage.set('displayMode', this.displayMode).then(() => {
          this.viewCtrl.dismiss({viewMode: this.viewMode, displayMode: this.displayMode});
        });
      });
    });
  }

  public cancel() {
    this.viewCtrl.dismiss({cancel: true});
  }

}
