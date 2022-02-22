import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TrackOrder } from 'src/app/modal/orderTracking';
import { ToastrService } from 'src/app/services/toastr.service';
import { TrackService } from 'src/app/services/track.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-track-order',
  templateUrl: './track-order.component.html',
  styleUrls: ['./track-order.component.css'],
})
export class TrackOrderComponent implements OnInit {
  order: TrackOrder;

  constructor(
    private route: ActivatedRoute,
    private trackService: TrackService,
    private toastrService: ToastrService,
    public utility: UtilityService
  ) {
    utility.setTitle('Track Order');
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.getOrder(params['id']);
    });
  }

  getOrder(id: number) {
    this.trackService.getModal(id).subscribe((response) => {
      this.order = response;
    });
  }

  receiveOrder() {
    this.trackService
      .receiveOrder(this.order.id, this.order.currentEvent?.locationId)
      .subscribe((response) => {
        response.status = response.status + '*';
        response.currentEvent.status = response.currentEvent.status + '*';
        this.order = response;
        this.toastrService.success('Order received');
      });
  }

  departOrder() {
    this.trackService
      .departOrder(this.order.id, this.order.currentEvent?.locationId)
      .subscribe((response) => {
        response.status = response.status + '*';
        response.currentEvent.status = response.currentEvent.status + '*';
        this.order = response;
        this.toastrService.success('Order departed');
      });
  }

  startDelivery() {
    this.trackService
      .startDelivery(this.order.id, this.order.currentEvent?.locationId)
      .subscribe((response) => {
        response.status = response.status + '*';
        response.currentEvent.status = response.currentEvent.status + '*';
        this.order = response;
        this.toastrService.success('Order moved for delivery');
      });
  }
}
