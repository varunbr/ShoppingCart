import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Params } from '@angular/router';
import { StoreAgent, StoreAgentContext } from 'src/app/modal/agent';

@Component({
  selector: 'app-store-role',
  templateUrl: './store-role.component.html',
  styleUrls: ['./store-role.component.css'],
})
export class StoreRoleComponent implements OnInit {
  @Input() agents: StoreAgent[];
  @Input() context: StoreAgentContext = new StoreAgentContext();
  @Input() title: string;
  @Output() contextChange = new EventEmitter<StoreAgentContext>();
  @Output() removeRole = new EventEmitter<StoreAgent>();
  @Output() applyParams = new EventEmitter<Params>();
  @Output() addRole = new EventEmitter();
  roles = [
    { name: 'Any (Default)', value: null },
    { name: 'StoreAgent', value: 'StoreAgent' },
    { name: 'StoreAdmin', value: 'StoreAdmin' },
  ];
  step = 0;
  constructor() {}

  ngOnInit(): void {}

  clear() {
    this.context.storeName = '';
    this.context.userName = '';
    this.context.role = '';
  }

  apply() {
    this.applyParams.emit(this.getParams());
  }

  getParams() {
    let params = {};
    if (this.context.storeName) params['storeName'] = this.context.storeName;
    if (this.context.role) params['role'] = this.context.role;
    if (this.context.userName) params['userName'] = this.context.userName;
    return params;
  }

  removeFromRole(agent: StoreAgent) {
    this.removeRole.emit(agent);
  }

  pageChange(page: number) {
    let params = this.getParams();
    if (page > 1) params['pageNumber'] = page;
    this.applyParams.emit(params);
  }
}
