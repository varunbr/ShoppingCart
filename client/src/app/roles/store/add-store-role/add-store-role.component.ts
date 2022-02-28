import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { finalize } from 'rxjs';
import { StoreAgent } from 'src/app/modal/agent';
import { StoreRole } from 'src/app/modal/role';
import { StoreInfo } from 'src/app/modal/store';
import { UserInfo } from 'src/app/modal/user';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-add-store-role',
  templateUrl: './add-store-role.component.html',
  styleUrls: ['./add-store-role.component.css'],
})
export class AddStoreRoleComponent implements OnInit {
  @Output() addRole = new EventEmitter<StoreRole>();
  @Input() for: string;
  private _agent: StoreAgent;
  @Input()
  public get agent(): StoreAgent {
    return this._agent;
  }
  public set agent(value: StoreAgent) {
    this.initilize();
    this._agent = value;
  }
  userName = '';
  user: UserInfo;
  storeRole = new StoreRole();
  roles = ['StoreAgent', 'StoreAdmin'];
  loading = false;
  store: string;
  stores: StoreInfo[] = [];

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {}

  initilize() {
    this.userName = '';
    this.store = '';
    this.stores = [];
    this.storeRole = new StoreRole();
  }

  searchStores() {
    this.stores = [];
    if (this.store?.length >= 3) {
      this.loading = true;
      this.adminService
        .searchStores(this.store, this.for)
        .pipe(
          finalize(() => {
            this.loading = false;
          })
        )
        .subscribe((response) => {
          this.stores = response;
        });
    }
  }

  userChange(user: UserInfo) {
    this.user = user;
    this.storeRole.userId = user?.id;
  }

  onSubmit() {
    this.addRole.emit(this.storeRole);
  }
}
