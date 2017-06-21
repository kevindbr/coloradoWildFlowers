import {Component} from '@angular/core';
import {NavController, Platform, AlertController} from 'ionic-angular';
import {SocialSharing} from 'ionic-native';
import {IntroductionPage} from '../introduction/introduction';
import {FlowerListPage} from '../flower-list/flower-list';
import {ZonesPage} from '../zones/zones';
import {ReferencesPage} from '../references/references';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {

  private emailAvailable: boolean;
  private emailTemplate: any;

  constructor(private navCtrl: NavController, private platform: Platform, private alertCtrl: AlertController) {
    this.emailAvailable = false;
    this.emailTemplate = {
      to: 'info@easterncoloradowildflowers.com',
      subject: 'Colorado Wildflowers App',
      isHtml: true
    };

    this.platform.ready().then(() => {

      SocialSharing.canShareViaEmail().then(() => {
        this.emailAvailable = true;
      }).catch(() => {
        this.emailAvailable = false;
      });
    });
  }

  goToIntroduction() {
    this.navCtrl.push(IntroductionPage);
  }

  goToFlowerList() {
    this.navCtrl.push(FlowerListPage);
  }

  goToZones() {
    this.navCtrl.push(ZonesPage);
  }

  goToReferences() {
    this.navCtrl.push(ReferencesPage);
  }

  openEmail() {

    if (!this.emailAvailable) {
      let emailAlert = this.alertCtrl.create({
        title: 'Contact',
        subTitle: 'Please send an email to ' + this.emailTemplate.to,
        buttons: ['Ok']
      });
      return emailAlert.present();
    }

    SocialSharing.shareViaEmail('<body></body>', this.emailTemplate.subject, this.emailTemplate.to).then(() => {
      // Success!
    }).catch(() => {
      // Error!
    });
  }

}
