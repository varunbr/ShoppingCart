import { LocationStrategy } from '@angular/common';
import { Injectable } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer, Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class UtilityService {
  baseUrl: string;
  constructor(
    locationStrategy: LocationStrategy,
    private router: Router,
    private titleService: Title,
    private matIconRegistry: MatIconRegistry,
    private domSanitizer: DomSanitizer
  ) {
    this.baseUrl = location.origin + locationStrategy.getBaseHref();
  }

  getProductUrl(id: number) {
    return this.getRelativePath(`product/${id}`);
  }

  getUrl(fullPath: string, queryParams = {}) {
    let path = this.router.serializeUrl(
      this.router.createUrlTree([fullPath], { queryParams: queryParams })
    );
    return location.origin + path;
  }

  setTitle(title: string) {
    this.titleService.setTitle(title + ' - ShoppingCart');
  }

  private getRelativePath(appPath: string, queryParams = {}) {
    let path = this.router.serializeUrl(
      this.router.createUrlTree([appPath], { queryParams: queryParams })
    );
    return `${this.baseUrl.replace(/\/+$/, '')}${path}`;
  }

  addCustomIcon(name: string, svgPath: string) {
    this.matIconRegistry.addSvgIcon(
      name,
      this.domSanitizer.bypassSecurityTrustResourceUrl(svgPath)
    );
  }
}
