namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpHardwareProfile
	{
		public ErpHardwareProfile()
		{
		}
		public int LineDisplayDelayForLinkedItems	{ get; set; }//;
		public string LineDisplayBalanceText	{ get; set; }//;
		public bool LineDisplayBinaryConversion	{ get; set; }//;
		public int LineDisplayCharacterSet	{ get; set; }//;
		public string LineDisplayClosedLine1	{ get; set; }//;
		public string LineDisplayClosedLine2	{ get; set; }//;
		public string LineDisplayDeviceDescription	{ get; set; }//;
		public ErpDeviceType LineDisplayDeviceType	{ get; set; }//;
		public int LineDisplayDeviceTypeValue	{ get; set; }//;
		public string LineDisplayDeviceName	{ get; set; }//;
		public bool LineDisplayDisplayLinkedItem	{ get; set; }//;
		public bool LineDisplayDisplayTerminalClosed	{ get; set; }//;
		public string LineDisplayTotalText	{ get; set; }//;
		public bool DualDisplayActive	{ get; set; }//;
		public string DualDisplayWebBrowserUrl	{ get; set; }//;
		public int DualDisplayImageRotatorInterval	{ get; set; }//;
		public string DualDisplayImageRotatorPath	{ get; set; }//;
		public decimal DualDisplayReceiptWidthPercentage	{ get; set; }//;
		public ErpDualDisplayType DualDisplayDisplayType	{ get; set; }//;
		public int EFTTypeId	{ get; set; }//;
		public string EFTCompanyId	{ get; set; }//;
		public int EFTConfiguration	{ get; set; }//;
		public string EFTPaymentConnectorName	{ get; set; }//;
		public string EFTMerchantPropertyXML	{ get; set; }//;
		public string EFTData	{ get; set; }//;
		public string EFTDescription	{ get; set; }//;
		public string EFTMerchantId	{ get; set; }//;
		public string EFTPassword	{ get; set; }//;
		public string EFTServerName	{ get; set; }//;
		public string EFTServerPort	{ get; set; }//;
		public string EFTUserId	{ get; set; }//;
		public string MSREndTrack1	{ get; set; }//;
		public string MSREndTrack2	{ get; set; }//;
		public string FiscalPrinterManagementReportPAFIdentification	{ get; set; }//;
		public string FiscalPrinterManagementReportConfigParameter	{ get; set; }//;
		public string FiscalPrinterManagementReportTenderType	{ get; set; }//;
		public string FiscalPrinterManagementReportGiftCard	{ get; set; }//;
		public string KeyboardMappingId	{ get; set; }//;
		public ErpDeviceType KeyLockDeviceType	{ get; set; }//;
		public int KeyLockDeviceTypeValue	{ get; set; }//;
		public string KeyLockDeviceDescription	{ get; set; }//;
		public string KeyLockDeviceName	{ get; set; }//;
		public ErpDeviceType MSRDeviceType	{ get; set; }//;
		public int MSRDeviceTypeValue	{ get; set; }//;
		public string MSRDeviceDescription	{ get; set; }//;
		public string MSRDeviceName	{ get; set; }//;
		public string MSRMake	{ get; set; }//;
		public string MSRModel	{ get; set; }//;
		public string MSRSeparator	{ get; set; }//;
		public string MSRStartTrack	{ get; set; }//;
		public ErpDeviceType PinPadDeviceType	{ get; set; }//;
		public int PinPadDeviceTypeValue	{ get; set; }//;
		public string PinPadDeviceName	{ get; set; }//;
		public string PinPadMake	{ get; set; }//;
		public string PinPadModel	{ get; set; }//;
		public string PinPadDeviceDescription	{ get; set; }//;
		public string ProfileDescription	{ get; set; }//;
		public string ProfileId	{ get; set; }//;
		public ErpDeviceType RFIDDeviceType	{ get; set; }//;
		public int RFIDDeviceTypeValue	{ get; set; }//;
		public string RFIDDeviceName	{ get; set; }//;
		public string RFIDDeviceDescription	{ get; set; }//;
		public ErpDeviceType ScaleDeviceType	{ get; set; }//;
		public int ScaleDeviceTypeValue	{ get; set; }//;
		public string ScaleDeviceName	{ get; set; }//;
		public string ScaleDeviceDescription	{ get; set; }//;
		public bool ScaleManualInputAllowed	{ get; set; }//;
		public int ScaleTimeoutInSeconds	{ get; set; }//;
		public ErpDeviceType SignatureCaptureDeviceType	{ get; set; }//;
		public int SignatureCaptureDeviceTypeValue	{ get; set; }//;
		public string SignatureCaptureDeviceName	{ get; set; }//;
		public string SignatureCaptureMake	{ get; set; }//;
		public string SignatureCaptureModel	{ get; set; }//;
		public string SignatureCaptureDeviceDescription	{ get; set; }//;
		public string SignatureCaptureFormName	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<ErpHardwareProfilePrinter> Printers	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<ErpHardwareProfileScanner> Scanners	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<ErpHardwareProfileCashDrawer> CashDrawers	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
