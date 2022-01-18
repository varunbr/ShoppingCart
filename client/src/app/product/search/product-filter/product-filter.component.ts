import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CategoryProperty, ProductContext } from 'src/app/modal/product';

@Component({
  selector: 'app-product-filter',
  templateUrl: './product-filter.component.html',
  styleUrls: ['./product-filter.component.css'],
})
export class ProductFilterComponent implements OnInit {
  _context: ProductContext;
  brand: CategoryProperty;
  price: CategoryProperty;
  value: any = {};
  @Input() set context(value: ProductContext) {
    this._context = value;
    this.brand = value?.properties.find((e) => e.name === 'Brand');
    this.price = value?.properties.find((e) => e.name === 'Price');
    this.getFilterValue();
  }
  constructor(private router: Router) {}

  ngOnInit(): void {}

  apply() {
    let param = {
      q: this._context?.searchText,
    };
    for (const key in this.value) {
      if (this.value[key]) {
        param[key] = this.value[key].trim();
      }
    }
    this.router.navigate(['/search'], {
      queryParams: param,
    });
  }

  clear() {
    this.value = {};
  }

  getFilterValue() {
    this.value['Price'] = this._context?.price;
    this.value['Brand'] = this._context?.brand;
    this.value['Category'] = this._context?.category;
    let filters = this._context?.filters;
    for (const key in filters) {
      this.value[key] = filters[key];
    }
  }

  categoryList = [
    {
      name: 'Electronics',
      children: [
        'Mobiles',
        'Memory Cards',
        'Power Banks',
        'Headphones',
        'Laptops',
        'Camera',
        'Speakers',
      ],
    },
    {
      name: 'Home & Appliances',
      children: [
        'Television',
        'Washing Machine',
        'Air Conditioners',
        'Refrigerators',
      ],
    },
    {
      name: 'Kitchen & Appliances',
      children: [
        'Microwave Ovens',
        'Juicer Mixer Grinder',
        'Electric Cookers',
        'Dishwashers',
      ],
    },
  ];
}
