import { DiscountRoutingModule } from "./discount/discount.module";
import { DiscountComponent } from "./discount/discount.component";
import { PriceComponent } from "./price/price.component";
import { PriceModule } from "./price/price.module";
import { InventoryModule } from "./inventory/inventory.module";
import { InventoryComponent } from "./inventory/inventory.component";
import { CLControlModule } from "./../../../../controls/clcontrol.module";
import { CatalogModule } from "./catalog/catalog.module";
import { CatalogComponent } from "./catalog/catalog.component";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Routes, RouterModule } from "@angular/router";
import { JobsComponent } from "./jobs.component";
import { LayoutModule } from "../../../layouts/layout.module";
import { DefaultComponent } from "../../default.component";
import { SaleOrderJobComponent } from "./saleorder/saleorderjob.component";
import { SaleOrderJobModule } from "./saleorder/saleorderjob.module";

const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "index",
                component: JobsComponent
            },
            {
                path: "catalog",
                component: CatalogComponent
            },
            {
                path: "inventory",
                component: InventoryComponent
            },
            {
                path: "price",
                component: PriceComponent
            },
            {
                path: "discount",
                component: DiscountComponent
            },
            {
                path: "saleorder",
                component: SaleOrderJobComponent
            }
        ]
    }
];
@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        LayoutModule,
        CatalogModule,
        CLControlModule,
        InventoryModule,
        PriceModule,
        DiscountRoutingModule,
        SaleOrderJobModule
    ],
    exports: [RouterModule],
    declarations: [JobsComponent]
})
export class JobsModule { }
