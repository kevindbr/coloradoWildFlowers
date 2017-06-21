import { Injectable } from '@angular/core';
import flowers from '../assets/data/flowers.json';
import zones from '../assets/data/zones.json';
import origins from '../assets/data/origins.json';

/*
  Generated class for the DataService provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class DataService {

  private flowers: any;
  private zones: any;
  private origins: any;

  constructor() {
    this.flowers = flowers;
    this.zones = zones;
    this.origins = origins;
  }

  getFlowers() {
    return this.flowers;
  }

  getZones() {
    return this.zones;
  }

  getOrigins() {
    return this.origins;
  }

  searchFlowers(criteria: any) {

    return this.flowers.filter(val => {
      if (criteria.name && criteria.name.length > 0) {
        if (!val.scientificName.toLowerCase().includes(criteria.name.toLowerCase()) && !val.commonName.toLowerCase().includes(criteria.name.toLowerCase()))
          return false;
      }

      if (criteria.scientificFamily && criteria.scientificFamily.length > 0) {
        if (!val.scientificFamily.toLowerCase().includes(criteria.scientificFamily.toLowerCase()))
          return false;
      }

      if (criteria.commonFamily && criteria.commonFamily.length > 0) {
        if (!val.commonFamily.toLowerCase().includes(criteria.commonFamily.toLowerCase()))
          return false;
      }

      if (criteria.zone) {
        if (val.zones.indexOf(criteria.zone.toLowerCase()) < 0)
          return false;
      }

      if (criteria.altitude) {
        if (criteria.altitude < val.minAltitude || criteria.altitude > val.maxAltitude)
          return false;
      }

      if (criteria.bloom) {
        if (criteria.bloom < val.minBloom || criteria.bloom > val.maxBloom)
          return false;
      }

      if (criteria.origin) {
        if (!val.origin.toLowerCase().includes(criteria.origin.toLowerCase()))
          return false;
      }

      if (criteria.color) {
        if (val.colors.indexOf(criteria.color) < 0)
          return false;
      }

      return val;
    });
  }
}
