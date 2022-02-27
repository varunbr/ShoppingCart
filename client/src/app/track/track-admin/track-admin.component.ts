import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { TrackAgent, TrackAgentContext } from 'src/app/modal/agent';
import { AdminService } from 'src/app/services/admin.service';
import { ToastrService } from 'src/app/services/toastr.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-track-admin',
  templateUrl: './track-admin.component.html',
  styleUrls: ['./track-admin.component.css'],
})
export class TrackAdminComponent implements OnInit {
  context: TrackAgentContext;
  agents: TrackAgent[];

  constructor(
    private adminService: AdminService,
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
    utility: UtilityService
  ) {
    utility.setTitle('Roles - Track Admin');

    this.route.queryParams.subscribe((params) => {
      this.getRoles(params);
    });
  }

  ngOnInit(): void {}

  getRoles(params: Params) {
    this.adminService
      .getTrackAgentsForTrackAdmin(params)
      .subscribe((response) => {
        this.context = response.context;
        this.agents = response.items;
      });
  }

  removeRole(agent: TrackAgent) {
    this.adminService
      .removeRoleByTrackAdmin({
        locationId: agent.locationId,
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

  applyParams(params: Params) {
    this.router.navigate(['/admin/track-role'], {
      queryParams: params,
    });
  }

  navigateToAddRole() {
    this.router.navigate(['/admin/track-role/add']);
  }
}
