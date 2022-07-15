export class WorkFlow {
    public Id: number = null;
    public InstanceName: string = null;
    public Created: Date = null;
    public EntityId: number = null;
    public JobId: number = null;
    public StoreId: number = null;
    public Updated: Date = null;
    public Status: string = null;
}

export class WorkflowTransition {
    public WorkFlowTransitionID: number = null;
    public InstanceID: number = null;
    public StateID: number = null;
    public Created: Date = null;
}

export class CatalogStats {
    public CatalogStatisticsIdb: number = 0;
    public FileName: string = null;
    public AXProducts: number = 0;
    public MasterProducts: number = 0;
    public VariantProducts: number = 0;
    public SimpleProducts: number = 0;
    public Categories: number = 0;
    public CategoryAssignments: number = 0;
    public MinCustomAttributes: number = 0;
    public MaxCustomAttributes: number = 0;
    public Created: string = null;
    public CreatedByUser_FK: string = null;
    public Modified: string = null;
    public ModifiedByUser_FK: string = null;
    public StoreId_FK: string = null;
}
