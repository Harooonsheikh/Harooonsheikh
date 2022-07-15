using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Linq;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.UI.Helpers;

namespace VSI.EDGEAXConnector.UI.ViewModel
{
    public class ResolverViewModel
    {
        private VSI.EDGEAXConnector.Common.Resolver _resolver;
        public ResolverViewModel()
        {
            ExistingResolvers = new ObservableCollection<Resolver>();
            _resolver = new Resolver();
        }
        
        public VSI.EDGEAXConnector.Common.Resolver SelectedResolver
        {
            get { return _resolver; }
            set { _resolver = value; }
        }

        public static ObservableCollection<VSI.EDGEAXConnector.Common.Resolver> ExistingResolvers { get; set; }

        public void ConfigureResolvers()
        {
            LoadResolvers();
        }

        public static void LoadResolvers()
        {
            /// AQ: Commented to get UI working
            //try
            //{
            //    var path = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.ResolversXmlPath;
            //    var files = new DirectoryInfo(path).GetFiles("*.xml");

            //    foreach (var file in files)
            //    {
            //        //Paragraph paragraph = new Paragraph();
            //        //paragraph.Inlines.Add(new Run { Text = System.IO.File.ReadAllText(file.FullName) });
            //        //paragraph.LineHeight = 24;

            //        Resolver res = new Resolver();
            //        //res.Name = file.Name;
            //        //res.code = System.IO.File.ReadAllText(file.FullName);

            //        res = LoadResolverFromFile(file.FullName);

            //        ExistingResolvers.Add(res);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ExceptionHelper.ShowMessage(ex);
            //    CustomLogger.LogException(ex, "Console");
            //}
        }

        public static Resolver LoadResolverFromFile(string fileName)
        {
            var resolver = new Resolver();
            try
            {
                if (File.Exists(fileName))
                {
                    var doc = XElement.Load(fileName);
                    var nameEle = doc.Element("ResolverDefinition");
                    var codeEle = doc.Element("Code");

                    
                    if (nameEle != null)
                    {
                        resolver.Name = nameEle.Attributes("Name").FirstOrDefault().Value;
                        var cData = doc.DescendantNodes().Single(el => el.NodeType == XmlNodeType.CDATA);
                        var content = cData.Parent.Value.Trim();
                        resolver.code = content;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowMessage(ex);
                CustomLogger.LogException(ex, "Console");
                resolver = null;
            }
            return resolver;
        }

        public static void DeleteResolver(VSI.EDGEAXConnector.Common.Resolver res)
        {
            /// AQ: Commented to get UI Project working
            //try
            //{
            //    var path = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.ResolversXmlPath;
            //    var className = string.Format("{0}", res.Name + ".xml");

            //    if (File.Exists(System.IO.Path.Combine(path, className)))
            //    {
            //        var newFolder = "DeletedResolvers";
            //        var deletedPath = System.IO.Path.Combine(path, newFolder);

            //        if (!Directory.Exists(deletedPath))
            //        {
            //            System.IO.Directory.CreateDirectory(deletedPath);
            //        }
            //        //Copying file in deleted folder
            //        //TODO: Add DateTime & User info while delete file
            //        File.Copy(System.IO.Path.Combine(path, className), System.IO.Path.Combine(deletedPath, className), true);
            //        File.Delete(System.IO.Path.Combine(path, className));
            //        //Removing map from UI
            //        ExistingResolvers.Remove(res);
            //    }
            //}
            //catch (Exception exp)
            //{
            //    ExceptionHelper.ShowMessage(exp);
            //    CustomLogger.LogException(exp);
            //}
        }

        public static bool GenerateXmlFileResolver(Resolver res)
        {
            //try
            //{
            //    if(res.Name != "")
            //    {
            //        var resolverName = string.Format("{0}", res.Name);
            //        XNamespace ns = "http://www.w3.org/2001/XMLSchema-instance";
            //        var parent = new XElement("ResolverTemplate");
            //        parent.Add(new XAttribute(XNamespace.Xmlns + "xsi", ns),
            //            new XElement("ResolverDefinition", new XAttribute("Name", res.Name)),
            //            new XElement("Code", new XCData(res.code)));

            //        var doc = new XDocument();
            //        doc.Add(parent);

            //        doc.Save(Path.Combine(Configuration.ConfigurationHelper.ResolversXmlPath , resolverName + ".xml"));

            //        var selectedResolver = ExistingResolvers.Where(r => r.Name == res.Name).FirstOrDefault();
            //        ExistingResolvers.Remove(selectedResolver);
            //        ExistingResolvers.Add(res);
            //    }
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    ExceptionHelper.ShowMessage(ex);
            //    CustomLogger.LogException(ex, "Console");
            //    return false;
            //}
            return true;
        }
    }
}
