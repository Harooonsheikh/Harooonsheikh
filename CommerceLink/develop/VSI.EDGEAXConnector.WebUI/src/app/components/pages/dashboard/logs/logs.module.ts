import { FormsModule } from "@angular/forms";
import { WorkflowErrorLogComponent } from "./error/error.component";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Routes, RouterModule } from "@angular/router";
import { LayoutModule } from "../../../layouts/layout.module";
import { DefaultComponent } from "../../default.component";

const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "error",
                component: WorkflowErrorLogComponent
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
    declarations: [WorkflowErrorLogComponent]
})
export class LogModule { }
