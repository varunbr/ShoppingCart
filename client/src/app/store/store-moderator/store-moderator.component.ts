import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { StoreAgent, StoreAgentContext } from 'src/app/modal/agent';
import { AdminService } from 'src/app/services/admin.service';
import { ToastrService } from 'src/app/services/toastr.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-store-moderator',
  templateUrl: './store-moderator.component.html',
  styleUrls: ['./store-moderator.component.css'],
})
export class StoreModeratorComponent implements OnInit {
  context: StoreAgentContext;
  agents: StoreAgent[];

  constructor(
    private adminService: AdminService,
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
    utility: UtilityService
  ) {
    utility.setTitle('Moderate - Store Roles');

    this.route.queryParams.subscribe((params) => {
      this.getRoles(params);
    });
  }

  ngOnInit(): void {}

  getRoles(params: Params) {
    this.adminService
      .getStoreAgentsForModerator(params)
      .subscribe((response) => {
        this.context = response.context;
        this.agents = response.items;
      });
  }

  removeRole(agent: StoreAgent) {
    this.adminService
      .removeStoreRoleByModerator({
        role: agent.role,
        storeId: agent.storeId,
        userId: agent.userId,
      })
      .subscribe(() => {
        this.agents.splice(this.agents.indexOf(agent), 1);
        this.toastrService.success(
          `${agent.name} is removed from role ${agent.role}`
        );
      });
  }

  applyParams(params: Params) {
    this.router.navigate(['/admin/moderate/store-role'], {
      queryParams: params,
    });
  }

  navigateToAddRole() {
    this.router.navigate(['/admin/moderate/store-role/add']);
  }
}
