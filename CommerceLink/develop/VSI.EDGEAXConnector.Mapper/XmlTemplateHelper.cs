using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.Mapper
{
    public class XmlTemplateHelper
    {
        //Values for Generate XML
        private bool isToLower = false;
        private bool isToUpper = false;
        private bool isConstant = false;
        private bool isDefault = false;
        private bool isExpression = false;
        private bool isCustomAttributeValue = false;
        private string defaultValue = "";
        private MappingTemplateDAL mappingTemplateDAL = null;
        private ConfigurationHelper configurationHelper;
        private int StoreId { get; set; }
        private string CreatedBy { get; set; } 


        #region Generate XML
        public XmlTemplateHelper(StoreDto store)
        {
            this.configurationHelper = new ConfigurationHelper(store.StoreKey);
            mappingTemplateDAL = new MappingTemplateDAL(store.StoreKey);
            StoreId = store.StoreId;
            CreatedBy = store.CreatedBy;
        }
        public void GenerateXmlUsingTemplateFromDatabase(string fileName, string outputPath, XmlSourceDirection sourceDirection, object templateObject)
        {
            try
            {
                string strMainClassName = templateObject.GetType().Name;
                string strDirection = sourceDirection == XmlSourceDirection.CREATE ? "CREATE." : "READ.";

                #region DB changes by usman raza on 30th Dec 2016

                string filename = strDirection + strMainClassName;

                MappingTemplate obj = mappingTemplateDAL.GetMappingTemplateByName(filename);

                #endregion
                /******* moving to DB changes by usman raza on 30th Dec 2016 ********/

                if (obj != null)
                {
                    //Logic to avoid multi-threading issue
                    //for (int i = 0; i < ConfigurationHelper.XmlTemplateRetryCounter; i++)
                    int fileRetryCount = configurationHelper.GetSetting(PRODUCT.File_Generate_Retry).IntValue();
                    for (int i = 0; i < fileRetryCount; i++)
                    {

                        XmlDocument doc = new XmlDocument();
                        var xmlDoc = XDocument.Parse(obj.XML);
                        doc = ConvertToXmlDocument(xmlDoc);



                        var xmlNodesList = doc.ChildNodes;

                        if (xmlNodesList.Count > 0)
                        {
                            XmlDocument newDoc = new XmlDocument();
                            //XmlNode docNode = newDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                            //newDoc.AppendChild(docNode);

                            foreach (XmlNode node in xmlNodesList)
                            {
                                XmlNode newNode = GetNodeXml(node, templateObject);
                                if (newNode != null)
                                {
                                    //Removing attributes which are not required in output
                                    RemoveExtrasAttributes(newNode);

                                    newDoc.AppendChild(newDoc.ImportNode(newNode, true));
                                }
                            }

                            //Manageing ShowNode 
                            ManageShowNode(newDoc);
                            ManageRepeatNode(newDoc);

                            //TODO: Product Specific part
                            //Description: Unable to hanlde expression on attribute & Dewandware does not accept mode=""
                            if (strMainClassName.Equals("ErpCatalog"))
                            {
                                ManageDeleteProducts(newDoc);
                            }

                            var settings = new XmlWriterSettings
                            {
                                Encoding = new UTF8Encoding(false),
                                Indent = true,
                            };

                            //Check if file has not any exception then write it otherwise moved to failed folder & try again
                            if (!newDoc.InnerXml.Contains("EvalExpression:Exception"))
                            {
                                this.configurationHelper.GetDirectory(outputPath + "\\Processed");
                                using (var writer = XmlWriter.Create(Path.Combine(outputPath, fileName), settings))
                                {
                                    newDoc.Save(writer);
                                }
                                //newDoc.Save(Path.Combine(outputPath,fileName));
                                break;
                            }
                            else
                            {
                                this.configurationHelper.GetDirectory(outputPath + "\\Failed");
                                using (var writer = XmlWriter.Create(Path.Combine(Path.Combine(outputPath + "\\Failed"), "Ignore_" + fileName), settings))
                                {
                                    newDoc.Save(writer);
                                }
                                //newDoc.Save(Path.Combine(Path.Combine(outputPath + "Failed"), "Ignore_" + fileName));
                            }
                        }
                    }

                }
                /***********/

            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, CreatedBy, "XmlTemplateEngine");
                throw;
            }
        }

        public void GenerateXmlUsingTemplateFromFile(string fileName, string outputPath, XmlSourceDirection sourceDirection, object templateObject)
        {
            try
            {
                string strMainClassName = templateObject.GetType().Name;
                string strDirection = sourceDirection == XmlSourceDirection.CREATE ? "CREATE." : "READ.";
                //string xmlTemplate = Path.Combine(ConfigurationHelper.XmlPath, strDirection + strMainClassName + ".xml");
                string xmlTemplate = Path.Combine(this.configurationHelper.GetDirectory(
                    configurationHelper.GetSetting(APPLICATION.XML_Base_Path)), strDirection + strMainClassName + ".xml");

                if (File.Exists(xmlTemplate))
                {
                    //Logic to avoid multi-threading issue
                    //for (int i = 0; i < ConfigurationHelper.XmlTemplateRetryCounter; i++)
                    int fileRetryCount = configurationHelper.GetSetting(PRODUCT.File_Generate_Retry).IntValue();
                    for (int i = 0; i < fileRetryCount; i++)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(xmlTemplate);
                        var xmlNodesList = doc.ChildNodes;
                        if (xmlNodesList.Count > 0)
                        {
                            XmlDocument newDoc = new XmlDocument();
                            foreach (XmlNode node in xmlNodesList)
                            {
                                XmlNode newNode = GetNodeXml(node, templateObject);
                                if (newNode != null)
                                {
                                    //Removing attributes which are not required in output
                                    RemoveExtrasAttributes(newNode);
                                    newDoc.AppendChild(newDoc.ImportNode(newNode, true));
                                }
                            }
                            //Manageing ShowNode 
                            ManageShowNode(newDoc);
                            ManageRepeatNode(newDoc);
                            //TODO: Product Specific part
                            //Description: Unable to hanlde expression on attribute & Dewandware do es not accept mode=""
                            if (strMainClassName.Equals("ErpCatalog"))
                            {
                                ManageDeleteProducts(newDoc);
                            }
                            var settings = new XmlWriterSettings
                            {
                                Encoding = new UTF8Encoding(false),
                                Indent = true,
                            };

                            //Check if file has not any exception then write it otherwise moved to failed folder & try again
                            if (!newDoc.InnerXml.Contains("EvalExpression:Exception"))
                            {
                                this.configurationHelper.GetDirectory(outputPath + "\\Processed");
                                using (var writer = XmlWriter.Create(Path.Combine(outputPath, fileName), settings))
                                {
                                    newDoc.Save(writer);
                                }
                                //newDoc.Save(Path.Combine(outputPath,fileName));
                                break;
                            }
                            else
                            {
                                this.configurationHelper.GetDirectory(outputPath + "\\Failed");
                                using (var writer = XmlWriter.Create(Path.Combine(Path.Combine(outputPath + "\\Failed"), "Ignore_" + fileName), settings))
                                {
                                    newDoc.Save(writer);
                                }
                                //newDoc.Save(Path.Combine(Path.Combine(outputPath + "Failed"), "Ignore_" + fileName));
                            }
                        }
                    }
                }
                else
                {
                    throw new CommerceLinkError(string.Format("Template not found {0}", xmlTemplate));
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, CreatedBy, "XmlTemplateEngine");
                throw;
            }
        }

        public XmlDocument GenerateXmlUsingTemplate(string fileName, string outputPath, XmlSourceDirection sourceDirection, object templateObject)
        {
            string strMainClassName = templateObject.GetType().Name;
            string strDirection = sourceDirection == XmlSourceDirection.CREATE ? "CREATE." : "READ.";

            try
            {
                XmlDocument doc = new XmlDocument();
                XmlDocument newDoc = new XmlDocument();
                //Reading template from database
                if (System.Configuration.ConfigurationManager.AppSettings["IsLoadTemplateFromDatabase"] != null && System.Configuration.ConfigurationManager.AppSettings["IsLoadTemplateFromDatabase"] == "true")
                {
                    string filename = strDirection + strMainClassName;
                    MappingTemplate dbTemplate = mappingTemplateDAL.GetMappingTemplateByName(filename);
                    if (dbTemplate != null)
                    {
                        var xmlDoc = XDocument.Parse(dbTemplate.XML);
                        doc = ConvertToXmlDocument(xmlDoc);
                    }
                    else
                    {
                        throw new CommerceLinkError(string.Format("Template not found from database {0}", filename));
                    }
                }
                //Reading template from file system
                else
                {
                    string xmlTemplate = Path.Combine(this.configurationHelper.GetDirectory(configurationHelper.GetSetting(APPLICATION.XML_Base_Path)), strDirection + strMainClassName + ".xml");

                    if (File.Exists(xmlTemplate))
                    {
                        doc.Load(xmlTemplate);
                    }
                    else
                    {
                        throw new CommerceLinkError(string.Format("Template not found from file system {0}", xmlTemplate));
                    }
                }

                //Logic to avoid multi-threading issue
                //for (int i = 0; i < ConfigurationHelper.XmlTemplateRetryCounter; i++)
                int fileRetryCount = configurationHelper.GetSetting(PRODUCT.File_Generate_Retry).IntValue();
                for (int i = 0; i < fileRetryCount; i++)
                {
                    var xmlNodesList = doc.ChildNodes;

                    if (xmlNodesList.Count > 0)
                    {
                      
                        //XmlNode docNode = newDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                        //newDoc.AppendChild(docNode);
                        foreach (XmlNode node in xmlNodesList)
                        {
                            XmlNode newNode = GetNodeXml(node, templateObject);
                            if (newNode != null)
                            {
                                //Removing attributes which are not required in output
                                RemoveExtrasAttributes(newNode);
                                newDoc.AppendChild(newDoc.ImportNode(newNode, true));
                            }
                        }

                        //Manageing ShowNode 
                        ManageShowNode(newDoc);
                        ManageRepeatNode(newDoc);

                        //TODO: Product Specific part
                        //Description: Unable to hanlde expression on attribute & Dewandware do es not accept mode=""
                        if (strMainClassName.Equals("ErpCatalog"))
                        {
                            ManageDeleteProducts(newDoc);
                        }

                        var settings = new XmlWriterSettings
                        {
                            Encoding = new UTF8Encoding(false),
                            Indent = true,
                        };

                        //Check if file has not any exception then write it otherwise moved to failed folder & try again
                        if (!newDoc.InnerXml.Contains("EvalExpression:Exception"))
                        {
                            this.configurationHelper.GetDirectory(outputPath + "\\Processed");
                            using (var writer = XmlWriter.Create(Path.Combine(outputPath, fileName), settings))
                            {
                                newDoc.Save(writer);
                            }
                            processCDATA(Path.Combine(outputPath, fileName));
                            break;
                        }
                        else
                        {
                            this.configurationHelper.GetDirectory(outputPath + "\\Failed");
                            using (var writer = XmlWriter.Create(Path.Combine(Path.Combine(outputPath + "\\Failed"), "Ignore_" + fileName), settings))
                            {
                                newDoc.Save(writer);
                            }
                            processCDATA(Path.Combine(Path.Combine(outputPath + "\\Failed"), "Ignore_" + fileName));
                        }
                    }
                }
                return newDoc;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, CreatedBy, "XmlTemplateEngine");
                throw new CommerceLinkError(string.Format("Template not found in file system or database {0}", strMainClassName));
            }
        }

        public XmlDocument GenerateXmlUsingTemplate(XmlSourceDirection sourceDirection, object templateObject)
        {
            string strMainClassName = templateObject.GetType().Name;
            string strDirection = sourceDirection == XmlSourceDirection.CREATE ? "CREATE." : "READ.";
            XmlDocument xmlDocumentReturn = null;

            try
            {
                XmlDocument doc = new XmlDocument();

                //Reading template from database
                if (System.Configuration.ConfigurationManager.AppSettings["IsLoadTemplateFromDatabase"] != null && System.Configuration.ConfigurationManager.AppSettings["IsLoadTemplateFromDatabase"] == "true")
                {
                    string filename = strDirection + strMainClassName;
                    MappingTemplate dbTemplate = mappingTemplateDAL.GetMappingTemplateByName(filename);

                    if (dbTemplate != null)
                    {
                        var xmlDoc = XDocument.Parse(dbTemplate.XML);
                        doc = ConvertToXmlDocument(xmlDoc);
                    }
                    else
                    {
                        throw new CommerceLinkError(string.Format("Template not found from database {0}", filename));
                    }
                }
                //Reading template from file system
                else
                {
                    string xmlTemplate = Path.Combine(this.configurationHelper.GetDirectory(configurationHelper.GetSetting(APPLICATION.XML_Base_Path)), strDirection + strMainClassName + ".xml");

                    if (File.Exists(xmlTemplate))
                    {
                        doc.Load(xmlTemplate);
                    }
                    else
                    {
                        throw new CommerceLinkError(string.Format("Template not found from file system {0}", xmlTemplate));
                    }
                }
                //Logic to avoid multi-threading issue
                //for (int i = 0; i < ConfigurationHelper.XmlTemplateRetryCounter; i++)
                int fileRetryCount = configurationHelper.GetSetting(PRODUCT.File_Generate_Retry).IntValue();
                for (int i = 0; i < fileRetryCount; i++)
                {
                    var xmlNodesList = doc.ChildNodes;

                    if (xmlNodesList.Count > 0)
                    {
                        XmlDocument newDoc = new XmlDocument();
                        //XmlNode docNode = newDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                        //newDoc.AppendChild(docNode);

                        foreach (XmlNode node in xmlNodesList)
                        {
                            XmlNode newNode = GetNodeXml(node, templateObject);
                            if (newNode != null)
                            {
                                //Removing attributes which are not required in output
                                RemoveExtrasAttributes(newNode);
                                newDoc.AppendChild(newDoc.ImportNode(newNode, true));
                            }
                        }
                        //Manageing ShowNode 
                        ManageShowNode(newDoc);
                        ManageRepeatNode(newDoc);
                        //TODO: Product Specific part
                        //Description: Unable to hanlde expression on attribute & Dewandware do es not accept mode=""
                        if (strMainClassName.Equals("ErpCatalog"))
                        {
                            ManageDeleteProducts(newDoc);
                        }

                        var settings = new XmlWriterSettings
                        {
                            Encoding = new UTF8Encoding(false),
                            Indent = true,
                        };

                        //Check if file has not any exception then write it otherwise moved to failed folder & try again
                        if (!newDoc.InnerXml.Contains("EvalExpression:Exception"))
                        {
                            xmlDocumentReturn = newDoc;
                            //newDoc.Save(Path.Combine(outputPath,fileName));
                            //break;
                        }
                        else
                        {
                            xmlDocumentReturn = newDoc;
                            //newDoc.Save(Path.Combine(Path.Combine(outputPath + "Failed"), "Ignore_" + fileName));
                        }
                    }
                }

                return xmlDocumentReturn;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, CreatedBy, "XmlTemplateEngine");
                throw new CommerceLinkError(string.Format("Template not found in file system or database {0}", strMainClassName));
            }
        }

        private void ManageShowNode(XmlDocument xdoc)
        {
            string xNodePath = @"//*[@show-node='false']";
            XmlNodeList xmlShowNodeList = xdoc.SelectNodes(xNodePath);
            foreach (XmlNode xmlNode in xmlShowNodeList)
            {
                xmlNode.ParentNode.RemoveChild(xmlNode);
            }
            xNodePath = @"//*[@show-node='False']";
            xmlShowNodeList = xdoc.SelectNodes(xNodePath);
            foreach (XmlNode xmlNode in xmlShowNodeList)
            {
                xmlNode.ParentNode.RemoveChild(xmlNode);
            }

            xNodePath = @"//*[@show-node='true']";
            xmlShowNodeList = xdoc.SelectNodes(xNodePath);
            foreach (XmlNode xmlNode in xmlShowNodeList)
            {
                xmlNode.Attributes.Remove(xmlNode.Attributes["show-node"]);
            }
            xNodePath = @"//*[@show-node='True']";
            xmlShowNodeList = xdoc.SelectNodes(xNodePath);
            foreach (XmlNode xmlNode in xmlShowNodeList)
            {
                xmlNode.Attributes.Remove(xmlNode.Attributes["show-node"]);
            }
        }

        private void ManageRepeatNode(XmlDocument xdoc)
        {
            string xNodePath = @"//*[@repeat='true']";
            XmlNodeList xmlShowNodeList = xdoc.SelectNodes(xNodePath);

            for (int i = 0; i < xmlShowNodeList.Count; i++)
            {
                XmlNode pNode = xmlShowNodeList[i].ParentNode;
                pNode.RemoveChild(xmlShowNodeList[i]);
            }

            xNodePath = @"//*[@repeat='True']";
            xmlShowNodeList = xdoc.SelectNodes(xNodePath);
            for (int i = 0; i < xmlShowNodeList.Count; i++)
            {
                XmlNode pNode = xmlShowNodeList[i].ParentNode;
                pNode.RemoveChild(xmlShowNodeList[i]);
            }

            xNodePath = @"//*[@repeat='false']";
            xmlShowNodeList = xdoc.SelectNodes(xNodePath);
            foreach (XmlNode xmlNode in xmlShowNodeList)
            {
                xmlNode.Attributes.Remove(xmlNode.Attributes["repeat"]);
            }
            xNodePath = @"//*[@repeat='False']";
            xmlShowNodeList = xdoc.SelectNodes(xNodePath);
            foreach (XmlNode xmlNode in xmlShowNodeList)
            {
                xmlNode.Attributes.Remove(xmlNode.Attributes["repeat"]);
            }
        }

        private void RemoveExtrasAttributes(XmlNode newNode)
        {
            newNode.Attributes.Remove(newNode.Attributes["file-source"]);
            newNode.Attributes.Remove(newNode.Attributes["data-source"]);
            newNode.Attributes.Remove(newNode.Attributes["data-object"]);
            newNode.Attributes.Remove(newNode.Attributes["default-value"]);
            newNode.Attributes.Remove(newNode.Attributes["constant-value"]);
            newNode.Attributes.Remove(newNode.Attributes["expression"]);
            newNode.Attributes.Remove(newNode.Attributes["to-lower"]);
            newNode.Attributes.Remove(newNode.Attributes["to-upper"]);
            newNode.Attributes.Remove(newNode.Attributes["custom-attribute-value"]);

            foreach (XmlNode innerNode in newNode.ChildNodes)
            {
                if (innerNode != null && innerNode.NodeType != XmlNodeType.Text && innerNode.NodeType != XmlNodeType.Comment)
                {
                    RemoveExtrasAttributes(innerNode);
                }
            }
        }

        private void ManageDeleteProducts(XmlDocument xdoc)
        {
            string xNodePath = @"//*[@mode='Insert']";
            XmlNodeList xmlShowNodeList = xdoc.SelectNodes(xNodePath);
            foreach (XmlNode xmlNode in xmlShowNodeList)
            {
                xmlNode.Attributes.Remove(xmlNode.Attributes["mode"]);
            }

            xNodePath = @"//*[@mode='Delete']";
            xmlShowNodeList = xdoc.SelectNodes(xNodePath);
            foreach (XmlNode xmlNode in xmlShowNodeList)
            {
                xmlNode.Attributes["mode"].Value = "delete";
                xmlNode.InnerXml = "";
            }
        }

        private XmlNode GetNodeXml(XmlNode node, object dataObj)
        {
            bool checkChild = true;
            try
            {
                if (node.Attributes != null)
                {
                    foreach (var attribute in node.Attributes)
                    {
                        XmlAttribute attri = (attribute as XmlAttribute);
                        if (attri.Name.ToLower().Equals("file-source") && node.Attributes["data-source"] != null)
                        {
                            GenerateAssociateTemplateNode(attri, node, dataObj);
                        }
                        //Handling repeating part of template  
                        else if (attri.Name.ToLower().Equals("data-source") && node.Attributes["file-source"] == null)
                        {
                            checkChild = GenerateRepeatedNode(attri, node, dataObj, checkChild);
                        }
                        else if (attri.Name.ToLower().Equals("show-node"))
                        {
                            ResolveShowNodeAttribute(attri, node, dataObj);
                        }
                        else if (attri.Name.ToLower() != "data-source" && attri.Name.ToLower() != "data-object")
                        {
                            if (attri.Name.ToLower() == "to-lower")
                            {
                                isToLower = true;
                            }
                            else if (attri.Name.ToLower() == "to-upper")
                            {
                                isToUpper = true;
                            }
                            else if (attri.Name.ToLower() == "constant-value")
                            {
                                isConstant = true;
                            }
                            else if (attri.Name.ToLower() == "default-value")
                            {
                                isDefault = true;
                                defaultValue = attri.Value;
                            }
                            else if (attri.Name.ToLower() == "expression")
                            {
                                isExpression = true;
                            }
                            else if (attri.Name.ToLower() == "custom-attribute-value")
                            {
                                isCustomAttributeValue = true;
                            }
                            else if (attri.Name.ToLower() == "data-object")
                            {
                                //Do nothing
                            }
                            //Assigning value to attribute from source object
                            else
                            {
                                GenerateAttribute(attri, node, dataObj);
                            }
                        }
                    }
                }
                //Handling children nodes of current node
                if (node.HasChildNodes && checkChild)
                {
                    foreach (XmlNode node1 in node.ChildNodes)
                    {
                        XmlNode newNode = GetNodeXml(node1, dataObj);
                        if (newNode != null)
                        {
                            node.ReplaceChild(newNode, node1);
                        }
                    }
                }
                //skiping declaration node
                else if (node.NodeType == XmlNodeType.XmlDeclaration)
                {
                    node = null;
                }
                //Basic node creation part
                else
                {
                    if (isConstant)
                    {
                        //Do nothing
                    }
                    else if (isCustomAttributeValue)
                    {
                        GenerateCustomAttributeValueNode(node, dataObj);
                    }
                    else if (isExpression)
                    {
                        GenerateExpressionNode(node, dataObj);
                    }
                    else
                    {
                        //Handling custom attributes which have key name in template
                        if (node.ParentNode != null && node.ParentNode.Name.Equals("custom-attribute"))
                        {
                            GenerateCustomAttribute(node, dataObj);
                        }
                        //Handling basic node which contains value
                        else
                        {
                            GenerateBasicNode(node, dataObj);
                        }
                    }
                    //Reset falgs
                    isToLower = false;
                    isToUpper = false;
                    isConstant = false;
                    isDefault = false;
                    isExpression = false;
                    isCustomAttributeValue = false;
                    defaultValue = "";
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, CreatedBy, "XmlTemplateEngine");
                throw;
            }
            return node;
        }

        private void GenerateAssociateTemplateNode(XmlAttribute attri, XmlNode node, object dataObj)
        {
            var nodeName = node.Name;
            var fileSource = attri.Value;
            //string XmlTemplate = Path.Combine(ConfigurationHelper.XmlPath, fileSource);
            string XmlTemplate = Path.Combine(configurationHelper.GetSetting(APPLICATION.XML_Base_Path), fileSource);
            var dataSource = node.Attributes["data-source"];

            if (dataSource != null && dataSource.Value != "")
            {
                var dataSourceAttri = dataSource.Value.Split('~');
                var dataSourceProperty = dataObj.GetType().GetProperty(dataSourceAttri[1]);

                if (File.Exists(XmlTemplate) && dataSourceProperty != null)
                {
                    if (dataSourceProperty.PropertyType.IsGenericType && (dataSourceProperty.PropertyType.GetGenericTypeDefinition() == typeof(List<>) || dataSourceProperty.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)))
                    {
                        var listValuesProperty = dataObj.GetType().GetProperty(dataSourceAttri[1]);
                        if (listValuesProperty != null)
                        {
                            var listValues = listValuesProperty.GetValue(dataObj);
                            IList lstProp = listValues as IList;
                            if (lstProp != null)
                            {
                                foreach (var listItemDataObject in lstProp)
                                {
                                    XmlDocument doc = new XmlDocument();
                                    doc.Load(XmlTemplate);

                                    var xmlNodesList = doc.ChildNodes;

                                    //Removing declaration node if exist from inner templates
                                    foreach (XmlNode innerNode in xmlNodesList)
                                    {
                                        if (innerNode.NodeType == XmlNodeType.XmlDeclaration)
                                        {
                                            doc.RemoveChild(innerNode);
                                        }
                                    }

                                    if (dataSourceAttri.Count() > 1)
                                    {
                                        foreach (XmlNode innerNode in xmlNodesList)
                                        {
                                            if (innerNode.Name.Equals("custom-attribute"))
                                            {
                                                var customAttribute = listItemDataObject as KeyValuePair<string, string>?;
                                                if (customAttribute != null)
                                                {
                                                    innerNode.Attributes["attribute-id"].Value = customAttribute.Value.Key;
                                                }
                                            }
                                            XmlNode newNode = GetNodeXml(innerNode, listItemDataObject);
                                            if (newNode != null)
                                            {
                                                doc.ReplaceChild(newNode, innerNode);
                                            }
                                        }
                                    }
                                    node.InnerXml += doc.InnerXml;
                                }
                            }
                        }
                    }
                    else
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(XmlTemplate);

                        var xmlNodesList = doc.ChildNodes;

                        //Removing declaration node if exist from inner templates
                        foreach (XmlNode innerNode in xmlNodesList)
                        {
                            if (innerNode.NodeType == XmlNodeType.XmlDeclaration)
                            {
                                doc.RemoveChild(innerNode);
                            }
                        }

                        if (dataSourceAttri.Count() > 1)
                        {
                            foreach (XmlNode innerNode in xmlNodesList)
                            {
                                var property = dataObj.GetType().GetProperty(dataSourceAttri[1]);
                                if (property != null)
                                {
                                    var data = property.GetValue(dataObj);
                                    XmlNode newNode = GetNodeXml(innerNode, data);
                                    doc.ReplaceChild(newNode, innerNode);
                                }
                            }
                        }
                        //inserting only inner nodes of document no need to insert root tag
                        node.InnerXml += doc.DocumentElement.InnerXml;
                    }
                }
            }
        }
        private bool GenerateRepeatedNode(XmlAttribute attri, XmlNode node, object dataObj, bool checkChild)
        {
            var repeat = node.Attributes["repeat"];
            XmlNode templateNode = node.Clone();
            templateNode.Attributes.Remove(templateNode.Attributes["data-source"]);
            templateNode.Attributes.Remove(templateNode.Attributes["repeat"]);

            if (repeat != null && repeat.Value != "")
            {
                var dataSourceAttri = attri.Value.Split('~');
                var dataSourceProperty = dataObj.GetType().GetProperty(dataSourceAttri[1]);

                if (dataSourceProperty != null)
                {
                    if (repeat.Value == "true" && dataSourceProperty.PropertyType.IsGenericType && (dataSourceProperty.PropertyType.GetGenericTypeDefinition() == typeof(List<>) || dataSourceProperty.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)))
                    {
                        checkChild = false;

                        var listValuesProperty = dataObj.GetType().GetProperty(dataSourceAttri[1]);
                        if (listValuesProperty != null)
                        {
                            var listValues = listValuesProperty.GetValue(dataObj);
                            IList lstProp = listValues as IList;
                            if (lstProp != null)
                            {
                                foreach (var listItemDataObject in lstProp)
                                {
                                    XmlNode newNode = GetNodeXml(templateNode.Clone(), listItemDataObject);
                                    if (newNode != null)
                                    {
                                        node.ParentNode.InsertBefore(newNode, node);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var dataProperty = dataObj.GetType().GetProperty(dataSourceAttri[1]);
                        if (dataProperty != null)
                        {
                            var dataObject = dataProperty.GetValue(dataObj);
                            XmlNode newNode = GetNodeXml(templateNode.Clone(), dataObject);
                            if (newNode != null)
                            {
                                node.ParentNode.InsertBefore(newNode.Clone(), node);
                            }
                        }
                    }
                }
            }

            return checkChild;
        }

        private void ResolveShowNodeAttribute(XmlAttribute attri, XmlNode node, object dataObj)
        {
            var expression = attri.Value;
            if (expression.ToLower() != "true" && expression.ToLower() != "false")
            {
                //Evaluating runtime expression for show-node with data-object
                var attributeObject = node.Attributes["data-object"];
                if (attributeObject != null)
                {
                    var propName = attributeObject.Value.Split('~');
                    if (propName.Count() > 1)
                    {
                        var propValueProperty = dataObj.GetType().GetProperty(propName[1]);
                        if (propValueProperty != null)
                        {
                            object result = EvalExpression(expression, dataObj);
                            attri.Value = result.ToString();
                        }
                    }
                }
                //Evaluating runtime expression for show-node
                else
                {

                    object result = EvalExpression(expression, dataObj);
                    attri.Value = result.ToString();
                }
            }
            else
            {
                attri.Value = expression.ToLower();
            }
        }
        private void GenerateAttribute(XmlAttribute attri, XmlNode node, object dataObj)
        {
            //Handling expression on attribute value
            if (attri.Value.Length > 2) // This will address if Attribute Value has atleast 2 characters
            {
                if (attri.Value.Substring(0, 2) == "{{" && attri.Value.Substring(attri.Value.Length - 2, 2) == "}}")
                {
                    var attributeObject = node.Attributes["data-object"];
                    if (attributeObject != null)
                    {
                        var propName = attributeObject.Value.Split('~');
                        if (propName.Count() > 1)
                        {
                            var propValueProperty = dataObj.GetType().GetProperty(propName[1]);
                            if (propValueProperty != null)
                            {
                                string strExpression = attri.Value.Substring(2, attri.Value.Length - 4);
                                attri.Value = EvalExpression(strExpression, dataObj).ToString();
                            }
                        }
                    }
                    else
                    {
                        string strExpression = attri.Value.Substring(2, attri.Value.Length - 4);
                        attri.Value = EvalExpression(strExpression, dataObj).ToString();
                    }
                }
                //Handling simple value on attribute value
                else
                {
                    var propName = attri.Value.Split('~');
                    if (propName.Count() > 1)
                    {
                        var propValueProperty = dataObj.GetType().GetProperty(propName[1]);
                        if (propValueProperty != null)
                        {
                            var propValue = propValueProperty.GetValue(dataObj);
                            if (propValue != null)
                            {
                                attri.Value = propValue.ToString();
                            }
                            else
                            {
                                attri.Value = string.Empty;
                            }
                        }
                    }
                }
            }
        }

        private void GenerateCustomAttributeValueNode(XmlNode node, object dataObj)
        {
            var AttibuteName = node.InnerText;
            if (AttibuteName != "")
            {
                if (dataObj.GetType().GetProperty("CustomAttributes") != null)
                {
                    var customAttributesProperty = dataObj.GetType().GetProperty("CustomAttributes");
                    if (customAttributesProperty != null)
                    {
                        var customAttributes = customAttributesProperty.GetValue(dataObj);
                        List<KeyValuePair<string, string>> lstCustomAttributes = customAttributes as List<KeyValuePair<string, string>>;
                        if (lstCustomAttributes != null)
                        {
                            var propValue = lstCustomAttributes.Where(k => k.Key.Replace(" ", string.Empty).ToLower() == AttibuteName.ToLower()).FirstOrDefault();
                            if (propValue.Value != null && propValue.Value != "")
                            {
                                if (isToLower)
                                {
                                    node.InnerText = propValue.Value.ToString().ToLower();
                                }
                                else if (isToUpper)
                                {
                                    node.InnerText = propValue.Value.ToString().ToUpper();
                                }
                                else
                                {
                                    node.InnerText = propValue.Value;
                                }
                            }
                            else
                            {
                                if (isDefault)
                                {
                                    node.InnerText = defaultValue;
                                }
                                else
                                {
                                    node.InnerText = string.Empty;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                node.InnerText = string.Empty;
            }
        }

        private void GenerateExpressionNode(XmlNode node, object dataObj)
        {
            var expression = node.InnerText;
            if (expression.ToLower() != "true" && expression.ToLower() != "false")
            {
                //Evaluating runtime expression
                object result = EvalExpression(expression, dataObj);
                node.InnerText = result + "";

                if (result != null && node.ParentNode != null && node.ParentNode.Name.Equals("custom-attribute"))
                {
                    node.ParentNode.Attributes["show-node"].Value = "true";
                }
            }
            else
            {
                node.InnerText = expression.ToLower();
            }
        }
        private void GenerateBasicNode(XmlNode node, object dataObj)
        {
            var text = node.InnerText.Split('~');
            if (text.Count() > 1)
            {
                var propValueProperty = dataObj.GetType().GetProperty(text[1]);
                if (propValueProperty != null)
                {
                    var propValue = propValueProperty.GetValue(dataObj);
                    if (propValue != null)
                    {
                        if (isToLower)
                        {
                            node.InnerText = propValue.ToString().ToLower();
                        }
                        else if (isToUpper)
                        {
                            node.InnerText = propValue.ToString().ToUpper();
                        }
                        else
                        {
                            node.InnerText = propValue.ToString();
                        }
                    }
                    else
                    {
                        if (isDefault)
                        {
                            node.InnerText = defaultValue;
                        }
                        else
                        {
                            node.InnerText = string.Empty;
                        }
                    }
                }
            }
        }
        private void GenerateCustomAttribute(XmlNode node, object dataObj)
        {
            //Handling custom attributes from collection
            if (dataObj.GetType().GetProperty("Key") != null && dataObj.GetType().GetProperty("Value") != null)
            {
                var key = dataObj as KeyValuePair<string, string>?;
                if (key != null)
                {
                    node.InnerText = key.Value.Value;
                }
                else
                {
                    node.InnerText = string.Empty;
                }
            }
            //Handling custom attributes writen in XML template                            
            else
            {
                var key = node.ParentNode.Attributes["attribute-id"];
                if (key != null && dataObj.GetType().GetProperty("CustomAttributes") != null)
                {
                    var customAttributesProperty = dataObj.GetType().GetProperty("CustomAttributes");
                    if (customAttributesProperty != null)
                    {
                        var customAttributes = customAttributesProperty.GetValue(dataObj);
                        List<KeyValuePair<string, string>> lstCustomAttributes = customAttributes as List<KeyValuePair<string, string>>;
                        if (lstCustomAttributes != null)
                        {
                            //1-Expression will be resolved at isExpression area so no need to handle here

                            //2-for same attribute-id 
                            if (node.ParentNode != null && node.ParentNode.InnerText.ToLower().Equals(key.Value.ToLower()))
                            {
                                //Due to AX dependency that Change this line
                                //AX handles attributes as case sensetive so we need to handle them in mapping framework
                                var propValue = lstCustomAttributes.Where(k => k.Key.Replace(" ", string.Empty).ToLower() == key.Value.ToLower()).FirstOrDefault();
                                if (propValue.Value != null && propValue.Value != "")
                                {
                                    node.ParentNode.Attributes["show-node"].Value = "true";
                                    if (isToLower)
                                    {
                                        node.InnerText = propValue.Value.ToString().ToLower();
                                    }
                                    else if (isToUpper)
                                    {
                                        node.InnerText = propValue.Value.ToString().ToUpper();
                                    }
                                    else
                                    {
                                        node.InnerText = propValue.Value;
                                    }
                                }
                                else
                                {
                                    if (isDefault)
                                    {
                                        node.InnerText = defaultValue;
                                        node.ParentNode.Attributes["show-node"].Value = "true";
                                    }
                                    else
                                    {
                                        node.InnerText = string.Empty;
                                    }
                                }
                            }
                            //3-for differenet attribute-id
                            else if (node.ParentNode != null && node.ParentNode.InnerText.ToLower() != key.Value.ToLower())
                            {
                                //3.1-Handling property from data object
                                var text = node.InnerText.Split('~');
                                if (text.Count() > 1)
                                {
                                    var propValueProperty = dataObj.GetType().GetProperty(text[1]);
                                    if (propValueProperty != null)
                                    {
                                        var propValue = propValueProperty.GetValue(dataObj);
                                        if (propValue != null)
                                        {
                                            node.InnerText = propValue.ToString();
                                            node.ParentNode.Attributes["show-node"].Value = "true";
                                        }
                                        else
                                        {
                                            if (isDefault)
                                            {
                                                node.InnerText = defaultValue;
                                                node.ParentNode.Attributes["show-node"].Value = "true";
                                            }
                                            else
                                            {
                                                node.InnerText = string.Empty;
                                            }
                                        }
                                    }
                                }
                                //3.2-Handling different attribute-id & value
                                else
                                {
                                    var propValue = lstCustomAttributes.Where(k => k.Key.Replace(" ", string.Empty).ToLower() == text[0].ToLower()).FirstOrDefault();
                                    if (propValue.Value != null && propValue.Value != "")
                                    {
                                        node.ParentNode.Attributes["show-node"].Value = "true";
                                        if (isToLower)
                                        {
                                            node.InnerText = propValue.Value.ToString().ToLower();
                                        }
                                        else if (isToUpper)
                                        {
                                            node.InnerText = propValue.Value.ToString().ToUpper();
                                        }
                                        else
                                        {
                                            node.InnerText = propValue.Value;
                                        }
                                    }
                                    else
                                    {
                                        if (isDefault)
                                        {
                                            node.InnerText = defaultValue;
                                            node.ParentNode.Attributes["show-node"].Value = "true";
                                        }
                                        else
                                        {
                                            node.InnerText = string.Empty;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void processCDATA(string strFullFileNameWithPath)
        {
            string strFileText;

            strFileText = File.ReadAllText(strFullFileNameWithPath);

            strFileText = strFileText.Replace("<description>", "<description><![CDATA[");
            strFileText = strFileText.Replace("</description>", "]]></description>");

            strFileText = strFileText.Replace("<short_description>", "<short_description><![CDATA[");
            strFileText = strFileText.Replace("</short_description>", "]]></short_description>");

            strFileText = strFileText.Replace("<cl_config_body_content>", "<cl_config_body_content><![CDATA[");
            strFileText = strFileText.Replace("</cl_config_body_content>", "]]></cl_config_body_content>");

            strFileText = strFileText.Replace("<cl_config_header_content>", "<cl_config_header_content><![CDATA[");
            strFileText = strFileText.Replace("</cl_config_header_content>", "]]></cl_config_header_content>");

            strFileText = strFileText.Replace("<cart_description>", "<cart_description><![CDATA[");
            strFileText = strFileText.Replace("</cart_description>", "]]></cart_description>");

            strFileText = strFileText.Replace("<mobile_description>", "<mobile_description><![CDATA[");
            strFileText = strFileText.Replace("</mobile_description>", "]]></mobile_description>");

            strFileText = strFileText.Replace("<plp_description>", "<plp_description><![CDATA[");
            strFileText = strFileText.Replace("</plp_description>", "]]></plp_description>");

            strFileText = strFileText.Replace("&lt;", "<");
            strFileText = strFileText.Replace("&gt;", ">");

            File.WriteAllText(strFullFileNameWithPath, strFileText);
        }

        #endregion

        #region Generate Object


        public object GenerateObjectByTemplateFromDatabase(string fileNameWithInputPath, XmlSourceDirection sourceDirection, object outputObject)
        {
            string strMainClassName = outputObject.GetType().Name;
            string strDirection = sourceDirection == XmlSourceDirection.READ ? "READ." : "CREATE.";


            #region DB changes by usman raza on 30th Dec 2016

            string filename = strDirection + strMainClassName;

            MappingTemplate obj = mappingTemplateDAL.GetMappingTemplateByName(filename);

            #endregion


            try
            {
                if (obj != null)
                {
                    XmlDocument templateDoc = new XmlDocument();
                    var xmlDoc = XDocument.Parse(obj.XML);
                    templateDoc = ConvertToXmlDocument(xmlDoc);
                    XmlDocument dataDoc = new XmlDocument();
                    dataDoc.Load(fileNameWithInputPath);

                    var xmlTemplateNodesList = templateDoc.SelectNodes(@"//Targets/Target");

                    foreach (XmlNode templateNode in xmlTemplateNodesList)
                    {
                        InitializeProperty(templateNode, dataDoc, null, outputObject);
                    }
                }
                //if (File.Exists(xmlTemplate) && File.Exists(fileNameWithInputPath))
                //{
                //    XmlDocument templateDoc = new XmlDocument();
                //    templateDoc.Load(xmlTemplate);

                //    XmlDocument dataDoc = new XmlDocument();
                //    dataDoc.Load(fileNameWithInputPath);

                //    var xmlTemplateNodesList = templateDoc.SelectNodes(@"//Targets/Target");

                //    foreach (XmlNode templateNode in xmlTemplateNodesList)
                //    {
                //        InitializeProperty(templateNode, dataDoc, null, outputObject);
                //    }
                //}
                else
                {
                    throw new CommerceLinkError(string.Format("Unable to Load Template from database {0}", filename));
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, CreatedBy, "XmlTemplateEngine");
                throw new CommerceLinkError(string.Format("Unable to Load Template from database {0}", filename));
            }
            return outputObject;
        }

        public object GenerateObjectByTemplateFromFile(string fileNameWithInputPath, XmlSourceDirection sourceDirection, object outputObject)
        {
            string strMainClassName = outputObject.GetType().Name;
            string strDirection = sourceDirection == XmlSourceDirection.READ ? "READ." : "CREATE.";
            //string xmlTemplate = Path.Combine(ConfigurationHelper.XmlPath, strDirection + strMainClassName + ".xml");
            string xmlTemplate = Path.Combine(this.configurationHelper.GetDirectory(
                configurationHelper.GetSetting(APPLICATION.XML_Base_Path)), strDirection + strMainClassName + ".xml");
            try
            {
                if (File.Exists(xmlTemplate) && File.Exists(fileNameWithInputPath))
                {
                    XmlDocument templateDoc = new XmlDocument();
                    templateDoc.Load(xmlTemplate);

                    XmlDocument dataDoc = new XmlDocument();
                    dataDoc.Load(fileNameWithInputPath);

                    var xmlTemplateNodesList = templateDoc.SelectNodes(@"//Targets/Target");

                    foreach (XmlNode templateNode in xmlTemplateNodesList)
                    {
                        InitializeProperty(templateNode, dataDoc, null, outputObject);
                    }
                }
                else
                {
                    throw new CommerceLinkError(string.Format("Template not found {0}", xmlTemplate));
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, CreatedBy, "XmlTemplateEngine");
                throw new CommerceLinkError(string.Format("Template not found {0}", xmlTemplate));
            }
            return outputObject;
        }

        public object GenerateObjectByTemplateFromXMLFile(string fileNameWithInputPath, XmlSourceDirection sourceDirection, object outputObject)
        {
            string strMainClassName = outputObject.GetType().Name;
            string strDirection = sourceDirection == XmlSourceDirection.READ ? "READ." : "CREATE.";

            try
            {
                XmlDocument templateDoc = new XmlDocument();

                //Reading template from database
                if (System.Configuration.ConfigurationManager.AppSettings["IsLoadTemplateFromDatabase"] != null && System.Configuration.ConfigurationManager.AppSettings["IsLoadTemplateFromDatabase"] == "true")
                {
                    string filename = strDirection + strMainClassName;
                    MappingTemplate dbTemplate = mappingTemplateDAL.GetMappingTemplateByName(filename);

                    if (dbTemplate != null)
                    {
                        var xmlDoc = XDocument.Parse(dbTemplate.XML);
                        templateDoc = ConvertToXmlDocument(xmlDoc);
                    }
                    else
                    {
                        throw new CommerceLinkError(string.Format("Template not found on database {0}", filename));
                    }
                }
                //Reading template from system files
                else
                {
                    string xmlTemplate = Path.Combine(this.configurationHelper.GetDirectory(configurationHelper.GetSetting(APPLICATION.XML_Base_Path)), strDirection + strMainClassName + ".xml");

                    if (File.Exists(xmlTemplate) && File.Exists(fileNameWithInputPath))
                    {
                        templateDoc.Load(xmlTemplate);
                    }
                    else
                    {
                        throw new CommerceLinkError(string.Format("Template not found on file system {0}", xmlTemplate));
                    }
                }

                XmlDocument dataDoc = new XmlDocument();
                dataDoc.Load(fileNameWithInputPath);

                var xmlTemplateNodesList = templateDoc.SelectNodes(@"//Targets/Target");

                foreach (XmlNode templateNode in xmlTemplateNodesList)
                {
                    InitializeProperty(templateNode, dataDoc, null, outputObject);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, CreatedBy, "XmlTemplateEngine_GenerateObjectByTemplateFromXMLFile");
                throw new CommerceLinkError(string.Format("XmlTemplateEngine_GenerateObjectByTemplateFromXMLFile: Template not found in file system or database {0}", strMainClassName));
            }
            return outputObject;
        }

        public object GenerateObjectByTemplateFromXML(XmlDocument dataDoc, XmlSourceDirection sourceDirection, object outputObject)
        {
            string strMainClassName = outputObject.GetType().Name;
            string strDirection = sourceDirection == XmlSourceDirection.READ ? "READ." : "CREATE.";

            try
            {
                XmlDocument templateDoc = new XmlDocument();

                //Reading template from database
                if (System.Configuration.ConfigurationManager.AppSettings["IsLoadTemplateFromDatabase"] != null && System.Configuration.ConfigurationManager.AppSettings["IsLoadTemplateFromDatabase"] == "true")
                {
                    string filename = strDirection + strMainClassName;
                    MappingTemplate dbTemplate = mappingTemplateDAL.GetMappingTemplateByName(filename);

                    if (dbTemplate != null)
                    {
                        var xmlDoc = XDocument.Parse(dbTemplate.XML);
                        templateDoc = ConvertToXmlDocument(xmlDoc);
                    }
                    else
                    {
                        throw new CommerceLinkError(string.Format("Template not found on database {0}", filename));
                    }
                }
                //Reading template from system files
                else
                {
                    string xmlTemplate = Path.Combine(this.configurationHelper.GetDirectory(configurationHelper.GetSetting(APPLICATION.XML_Base_Path)), strDirection + strMainClassName + ".xml");

                    if (File.Exists(xmlTemplate))
                    {
                        templateDoc.Load(xmlTemplate);
                    }
                    else
                    {
                        throw new CommerceLinkError(string.Format("Template not found on file system {0}", xmlTemplate));
                    }
                }
                GenerateObjectByGivenTemplate(templateDoc, dataDoc, outputObject);
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, CreatedBy, "XmlTemplateEngine_GenerateObjectByTemplateFromXML");
                throw new CommerceLinkError(string.Format("XmlTemplateEngine_GenerateObjectByTemplateFromXML: Template not found in file system or database {0}", strMainClassName));
            }
            return outputObject;
        }

        private void GenerateObjectByGivenTemplate(XmlDocument templateDoc, XmlDocument dataDoc, object outputObject)
        {
            var xmlTemplateNodesList = templateDoc.SelectNodes(@"//Targets/Target");

            foreach (XmlNode templateNode in xmlTemplateNodesList)
            {
                InitializeProperty(templateNode, dataDoc, null, outputObject);
            }
        }
        public object GenerateObjectByTemplateIngram(XmlDocument dataDoc, XmlSourceDirection sourceDirection, object outputObject)
        {
            string strMainClassName = outputObject.GetType().Name;
            string strDirection = sourceDirection == XmlSourceDirection.READ ? "READ." : "CREATE.";

            try
            {
                XmlDocument templateDoc = new XmlDocument();

                //Reading template from database
                if (System.Configuration.ConfigurationManager.AppSettings["IsLoadTemplateFromDatabase"] != null && System.Configuration.ConfigurationManager.AppSettings["IsLoadTemplateFromDatabase"] == "true")
                {
                    string filename = strDirection + strMainClassName;
                    MappingTemplate dbTemplate = mappingTemplateDAL.GetMappingTemplateByName(filename);

                    if (dbTemplate != null)
                    {
                        var xmlDoc = XDocument.Parse(dbTemplate.IngramTemplate);
                        templateDoc = ConvertToXmlDocument(xmlDoc);
                    }
                    else
                    {
                        throw new CommerceLinkError(string.Format("Template not found on database {0}", filename));
                    }
                }
                //Reading template from system files
                else
                {
                    string xmlTemplate = Path.Combine(this.configurationHelper.GetDirectory(configurationHelper.GetSetting(APPLICATION.XML_Base_Path)), strDirection + strMainClassName + ".xml");

                    if (File.Exists(xmlTemplate))
                    {
                        templateDoc.Load(xmlTemplate);
                    }
                    else
                    {
                        throw new CommerceLinkError(string.Format("Template not found on file system {0}", xmlTemplate));
                    }
                }
                GenerateObjectByGivenTemplate(templateDoc, dataDoc, outputObject);
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, CreatedBy, "XmlTemplateEngine_GenerateObjectByTemplateFromXML");
                throw new CommerceLinkError(string.Format("XmlTemplateEngine_GenerateObjectByTemplateFromXML: Template not found in file system or database {0}", strMainClassName));
            }
            return outputObject;        
        }

        public static XmlDocument ConvertToXmlDocument(XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var reader = xDocument.CreateReader())
            {
                xmlDocument.Load(reader);
            }

            var xDeclaration = xDocument.Declaration;
            if (xDeclaration != null)
            {
                var xmlDeclaration = xmlDocument.CreateXmlDeclaration(
                    xDeclaration.Version,
                    xDeclaration.Encoding,
                    xDeclaration.Standalone);

                xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.FirstChild);
            }

            return xmlDocument;
        }
        private void InitializeProperty(XmlNode tempNode, XmlDocument dataDoc, XmlNode elementNode, object outputObject, string prevPath = "")
        {
            string strTargetProperty = string.Empty;
            string strTargetSourcePath = string.Empty;
            string strTargetSource = string.Empty;
            bool isCustomAttribute = false;
            string attributeId = string.Empty;
            string attributeName = string.Empty;
            bool isAttribute = false;
            bool isRepeat = false;
            string defaultValue = string.Empty;
            bool isConcatenate = false;
            bool isConstant = false;
            string constantValue = string.Empty;
            bool isDefault = false;

            try
            {
                //Reading template node attributes
                if (tempNode.Attributes.Count > 0)
                {
                    foreach (XmlAttribute attri in tempNode.Attributes)
                    {
                        if (attri.Name.Equals("property"))
                        {
                            string[] values = attri.Value.Split('~');
                            if (values.Count() > 1)
                            {
                                strTargetProperty = values[1];
                            }
                        }
                        else if (attri.Name.Equals("source-path"))
                        {
                            strTargetSourcePath = attri.Value;
                        }
                        else if (attri.Name.Equals("repeat"))
                        {
                            isRepeat = attri.Value.Equals("true") ? true : false;
                        }
                        else if (attri.Name.Equals("target-source"))
                        {
                            strTargetSource = attri.Value;
                        }
                        else if (attri.Name.Equals("is-custom-attribute"))
                        {
                            isCustomAttribute = attri.Value.Equals("true") ? true : false;
                        }
                        else if (attri.Name.Equals("attribute-id"))
                        {
                            attributeId = attri.Value;
                        }
                        else if (attri.Name.Equals("default-value"))
                        {
                            isDefault = true;
                            defaultValue = attri.Value;
                        }
                        else if (attri.Name.Equals("concatenate"))
                        {
                            isConcatenate = attri.Value.Equals("true") ? true : false;
                        }
                        else if (attri.Name.Equals("attribute-name"))
                        {
                            isAttribute = true;
                            attributeName = attri.Value;
                        }
                        else if (attri.Name.Equals("constant-value"))
                        {
                            isConstant = true;
                            constantValue = attri.Value;
                        }
                    }
                }

                if (strTargetSourcePath != "")
                {
                    //Getting node from data document
                    XmlNode dataNode;
                    if (elementNode == null && strTargetSourcePath != "same")
                    {
                        dataNode = dataDoc.SelectSingleNode(prevPath + strTargetSourcePath);
                    }
                    else
                    {
                        dataNode = elementNode.SelectSingleNode(strTargetSourcePath);
                    }

                    if (dataNode != null || strTargetSourcePath == "same")
                    {
                        if (isAttribute)
                        {
                            foreach (XmlAttribute attribute in dataNode.Attributes)
                            {
                                if (attribute.Name.Equals(attributeName))
                                {
                                    SetPropertyValue(outputObject, strTargetProperty, attribute.Value);
                                }
                            }
                        }
                        else if (isRepeat && strTargetSource != "")
                        {
                            //Creating List
                            object newDataObject = null;
                            newDataObject = CreateObjectByPropertyName(outputObject, strTargetProperty);

                            if (newDataObject != null)
                            {
                                //Getting data elements from data document
                                XmlNodeList lstRepeatedDataNodes;
                                if (elementNode != null)
                                {
                                    lstRepeatedDataNodes = elementNode.SelectNodes(strTargetSourcePath);
                                }
                                else
                                {
                                    lstRepeatedDataNodes = dataDoc.SelectNodes(strTargetSourcePath);
                                }
                                foreach (XmlNode node in lstRepeatedDataNodes)
                                {
                                    PropertyInfo propertyInfo = outputObject.GetType().GetProperty(strTargetProperty);
                                    if (propertyInfo != null)
                                    {
                                        //Creating object for list
                                        object newObject = GetNewObject(propertyInfo.PropertyType.GenericTypeArguments[0]);
                                        if (newObject != null)
                                        {
                                            XmlNodeList lstPropertyNodes = tempNode.SelectNodes(@"" + GetXPathToNode(tempNode) + "/Properties/Target");
                                            foreach (XmlNode tNode in lstPropertyNodes)
                                            {
                                                InitializeProperty(tNode, dataDoc, node, newObject, "");
                                            }
                                            //Adding object into list
                                            newDataObject.GetType().GetMethod("Add").Invoke(newDataObject, new[] { newObject });
                                        }
                                    }
                                }
                                AssignPropertyValueToList(outputObject, strTargetProperty, newDataObject);
                            }
                        }
                        else if (isRepeat == false && strTargetSource != "")
                        {
                            object newDataObject = null;
                            newDataObject = CreateObjectByPropertyName(outputObject, strTargetProperty);

                            if (newDataObject != null)
                            {
                                XmlNode dNode;
                                if (elementNode != null || strTargetSourcePath == "same")
                                {
                                    if (strTargetSourcePath == "same")
                                    {
                                        dNode = elementNode;
                                    }
                                    else
                                    {
                                        dNode = elementNode.SelectSingleNode(strTargetSourcePath);
                                    }
                                }
                                else
                                {
                                    dNode = dataDoc.SelectSingleNode(strTargetSourcePath);
                                }

                                XmlNodeList lstPropertyNodes = tempNode.SelectNodes(@"" + GetXPathToNode(tempNode) + "/Properties/Target");
                                foreach (XmlNode tNode in lstPropertyNodes)
                                {
                                    InitializeProperty(tNode, dataDoc, dNode, newDataObject, "");
                                }
                                SetPropertyValue(outputObject, strTargetProperty, newDataObject);
                            }
                        }
                        else if (isCustomAttribute)
                        {
                            List<KeyValuePair<string, string>> newCustomAttribute = new List<KeyValuePair<string, string>>();
                            //TODO: remove this check after deivery customization in DW
                            if (isConstant)
                            {
                                newCustomAttribute.Add(new KeyValuePair<string, string>(attributeId, constantValue));
                            }
                            else if (isDefault && string.IsNullOrEmpty(dataNode.InnerText))
                            {
                                newCustomAttribute.Add(new KeyValuePair<string, string>(attributeId, defaultValue));
                            }
                            else
                            {
                                //if (dataNode.InnerText.Equals(ConfigurationHelper.NullDeliveryDateConstant))
                                if (dataNode.InnerText.Equals(configurationHelper.GetSetting(SALESORDER.Delivery_Date_Null_Constant)))
                                {
                                    newCustomAttribute.Add(new KeyValuePair<string, string>(attributeId, null));
                                }
                                else
                                {
                                    newCustomAttribute.Add(new KeyValuePair<string, string>(attributeId, dataNode.InnerText));
                                }
                            }
                            AssignPropertyValueToList(outputObject, "CustomAttributes", newCustomAttribute);
                        }
                        else
                        {
                            string propertyValue = dataNode.InnerText != "" ? dataNode.InnerText : defaultValue;
                            SetPropertyValue(outputObject, strTargetProperty, propertyValue, isConcatenate);
                        }
                    }
                    //If data node does not exist then just add empty custom attribute
                    else if (isCustomAttribute)
                    {
                        List<KeyValuePair<string, string>> newCustomAttribute = new List<KeyValuePair<string, string>>();
                        if (isConstant)
                        {
                            newCustomAttribute.Add(new KeyValuePair<string, string>(attributeId, constantValue));
                        }
                        else if (isDefault)
                        {
                            newCustomAttribute.Add(new KeyValuePair<string, string>(attributeId, defaultValue));
                        }
                        else
                        {
                            newCustomAttribute.Add(new KeyValuePair<string, string>(attributeId, ""));
                        }
                        AssignPropertyValueToList(outputObject, "CustomAttributes", newCustomAttribute);
                    }
                }
                else if (isConstant)
                {
                    if (isCustomAttribute)
                    {
                        List<KeyValuePair<string, string>> newCustomAttribute = new List<KeyValuePair<string, string>>();
                        if (isConstant)
                        {
                            newCustomAttribute.Add(new KeyValuePair<string, string>(attributeId, constantValue));
                        }
                        AssignPropertyValueToList(outputObject, "CustomAttributes", newCustomAttribute);
                    }
                    else
                    {
                        SetPropertyValue(outputObject, strTargetProperty, constantValue, isConcatenate);
                    }
                }
                //Handling attribute from current Node without source path
                else if (isAttribute)
                {
                    string strValue = "";
                    if (elementNode != null)
                    {
                        XmlAttribute attribute = elementNode.Attributes[attributeName];
                        strValue = (attribute != null) ? attribute.Value : "";
                    }
                    SetPropertyValue(outputObject, strTargetProperty, strValue, isConcatenate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                strTargetProperty = string.Empty;
                strTargetSourcePath = string.Empty;
                strTargetSource = string.Empty;
                isCustomAttribute = false;
                attributeId = string.Empty;
                attributeName = string.Empty;
                isAttribute = false;
                isRepeat = false;
                isDefault = false;
                defaultValue = string.Empty;
                isConcatenate = false;
                isConstant = false;
                constantValue = string.Empty;
            }
        }

        private void SetPropertyValue(object outputObject, string propertyName, object propertyValue, bool concatenateValue = false)
        {
            try
            {
                PropertyInfo propertyInfo = outputObject.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {

                    if (propertyInfo.PropertyType.Name == "Nullable`1")
                    {
                        var proType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                        if (proType.Name == "DateTimeOffset")
                        {
                            //TODO: remove this check after deivery customization in DW
                            //if (propertyValue.Equals(ConfigurationHelper.NullDeliveryDateConstant))
                            if (propertyValue.Equals(configurationHelper.GetSetting(SALESORDER.Delivery_Date_Null_Constant)))
                            {
                                //Do nothing
                            }
                            else
                            {
                                propertyInfo.SetValue(outputObject, DateTimeOffset.Parse(propertyValue.ToString()), null);
                            }
                        }
                        else if (proType.Name == "Int32")
                        {
                            propertyInfo.SetValue(outputObject, Convert.ToInt32(propertyValue.ToString()), null);
                        }
                        else if (proType.Name == "Int64")
                        {
                            propertyInfo.SetValue(outputObject, Convert.ToInt64(propertyValue.ToString()), null);
                        }
                        else if (proType.Name == "Decimal")
                        {
                            propertyInfo.SetValue(outputObject, Convert.ToDecimal(propertyValue.ToString()), null);
                        }
                    }
                    else if (propertyInfo.PropertyType.Name == "DateTimeOffset")
                    {
                        //TODO: remove this check after deivery customization in DW
                        //if (propertyValue.Equals(ConfigurationHelper.NullDeliveryDateConstant))
                        if (propertyValue.Equals(configurationHelper.GetSetting(SALESORDER.Delivery_Date_Null_Constant)))
                        {
                            //Do nothing
                        }
                        else
                        {
                            propertyInfo.SetValue(outputObject, DateTimeOffset.Parse(propertyValue.ToString()), null);
                        }
                    }
                    else if (propertyInfo.PropertyType.IsEnum)
                    {
                        propertyInfo.SetValue(outputObject, Enum.Parse(propertyInfo.PropertyType, propertyValue.ToString()), null);
                    }
                    else if (concatenateValue)
                    {
                        var propOldValue = propertyInfo.GetValue(outputObject, null);
                        propertyInfo.SetValue(outputObject, Convert.ChangeType(propOldValue + " " + propertyValue, propertyInfo.PropertyType), null);
                    }
                    else
                    {
                        if (propertyInfo.PropertyType.FullName.Equals("System.String"))
                        {
                            propertyValue = HttpUtility.HtmlDecode(propertyValue.ToString());
                        }
                        propertyInfo.SetValue(outputObject, Convert.ChangeType(propertyValue, propertyInfo.PropertyType), null);
                    }

                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, CreatedBy, "XmlTemplateEngine");
                throw ex;
            }
        }

        private void AssignPropertyValueToList(object outputObject, string propertyName, object propertyValue)
        {
            try
            {
                PropertyInfo propertyInfo = outputObject.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    var value = propertyInfo.GetValue(outputObject, null);
                    IList list = value as IList;
                    if (list != null)
                    {
                        if (list.Count > 0)
                        {
                            IList proList = propertyValue as IList;
                            foreach (var item in proList)
                            {
                                list.Add(item);
                            }
                        }
                    }
                    else
                    {
                        value = propertyValue;
                    }
                    propertyInfo.SetValue(outputObject, value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private object CreateObjectByPropertyName(object outputObject, string propertyName)
        {
            try
            {
                PropertyInfo propertyInfo = outputObject.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    if (propertyInfo.PropertyType.IsGenericType)
                    {
                        if (propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(List<>) || propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) || propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                        {
                            return GetNewListObject(propertyInfo.PropertyType.GenericTypeArguments[0]);
                        }
                        return null;
                    }
                    else
                    {
                        return GetNewObject(propertyInfo.PropertyType);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //return null;
            }
        }

        public object GetNewObject(Type t)
        {
            try
            {
                return t.GetConstructor(new Type[] { }).Invoke(new object[] { });
            }
            catch (Exception ex)
            {
                throw ex;
                //return null;
            }
        }

        private IList GetNewListObject(Type t)
        {
            try
            {
                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(t);
                var instance = Activator.CreateInstance(constructedListType);
                return (IList)instance;
            }
            catch (Exception ex)
            {
                throw ex;
                //return null;
            }
        }

        public string GetXPathToNode(XmlNode node, bool withoutIndexes = false)
        {
            if (node.NodeType == XmlNodeType.Attribute)
            {
                // attributes have an OwnerElement, not a ParentNode; also they have             
                // to be matched by name, not found by position             
                return String.Format("{0}/@{1}", GetXPathToNode(((XmlAttribute)node).OwnerElement), node.Name);
            }
            if (node.ParentNode == null)
            {
                // the only node with no parent is the root node, which has no path
                return "";
            }

            // Get the Index
            int indexInParent = 1;
            XmlNode siblingNode = node.PreviousSibling;
            // Loop thru all Siblings
            while (siblingNode != null)
            {
                // Increase the Index if the Sibling has the same Name
                if (siblingNode.Name == node.Name)
                {
                    indexInParent++;
                }
                siblingNode = siblingNode.PreviousSibling;
            }

            if (!withoutIndexes)
            {
                // the path to a node is the path to its parent, plus "/node()[n]", where n is its position among its siblings.         
                return String.Format("{0}/{1}[{2}]", GetXPathToNode(node.ParentNode), node.Name, indexInParent);
            }
            else
            {
                return String.Format("{0}/{1}", GetXPathToNode(node.ParentNode), node.Name);
            }
        }

        #endregion

        public enum XmlSourceDirection
        {
            CREATE = 1,
            READ = 2
        }

        private List<EvalFunction> EvalFunctions = new List<EvalFunction>();

        private object EvalExpression(string sExpression, object sourceObject)
        {
            try
            {
                EvalFunction function = EvalFunctions.Where(f => f.DataObjectName == sourceObject.GetType().FullName && f.Expression == sExpression).FirstOrDefault();

                if (function == null)
                {
                    CSharpCodeProvider c = new CSharpCodeProvider();
                    CompilerParameters cp = new CompilerParameters();

                    cp.ReferencedAssemblies.Add("system.dll");
                    cp.ReferencedAssemblies.Add("system.core.dll");
                    //Remove ECom so no need this line
                    //cp.ReferencedAssemblies.Add(sourceObject.GetType().Assembly.ToString().Split(',')[0] + ".dll");
                    var location = System.Reflection.Assembly.GetEntryAssembly().Location;
                    var directoryPath = Path.GetDirectoryName(location);
                    cp.ReferencedAssemblies.Add(Path.Combine(directoryPath, "VSI.EDGEAXConnector.ERPDataModels.dll"));
                    cp.ReferencedAssemblies.Add(Path.Combine(directoryPath, "VSI.EDGEAXConnector.Configuration.dll"));
                    cp.ReferencedAssemblies.Add(Path.Combine(directoryPath, "VSI.EDGEAXConnector.Data.dll"));

                    cp.CompilerOptions = "/t:library";
                    cp.GenerateInMemory = true;

                    StringBuilder sb = new StringBuilder("");
                    sb.Append("using System;\n");
                    sb.Append("using System.Linq;\n");
                    //Remove ECom so no need this line
                    //sb.Append("using " + sourceObject.GetType().Assembly.ToString().Split(',')[0] + ";\n");
                    sb.Append("using VSI.EDGEAXConnector.ERPDataModels;\n");
                    sb.Append("using VSI.EDGEAXConnector.Configuration;\n");
                    sb.Append("using VSI.EDGEAXConnector.Data;\n");

                    sb.Append("namespace CSCodeEvaler{ \n");
                    sb.Append("public class CSCodeEvaler{ \n");
                    sb.Append("public object EvalCode(" + sourceObject.GetType().FullName + " sourceObject){\n");
                    sb.Append("return (" + sExpression + "); \n");
                    sb.Append("} \n");
                    sb.Append("} \n");
                    sb.Append("}\n");

                    CompilerResults cr = c.CompileAssemblyFromSource(cp, sb.ToString());
                    if (cr.Errors.Count > 0)
                    {
                        throw new InvalidExpressionException(
                            string.Format("Error ({0}) evaluating runtime expression: {1}",
                            cr.Errors[0].ErrorText, sExpression));
                    }

                    System.Reflection.Assembly a = cr.CompiledAssembly;
                    object o = a.CreateInstance("CSCodeEvaler.CSCodeEvaler");

                    Type t = o.GetType();
                    MethodInfo mi = t.GetMethod("EvalCode");

                    object s = mi.Invoke(o, new[] { sourceObject });

                    //Store function before exit
                    EvalFunction func = new EvalFunction();
                    func.DataObjectName = sourceObject.GetType().FullName;
                    func.Expression = sExpression;
                    func.CompileResultsObject = o;
                    func.TypeObject = t;

                    EvalFunctions.Add(func);

                    return s;
                }
                else
                {
                    object o = function.CompileResultsObject;
                    Type t = function.TypeObject;
                    MethodInfo mi = t.GetMethod("EvalCode");

                    object s = mi.Invoke(o, new[] { sourceObject });
                    return s;
                }
            }
            catch (Exception ex)
            {
                //TODO: Uncomment this line to see actual exception in expression
                //CustomLogger.LogException(ex, "XmlTemplateEngine");
                return "EvalExpression:Exception";
            }
        }
    }

    public class EvalFunction
    {
        public string DataObjectName { get; set; }
        public string Expression { get; set; }

        public Type TypeObject { get; set; }

        public object CompileResultsObject { get; set; }
    }
}
