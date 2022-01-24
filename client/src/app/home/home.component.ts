import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { HomePage } from '../modal/product';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  homePage: HomePage;
  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.productService.getHomePage().subscribe((response) => {
      this.homePage = response;
    });
  }
}
