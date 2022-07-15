import { Component, OnInit, ViewEncapsulation, AfterViewInit, ViewChild } from '@angular/core';
import { XMLMap } from '../../Entities/XMLMap';
import { KeyValue } from '../../Entities/Common';
import { ScriptLoaderService } from '../../_services/script-loader.service';
import { MappingService } from '../../_services/mapping.service';
import { Input } from '@angular/core';
import { AfterViewChecked } from '@angular/core/src/metadata/lifecycle_hooks';
import { Router, ActivatedRoute, Params } from '@angular/router';
import * as vkbeautify from 'vkbeautify';
declare var jQuery: any;
declare var $: any;
declare var toastr: any;

@Component({
    selector: "xml-map",
    templateUrl: "xmlmap.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class XMLMapComponent implements OnInit, AfterViewInit {
    @ViewChild('editor') editor;
    @Input() public mapType: string;
    @Input() public map: XMLMap;
    public entities: Array<string> = null;
    public properties: Array<KeyValue<string>> = null;
    public selectedEntity: string = "";
    public generateXML: boolean = true;
    public actions: Array<KeyValue<string>> = null;
    public templates: Array<KeyValue<string>> = null;
    public selectedTypeTemplates: Array<KeyValue<string>> = null;
    public showError: boolean = false;
    constructor(private _script: ScriptLoaderService, private _mapSer: MappingService, private route: ActivatedRoute, private router: Router) {

    }

    ngOnInit() {

        if (this.map == null) {
            this.map = new XMLMap();
            this.map.Type = this.mapType;
            if (this.map.Type == "READ") {
                this.map.XML = "<Targets></Targets>";
            }
            $("#xmlTextArea").val(this.map.XML);
        }
        this.selectedEntity = this.map.SourceEntity;
        $("#xmlTextArea").val(this.map.XML);
        this.loadProperties(this.selectedEntity);
        this.map.Name = this.selectedEntity;

        this.map.XML = vkbeautify.xml(this.map.XML);

        this.editor.nativeElement.env.editor.setShowPrintMargin(false);
        this.editor.nativeElement.env.editor.getSession().setValue(this.map.XML);

        this.editor.nativeElement.env.editor.resize();
        this.entities = new Array<any>();
        this.properties = new Array<KeyValue<string>>();
        this.actions = new Array<KeyValue<string>>();
        this.templates = Array<KeyValue<string>>();
        this.selectedTypeTemplates = new Array<KeyValue<string>>();
        this.editor.nativeElement.env.editor.focus(); //To focus the ace editor
        var n = this.editor.nativeElement.env.editor.getSession().getValue().split("\n").length; // To count total no. of lines
        this.editor.nativeElement.env.editor.gotoLine(n); //Go to end of document
        this.editor.nativeElement.env.editor.setOption("wrap", true);
        var session = this.editor.nativeElement.env.editor.getSession();

        session.on("changeAnnotation", function() {
            var annotations = session.getAnnotations() || [], i = annotations.length;
            var len = annotations.length;
            while (i--) {
                if (/doctype first\. Expected/.test(annotations[i].text)) {
                    annotations.splice(i, 1);
                }
            }
            if (len > annotations.length) {
                session.setAnnotations(annotations);
            }
        });
    }
    ngAfterViewInit() {
        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": true,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

        this.map.Type = this.mapType;
        this._script.load('.m-grid__item.m-grid__item--fluid.m-wrapper',
            'assets/app/js/dashboard.js');
        this.loadActions();
        this.loadTemplates();

        this._mapSer.getSourceEntities().subscribe(m => {
            m.forEach((val: string, index: number) => {
                let entity: string = val.split(",")[0];

                if (entity.startsWith("VSI.EDGEAXConnector.ERPDataModels.Custom.")) {
                    this.entities.push(entity.substring(41, entity.length));
                } else if (entity.startsWith("VSI.EDGEAXConnector.ERPDataModels.CRTClasses.")) {
                    this.entities.push(entity.substring(45, entity.length));
                } else if (entity == "VSI.EDGEAXConnector.ERPDataModels.ClassInfo+<>c") {
                    this.entities.push("<>c");
                } else if (entity.startsWith("VSI.EDGEAXConnector.ERPDataModels.")) {
                    this.entities.push(entity.substring(34, entity.length));
                } else {
                    this.entities.push(entity);
                }
            });

            if (this.selectedEntity == "") {
                this.selectedEntity = this.entities[0];
            }
            this.loadProperties(this.selectedEntity);
        });
    }

    Cancel() {
        this.router.navigate(["/mapping/home"]);
    }

    loadProperties(model: any) {
        this.properties = new Array<KeyValue<string>>();
        this.selectedEntity = model;
        this.map.SourceEntity = this.selectedEntity;
        this.map.Type = this.mapType;
        this.map.Name = this.map.Type + "." + model;
        this._mapSer.getSourceProperties(this.selectedEntity).subscribe(m => {
            m.forEach((val: any, index: number) => {
                let prop = new KeyValue<string>();
                prop.Key = val.Key.Name;
                prop.Value = val.Value;
                this.properties.push(prop);
            });
        });
    }

    insertProperty(prop: string) {
        // insertAtCaret("xmlTextArea", this.selectedEntity + "~" + prop);
        this.editor.nativeElement.env.editor.insert(this.selectedEntity + "~" + prop);

    }

    loadActions() {
        if (this.map.Type == "CREATE") {
            this.generateXML = true;
        }
        else {
            this.generateXML = false;
        }
        this._mapSer.getActions(this.generateXML).subscribe(m => {
            this.actions = m;
        });
    }

    insertActions(actionName: string) {
        this.editor.nativeElement.env.editor.insert(this.actions.filter(m => m.Key == actionName)[0].Value);
        // insertAtCaret("xmlTextArea", this.actions.filter(m => m.Key == actionName)[0].Value);
    }

    loadTemplates() {
        this._mapSer.getTemplatesXML().subscribe(m => {
            this.templates = m;
            this.selectedTypeTemplates = new Array<KeyValue<string>>();
            this.templates.forEach(m => {
                if (m.Key.startsWith(this.map.Type + ".")) {
                    this.selectedTypeTemplates.push(m);
                    // file-source=\"" + selectedTemplate.Name + "\" data-source=\"\""
                }
            });
        });
    }

    insertTemplate(tempName: string) {
        insertAtCaret("xmlTextArea", this.selectedTypeTemplates.filter(m => m.Key == tempName)[0].Value);
    }

    onSubmit(isFormValid: boolean) {
        if (isFormValid) {
            this.map.XML = this.editor.nativeElement.env.editor.getValue();
            this.map.isActive = true;
            this._mapSer.Save(this.map).subscribe(m => {

                if (m == "ADDED") {
                    toastr.success("XML Map added successfully.");
                } else if (m == "UPDATED") {
                    toastr.success("XML Map updated successfully.")
                }
            }, e => this.handleError(e));
        }
        else {
            this.showError = true;
            return false;
        }
    }

    Save() {
        this.map.XML = this.editor.nativeElement.env.editor.getValue();
        this.map.isActive = true;
        this._mapSer.Save(this.map).subscribe(m => {
            if (m == "ADDED") {
                toastr.success("XML Map added successfully.");
            } else if (m == "UPDATED") {
                toastr.success("XML Map updated successfully.")
            }
        }, e => this.handleError(e));
    }

    handleError(err: any) {
        if (err.status == 400) {
            toastr.error("Invalid XML Format");
        }
        else {
            toastr.error("Unable to Perform Required Action.");
        }

    }

    ShowModal() {
        $("#readInstructions").modal('show');
    }
}
function insertAtCaret(areaId, text) {
    var txtarea: any = document.getElementById(areaId);
    if (!txtarea) { return; }

    var scrollPos = txtarea.scrollTop;
    var strPos = 0;
    var selection: any = document.getSelection();
    var br = ((txtarea.selectionStart || txtarea.selectionStart == '0') ?
        "ff" : (selection ? "ie" : false));
    if (br == "ie") {
        txtarea.focus();
        var range = selection.createRange();
        range.moveStart('character', -txtarea.value.length);
        strPos = range.text.length;
    } else if (br == "ff") {
        strPos = txtarea.selectionStart;
    }

    var front = (txtarea.value).substring(0, strPos);
    var back = (txtarea.value).substring(strPos, txtarea.value.length);
    txtarea.value = front + text + back;
    strPos = strPos + text.length;
    if (br == "ie") {
        txtarea.focus();
        var ieRange = selection.createRange();
        ieRange.moveStart('character', -txtarea.value.length);
        ieRange.moveStart('character', strPos);
        ieRange.moveEnd('character', 0);
        ieRange.select();
    } else if (br == "ff") {
        txtarea.selectionStart = strPos;
        txtarea.selectionEnd = strPos;
        txtarea.focus();
    }
    txtarea.scrollTop = scrollPos;
}
