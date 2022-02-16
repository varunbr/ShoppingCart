import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UtilityService } from 'src/app/services/utility.service';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  modal: any = {};
  redirectUrl: string;
  constructor(
    private accountService: AccountService,
    private router: Router,
    private route: ActivatedRoute,
    utility: UtilityService
  ) {
    utility.setTitle('Login');
  }

  ngOnInit(): void {
    this.redirectUrl = this.route.snapshot.queryParams.redirectTo;
    if (this.accountService.loggedIn) {
      this.redirect();
    }
  }

  onSubmit() {
    this.accountService.login(this.modal).subscribe(() => {
      location.reload();
    });
  }

  loginAsTester() {
    this.accountService
      .login({ username: 'test_user', password: 'notpassword' })
      .subscribe(() => {
        location.reload();
      });
  }

  redirect() {
    this.router.navigateByUrl(this.redirectUrl ? this.redirectUrl : '/', {
      replaceUrl: true,
    });
  }
}
