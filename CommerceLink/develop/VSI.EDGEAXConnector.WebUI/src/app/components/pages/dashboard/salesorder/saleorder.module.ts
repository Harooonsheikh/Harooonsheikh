import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Routes, RouterModule } from "@angular/router";
import { LayoutModule } from "../../../layouts/layout.module";
import { DefaultComponent } from "../../default.component";
import { SalesOrderWidgetComponent } from "./widgets/widgets.component";
import { SaleOrderWorkflowTableComponent } from "./workflowtable/workflowtable.component";
import { FormsModule } from "@angular/forms";
import { SalesOrderComponent } from "./saleorder.component";

const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "",
                component: SalesOrderComponent
            }
        ]
    }
];
@NgModule({
    imports: [
        FormsModule,
        CommonModule,
        RouterModule.forChild(routes),
        LayoutModule
    ],
    exports: [RouterModule],
    declarations: [
        SaleOrderWorkflowTableComponent,
        SalesOrderWidgetComponent,
        SalesOrderComponent
    ]
})
export class SaleOrderModule { }
