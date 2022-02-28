import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './authentication/login/login.component';
import { ProductDetailComponent } from './product/product-detail/product-detail.component';
import { SearchComponent } from './product/search/search.component';
import { RegisterComponent } from './authentication/register/register.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { CartComponent } from './cart/cart.component';
import { ProductCheckoutComponent } from './order/product-checkout/product-checkout.component';
import { OrderPaymentComponent } from './order/order-payment/order-payment.component';
import { OrderDetailComponent } from './order/order-detail/order-detail.component';
import { OrderListComponent } from './order/order-list/order-list.component';
import { WalletComponent } from './wallet/wallet.component';
import { AboutComponent } from './about/about.component';
import { TrackListComponent } from './track/track-list/track-list.component';
import { StoreOrderListComponent } from './store/store-order-list/store-order-list.component';
import { StoreOrderComponent } from './store/store-order/store-order.component';
import { TrackOrderComponent } from './track/track-order/track-order.component';
import { AddAdminRoleComponent } from './roles/admin/add-admin-role/add-admin-role.component';
import { AdminRolesComponent } from './roles/admin/admin-roles/admin-roles.component';
import { StoreAdminAddComponent } from './roles/store/store-admin-add/store-admin-add.component';
import { StoreAdminComponent } from './roles/store/store-admin/store-admin.component';
import { StoreModeratorAddComponent } from './roles/store/store-moderator-add/store-moderator-add.component';
import { StoreModeratorComponent } from './roles/store/store-moderator/store-moderator.component';
import { TrackAdminAddComponent } from './roles/track/track-admin-add/track-admin-add.component';
import { TrackAdminComponent } from './roles/track/track-admin/track-admin.component';
import { TrackModeratorAddComponent } from './roles/track/track-moderator-add/track-moderator-add.component';
import { TrackModeratorComponent } from './roles/track/track-moderator/track-moderator.component';
import { AuthGuard } from './components/guards/auth.guard';
import { PageNotFoundComponent } from './error/page-not-found/page-not-found.component';
import { ServerErrorComponent } from './error/server-error/server-error.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'about', component: AboutComponent },
  { path: 'search', component: SearchComponent },
  { path: 'product/:id', component: ProductDetailComponent },
  { path: 'edit-profile', component: EditProfileComponent },
  { path: 'order/:id', component: OrderDetailComponent },
  { path: 'order', component: OrderListComponent },
  { path: 'cart', component: CartComponent },
  { path: 'checkout', component: ProductCheckoutComponent },
  { path: 'order-payment', component: OrderPaymentComponent },
  { path: 'wallet', component: WalletComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    data: { roles: ['StoreAgent', 'StoreAdmin'] },
    children: [
      { path: 'store/order/:id', component: StoreOrderComponent },
      { path: 'store/order', component: StoreOrderListComponent },
    ],
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    data: { roles: ['TrackAgent', 'TrackAdmin'] },
    children: [
      { path: 'track/:id', component: TrackOrderComponent },
      { path: 'track', component: TrackListComponent },
    ],
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    data: { roles: ['StoreAdmin'] },
    children: [
      { path: 'admin/store-role/add', component: StoreAdminAddComponent },
      { path: 'admin/store-role', component: StoreAdminComponent },
    ],
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    data: { roles: ['TrackAdmin'] },
    children: [
      { path: 'admin/track-role/add', component: TrackAdminAddComponent },
      { path: 'admin/track-role', component: TrackAdminComponent },
    ],
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    data: { roles: ['Admin', 'StoreModerator'] },
    children: [
      {
        path: 'admin/moderate/store-role/add',
        component: StoreModeratorAddComponent,
      },
      { path: 'admin/moderate/store-role', component: StoreModeratorComponent },
    ],
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    data: { roles: ['Admin', 'TrackModerator'] },
    children: [
      {
        path: 'admin/moderate/track-role/add',
        component: TrackModeratorAddComponent,
      },
      {
        path: 'admin/moderate/track-role',
        component: TrackModeratorComponent,
      },
    ],
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    data: { roles: ['Admin'] },
    children: [
      {
        path: 'admin/moderate/admin-role/add',
        component: AddAdminRoleComponent,
      },
      { path: 'admin/moderate/admin-role', component: AdminRolesComponent },
    ],
  },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
