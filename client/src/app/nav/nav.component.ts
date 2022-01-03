import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  value: string;
  @Input() isDark = false;
  @Output() changeTheme = new EventEmitter<boolean>();

  constructor(public accountService: AccountService) {}

  ngOnInit(): void {}

  toggleTheme(): void {
    this.isDark = !this.isDark;
    this.changeTheme.emit(this.isDark);
  }

  logout() {
    this.accountService.logout();
  }
}
