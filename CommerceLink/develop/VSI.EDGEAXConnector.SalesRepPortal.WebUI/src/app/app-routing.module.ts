import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AuthGuard } from './_shared';
import { SaleOrderTransactionComponent } from './components/sale-order/sale-order-transaction/sale-order-transaction.component';
import { AccessDeniedComponent } from './_shared/errors/access-denied/access-denied.component';
import { NotFoundComponent } from './_shared/errors/not-found/not-found.component';
import { ServerErrorComponent } from './_shared/errors/server-error/server-error.component';
//import { AuthenticationGuard } from 'microsoft-adal-angular6';
import { QuotationComponent } from './components/quotation/quotation.component';
const routes: Routes = [
    // {
    //     path: '',
    //     component: SaleOrderTransactionComponent,
    //     // canActivate: [AuthenticationGuard]
    // },
    // {
    //     path: 'customer/quote/:id/:salesRap/:qouteId',
    //     component: QuotationComponent,
    // },
    {
        path: 'customer/:id/quote/:qouteId/:salesRap/:erpId', 
        component: QuotationComponent,
    },
    {
        path: 'customer/:id/quote/:qouteId/:salesRap', 
        component: QuotationComponent,
    },
    {
        path: 'customer/:id/quote/:qouteId/opportunity/:oppurtunityId/:salesRap', 
        component: QuotationComponent,
    },
    {
        path: 'customer/:id/quote/:qouteId/opportunity/:oppurtunityId/:salesRap/:erpId', 
        component: QuotationComponent,
    },
    // {
    //     path: 'customer/contract',
    //     component: SaleOrderTransactionComponent
    // },
    // {
    //     path: 'customer/contract/:id//:salesRap/',
    //     component: SaleOrderTransactionComponent
    // },
    // {
    //     path: 'customer/contract/:id/:erpId/:salesRap',
    //     component: SaleOrderTransactionComponent
    // },
    {
        path: 'customer/:id/contract/:contractId/:salesRap/:erpId', 
        component: SaleOrderTransactionComponent
    },
    {
        path: 'customer/:id/contract/:contractId/:salesRap', 
        component: SaleOrderTransactionComponent
    },
    {
        path: 'error',
        component: ServerErrorComponent
    },
    {
        path: 'access-denied',
        component: AccessDeniedComponent
    },
    {
        path: 'not-found',
        component: NotFoundComponent
    },
    { path: '**', redirectTo: 'not-found' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
