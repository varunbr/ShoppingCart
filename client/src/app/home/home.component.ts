import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { HomePage } from '../modal/product';
import { ProductService } from '../services/product.service';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  homePage: HomePage;
  constructor(private productService: ProductService, utility: UtilityService) {
    utility.setTitle('Home');
  }

  ngOnInit(): void {
    this.productService.getHomePage().subscribe((response) => {
      this.homePage = response;
    });
  }
}
