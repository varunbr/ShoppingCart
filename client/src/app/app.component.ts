import { Component, OnInit } from '@angular/core';
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
    public busy: BusyService
  ) {}

  ngOnInit() {
    this.getTheme();
    this.setCurrentUser();
  }

  toggleTheme(isDark): void {
    this.isDark = isDark;
    this.setTheme(isDark ? 'dark-theme' : 'light-theme');
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
    if (value === 'dark-theme') {
      this.isDark = true;
    } else {
      this.isDark = false;
    }
  }

  setTheme(theme: string) {
    localStorage.setItem('theme', theme);
  }
}
