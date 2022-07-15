using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VSI.EDGEAXConnector.Common
{
    public  class ObjectToCsvConverter
    {

        /// <summary>
        /// Function that converts an object array into csv depending upon the configurations found in xml file
        /// </summary>
        /// <param name="objectArrayToBeTransformed"></param>
        /// <param name="strTemplateXml"></param>
        /// <param name="strXmlConfigurationElementName">Use this parameter when there are more than one mapping in one mapping file</param>
        /// <param name="strCsvFileDirectory"></param>
        /// <param name="strCsvFileName"></param>
        /// <returns></returns>
        public  string ConvertObjectToCsv(Object[] objectArrayToBeTransformed, String strTemplateXml, 
            String strCsvFileDirectory, String strCsvFileName, Boolean oneMappingElementPerFile, String strXmlConfigurationElementName)
        {
            String strTopRowHeader = string.Empty;

            // If input is not valid return false
            if (validateInput(objectArrayToBeTransformed, strTemplateXml, strCsvFileDirectory) == false)
            {
                Console.WriteLine("Input is invalid");
                return null;
            }

            List<String> listCSV = new List<String>();
            if (oneMappingElementPerFile)
            {
                if (objectArrayToBeTransformed.Length > 0)
                {
                    strTopRowHeader = getHeadersAgainstObjectAndTemplateFile(objectArrayToBeTransformed[0], strTemplateXml);
                }
                else
                {
                    strTopRowHeader = getHeadersAgainstObjectAndTemplateFile(null, strTemplateXml);
                }
            }
            else
            {
                strTopRowHeader = getHeadersAgainstObjectAndTemplateFile(objectArrayToBeTransformed[0], strTemplateXml, strXmlConfigurationElementName);
            }

            if (String.IsNullOrEmpty(strTopRowHeader))
            {
                Console.WriteLine("Headers were returned empty");
                return null;
            }
            else
            {
                if (oneMappingElementPerFile)
                {
                    listCSV = getRowsForCSV(objectArrayToBeTransformed, strTemplateXml, strTopRowHeader.Split(',').Length);
                }
                else
                {
                    listCSV = getRowsForCSV(objectArrayToBeTransformed, strTemplateXml, strTopRowHeader.Split(',').Length, strXmlConfigurationElementName);
                }
            }
            if (listCSV != null)
            {
                listCSV.Insert(0, strTopRowHeader);
            }
            convertListOfStringTOCSV(listCSV, strCsvFileDirectory + strCsvFileName);
            return String.Join("\n", listCSV);
        }

        /// <summary>
        /// validate inputs
        /// </summary>
        /// <param name="objectArrayToBeTransformed"></param>
        /// <param name="strTemplateXml"></param>
        /// <param name="strCsvFileDirectory"></param>
        /// <returns></returns>
        private  Boolean validateInput(Object[] objectArrayToBeTransformed, String strTemplateXml, String strCsvFileDirectory)
        {
            #region objectArrayToBeTransformed null validations
            if (objectArrayToBeTransformed == null)
            {
                return false;
            }
            #endregion

            // if objectArrayToBeTransformed contains no element in it then the file with headers only will be generated
            if (objectArrayToBeTransformed.Length != 0)
            {
                #region objectArrayToBeTransformed object array validations
                for (int i = 0; i < objectArrayToBeTransformed.Length; i++)
                {
                    if (i != objectArrayToBeTransformed.Length - 1 && objectArrayToBeTransformed[i] != null && objectArrayToBeTransformed[i + 1] != null)
                    {
                        if (objectArrayToBeTransformed[i].GetType() != objectArrayToBeTransformed[i + 1].GetType())
                        {
                            return false;
                        }
                    }
                }
                #endregion
            }

            #region strTemplateXml validations
            if (String.IsNullOrEmpty(strTemplateXml))
            {
                return false;
            }
            #endregion

            #region strCsvFileDirectory validations
            if (strCsvFileDirectory == null)
            {
                return false;
            }

            try
            {
                if (!Directory.Exists(strCsvFileDirectory))
                {
                    Directory.CreateDirectory(strCsvFileDirectory);
                }
            }
            catch (Exception)
            {
                return false;
            }
            #endregion

            return true;
        }

        /// <summary>
        /// Read the template file and get the headers for the csv
        /// </summary>
        /// <param name="objecToBeTransofrmedIntoCSV"></param>
        /// <param name="strTemplateXml"></param>
        /// <returns></returns>
        private  String getHeadersAgainstObjectAndTemplateFile(Object objecToBeTransofrmedIntoCSV, String strTemplateXml,
            String strXmlConfigurationElementName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(strTemplateXml);
            XmlElement xmlElement = (XmlElement)xmlDocument.GetElementsByTagName("ObjectToCSVMapping")[0];
            XmlNodeList objectInfoNodeList = xmlElement.GetElementsByTagName("ObjectInfo");

            String strHeader = String.Empty;

            for (int i = 0; i < objectInfoNodeList.Count; i++)
            {
                if (objectInfoNodeList[i].Attributes["fullyQualifiedName"] != null && objectInfoNodeList[i].Attributes["fullyQualifiedName"].Value == objecToBeTransofrmedIntoCSV.GetType().ToString() &&
                    objectInfoNodeList[i].Attributes["objectCsvMappingName"] != null && objectInfoNodeList[i].Attributes["objectCsvMappingName"].Value == strXmlConfigurationElementName)
                {
                    XmlElement csvHeaderXmlElement = (XmlElement)(((XmlElement)objectInfoNodeList[i]).GetElementsByTagName("CsvHeader")[0]);

                    XmlNodeList headerItemNodeList = csvHeaderXmlElement.GetElementsByTagName("HeaderItem");

                    for (int j = 0; j < headerItemNodeList.Count; j++)
                    {
                        strHeader = strHeader + ((XmlElement)headerItemNodeList[j]).Attributes["name"].Value;

                        if (j != headerItemNodeList.Count - 1)
                        {
                            strHeader = strHeader + ","; 
                        }
                    }
                    break; 
                }
            }
            return strHeader;
        }

        /// <summary>
        /// Read the template file and get the headers for the csv
        /// </summary>
        /// <param name="objecToBeTransofrmedIntoCSV"></param>
        /// <param name="strTemplateXml"></param>
        /// <returns></returns>
        private  String getHeadersAgainstObjectAndTemplateFile(Object objecToBeTransofrmedIntoCSV, String strTemplateXml)
        {            
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(strTemplateXml);
            XmlElement xmlElement = (XmlElement)xmlDocument.GetElementsByTagName("ObjectToCSVMapping")[0];
            XmlNodeList objectInfoNodeList = xmlElement.GetElementsByTagName("ObjectInfo");
            String strHeader = String.Empty;

            for (int i = 0; i < objectInfoNodeList.Count; i++)
            {
                if (objecToBeTransofrmedIntoCSV != null)
                {
                    if (objectInfoNodeList[i].Attributes["fullyQualifiedName"] != null &&
                        objectInfoNodeList[i].Attributes["fullyQualifiedName"].Value == objecToBeTransofrmedIntoCSV.GetType().ToString())
                    {
                        XmlElement csvHeaderXmlElement = (XmlElement)(((XmlElement)objectInfoNodeList[i]).GetElementsByTagName("CsvHeader")[0]);
                        XmlNodeList headerItemNodeList = csvHeaderXmlElement.GetElementsByTagName("HeaderItem");
                        for (int j = 0; j < headerItemNodeList.Count; j++)
                        {
                            strHeader = strHeader + ((XmlElement)headerItemNodeList[j]).Attributes["name"].Value;

                            if (j != headerItemNodeList.Count - 1)
                            {
                                strHeader = strHeader + ",";
                            }
                        }
                        break; // Break the loop if the elemnent is found
                    }
                }
                else
                {
                    if (objectInfoNodeList[i].Attributes["fullyQualifiedName"] != null)
                    {
                        XmlElement csvHeaderXmlElement = (XmlElement)(((XmlElement)objectInfoNodeList[i]).GetElementsByTagName("CsvHeader")[0]);
                        XmlNodeList headerItemNodeList = csvHeaderXmlElement.GetElementsByTagName("HeaderItem");
                        for (int j = 0; j < headerItemNodeList.Count; j++)
                        {
                            strHeader = strHeader + ((XmlElement)headerItemNodeList[j]).Attributes["name"].Value;

                            if (j != headerItemNodeList.Count - 1)
                            {
                                strHeader = strHeader + ",";
                            }
                        }

                        break; // Break the loop if the elemnent is found
                    }
                }
            }

            return strHeader;
        }

        /// <summary>
        /// Get the rows for CSV
        /// </summary>
        /// <param name="objectListToBeTransofrmedIntoCSV"></param>
        /// <param name="strObjectToCSVTemplateFileLocation"></param>
        /// <param name="iNoOfCSVColumns"></param>
        /// <returns></returns>
        private  List<String> getRowsForCSV(Object[] objectListToBeTransofrmedIntoCSV, string strObjectToCSVTemplateFileLocation, int iNoOfCSVColumns, string strXmlConfigurationElementName)
        {
            List<String> listCSVRows = new List<string>();

            // If objectListToBeTransofrmedIntoCSV is null or does not contain anything in it then return from here.
            if (objectListToBeTransofrmedIntoCSV == null || objectListToBeTransofrmedIntoCSV.Length <= 0)
            {
                return listCSVRows;
            }

            XmlNode objectInfoXmlNodeContainingObjectToConvertIntoCSV = GetNodeThatContainsObjectForTransformation(objectListToBeTransofrmedIntoCSV[0], false, objectListToBeTransofrmedIntoCSV[0].GetType(),
                strObjectToCSVTemplateFileLocation, strXmlConfigurationElementName);

            XmlNodeList dataMemberNodeList = ((XmlElement)objectInfoXmlNodeContainingObjectToConvertIntoCSV).GetElementsByTagName("DataMember");

            for (int iCollectionCounter = 0; iCollectionCounter < objectListToBeTransofrmedIntoCSV.Length; iCollectionCounter++)
            {
                #region Iterate on every object to be coverted into row of CSV
                List<String> listValuesInCorrespondingIndex = InitializeListOfString(iNoOfCSVColumns);

                for (int i = 0; i < dataMemberNodeList.Count; i++)
                {
                    #region iterate on every column of the csv  and get the value against that cell
                    if (((XmlElement)dataMemberNodeList[i]).Attributes["isPrimitveType"] != null && ((XmlElement)dataMemberNodeList[i]).Attributes["isPrimitveType"].Value == "true")
                    {
                        string strValueOfDataMember = getValueOfPrimitiveType(objectListToBeTransofrmedIntoCSV[iCollectionCounter],
                            ((XmlElement)dataMemberNodeList[i]).Attributes["instanceName"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["instanceName"].Value);

                        string strOrderOfDataMemberInCSV = ((XmlElement)dataMemberNodeList[i]).Attributes["headerItemOrder"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["headerItemOrder"].Value;
                        //string strIsConditional = ((XmlElement)dataMemberNodeList[i]).Attributes["isConditional"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["isConditional"].Value;

                        // If defaultValue is set then set the default value in CSV against relevant cell. Otherwise set the value returned from the relevant object data member
                        if (((XmlElement)dataMemberNodeList[i]).Attributes["defaultValue"] != null)
                        {
                            listValuesInCorrespondingIndex[Convert.ToInt32(strOrderOfDataMemberInCSV)] = ((XmlElement)dataMemberNodeList[i]).Attributes["defaultValue"].Value;
                        }
                        else
                        {
                            listValuesInCorrespondingIndex[Convert.ToInt32(strOrderOfDataMemberInCSV)] = "\"" + strValueOfDataMember + "\""; // Using "" on start and end will not break even if the cell value contains ,
                        }
                    }
                    else if (((XmlElement)dataMemberNodeList[i]).Attributes["isPrimitveType"] != null && ((XmlElement)dataMemberNodeList[i]).Attributes["isPrimitveType"].Value == "false")
                    {
                        if (((XmlElement)dataMemberNodeList[i]).Attributes["isCollectionObject"] != null && ((XmlElement)dataMemberNodeList[i]).Attributes["isCollectionObject"].Value == "false")
                        {
                            string strValueOfDataMember = getValueOfNonPrimitiveType(objectListToBeTransofrmedIntoCSV[iCollectionCounter],
                                ((XmlElement)dataMemberNodeList[i]).Attributes["className"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["className"].Value,
                                ((XmlElement)dataMemberNodeList[i]).Attributes["instanceName"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["instanceName"].Value,
                                ((XmlElement)dataMemberNodeList[i]).Attributes["desiredDataMember"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["desiredDataMember"].Value);
                            string strOrderOfDataMemberInCSV = ((XmlElement)dataMemberNodeList[i]).Attributes["headerItemOrder"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["headerItemOrder"].Value;

                            listValuesInCorrespondingIndex[Convert.ToInt32(strOrderOfDataMemberInCSV)] = "\"" + strValueOfDataMember + "\"";  // Using "" on start and end will not break even if the cell value contains ,
                        }
                        else if (((XmlElement)dataMemberNodeList[i]).Attributes["isCollectionObject"] != null && ((XmlElement)dataMemberNodeList[i]).Attributes["isCollectionObject"].Value == "true")
                        {
                            string strValueOfDataMember = getValueOfNonPrimitiveTypeOfCollection(objectListToBeTransofrmedIntoCSV[iCollectionCounter],
                                ((XmlElement)dataMemberNodeList[i]).Attributes["className"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["className"].Value,
                                ((XmlElement)dataMemberNodeList[i]).Attributes["instanceName"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["instanceName"].Value,
                                ((XmlElement)dataMemberNodeList[i]).Attributes["desiredCollectionIndex"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["desiredCollectionIndex"].Value,
                                ((XmlElement)dataMemberNodeList[i]).Attributes["desiredDataMember"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["desiredDataMember"].Value,
                                ((XmlElement)dataMemberNodeList[i]).Attributes["desiredCollectionDelimiter"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["desiredCollectionDelimiter"].Value

                                );
                            string strOrderOfDataMemberInCSV = ((XmlElement)dataMemberNodeList[i]).Attributes["headerItemOrder"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["headerItemOrder"].Value;

                            listValuesInCorrespondingIndex[Convert.ToInt32(strOrderOfDataMemberInCSV)] = "\"" + strValueOfDataMember + "\"";  // Using "" on start and end will not break even if the cell value contains ,
                        }
                    }
                    #endregion

                }

                String strCSVRow = makeRow(listValuesInCorrespondingIndex, ",");

                listCSVRows.Add(strCSVRow);
                #endregion
            }


            return listCSVRows;
        }

        /// <summary>
        /// Get the rows for CSV
        /// </summary>
        /// <param name="objectListToBeTransofrmedIntoCSV"></param>
        /// <param name="strTemplateXml"></param>
        /// <param name="iNoOfCSVColumns"></param>
        /// <returns></returns>
        private  List<String> getRowsForCSV(Object[] objectListToBeTransofrmedIntoCSV, string strTemplateXml, int iNoOfCSVColumns)
        {
            List<String> listCSVRows = new List<string>();

            // If objectListToBeTransofrmedIntoCSV is null or does not contain anything in it then return from here.
            if (objectListToBeTransofrmedIntoCSV == null || objectListToBeTransofrmedIntoCSV.Length <=0 )
            {
                return listCSVRows;
            }

            XmlNode objectInfoXmlNodeContainingObjectToConvertIntoCSV = GetNodeThatContainsObjectForTransformation(objectListToBeTransofrmedIntoCSV[0], false, objectListToBeTransofrmedIntoCSV[0].GetType(),
                strTemplateXml);

            XmlNodeList dataMemberNodeList = ((XmlElement)objectInfoXmlNodeContainingObjectToConvertIntoCSV).GetElementsByTagName("DataMember");


            for (int iCollectionCounter = 0; iCollectionCounter < objectListToBeTransofrmedIntoCSV.Length; iCollectionCounter++)
            {
                #region Iterate on every object to be coverted into row of CSV
                List<String> listValuesInCorrespondingIndex = InitializeListOfString(iNoOfCSVColumns);

                for (int i = 0; i < dataMemberNodeList.Count; i++)
                {
                    #region iterate on every column of the csv  and get the value against that cell
                    if (((XmlElement)dataMemberNodeList[i]).Attributes["isPrimitveType"] != null && ((XmlElement)dataMemberNodeList[i]).Attributes["isPrimitveType"].Value == "true")
                    {
                        string strValueOfDataMember = getValueOfPrimitiveType(objectListToBeTransofrmedIntoCSV[iCollectionCounter],
                            ((XmlElement)dataMemberNodeList[i]).Attributes["instanceName"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["instanceName"].Value);

                        string strOrderOfDataMemberInCSV = ((XmlElement)dataMemberNodeList[i]).Attributes["headerItemOrder"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["headerItemOrder"].Value;
                        //string strIsConditional = ((XmlElement)dataMemberNodeList[i]).Attributes["isConditional"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["isConditional"].Value;

                        // If defaultValue is set then set the default value in CSV against relevant cell. Otherwise set the value returned from the relevant object data member
                        if (((XmlElement)dataMemberNodeList[i]).Attributes["defaultValue"] != null)
                        {
                            listValuesInCorrespondingIndex[Convert.ToInt32(strOrderOfDataMemberInCSV)] = ((XmlElement)dataMemberNodeList[i]).Attributes["defaultValue"].Value;
                        }
                        else
                        {
                            listValuesInCorrespondingIndex[Convert.ToInt32(strOrderOfDataMemberInCSV)] = "\"" + strValueOfDataMember + "\""; // Using "" on start and end will not break even if the cell value contains ,
                        }

                        //if ( !String.IsNullOrEmpty(strIsConditional) && strIsConditional.Trim().ToLower() == "true")
                        //{
                        //    string strCondition = ((XmlElement)dataMemberNodeList[i]).Attributes["condition"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["condition"].Value;

                        //    strCondition = strCondition.Replace("'", "\"");

                        //    listValuesInCorrespondingIndex[Convert.ToInt32(strOrderOfDataMemberInCSV)] = 
                        //}


                    }
                    else if (((XmlElement)dataMemberNodeList[i]).Attributes["isPrimitveType"] != null && ((XmlElement)dataMemberNodeList[i]).Attributes["isPrimitveType"].Value == "false")
                    {
                        if (((XmlElement)dataMemberNodeList[i]).Attributes["isCollectionObject"] != null && ((XmlElement)dataMemberNodeList[i]).Attributes["isCollectionObject"].Value == "false")
                        {
                            string strValueOfDataMember = getValueOfNonPrimitiveType(objectListToBeTransofrmedIntoCSV[iCollectionCounter],
                                ((XmlElement)dataMemberNodeList[i]).Attributes["className"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["className"].Value,
                                ((XmlElement)dataMemberNodeList[i]).Attributes["instanceName"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["instanceName"].Value,
                                ((XmlElement)dataMemberNodeList[i]).Attributes["desiredDataMember"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["desiredDataMember"].Value);
                            string strOrderOfDataMemberInCSV = ((XmlElement)dataMemberNodeList[i]).Attributes["headerItemOrder"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["headerItemOrder"].Value;

                            listValuesInCorrespondingIndex[Convert.ToInt32(strOrderOfDataMemberInCSV)] = "\"" + strValueOfDataMember + "\"";  // Using "" on start and end will not break even if the cell value contains ,
                        }
                        else if (((XmlElement)dataMemberNodeList[i]).Attributes["isCollectionObject"] != null && ((XmlElement)dataMemberNodeList[i]).Attributes["isCollectionObject"].Value == "true")
                        {
                            string strValueOfDataMember = getValueOfNonPrimitiveTypeOfCollection(objectListToBeTransofrmedIntoCSV[iCollectionCounter],
                                ((XmlElement)dataMemberNodeList[i]).Attributes["className"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["className"].Value,
                                ((XmlElement)dataMemberNodeList[i]).Attributes["instanceName"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["instanceName"].Value,
                                ((XmlElement)dataMemberNodeList[i]).Attributes["desiredCollectionIndex"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["desiredCollectionIndex"].Value,
                                ((XmlElement)dataMemberNodeList[i]).Attributes["desiredDataMember"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["desiredDataMember"].Value,
                                ((XmlElement)dataMemberNodeList[i]).Attributes["desiredCollectionDelimiter"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["desiredCollectionDelimiter"].Value

                                );
                            string strOrderOfDataMemberInCSV = ((XmlElement)dataMemberNodeList[i]).Attributes["headerItemOrder"] == null ? "" : ((XmlElement)dataMemberNodeList[i]).Attributes["headerItemOrder"].Value;

                            listValuesInCorrespondingIndex[Convert.ToInt32(strOrderOfDataMemberInCSV)] = "\"" + strValueOfDataMember + "\"";  // Using "" on start and end will not break even if the cell value contains ,
                        }
                    }
                    #endregion

                }

                String strCSVRow = makeRow(listValuesInCorrespondingIndex, ",");

                listCSVRows.Add(strCSVRow);
                #endregion
            }


            return listCSVRows;
        }


        /// <summary>
        /// GEts the Node that contains object for transformation 
        /// </summary>
        /// <param name="objecToBeTransofrmedIntoCSV"></param>
        /// <param name="isCollection"></param>
        /// <param name="type"></param>
        /// <param name="strTemplateXml"></param>
        /// <returns></returns>
        private  XmlNode GetNodeThatContainsObjectForTransformation(Object objecToBeTransofrmedIntoCSV, Boolean isCollection, Type type, String strTemplateXml, 
            string strXmlConfigurationElementName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(strTemplateXml);

            XmlElement xmlElement = (XmlElement)xmlDocument.GetElementsByTagName("ObjectToCSVMapping")[0];

            XmlNodeList objectInfoNodeList = xmlElement.GetElementsByTagName("ObjectInfo");


            for (int i = 0; i < objectInfoNodeList.Count; i++)
            {
                if (objectInfoNodeList[i].Attributes["fullyQualifiedName"] != null && objectInfoNodeList[i].Attributes["fullyQualifiedName"].Value == objecToBeTransofrmedIntoCSV.GetType().ToString() &&
                    objectInfoNodeList[i].Attributes["objectCsvMappingName"] != null && objectInfoNodeList[i].Attributes["objectCsvMappingName"].Value == strXmlConfigurationElementName)
                {
                    if (isCollection == true)
                    {
                        if (objectInfoNodeList[i].Attributes["fullyQualifiedName"].Value == type.ToString())
                        {
                            return objectInfoNodeList[i];
                        }
                    }
                    else
                    {
                        if (objectInfoNodeList[i].Attributes["fullyQualifiedName"].Value == objecToBeTransofrmedIntoCSV.GetType().ToString())
                        {
                            return objectInfoNodeList[i];
                        }
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// GEts the Node that contains object for transformation 
        /// </summary>
        /// <param name="objecToBeTransofrmedIntoCSV"></param>
        /// <param name="isCollection"></param>
        /// <param name="type"></param>
        /// <param name="strTemplateXml"></param>
        /// <returns></returns>
        private  XmlNode GetNodeThatContainsObjectForTransformation(Object objecToBeTransofrmedIntoCSV, Boolean isCollection, Type type, String strTemplateXml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(strTemplateXml);

            XmlElement xmlElement = (XmlElement)xmlDocument.GetElementsByTagName("ObjectToCSVMapping")[0];

            XmlNodeList objectInfoNodeList = xmlElement.GetElementsByTagName("ObjectInfo");


            for (int i = 0; i < objectInfoNodeList.Count; i++)
            {
                if (objectInfoNodeList[i].Attributes["fullyQualifiedName"] != null &&
                    objectInfoNodeList[i].Attributes["fullyQualifiedName"].Value == objecToBeTransofrmedIntoCSV.GetType().ToString() )
                {
                    if (isCollection == true)
                    {
                        if (objectInfoNodeList[i].Attributes["fullyQualifiedName"].Value == type.ToString())
                        {
                            return objectInfoNodeList[i];
                        }
                    }
                    else
                    {
                        if (objectInfoNodeList[i].Attributes["fullyQualifiedName"].Value == objecToBeTransofrmedIntoCSV.GetType().ToString())
                        {
                            return objectInfoNodeList[i];
                        }
                    }
                }
            }

            return null;
        }

        private  List<String> InitializeListOfString(int iCount)
        {
            List<String> listValuesInCorrespondingIndex = new List<string>();

            for (int i = 0; i < iCount; i++)
            {
                listValuesInCorrespondingIndex.Add("");
            }

            return listValuesInCorrespondingIndex;
        }

        private  string getValueOfPrimitiveType(Object o, string primitiveTypeDataMemberName)
        {
            Type type = o.GetType();

            FieldInfo[] fields = type.GetFields(BindingFlags.Public |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Instance |
                                              BindingFlags.Static |
                                              BindingFlags.FlattenHierarchy);

            if (fields != null)
            {
                foreach (var f in fields)
                {
                    // f.Name == primitiveTypeDataMemberName will work for class members declared without get and set
                    // f.Name.Contains("<"+ primitiveTypeDataMemberName+">") will work for class members declared with get and set
                    if (f.Name == primitiveTypeDataMemberName || f.Name.Contains("<" + primitiveTypeDataMemberName + ">"))
                    {
                        object extractedObject = f.GetValue(o);

                        string primitiveTypeDataMemberValue = extractedObject == null ? "" : extractedObject.ToString();

                        return primitiveTypeDataMemberValue;
                    }
                }
            }

            return "";
        }

        private  string getValueOfNonPrimitiveType(Object o, string className, string instanceName, string desiredDataMember)
        {
            Type t = o.GetType();

            FieldInfo[] fields = t.GetFields(BindingFlags.Public |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Instance |
                                              BindingFlags.Static |
                                              BindingFlags.FlattenHierarchy);

            foreach (var f in fields)
            {
                if (f.FieldType.FullName == className && f.Name == instanceName)
                {
                    FieldInfo[] innerFields = f.FieldType.GetFields(BindingFlags.Public |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Instance |
                                              BindingFlags.Static |
                                              BindingFlags.FlattenHierarchy);

                    // Get Value of object set in instanceName of type className
                    object obInstanceUpper = f.GetValue(o);

                    if (obInstanceUpper != null)
                    {
                        foreach (var innerF in innerFields)
                        {
                            if (innerF.Name == desiredDataMember)
                            {
                                object objGet = innerF.GetValue(obInstanceUpper);

                                return Convert.ToString(objGet);
                            }
                        }
                    }
                }
            }

            // retrun empty when no match found
            return "";
        }

        private  string getValueOfNonPrimitiveTypeOfCollection(Object objectInput, string strClassName, string strInstanceName, string strDesiredCollectionIndex,
            string strDesiredDataMember, string strDesiredCollectionDelimiter)
        {
            String strValueForAll = "";
            Type typeOfInputObject = objectInput.GetType();

            FieldInfo[] fieldInfoOfInputObject = typeOfInputObject.GetFields(BindingFlags.Public |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Instance |
                                              BindingFlags.Static |
                                              BindingFlags.FlattenHierarchy);

            for (int iFieldCountOfInputObject = 0; iFieldCountOfInputObject < fieldInfoOfInputObject.Length; iFieldCountOfInputObject++)
            {
                // f.FullName.Contains ("[" + className + ",") works for generic collection
                if (fieldInfoOfInputObject[iFieldCountOfInputObject].FieldType.FullName.Contains("[" + strClassName + ",") &&
                        (
                            // f.Name == primitiveTypeDataMemberName will work for class members declared without get and set
                            // f.Name.Contains("<"+ primitiveTypeDataMemberName+">") will work for class members declared with get and set
                            fieldInfoOfInputObject[iFieldCountOfInputObject].Name == strInstanceName ||
                            fieldInfoOfInputObject[iFieldCountOfInputObject].Name.Contains("<" + strInstanceName + ">")
                        )
                    )
                {
                    // Get Value of object set in instanceName of type className
                    object objInnerCollection = fieldInfoOfInputObject[iFieldCountOfInputObject].GetValue(objectInput);

                    if (objInnerCollection != null)
                    {
                        if (objInnerCollection.GetType().IsGenericType)
                        {
                            ICollection col = objInnerCollection as ICollection;

                            if (col != null)
                            {
                                IEnumerable enumerable = col as IEnumerable;

                                int iCounter = 0;
                                foreach (Object val in enumerable)
                                {
                                    if (strDesiredCollectionIndex == "last" && iCounter < col.Count - 1)
                                    {
                                        continue;
                                    }


                                    if (strDesiredCollectionIndex != "last" && strDesiredCollectionIndex != "all")
                                    {
                                        try
                                        {
                                            if (iCounter < Convert.ToInt32(strDesiredCollectionIndex))
                                            {
                                                continue;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            Console.WriteLine("Only allowed values for desiredCollectionIndex columns are last, all and numerics e.g. 0, 1, 2 etc");
                                            return "";
                                        }
                                    }

                                    Type typeOfVal = val.GetType();

                                    if (isPremitiveCollection(typeOfVal.ToString()))
                                    {
                                        if (strDesiredCollectionIndex != "all")
                                        {
                                            return Convert.ToString(val);
                                        }

                                        if (strDesiredCollectionIndex == "all")
                                        {
                                            strValueForAll += Convert.ToString(val) + strDesiredCollectionDelimiter;
                                        }
                                    }
                                    else
                                    {
                                        FieldInfo[] fieldsOfVal = typeOfVal.GetFields(BindingFlags.Public |
                                                                          BindingFlags.NonPublic |
                                                                          BindingFlags.Instance |
                                                                          BindingFlags.Static |
                                                                          BindingFlags.FlattenHierarchy);

                                        for (int iFieldsOfVal = 0; iFieldsOfVal < fieldsOfVal.Length; iFieldsOfVal++)
                                        {
                                            if (strDesiredCollectionIndex != "all" && fieldsOfVal[iFieldsOfVal].Name.Contains("<" + strDesiredDataMember + ">"))
                                            {
                                                return Convert.ToString(fieldsOfVal[iFieldsOfVal].GetValue(val));
                                            }

                                            if (strDesiredCollectionIndex == "all" && fieldsOfVal[iFieldsOfVal].Name.Contains("<" + strDesiredDataMember + ">"))
                                            {
                                                strValueForAll += Convert.ToString(fieldsOfVal[iFieldsOfVal].GetValue(val)) + strDesiredCollectionDelimiter;
                                                break;
                                            }
                                        }
                                    }

                                    iCounter++;
                                }
                            }
                        }
                    }
                }
                // f.FullName == strClassName works for simple collection like System.String[]
                else if (fieldInfoOfInputObject[iFieldCountOfInputObject].FieldType.FullName == strClassName &&
                        (
                            // f.Name == primitiveTypeDataMemberName will work for class members declared without get and set
                            // f.Name.Contains("<"+ primitiveTypeDataMemberName+">") will work for class members declared with get and set
                            fieldInfoOfInputObject[iFieldCountOfInputObject].Name == strInstanceName ||
                            fieldInfoOfInputObject[iFieldCountOfInputObject].Name.Contains("<" + strInstanceName + ">")
                        )
                    )
                {
                    // Get Value of object set in instanceName of type className
                    object objInnerCollection = fieldInfoOfInputObject[iFieldCountOfInputObject].GetValue(objectInput);

                    if (objInnerCollection != null)
                    {

                        ICollection col = objInnerCollection as ICollection;

                        if (col != null)
                        {
                            IEnumerable enumerable = col as IEnumerable;

                            int iCounter = 0;
                            foreach (Object val in enumerable)
                            {
                                if (strDesiredCollectionIndex == "last" && iCounter < col.Count - 1)
                                {
                                    continue;
                                }

                                if (strDesiredCollectionIndex != "all")
                                {
                                    return Convert.ToString(val);
                                }

                                if (strDesiredCollectionIndex == "all")
                                {
                                    strValueForAll += Convert.ToString(val) + strDesiredCollectionDelimiter;
                                }
                            }
                        }
                    }
                }
            }

            if (String.IsNullOrEmpty(strValueForAll) == false && String.IsNullOrEmpty(strDesiredCollectionDelimiter) == false)
            {
                strValueForAll = strValueForAll.Remove(strValueForAll.Length - 1);
            }

            // retrun empty when no match found
            return strValueForAll;
        }

        private  void convertListOfStringTOCSV(List<String> listToConvertToWriteOnFile, String strFileName)
        {
            try
            {
                if (listToConvertToWriteOnFile != null)
                {
                    File.WriteAllLines(strFileName, listToConvertToWriteOnFile.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public  string makeRow(List<String> listValuesInCorrespondingIndex, String strSeparator)
        {
            String strRow = string.Empty;

            for (int i = 0; i < listValuesInCorrespondingIndex.Count; i++)
            {
                strRow = strRow + listValuesInCorrespondingIndex[i];

                if (i != listValuesInCorrespondingIndex.Count - 1)
                {
                    strRow = strRow + strSeparator;
                }
            }

            return strRow;
        }

        public  Boolean isPremitiveCollection(String strType)
        {
            if (strType == "String")
            {
                return true;
            }
            else if (strType == "System.String")
            {
                return true;
            }
            else if (strType == "Int32")
            {
                return true;
            }
            else if (strType == "System.Int32")
            {
                return true;
            }
            else if (strType == "Int64")
            {
                return true;
            }
            else if (strType == "System.Int64")
            {
                return true;
            }
            else if (strType == "decimal")
            {
                return true;
            }
            else if (strType == "System.decimal")
            {
                return true;
            }

            return false;
        }
    }
}
