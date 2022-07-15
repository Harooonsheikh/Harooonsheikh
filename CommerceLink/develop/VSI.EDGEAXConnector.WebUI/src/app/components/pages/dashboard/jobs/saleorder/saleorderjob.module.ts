import { CLControlModule } from "./../../../../../controls/clcontrol.module";
import { DefaultComponent } from "./../../../default.component";
import { LayoutModule } from "./../../../../layouts/layout.module";
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { SaleOrderJobComponent } from "./saleorderjob.component";
import { AceEditorModule } from 'ng2-ace-editor';
const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "",
                component: SaleOrderJobComponent
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
        CLControlModule,
        AceEditorModule
    ],
    exports: [RouterModule],
    declarations: [SaleOrderJobComponent]

})
export class SaleOrderJobModule { }
