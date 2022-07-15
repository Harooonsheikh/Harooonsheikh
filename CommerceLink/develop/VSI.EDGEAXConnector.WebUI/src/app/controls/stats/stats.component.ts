import { KeyValue } from './../../Entities/Common';
import { Subject } from 'rxjs/Subject';
import { ViewEncapsulation } from '@angular/core';
import { WorkFlowService } from './../../_services/workflow.service';
import { Observable } from 'rxjs/Observable';
import { WorkFlow, CatalogStats } from './../../Entities/WorkFlow';
import { Input } from '@angular/core';
import { AfterViewInit } from '@angular/core';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'workflow-stat',
    templateUrl: 'stats.component.html',
    encapsulation: ViewEncapsulation.None
})

export class WorkflowStatComponent implements OnInit, AfterViewInit {
    @Input() public stats: Subject<Object>;
    @Input() public statEvent: Subject<string>;

    public catStats: Object = new Object();
    public properties: Array<KeyValue<string>> = new Array<KeyValue<string>>();
    constructor(private _wfService: WorkFlowService) { }

    ngOnInit() {
        this.stats.subscribe((m: any) => this.setupTable(m));
    }

    private setupTable(m: Array<Object>): void {
        if (m != null) {
            m.forEach((element: any) => {
                let ele: KeyValue<string> = new KeyValue<string>();
                ele.Key = element.Key;
                ele.Value = element.Value;
                this.properties.push(ele);
            });
            this.catStats = m;
        }
        else {
            this.properties = new Array<KeyValue<string>>();
        }
    }

    ngAfterViewInit() {

    }

    public changeEvent(m: string): void {
        this.statEvent.next(m);
    }

    private handleError(err: Response): void {
        console.error(err);
    }
}