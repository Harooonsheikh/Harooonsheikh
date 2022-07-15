import { KeyValue, CatalogModel } from "./../../../../../../Entities/Common";
import { Observable } from "rxjs";
import { WorkFlow } from "./../../../../../../Entities/WorkFlow";
import { WorkFlowService } from "./../../../../../../_services/workflow.service";
import { CatalogService } from "./../../../../../../_services/catalog.service";
import { Component, OnInit, AfterViewInit, Input } from "@angular/core";
declare var jquery: any;
declare var $: any;

export class Category {
    public Name: string = "";
    public CatId: string = "";
    public parent: KeyValue<string> = null;
    public position: string = "";
    public online: string = "";
    public customAttribute: Array<KeyValue<string>> = null;
    constructor() {
        this.customAttribute = new Array<KeyValue<string>>();
    }
}

@Component({
    selector: "catalog-category",
    templateUrl: "category.component.html"
})
export class CatalogCategoryComponent implements OnInit, AfterViewInit {
    @Input() public inputFile: string = "";
    public totalCategory: number = null;
    private categoriesOb: Observable<Object> = null;
    public categories: Array<Category> = null;
    public dataTable: any = null;
    public searchQuery: string = "";
    public searchResults: Array<Category> = null;

    constructor(
        private _cService: CatalogService,
        private _wfService: WorkFlowService
    ) { }

    ngOnInit() { }
    ngAfterViewInit() {

        this._cService.getCatalogModalCount(this.inputFile, CatalogModel.Category).subscribe(
            (number: number) => {
                this.totalCategory = number;
                this.setupProductTable();
            },
            (error: Response) => this.handleError(error)
        );
    }

    private setupProductTable(): void {

        this.categoriesOb = this._cService.getCatagoriesByCatalog(
            this.inputFile,
            0 * 10,
            10
        );
        this.categories = new Array<Category>();
        for (var i = 0; i < this.totalCategory; i++) {
            var element = new Category();
            this.categories.push(element);
        }

        this.dataTable = $("#categoryLog").mDatatable({
            data: {
                type: "local",
                source: this.categories,
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
                    field: "Name",
                    title: "Name",
                    width: "150",
                    textAlign: "center"
                },
                {
                    field: "online",
                    title: "Online",
                    width: "50",
                    textAlign: "center"
                },
                {
                    field: "parent",
                    title: "Parent",
                    width: "150",
                    textAlign: "center",
                    template: function(e: any) {
                        if (e.getDatatable().jsonData[e.rowIndex].parent) {
                            return e.getDatatable().jsonData[e.rowIndex].parent.Key;

                        }

                        return "";
                    }
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
        this.categoriesOb.subscribe(
            (m: string) => {
                self.reloadTable.apply(self, [m, pages, perPage]);
            },
            (err: Response) => {
                self.handleError(err);
            }
        );

        let self: CatalogCategoryComponent = this;
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
                    .getCatagoriesByCatalog(self.inputFile, pages * perPage, perPage)
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
                    2,
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
                    .getCatagoriesByCatalog(self.inputFile, pages * perPage, perPage)
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
                    2,
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
                $("#categoryLog")
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
                    .getSearchResultCount(this.inputFile, this.searchQuery, 2)
                    .subscribe(
                    (res: string) => {
                        let numb: number = Number.parseInt(res);
                        this.searchResults = new Array<Category>();
                        for (var n = 0; n < numb; n++) {
                            let prod: Category = new Category();
                            this.searchResults.push(prod);
                        }

                        var pages = 0;
                        var perPage = this.dataTable.getPageSize();

                        this._cService
                            .getSearchResult(
                            this.inputFile,
                            this.searchQuery,
                            2,
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
                this.dataTable.jsonData = this.categories; // Object.assign({},this.categories);
                this.dataTable.fullJsonData = this.categories;
                let pagi: any = this.dataTable.getDataSourceParam("pagination");
                pagi.page = 1;
                pagi.perpage = 10;
                this.dataTable.setDataSourceParam("pagination", pagi);
                this.dataTable.load();
            }
        }
    }


    public fillViewModel(proObj: Object): Category {
        let catVM = new Category();
        catVM.CatId = proObj["@category-id"];
        catVM.Name = proObj["display-name"]["#text"];
        catVM.parent = new KeyValue<string>();
        catVM.parent.Key = proObj["parent"];
        catVM.position = proObj["position"];
        catVM.online = proObj["online-flag"];

        catVM.customAttribute = new Array<KeyValue<string>>();
        if (proObj["custom-attributes"]) {
            if (proObj["custom-attributes"]["custom-attribute"]) {
                let arr: Array<Object> = new Array<Object>();
                Object.assign(arr, proObj["custom-attributes"]["custom-attribute"]);
                arr.forEach(ar => {
                    let keyV: KeyValue<string> = new KeyValue<string>();
                    keyV.Key = ar["@attribute-id"];
                    keyV.Value = ar["#text"];
                    catVM.customAttribute.push(keyV);
                });
            }
        }
        return catVM;
    }

    private reloadTable(products: string, page: number, perPage: number): void {
        let prodArray: Array<Category> = new Array<Category>();
        let json: Array<Object> = JSON.parse(products).map(m => m["Value"]);

        json.forEach(pro => {
            prodArray.push(this.fillViewModel(pro));
        });
        if (this.searchQuery == "") {
            this.categories.splice(page * perPage, perPage, ...prodArray);
            this.dataTable.fullJsonData = this.categories;
        } else {
            this.searchResults.splice(page * perPage, perPage, ...prodArray);
            this.dataTable.fullJsonData = this.searchResults;
        }

        this.dataTable.jsonData = Object.assign({}, prodArray);
        this.dataTable.load();
    }

    private handleError(err: Response): void {
        console.error(err);
    }
}
