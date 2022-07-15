import { KeyValue, CatalogModel } from "./../../../../../../Entities/Common";
import { Observable } from "rxjs";
import { WorkFlow } from "./../../../../../../Entities/WorkFlow";
import { WorkFlowService } from "./../../../../../../_services/workflow.service";
import { CatalogService } from "./../../../../../../_services/catalog.service";
import { Component, OnInit, AfterViewInit, Input } from "@angular/core";
declare var jquery: any;
declare var $: any;

export class CatAssign {
    public ProdId: string = "";
    public CatId: string = "";
    public primaryFlag: KeyValue<string> = null;
}

@Component({
    selector: "cat-assign",
    templateUrl: "catassign.component.html"
})
export class CatalogCatAssignComponent implements OnInit, AfterViewInit {
    @Input() public inputFile: string = "";
    public totalCatAssign: number = null;
    private catAssignOb: Observable<Object> = null;
    public catAssign: Array<CatAssign> = null;
    public dataTable: any = null;
    public searchQuery: string = "";
    public searchResults: Array<CatAssign> = null;

    constructor(
        private _cService: CatalogService,
        private _wfService: WorkFlowService
    ) { }

    ngOnInit() { }
    ngAfterViewInit() {
        this._cService.getCatalogModalCount(this.inputFile, CatalogModel.CategoryAssignment).subscribe(
            (number: number) => {
                this.totalCatAssign = number;
                this.setupProductTable();
            },
            (error: Response) => this.handleError(error)
        );
    }

    private setupProductTable(): void {

        this.catAssignOb = this._cService.getCatagoryAssignmentByCatalog(
            this.inputFile,
            0 * 10,
            10
        );
        this.catAssign = new Array<CatAssign>();
        for (var i = 0; i < this.totalCatAssign; i++) {
            var element = new CatAssign();
            this.catAssign.push(element);
        }

        this.dataTable = $("#catAssignLog").mDatatable({
            data: {
                type: "local",
                source: this.catAssign,
                pageSize: 10
            },
            layout: {
                theme: "default",
                class: "",
                scroll: !1,
                height: 450,
                footer: !1
            },
            pagination: true,
            filterable: !1,
            toolbar: {
                items: {
                    pagination: {
                        navigation: {
                            prev: true,
                            next: true,
                            first: true,
                            last: true
                        },
                        pages: {
                            desktop: {
                                layout: "default",
                                pagesNumber: 10
                            }
                        }
                    }
                }
            },
            columns: [
                {
                    field: "CatId",
                    title: "Category Id",
                    width: "150",
                    selector: !1,
                    textAlign: "center"
                },
                {
                    field: "ProdId",
                    title: "Product Id",
                    width: "150",
                    textAlign: "center"
                },
                {
                    field: "primaryFlag",
                    title: "Primary Flag",
                    width: "100",
                    textAlign: "center"
                }
            ]
        });

        this.initEvents();
    }

    private initEvents(): void {
        let pagi: any = this.dataTable.getDataSourceParam("pagination");
        pagi.page = 1;
        pagi.perpage = 10;
        this.dataTable.setDataSourceParam("pagination", pagi);

        var pages = this.dataTable.getCurrentPage();
        if (pages - 1 < 0) {
            pages = 0;
        } else {
            pages = pages - 1;
        }
        //var perPage = this.dataTable.getPageSize();
        var perPage = 10;
        //this._cService.getProductByCatalog(this.selectedCatalogFile, pages * perPage, perPage)
        this.catAssignOb.subscribe(
            (m: string) => {
                self.reloadTable.apply(self, [m, pages, perPage]);
            },
            (err: Response) => {
                self.handleError(err);
            }
        );

        let self: CatalogCatAssignComponent = this;
        // (function (component: any) {
        //     $("#productLog").on('click', 'tr > td:nth-child(8) a', function () {

        //         var data = $(this).closest("tr").find("td:nth-child(1) span").first().html();
        //         component.getProductDetail(data);
        //         return false;
        //     });
        // })(self);

        self.dataTable.on("m-datatable--on-goto-page", function(
            e: any,
            settings: any
        ) {
            var pages = settings.page;
            if (pages - 1 < 0) {
                pages = 0;
            } else {
                pages = pages - 1;
            }
            var perPage = settings.perpage;

            if (self.searchQuery == "") {
                self._cService
                    .getCatagoryAssignmentByCatalog(
                    self.inputFile,
                    pages * perPage,
                    perPage
                    )
                    .subscribe(
                    (m: string) => {
                        self.reloadTable.apply(self, [m, pages, perPage]);
                    },
                    (err: Response) => {
                        self.handleError(err);
                    }
                    );
            } else {
                self._cService
                    .getSearchResult(
                    self.inputFile,
                    self.searchQuery,
                    3,
                    pages * perPage,
                    perPage
                    )
                    .subscribe(
                    (m: string) => {
                        self.reloadTable.apply(self, [m, pages, perPage]);
                    },
                    (err: Response) => {
                        self.handleError(err);
                    }
                    );
            }
        });

        self.dataTable.on("m-datatable--on-update-perpage", function(
            e: any,
            settings: any
        ) {
            var pages = settings.page;
            if (pages - 1 < 0) {
                pages = 0;
            } else {
                pages = pages - 1;
            }
            var perPage = settings.perpage;


            if (self.searchQuery == "") {
                self._cService
                    .getCatagoryAssignmentByCatalog(
                    self.inputFile,
                    pages * perPage,
                    perPage
                    )
                    .subscribe(
                    (m: string) => {
                        self.reloadTable.apply(self, [m, pages, perPage]);
                    },
                    (err: Response) => {
                        self.handleError(err);
                    }
                    );
            } else {
                self._cService
                    .getSearchResult(
                    self.inputFile,
                    self.searchQuery,
                    3,
                    pages * perPage,
                    perPage
                    )
                    .subscribe(
                    (m: string) => {
                        self.reloadTable.apply(self, [m, pages, perPage]);
                    },
                    (err: Response) => {
                        self.handleError(err);
                    }
                    );
            }
        });

        self.dataTable.on("m-datatable--on-reloaded", function(
            e: any,
            settings: any
        ) {
            setTimeout(function() {
                $("#catAssignLog")
                    .find("tbody > tr")
                    .each(function(a, b) {
                        if ($(b).find("td:nth-child(1) span").length > 0) {
                            if (
                                $(b)
                                    .find("td:nth-child(1) span")
                                    .first()
                                    .html() == ""
                            ) {


                                $(b).hide();
                            }
                        } else {
                            if (a != 0) {

                                $(b).hide();
                            }
                        }
                    });
            }, 50);
        });
    }

    public searchData(event: any): void {
        if (event.which == 13 || event.which == 1) {
            if (this.searchQuery != "") {
                this._cService
                    .getSearchResultCount(this.inputFile, this.searchQuery, 3)
                    .subscribe(
                    (res: string) => {
                        let numb: number = Number.parseInt(res);
                        this.searchResults = new Array<CatAssign>();
                        for (var n = 0; n < numb; n++) {
                            let prod: CatAssign = new CatAssign();
                            this.searchResults.push(prod);
                        }

                        var pages = 0;
                        var perPage = this.dataTable.getPageSize();

                        this._cService
                            .getSearchResult(
                            this.inputFile,
                            this.searchQuery,
                            3,
                            pages * perPage,
                            perPage
                            )
                            .subscribe(
                            (m: string) => {
                                let pagi: any = this.dataTable.getDataSourceParam(
                                    "pagination"
                                );
                                pagi.page = 1;
                                pagi.perpage = 10;


                                this.dataTable.setDataSourceParam("pagination", pagi);

                                this.dataTable.load();
                                this.reloadTable.apply(this, [m, pages, perPage]);
                            },
                            (err: Response) => {
                                this.handleError(err);
                            }
                            );
                    },
                    (err: Response) => { }
                    );
            } else {
                this.dataTable.jsonData = this.catAssign; // Object.assign({},this.catAssign);
                this.dataTable.fullJsonData = this.catAssign;
                let pagi: any = this.dataTable.getDataSourceParam("pagination");
                pagi.page = 1;
                pagi.perpage = 10;
                this.dataTable.setDataSourceParam("pagination", pagi);
                this.dataTable.load();
            }
        }
    }


    public fillViewModel(proObj: Object): CatAssign {

        let catVM = new CatAssign();
        catVM.CatId = proObj["@category-id"];
        catVM.ProdId = proObj["@product-id"];
        catVM.primaryFlag = proObj["primary-flag"];

        return catVM;
    }

    private reloadTable(products: string, page: number, perPage: number): void {
        let prodArray: Array<CatAssign> = new Array<CatAssign>();
        let json: Array<Object> = JSON.parse(products).map(m => m["Value"]);

        json.forEach(pro => {
            prodArray.push(this.fillViewModel(pro));
        });
        if (this.searchQuery == "") {
            this.catAssign.splice(page * perPage, perPage, ...prodArray);
            this.dataTable.fullJsonData = this.catAssign;
        } else {
            this.searchResults.splice(page * perPage, perPage, ...prodArray);
            this.dataTable.fullJsonData = this.searchResults;
        }

        this.dataTable.jsonData = Object.assign({}, prodArray); // Object.assign({},this.catAssign);
        this.dataTable.load();
    }

    private handleError(err: Response): void {
        console.error(err);
    }
}
