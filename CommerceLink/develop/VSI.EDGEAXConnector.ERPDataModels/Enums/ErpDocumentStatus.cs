namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public enum ErpDocumentStatus
	{
        None = 0,
        Quotation = 1,
        PurchaseOrder = 2,
        Confirmation = 3,
        PickingList = 4,
        PackingSlip = 5,
        ReceiptsList = 6,
        Invoice = 7,
        ApproveJournal = 8,
        ProjectInvoice = 9,
        ProjectPackingSlip = 10,
        CRMQuotation = 11,
        Lost = 12,
        Canceled = 13,
        FreeTextInvoice = 14,
        RFQ = 15,
        RFQAccept = 16,
        RFQReject = 17,
        PurchaseRequest = 18,
        RFQResend = 19
    }
}
