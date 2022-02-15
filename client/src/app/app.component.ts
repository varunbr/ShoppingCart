import { Component, OnInit, Renderer2 } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { User } from './modal/user';
import { AccountService } from './services/account.service';
import { BusyService } from './services/busy.service';
import { UtilityService } from './services/utility.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'ShoppingCart';
  isDark = false;
  constructor(
    private accountSerice: AccountService,
    private route: ActivatedRoute,
    private utility: UtilityService,
    public busy: BusyService,
    private _renderer: Renderer2
  ) {
    this.addIcons();
  }

  ngOnInit() {
    this.getTheme();
    this.setCurrentUser();
    this.route.queryParams.subscribe((params) => {
      this.handelLogout(params);
    });
  }

  toggleTheme(isDark): void {
    this.applyTheme(isDark);
    this.setTheme(isDark ? 'dark-theme' : 'light-theme');
  }

  applyTheme(isDark: boolean) {
    this.isDark = isDark;
    if (isDark === true) {
      this._renderer.addClass(document.body, 'dark-theme');
      this._renderer.removeClass(document.body, 'light-theme');
    } else {
      this._renderer.addClass(document.body, 'light-theme');
      this._renderer.removeClass(document.body, 'dark-theme');
    }
  }

  setCurrentUser() {
    const value = localStorage.getItem('user');
    if (value) {
      const user: User = JSON.parse(value);
      this.accountSerice.setUser(user);
    }
  }

  getTheme() {
    const value = localStorage.getItem('theme');
    this.applyTheme(value === 'dark-theme');
  }

  setTheme(theme: string) {
    localStorage.setItem('theme', theme);
  }

  handelLogout(params: Params) {
    if (params.logout) {
      this.accountSerice.logout();
      window.location.replace(
        this.utility.getUrl(location.pathname, {
          redirectTo: params.redirectTo,
        })
      );
    }
  }

  addIcons() {
    var iconList = [
      {
        name: 'github',
        path: './assets/icons/github.svg',
      },
      {
        name: 'linkedin',
        path: './assets/icons/linkedin.svg',
      },
    ];

    for (let item of iconList) {
      this.utility.addCustomIcon(item.name, item.path);
    }
  }
}
