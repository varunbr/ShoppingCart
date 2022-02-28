import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css'],
})
export class ServerErrorComponent implements OnInit {
  message: string;
  details: string;

  constructor(
    private route: Router,
    utility: UtilityService,
    public accountService: AccountService
  ) {
    const state = this.route.getCurrentNavigation()?.extras?.state;
    if (state) {
      this.message = state?.error?.message;
      this.details = state?.error?.details;
    }
    utility.setTitle('Internal Server Error');
  }

  ngOnInit(): void {}
}
