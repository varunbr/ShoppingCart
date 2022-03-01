import { Component, OnInit } from '@angular/core';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css'],
})
export class AboutComponent implements OnInit {
  step = 0;

  contents: {
    title: string;
    description: string;
    icon: string;
    items: string[];
  }[];

  constructor(utility: UtilityService) {
    utility.setTitle('About');
  }

  ngOnInit(): void {
    this.setContent();
  }

  setContent() {
    this.contents = [
      {
        title: 'Home',
        description:
          'Home page lets user to quickly navigate to popular products.',
        icon: 'home',
        items: [
          'The first row on the home page lets users quickly select product categories.',
          'The next rows include products of Mobiles, Televisions, Laptops, Refrigerators and Washing Machines Categories.',
          'Each of these categories lists popular products of that category.',
          'This page will be cached in-memory for later usage.',
        ],
      },
      {
        title: 'Product Search and Filter',
        description: 'Search for product and apply filters.',
        icon: 'search',
        items: [
          'The search box in the navbar will allow users to search from any page.',
          'The category will be selected automatically based on the search keyword. If not detected automatically, products from all the categories will be searched.',
          'Users can also select categories from the filter section.',
          'If the category is selected, filters specific to that category will be shown dynamically.',
          'Users can select the range for integer values like RAM, Storage, Price, etc., select multiple values like Brand, Color, etc.',
        ],
      },
      {
        title: 'Product Information',
        description: 'Product features, specifications, variants and photos.',
        icon: 'category',
        items: [
          'Users can view photos of products in a gallery.',
          'Users can view Features, Descriptions and Specifications specific to each product.',
          'Users can quickly choose different variants of products like Color, Storage, etc.',
          'Users can proceed to buy or add to the cart for future purchases.',
        ],
      },
      {
        title: 'Cart',
        description: 'Add products to cart for future purchases.',
        icon: 'shopping_cart',
        items: [
          'Products added to carts are grouped by purchase store which can be purchased together and reduce the delivery price.',
          'Users can remove any or all products from the cart.',
          'Users can checkout single or all grouped items for purchase.',
        ],
      },
      {
        title: 'Order',
        description: 'Order products using ShoppingCart Wallet.',
        icon: 'shopping_bag',
        items: [
          'Users can order single or multiple items of same-store together.',
          'Users can view order details which include Item info, Number of units, Order status, Total Price Delivery charges and Total Amount paid.',
          'Users can view the paginated order list in the descending order of date.',
          'Delivery charge will be ₹60 for interstate and ₹40 for intrastate.',
          'Delivery charge will be applied only if Total Price is less than ₹500.',
        ],
      },
      {
        title: 'Wallet, Transactions and Transfer',
        description:
          'Transfer amount and View Wallet balance and Transactions.',
        icon: 'account_balance_wallet',
        items: [
          'All the registered users will have an initial wallet balance of ₹1,00,000.',
          'Users can view the paginated transaction list in the descending order of date.',
          'Users can view transaction date, description, amount, type of transaction, etc.',
          'Users can transfer amount to other users using their username.',
        ],
      },
      {
        title: 'User Authentication',
        description: 'Login or Register for authentication.',
        icon: 'account_circle',
        items: [
          'Users need to authenticate to perform a few actions like add to cart, order, edit profile, etc.',
          'Users can log in using a username and password or register as new users to authenticate.',
          'Authentication will expire in 24 hours after login.',
        ],
      },
      {
        title: 'Role Administration',
        description: 'Add, Remove and View Roles.',
        icon: 'security',
        items: [
          'Roles in ShoppingCart App: User, Store Agent, Track Agent, Store Admin, Track Admin,Store Moderator, Track Moderator &  Admin.',
          'User: All loged in users will be in this role.',
          'Store Agent: Each Store can have many Store Agents, Store Agents can Dispatch orders from stores.',
          'Store Admin: Each Store can have many Store Admins, Store Admins can Dispatch orders from stores & View, Add & Remove Store Agent or Store Admin roles of that store.',
          'Track Agent: Each Location can have many Track Agents, Store Agents can Receive or Dispatch order from that location',
          'Track Admin: Each Location can have many Track Admins, Store Admins can Receive or Dispatch order, also View, Add & Remove Track Agent or Track Admin roles of that location.',
          'Store Moderator: Store Moderator can View, Add & Remove Store Agent & Store Admin roles of any store.',
          'Track Moderator: Track Moderator can View, Add & Remove Track Agent & Track Admin roles of any location.',
          'Admin: Admin can View, Add & Remove Store Moderator & Track Moderator',
        ],
      },
      {
        title: 'Themes and Responsive UI',
        description: 'Switch between Light and Dark theme and Responsive UI.',
        icon: 'dark_mode',
        items: [
          'Users can swtich between Light and Dark theme using theme button in navbar.',
          'Users can use this responsive Web app across any Mobile, Tablet, Laptop, or Desktop.',
        ],
      },
    ];
  }
}
