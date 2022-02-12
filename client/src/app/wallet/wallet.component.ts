import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatExpansionPanel } from '@angular/material/expansion';
import { BaseListComponent } from '../base/component';
import { Transaction, TransactionContext } from '../modal/transaction';
import { User } from '../modal/user';
import { AccountService } from '../services/account.service';
import { PayService } from '../services/pay.service';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-wallet',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.css'],
})
export class WalletComponent
  extends BaseListComponent<Transaction, TransactionContext>
  implements OnInit
{
  user: User;
  modal = {};
  constructor(
    private accountService: AccountService,
    private payService: PayService,
    utility: UtilityService
  ) {
    super(payService);
    utility.setTitle('Wallet');
    this.loadItems({});
    this.accountService.user$.subscribe((u) => {
      this.user = u;
    });
  }

  ngOnInit(): void {}

  onSubmit(form: NgForm) {
    this.payService.transferAmount(this.modal).subscribe((response) => {
      this.context = response.context;
      this.items = response.items;
      form.resetForm();
    });
  }

  pageChange(event) {
    let params = {};
    if (event !== 1) {
      params = { pageNumber: event };
    }
    this.loadItems(params);
  }
}
