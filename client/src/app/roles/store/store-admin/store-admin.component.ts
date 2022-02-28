import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { StoreAgent, StoreAgentContext } from 'src/app/modal/agent';
import { AdminService } from 'src/app/services/admin.service';
import { ToastrService } from 'src/app/services/toastr.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-store-admin',
  templateUrl: './store-admin.component.html',
  styleUrls: ['./store-admin.component.css'],
})
export class StoreAdminComponent implements OnInit {
  context: StoreAgentContext;
  agents: StoreAgent[];

  constructor(
    private adminService: AdminService,
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
    utility: UtilityService
  ) {
    utility.setTitle('Roles - Store Admin');

    this.route.queryParams.subscribe((params) => {
      this.getRoles(params);
    });
  }

  ngOnInit(): void {}

  getRoles(params: Params) {
    this.adminService
      .getStoreAgentsForStoreAdmin(params)
      .subscribe((response) => {
        this.context = response.context;
        this.agents = response.items;
      });
  }

  removeRole(agent: StoreAgent) {
    this.adminService
      .removeRoleByStoreAdmin({
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
    this.router.navigate(['/admin/store-role'], {
      queryParams: params,
    });
  }

  navigateToAddRole() {
    this.router.navigate(['/admin/store-role/add']);
  }
}
