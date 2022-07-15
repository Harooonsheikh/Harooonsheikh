import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Routes, RouterModule } from "@angular/router";
import { LayoutModule } from "../../../layouts/layout.module";
import { DefaultComponent } from "../../default.component";
import { CLControlModule } from "../../../../controls/clcontrol.module";
import { FormsModule } from "@angular/forms";
import { MappingComponent } from "./mapping.component";
import { MapHomeComponent } from "./home/home.component";
import { EditMapComponent } from "./edit/edit.component";
import { FileUploadModule } from "ng2-file-upload";
import { AceEditorModule } from 'ng2-ace-editor';
const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "home",
                component: MapHomeComponent
            },
            {
                path: "edit",
                component: EditMapComponent
            },
            {
                path: "",
                component: MappingComponent
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
        FileUploadModule,
        AceEditorModule
    ],
    exports: [RouterModule],
    declarations: [
        MappingComponent,
        MapHomeComponent,
        EditMapComponent
    ]
})
export class MappingModule { }
