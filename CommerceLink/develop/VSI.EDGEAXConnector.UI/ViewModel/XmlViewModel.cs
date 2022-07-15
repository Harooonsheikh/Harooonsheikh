using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.UI.Helpers;

namespace VSI.EDGEAXConnector.UI.ViewModel
{
    public class XmlViewModel
    {
        private VSI.EDGEAXConnector.Common.XML _xml;
        public XmlViewModel()
        {
            ExistingXMLs = new ObservableCollection<XML>();
            _xml = new XML();
        }

        public VSI.EDGEAXConnector.Common.XML SelectedXML
        {
            get { return _xml; }
            set { _xml = value; }
        }

        public static ObservableCollection<VSI.EDGEAXConnector.Common.XML> ExistingXMLs { get; set; }

        public void ConfigureXmls()
        {
            LoadXMLs();
        }

        public static void LoadXMLs()
        {
            
            try
            {
                var loadFromDatabase = ConfigurationManager.AppSettings["IsLoadTemplateFromDatabase"].BoolValue();
                if (loadFromDatabase)
                {
                    MappingTemplateDAL dal = new MappingTemplateDAL(StoreService.StoreLkey);
                    dal.GetAllMappingTemplates().ForEach(ele =>
                    {
                        var xml = new XML();
                        XDocument doc;
                        using (StringReader s = new StringReader(ele.XML))
                        {
                            doc = XDocument.Load(s);
                        }
                        xml.Name = ele.Name;
                        xml.Xml = doc;
                        ExistingXMLs.Add(xml);

                    });
                }
                else
                {
                    /// AQ: Commented to get UI working
                    // var path = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.XmlPath;
                    var path = ConfigurationHelper.GetSetting(APPLICATION.XML_Base_Path);

                    if (path != null || path != "")
                    {
                        if (Directory.Exists(path))
                        {
                            var files = new DirectoryInfo(path).GetFiles("*.xml");

                            foreach (var file in files)
                            {

                                XML xml = new XML();

                                xml = LoadXMLFromFile(file.FullName);

                                ExistingXMLs.Add(xml);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowMessage(ex);
                CustomLogger.LogException(ex, "Console");
            }
        }

        public static XML LoadXMLFromFile(string fileName)
        {
            var xml = new XML();
            try
            {
                if (File.Exists(fileName))
                {
                    var doc = XDocument.Load(fileName);

                    var strName = fileName.Split('\\');

                    if (strName.Length > 0)
                    {
                        xml.Name = strName[strName.Length - 1];
                        xml.Xml = doc;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowMessage(ex);
                CustomLogger.LogException(ex, "Console");
                xml = null;
            }
            return xml;
        }

        public static void DeleteXML(VSI.EDGEAXConnector.Common.XML xml)
        {
            try
            {
                var loadFromDatabase = ConfigurationManager.AppSettings["IsLoadTemplateFromDatabase"].BoolValue();
                if (loadFromDatabase)
                {
                    MappingTemplateDAL tempDal = new MappingTemplateDAL(StoreService.StoreLkey);
                    MappingTemplate selectedTemplate = tempDal.GetMappingTemplateByName(xml.Name);
                    var deleted = tempDal.DeleteMappingTemplate(selectedTemplate.MappingTemplateId);
                    if (deleted)
                    {
                        ExistingXMLs.Remove(xml);
                    }
                    else
                    {
                        throw  new Exception("XML Could not deleted.");
                    }
                }
                else
                {
                    /// AQ: Code Commented to get UI working
                    // var path = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.XmlPath;
                    var path = ConfigurationHelper.GetSetting(APPLICATION.XML_Base_Path);
                    var className = string.Format("{0}", xml.Name);

                    if (File.Exists(System.IO.Path.Combine(path, className)))
                    {
                        var newFolder = "DeletedXML";
                        var deletedPath = System.IO.Path.Combine(path, newFolder);

                        if (!Directory.Exists(deletedPath))
                        {
                            System.IO.Directory.CreateDirectory(deletedPath);
                        }
                        //Copying file in deleted folder
                        //TODO: Add DateTime & User info while delete file
                        File.Copy(System.IO.Path.Combine(path, className), System.IO.Path.Combine(deletedPath, className), true);
                        File.Delete(System.IO.Path.Combine(path, className));
                        ExistingXMLs.Remove(xml);
                    }
                }
            }
            catch (Exception exp)
            {
                ExceptionHelper.ShowMessage(exp);
                CustomLogger.LogException(exp);
            }
        }
        public static bool GenerateXmlFileXmlMapping(XML xml)
        {
            
            try
            {
                if (xml.Name != "")
                {
                    var selectedXml = ExistingXMLs.Where(x => x.Name == xml.Name).FirstOrDefault();
                    var loadFromDatabase = ConfigurationManager.AppSettings["IsLoadTemplateFromDatabase"].BoolValue();
                    if (loadFromDatabase)
                    {

                        MappingTemplateDAL mapDal = new MappingTemplateDAL(StoreService.StoreLkey);
                        MappingTemplate template = mapDal.GetMappingTemplateByName(xml.Name);
                        if (template != null)
                        {
                            template.XML = xml.Xml.ToString();
                            mapDal.UpdateMappingTemplate(template);
                            ExistingXMLs.Remove(selectedXml);
                            var oldXML = ExistingXMLs.Where((m => m.Name == selectedXml.Name)).FirstOrDefault();
                            if (oldXML != null)
                            {
                                ExistingXMLs.Remove(oldXML);
                            }

                            ExistingXMLs.Add(xml);
                        }
                        else
                        {
                            template = new MappingTemplate();
                            template.Name = xml.Name;
                            template.XML = xml.Xml.ToString();
                            template.CreatedOn = DateTime.UtcNow;
                            template.ModifiedOn = DateTime.UtcNow;
                            template.ReadMode = template.Name.Split('.')[0]; // Renamed from Type
                            template.SourceEntity = template.Name.Split('.')[1];
                            template.CreatedBy = 0;
                            template.IsActive = true;
                            mapDal.AddMappingTemplate(template);
                            ExistingXMLs.Remove(selectedXml);
                            ExistingXMLs.Add(xml);
                        }
                    }
                    else
                    {
                        xml.Name += ".xml";
                        /// AQ: Commented to get UI working
                        // xml.Xml.Save(Path.Combine(Configuration.ConfigurationHelper.XmlPath, xml.Name));
                        xml.Xml.Save(Path.Combine(ConfigurationHelper.GetSetting(APPLICATION.XML_Base_Path), xml.Name));
                        //var selectedXml = ExistingXMLs.Where(x => x.Name == xml.Name).FirstOrDefault();
                        ExistingXMLs.Remove(selectedXml);
                        ExistingXMLs.Add(xml);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowMessage(ex);
                CustomLogger.LogException(ex, "Console");
                return false;
            }
        }
    }
}
