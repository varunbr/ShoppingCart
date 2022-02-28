import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Params } from '@angular/router';
import { TrackAgent, TrackAgentContext } from 'src/app/modal/agent';

@Component({
  selector: 'app-track-role',
  templateUrl: './track-roles.component.html',
  styleUrls: ['./track-roles.component.css'],
})
export class TrackRolesComponent implements OnInit {
  @Input() agents: TrackAgent[];
  @Input() context: TrackAgentContext = new TrackAgentContext();
  @Input() title: string;
  @Output() contextChange = new EventEmitter<TrackAgentContext>();
  @Output() removeRole = new EventEmitter<TrackAgent>();
  @Output() applyParams = new EventEmitter<Params>();
  @Output() addRole = new EventEmitter();

  roles = [
    { name: 'Any (Default)', value: null },
    { name: 'TrackAgent', value: 'TrackAgent' },
    { name: 'TrackAdmin', value: 'TrackAdmin' },
  ];
  step = 0;
  constructor() {}

  ngOnInit(): void {}

  clear() {
    this.context.location = '';
    this.context.userName = '';
    this.context.role = '';
  }

  apply() {
    this.applyParams.emit(this.getParams());
  }

  getParams() {
    let params = {};
    if (this.context.location) params['location'] = this.context.location;
    if (this.context.role) params['role'] = this.context.role;
    if (this.context.userName) params['userName'] = this.context.userName;
    return params;
  }

  removeFromRole(agent: TrackAgent) {
    this.removeRole.emit(agent);
  }

  pageChange(page: number) {
    let params = this.getParams();
    if (page > 1) params['pageNumber'] = page;
    this.applyParams.emit(params);
  }
}
