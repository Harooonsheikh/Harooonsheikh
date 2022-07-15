using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Enums.Enums
{

    /// <summary>
    /// Enum may contain invalid or incorrect comments, please ensure message in Resource file if any change
    /// </summary>
    public enum CommerceLinkLoggerMessages

    {
        // General Trace Messages
        VSICL10000, // Entering in method: {0}
        VSICL10001, // Supplied parameter(s) to method {0}: {1}
        VSICL10002, // Data provided by RS to method{0}: {1}
        VSICL10003, // Data to be returned by method {0}: {1}
        VSICL10004, // Exiting from method {0}
        VSICL10005, // Value in Variable {1} in Method {0} is {2}
        VSICL10006, // Calling function {1} from {0}

        // Product Trace Messages
        VSICL10201, //  Product {0} is Master, now going to generate its variants
        VSICL10202, // Going to traverse Custom Attributes which are present in Dimension Set table
        VSICL10203, // Traversing custom attributes => Dimension Key {0}
        VSICL10204, // Traversing custom attributes => Dimension values {0}
        VSICL10205, // Dimension {0} for Product {1}, cannot have more than one dimensions
        VSICL10206, // Attribute {0} is missing on Product {1}
        VSICL10207, // Related product: {0} for Master Product Id: {1} not found in catalog, either add in catalog or remove from related products
        VSICL10208, // Related/Child product {0} => of Parent Product {1} found and ready for xml generation
        VSICL10209, // Now added its Master Product {0} as well for xml generation
        VSICL10210, // Fetched products with IsMasterProduct == false in case of Flat Product HIerarchy
        VSICL10211, // Product Price as Trade AgreementPrice: {0} for Product: {1}
        VSICL10212, // Product Price as Base Price: {0} for Product: {1}
        VSICL10213, // Get the Discount Amount: {0} for Product: {1}
        VSICL10214, // New Available Physical Inventory: {0} for Product: {1}
        VSICL10215, // Get catalog \"{0}\" catalog id = {1} from Retail Server for Channel ID {2}
        VSICL10216, // AX Category: {0} have {1} No of Products Setup in that category
        VSICL10217, // Product {0} has {1} Custom Attributes
        VSICL10218, // Product {0}, 
        VSICL10219, // SearchProductResult is null for product with ItemId = {0}, and RecordId = {1} in method {2}
        VSICL10220, // Total number of Catalogs = {0}
        VSICL10221, // Catalog Name = {0}

        // Customer Trace Messages
        VSICL10301, // Address [{0}] exists in AX but yet not synched in Ecommerce by AX to Ecomm Update job. So skipping integration key generation for it
        VSICL10302, // Customer not found

        // Sales Order Trace Message
        VSICL10501, // Sales Order Json: {0}
        VSICL10502, // AX Sales Order {0} Json before landing: {1}

        // Sales Order Status Messages
        VSICL10801, // Sales Order Status {0}

        // General Warning Messages
        VSICL30000, // An error occured in method {0}. Error: {1}

        // Product Warning Messages
        VSICL30202, // Catalogs/Products not found
        VSICL30203, // No ERP Products found

        // General Fatal Messages
        VSICL40000, // An error occured in method {0}. Error: {1}
        VSICL40001, // Missing request object in method {0}
        VSICL40002, // Parameter missing in method {0}. Paramter name: {1}
        VSICL40003, // Parameter out of context in method {0}. {1}
        VSICL40004, // Data not found by method {0}
        VSICL40005, // Retail Proxy Exception: 
        VSICL40006, // Parameter length exceed from max input characters. Paramter name: {0}, Max Length: {1}
        VSICL40007, // StoreId {0} not found
        VSICL40008, // Ambiguous request
        VSICL40009, // Invalid or missing Parameter value in method {0}. Paramter name: {1}
        VSICL40010, // Invalid or missing {0} in request header
        VSICL400010,// Please provide at least one parameter either {0}, or {1}
        VSICL400011,// License ID length should be less than equal to 36.
        VSICL400012,// Either Customer Account or License must be provided
        VSICL400013,// Unable to delete record, transaction exists against payment method
        VSICL400014,// Either Ecom Customer Id or Channel Reference Id must be provided
        VSICL400015,// Error calling Aos Service:{0}
        VSICL400016,// Customer portal request has already been executed
        VSICL400017,// An error has occurred. Error ID:{0}
        VSICL400018,// Please provide either PACLicense or InvoiceId

        // Product Fatal Messages
        VSICL40201, // Error in fetching discounts for {1}
        VSICL40202, // Exception in GetIndependentProductPriceDiscount, found 0 Catalog
        VSICL40203, // Found 0 or null Discounts, Exception CRT Discounts
        VSICL40204, // Found 0 or null Inventory, Exception CRT Inventory;
        VSICL40205, // Found 0 or null Price, Exception CRT Price;
        VSICL40206, // Found 0 or null Categories, Exception CRT Products
        VSICL40207, // Found 0 or null Products, Exception CRT Products
        VSICL40208, // Exception in GetActiveProductPrice, Found 0 Catalog
        VSICL40209, // Found 0 or null Related Products ,Exception CRT Products

        // Customer Fatal Messages
        VSICL40301, // Customer with ECom customerid: {0} is not found. Cannot Sync Sales Order
        VSICL40302, // Integration Key generation failed in Create Customer Mode
        VSICL40303, // Customer does not exist while saving address
        VSICL40304, // Integration Key generation failed in Update/Create Customer Mode

        // Sales Order Fatal Messages
        VSICL40501, // No sales orders found
        VSICL40502, // Order TenderLines have not TenderTypeId for some TenderLine
        VSICL40503, // Product {0} not found in Integration DB. Sales order cannot be processed.
        VSICL40504, // Problem in getting Product from Integration DB
        VSICL40505, // Unable to found sales order's products prices (GetIndependentProductPriceDiscount) to calculate periodic discount. Sales order cannot be processed
        VSICL40506, // Unable to found products in sales order. Sales order cannot be processed.
        VSICL40507, // Unable to found any payment details/tender lines in sales order. Sales order cannot be processed.
        VSICL40508, // Sales order transaction processing has been failed!
        VSICL40509, // Sales order transaction has been created successfully.
        VSICL40510, // Unable to read order from XML, order No :{0}
        VSICL40511, // Unable to read order Id from XML.
        VSICL40512, // Unable to process sales order {0} because it has been processed earlier.
        VSICL40513, // Unable to process cart {0} because it has been processed earlier.
        VSICL40526, // No TMV Migration Sales Line Number specified for Migrated Order
        VSICL40527, // TMV Old Sales Line Number and TMV Old Sales Line Action should be specified for Old Sales Order
        VSICL40528, // Unable to process request {0} as it has already been processed earlier.
        VSICL40529, // Unable to process request {0} as contract line actions are contradicting.
        VSICL40530, // Unable to process request {0} as sales line action is not provided.
        VSICL40531, // Unable to process request {0} as salesOrder header level custom attributes not provided.
        VSICL40532, // Unable to process request {0} as sales line are not provided.
        VSICL40533, // Unable to process request {0} as tender lines are not provided.
        VSICL40534, // Provided payment method "{0} is not configured in CommerceLink.

        //Payment Connector errors
        VSICL40514, // Unable to create integration key for UniqueCardId.
        VSICL40515, // Unable to generate CardBlob.
        VSICL40516, // Unable to generate AuthBlob.
        VSICL40517, // Unable to find any payment connectors from AX.
        VSICL40518, // Payment connector ServiceAccountID does not match with configuration.
        
        VSICL40519, // Sales order transaction {0} without extensions has been created successfully.
        VSICL40520, // Sales order transaction {0} with extensions has been created successfully.
        VSICL40521, // Migrated sales order info has been created successfully.
        VSICL40522, // Unable to create contract relation info for sales order transaction {0} and migrated order number {0}.
        VSICL40523, // Invalid product found in salesorder request object. Product Name: {0}
        VSICL40524, // Contract {0} has been closed successfully.
        VSICL40525, // Unable to create contract becuase there is an error to close contract {0}. D365 error is : {1}.
        VSICL40535, // Parameter missing or invalid in method {0}. Paramter name: {1}

        // Customer Address/Address Fatal Messages
        VSICL40701, // ECom delivery address id not found. Sales order cannot be processed

        // Channel Fatal Messages
        VSICL41001, // Channel is not in Published or InProgress state
        VSICL41002, // Listing Attributes Count returned is '{0}'. Error details '{1}'
        VSICL41003, // The channel does not have any listing attributes. Make sure they are included in AX channel management UI
        VSICL41004, // Navigation categories count returned is '{0}'. Error details {1}
        VSICL41005, // The channel does not have any categories.
		/// <summary>
        /// {0}. Time of the failure: {1}
        /// </summary>
        VSICL41006, // {0}. Time of the failure: {1}
        VSICL41101, //Unable to find  ecommerce customer from integration database EcomCustomerId: {0}

        // Quotation Validation Messages
        VSICL401101, // Channel ReferenceId miising 
        VSICL401102, // Channel ReferenceId already exist 
        VSICL401103, // ContactPersonId value must not be null. 
        VSICL401104, // TMVContractValidFrom value must not be null. 
        VSICL401105, // Itemid must not be null. 
        VSICL401106, // customerQuotation object is null
        VSICL401107,  // Quotation Items missing in request

        // Price validation 
        VSICL401200, // Product {0} not found in ERP
        VSICL401201, // Sales Origin is missing in the request
        VSICL401202, // Price Validation: Sales Lines not found
        VSICL401203, // Price Validation: Price validation set to false, no need to validate prices
        VSICL401204, // Price Validation: Skip price validation as order is either apple app order or belongs to specific sales origin

        // Logging Messages
        VSICL500000, // Enters in CL
        VSICL500001, // Exists from CL
        VSICL500002, // Request sent to external system. 
        VSICL500003, // Response received from external system.
        VSICL500004, // Start Request Response Logging
        VSICL500005, // External system method throws exception
        VSICL500006, //  DB insertion started
        VSICL500007, //  DB insertion ended
        VSICL500009, // {0} Starts
        VSICL500010, // {0} Ends

        // Controller Messages
        VSICL600000, // Creating Controller
        VSICL600001, // Created Controller

        VSICL600003, // Executing
        VSICL600004, // Executed
        
        VSICL500008, // External System Response Time

        //Customer Portal Messages
        VSICL700000 //Customer billing address has been updated successfully.
    }

}
