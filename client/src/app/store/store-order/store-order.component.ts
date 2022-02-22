import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrderItem } from 'src/app/modal/order';
import { StoreOrder } from 'src/app/modal/store';
import { StoreService } from 'src/app/services/store.service';
import { ToastrService } from 'src/app/services/toastr.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-store-order',
  templateUrl: './store-order.component.html',
  styleUrls: ['./store-order.component.css'],
})
export class StoreOrderComponent implements OnInit {
  order: StoreOrder;
  constructor(
    private route: ActivatedRoute,
    private storeService: StoreService,
    private toastrService: ToastrService,
    public utility: UtilityService
  ) {
    utility.setTitle('Store Order');
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.getOrder(params['id']);
    });
  }

  getOrder(id: number) {
    this.storeService.getModal(id).subscribe((response) => {
      this.order = response;
    });
  }

  getTotal() {
    let total = 0;
    for (let item of this.order.orderItems) {
      total += item.price * item.count;
    }
    return total;
  }

  dispatchOrder() {
    this.storeService.dispatchOrder(this.order.id).subscribe((response) => {
      this.order = response;
      this.toastrService.success('Order dispatched');
    });
  }
}
