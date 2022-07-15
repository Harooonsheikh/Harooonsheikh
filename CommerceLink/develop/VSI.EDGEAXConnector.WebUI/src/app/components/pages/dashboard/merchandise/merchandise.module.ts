import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Routes, RouterModule } from "@angular/router";
import { LayoutModule } from "../../../layouts/layout.module";
import { DefaultComponent } from "../../default.component";
import { MerchandiseWidgetComponent } from "./widgets/widgets.component";
import { MerchandiseTableComponent } from "./workflowtable/workflowtable.component";
import { FormsModule } from "@angular/forms";
import { MerchandiseComponent } from "./merchandise.component";

const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "",
                component: MerchandiseComponent
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
        MerchandiseComponent,
        MerchandiseWidgetComponent,
        MerchandiseTableComponent
    ]
})
export class MerchandiseModule { }
