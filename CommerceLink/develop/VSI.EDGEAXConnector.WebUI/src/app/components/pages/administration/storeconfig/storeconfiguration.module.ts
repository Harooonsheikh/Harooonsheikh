import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Routes, RouterModule } from "@angular/router";
import { LayoutModule } from "../../../layouts/layout.module";
import { DefaultComponent } from "../../default.component";
import { CLControlModule } from "../../../../controls/clcontrol.module";
import { FormsModule } from "@angular/forms";
import { } from "./";
import { StoreConfigurationComponent } from "./stores/storeconfiguration.component";
import { StoreConfigurationTableComponent } from "./stores/storeconfigurationtable/storeconfigurationtable.component";
import { ActionComponent } from "./actions/action.component";
import { AceEditorModule } from "ng2-ace-editor";

const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "stores",
                component: StoreConfigurationComponent
            },
            {
                path: "actions",
                component: ActionComponent
            }
        ]
    }
];
@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        LayoutModule,
        FormsModule,
        CLControlModule,
        AceEditorModule
    ],
    exports: [RouterModule],
    declarations: [
        StoreConfigurationComponent,
        StoreConfigurationTableComponent,
        ActionComponent
    ]
})
export class StoreConfigurationModule { }
