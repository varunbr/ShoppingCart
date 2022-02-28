import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { BaseAgent, BaseAgentContext } from 'src/app/modal/agent';
import { AdminService } from 'src/app/services/admin.service';
import { ToastrService } from 'src/app/services/toastr.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-admin-roles',
  templateUrl: './admin-roles.component.html',
  styleUrls: ['./admin-roles.component.css'],
})
export class AdminRolesComponent implements OnInit {
  context: BaseAgentContext;
  agents: BaseAgent[];
  roles = [
    { name: 'Any (Default)', value: null },
    { name: 'Admin', value: 'Admin' },
    { name: 'TrackModerator', value: 'TrackModerator' },
    { name: 'StoreModerator', value: 'StoreModerator' },
  ];
  step = 0;

  constructor(
    private adminService: AdminService,
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
    utility: UtilityService
  ) {
    utility.setTitle('Admin Roles');
    this.route.queryParams.subscribe((params) => {
      this.getRoles(params);
    });
  }

  ngOnInit(): void {}

  getRoles(params: Params) {
    this.adminService.getModeratorsForAdmin(params).subscribe((response) => {
      this.context = response.context;
      this.agents = response.items;
      this.step = 0;
    });
  }

  removeRole(agent: BaseAgent) {
    this.adminService
      .removeModeratorByAdmin({
        role: agent.role,
        userId: agent.userId,
      })
      .subscribe(() => {
        this.agents.splice(this.agents.indexOf(agent), 1);
        this.toastrService.success(
          `${agent.name} is removed from role ${agent.role}`
        );
      });
  }

  clear() {
    this.context.userName = '';
    this.context.role = '';
  }

  apply() {
    this.router.navigate(['/admin/moderate/admin-role'], {
      queryParams: this.getParams(),
    });
  }

  getParams() {
    let params = {};
    if (this.context.role) params['role'] = this.context.role;
    if (this.context.userName) params['userName'] = this.context.userName;
    return params;
  }

  pageChange(page: number) {
    let params = this.getParams();
    if (page > 1) params['pageNumber'] = page;
    this.router.navigate(['/admin/moderate/admin-role'], {
      queryParams: params,
    });
  }
}
