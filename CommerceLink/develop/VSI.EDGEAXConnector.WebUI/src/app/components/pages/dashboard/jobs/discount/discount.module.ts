import { DefaultComponent } from "./../../../default.component";
import { CLControlModule } from "./../../../../../controls/clcontrol.module";
import { LayoutModule } from "./../../../../layouts/layout.module";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { DiscountComponent } from "./discount.component";
import { DiscountTableComponent } from "./discounttable/discounttable.component";

const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "",
                component: DiscountComponent
            }
        ]
    }
];

@NgModule({
    imports: [
        FormsModule,
        CommonModule,
        RouterModule.forChild(routes),
        LayoutModule,
        CLControlModule
    ],
    exports: [RouterModule],
    declarations: [DiscountComponent, DiscountTableComponent]
})
export class DiscountRoutingModule { }

export const routedComponents = [DiscountComponent];
