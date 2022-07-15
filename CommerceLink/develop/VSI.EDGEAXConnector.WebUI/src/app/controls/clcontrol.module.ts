import { WorkflowStatComponent } from "./stats/stats.component";
import { WorkflowProcessComponent } from "./process/process.component";
import { LayoutModule } from "./../components/layouts/layout.module";
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { XMLMapComponent } from "./xmlmap/xmlmap.component";
import { AceEditorModule } from 'ng2-ace-editor';
@NgModule({
    imports: [FormsModule, CommonModule, LayoutModule, AceEditorModule],
    exports: [WorkflowStatComponent, WorkflowProcessComponent, XMLMapComponent],
    declarations: [
        WorkflowStatComponent,
        WorkflowProcessComponent,
        XMLMapComponent

    ]
})
export class CLControlModule { }
