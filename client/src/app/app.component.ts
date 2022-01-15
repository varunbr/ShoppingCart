import { Component, OnInit, Renderer2 } from '@angular/core';
import { User } from './modal/user';
import { AccountService } from './services/account.service';
import { BusyService } from './services/busy.service';

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
    public busy: BusyService,
    private _renderer: Renderer2
  ) {}

  ngOnInit() {
    this.getTheme();
    this.setCurrentUser();
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
}
