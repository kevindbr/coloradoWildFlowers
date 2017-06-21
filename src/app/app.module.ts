import {NgModule, ErrorHandler, enableProdMode} from '@angular/core';
import {IonicApp, IonicModule, IonicErrorHandler} from 'ionic-angular';
import {IonicStorageModule} from '@ionic/storage';
import {MyApp} from './app.component';
import {DataService} from '../providers/data-service';
import {AboutPage} from '../pages/about/about';
import {ContactPage} from '../pages/contact/contact';
import {HomePage} from '../pages/home/home';
import {IntroductionPage} from '../pages/introduction/introduction';
import {ReferencesPage} from '../pages/references/references';
import {ZonesPage} from '../pages/zones/zones';
import {ZoneDetailsPage} from '../pages/zone-details/zone-details';
import {TabsPage} from '../pages/tabs/tabs';
import {FlowerListPage} from '../pages/flower-list/flower-list';
import {FlowerImagesPage} from '../pages/flower-images/flower-images';
import {FlowerDetailsPage} from '../pages/flower-details/flower-details';
import {SearchPage} from '../pages/search/search';
import {SettingsPage} from '../pages/settings/settings';

enableProdMode();

@NgModule({
  declarations: [
    MyApp,
    AboutPage,
    ContactPage,
    HomePage,
    IntroductionPage,
    ReferencesPage,
    ZonesPage,
    ZoneDetailsPage,
    TabsPage,
    FlowerListPage,
    FlowerImagesPage,
    FlowerDetailsPage,
    SearchPage,
    SettingsPage
  ],
  imports: [
    IonicModule.forRoot(MyApp),
    IonicStorageModule.forRoot()
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    AboutPage,
    ContactPage,
    HomePage,
    IntroductionPage,
    ReferencesPage,
    ZonesPage,
    ZoneDetailsPage,
    TabsPage,
    FlowerListPage,
    FlowerImagesPage,
    FlowerDetailsPage,
    SearchPage,
    SettingsPage
  ],
  providers: [
    {provide: ErrorHandler, useClass: IonicErrorHandler},
    DataService
  ]
})
export class AppModule {
}
