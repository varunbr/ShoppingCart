import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseListComponent } from 'src/app/base/component';
import { TrackContext, TrackOrder } from 'src/app/modal/orderTracking';
import { ToastrService } from 'src/app/services/toastr.service';
import { TrackService } from 'src/app/services/track.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-track-list',
  templateUrl: './track-list.component.html',
  styleUrls: ['./track-list.component.css'],
})
export class TrackListComponent
  extends BaseListComponent<TrackOrder, TrackContext>
  implements OnInit
{
  orderByValues = [
    { name: 'Latest (Default)', value: null },
    { name: 'Oldest', value: 'Oldest' },
  ];
  step = 0;
  locations: { name: string; type: string }[] = [];

  constructor(
    private trackService: TrackService,
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
    utility: UtilityService
  ) {
    super(trackService);
    utility.setTitle('Tracking Orders');
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.loadItems(params, false);
      this.step = 0;
    });
  }

  clear() {
    this.context.location = '';
    this.context.orderBy = '';
    this.context.status = '';
  }

  getParams() {
    let params = {};
    if (this.context.location) params['location'] = this.context.location;
    if (this.context.orderBy) params['orderBy'] = this.context.orderBy;
    if (this.context.status) params['status'] = this.context.status;
    return params;
  }

  apply() {
    this.router.navigate(['/track'], {
      queryParams: this.getParams(),
    });
  }

  pageChange(page: number) {
    let params = this.getParams();
    if (page > 1) params['pageNumber'] = page;
    this.router.navigate(['/track'], {
      queryParams: params,
    });
  }

  receiveOrder(index: number) {
    this.trackService
      .receiveOrder(
        this.items[index].id,
        this.items[index].currentEvent?.locationId
      )
      .subscribe((response) => {
        response.status = response.status + '*';
        response.currentEvent.status = response.currentEvent.status + '*';
        this.items[index] = response;
        this.toastrService.success('Order received');
      });
  }

  departOrder(index: number) {
    this.trackService
      .departOrder(
        this.items[index].id,
        this.items[index].currentEvent?.locationId
      )
      .subscribe((response) => {
        response.status = response.status + '*';
        response.currentEvent.status = response.currentEvent.status + '*';
        this.items[index] = response;
        this.toastrService.success('Order departed');
      });
  }

  startDelivery(index: number) {
    this.trackService
      .startDelivery(
        this.items[index].id,
        this.items[index].currentEvent?.locationId
      )
      .subscribe((response) => {
        response.status = response.status + '*';
        response.currentEvent.status = response.currentEvent.status + '*';
        this.items[index] = response;
        this.toastrService.success('Order moved for delivery');
      });
  }
}
