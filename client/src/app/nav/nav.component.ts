import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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

  constructor(
    public accountService: AccountService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.queryParamMap.subscribe((params) => {
      this.value = params.get('q');
    });
  }

  toggleTheme(): void {
    this.isDark = !this.isDark;
    this.changeTheme.emit(this.isDark);
  }

  logout() {
    this.accountService.logout();
  }

  search() {
    if (this.isValid()) {
      this.router.navigate(['/search'], {
        queryParams: { q: this.value.trim() },
      });
    }
  }

  isValid() {
    return this.value && this.value.trim();
  }
}
