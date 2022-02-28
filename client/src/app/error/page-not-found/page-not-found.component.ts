import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-page-not-found',
  templateUrl: './page-not-found.component.html',
  styleUrls: ['./page-not-found.component.css'],
})
export class PageNotFoundComponent implements OnInit {
  constructor(utility: UtilityService, public accountService: AccountService) {
    utility.setTitle('Page not found');
  }

  ngOnInit(): void {}
}
