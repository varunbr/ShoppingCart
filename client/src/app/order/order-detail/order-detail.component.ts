import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrderDetail } from 'src/app/modal/order';
import { OrderService } from 'src/app/services/order.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.css'],
})
export class OrderDetailComponent implements OnInit {
  order: OrderDetail;

  constructor(
    private route: ActivatedRoute,
    public utility: UtilityService,
    private orderService: OrderService
  ) {
    utility.setTitle('Order');
    this.route.params.subscribe((params) => {
      this.getOrder(params['id']);
    });
  }

  ngOnInit(): void {}

  getTotal() {
    let total = 0;
    for (let item of this.order.orderItems) {
      total += item.price * item.count;
    }
    return total;
  }

  getOrder(id: number) {
    this.orderService.getOrder(id).subscribe((response) => {
      if (new Date(response.delivery).getFullYear() < 2000) {
        response.delivery = null;
      }
      this.order = response;
    });
  }
}
