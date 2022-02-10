import {
  Component,
  EventEmitter,
  HostListener,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../modal/user';
import { AccountService } from '../services/account.service';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  value: string;
  @Input() isDark = false;
  @Output() changeTheme = new EventEmitter<boolean>();
  cartCount: number;
  user: User;

  constructor(
    public accountService: AccountService,
    private router: Router,
    private route: ActivatedRoute,
    private cartService: CartService
  ) {
    this.accountService.user$.subscribe((u) => (this.user = u));
  }

  ngOnInit(): void {
    this.route.queryParamMap.subscribe((params) => {
      this.value = params.get('q');
    });
    this.cartService.cartStore$.subscribe((response) => {
      let count = 0;
      for (let item of response) {
        count += item.cartItems.length;
      }
      this.cartCount = count;
    });
  }

  toggleTheme(): void {
    this.isDark = !this.isDark;
    this.changeTheme.emit(this.isDark);
  }

  logout() {
    this.router.navigate(['/'], { queryParams: { logout: true } });
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

  toScroll: boolean;
  topPosToStartShowing = 200;
  @HostListener('window:scroll')
  checkScroll() {
    const scrollPosition =
      window.pageYOffset ||
      document.documentElement.scrollTop ||
      document.body.scrollTop ||
      0;

    if (scrollPosition >= this.topPosToStartShowing) {
      this.toScroll = true;
    } else {
      this.toScroll = false;
    }
  }

  goToTop() {
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'smooth',
    });
  }
}
