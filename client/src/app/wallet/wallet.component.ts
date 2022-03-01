import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { BaseListComponent } from '../base/component';
import { Transaction, TransactionContext } from '../modal/transaction';
import { User, UserInfo } from '../modal/user';
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
  userName: string;
  user: User;
  modal = {};
  userInfo: UserInfo;
  constructor(
    private accountService: AccountService,
    private payService: PayService,
    utility: UtilityService
  ) {
    super(payService);
    utility.setTitle('Wallet');
    this.accountService.user$.subscribe((u) => {
      this.user = u;
    });
  }

  userChange(user: UserInfo) {
    this.userInfo = user;
    this.modal['userName'] = user?.userName;
  }

  ngOnInit(): void {
    this.loadItems({});
  }

  onSubmit(form: NgForm) {
    this.payService.transferAmount(this.modal).subscribe((response) => {
      this.context = response.context;
      this.items = response.items;
      form.resetForm();
      this.userInfo = null;
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
