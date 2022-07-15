using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.UI.Helpers;

namespace VSI.EDGEAXConnector.UI.ViewModel
{
    public class MapsViewModel : BaseViewModel
    {
        private MapDirection _direction;
        private Transformer _transformer;

        public MapsViewModel()
        {
            this.SourceProperties = new ObservableCollection<PropertyInfo>();
            this.ExistingMaps = new ObservableCollection<Transformer>();
        }

        public List<Type> SrcEntities { get; set; }
        public List<Type> DestEntities { get; set; }
        public ObservableCollection<PropertyInfo> SourceProperties { get; set; }
        public ObservableCollection<Transformer> ExistingMaps { get; set; }
        public string MapTitle { get; set; }

        public MapDirection Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                this.MapTitle = value == MapDirection.ErpToEcom ? "ERP To ECommerce" : "ECommerce To ERP";
                RaisePropertyChanged("MapTitle");
            }
        }

        public Transformer SelectedTransformer
        {
            get { return _transformer; }
            set
            {
                _transformer = value;
                RaisePropertyChanged("Transformer");
            }
        }

        public void ConfigureMap()
        {
            try
            {
                if (this.Direction == MapDirection.ErpToEcom)
                {
                    IClassInfo connectorInfo = new ECommerceDataModels.ClassInfo();
                    this.DestEntities = connectorInfo.GetClassesInfo();
                    connectorInfo = new VSI.EDGEAXConnector.ERPDataModels.ClassInfo();
                    this.SrcEntities = connectorInfo.GetClassesInfo();
                }
                else if (this.Direction == MapDirection.EcomToErp)
                {
                    IClassInfo connectorInfo = new VSI.EDGEAXConnector.ERPDataModels.ClassInfo();
                    this.DestEntities = connectorInfo.GetClassesInfo();
                    connectorInfo = new ECommerceDataModels.ClassInfo();
                    this.SrcEntities = connectorInfo.GetClassesInfo();
                }
                LoadTransformer();
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowMessage(ex);
                CustomLogger.LogException(ex, "Console");
            }
        }

        public void GenerateMap()
        {
            GenerateXmlMapping();
        }

        public void GenerateXmlMapping()
        {
            try
            {
                foreach (var map in this.ExistingMaps)
                {
                    var className = string.Format("{0}_{1}Map", map.SourceClass.Name, map.DestinationClass.Name);
                    XNamespace ns = "http://www.w3.org/2001/XMLSchema-instance";
                    var parent = new XElement("MapTemplate");
                    parent.Add(new XAttribute(XNamespace.Xmlns + "xsi", ns),
                        new XElement("MapDefinitions", new XAttribute("Type", this.Direction.ToString())));

                    //parent.Add(name);
                    var parentMap = new XElement("MapDefinition", new XAttribute("Type", "Parent"),
                        new XAttribute("SourceClass", map.SourceClass.FullName),
                        new XAttribute("DestinationClass", map.DestinationClass.FullName),
                        new XElement("Name", map.SourceClass + " To " + map.DestinationClass),
                        new XElement("Entries",
                            from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                            where !p.IsComplex && !p.IsCustomLogic && !p.BooleanValue.IsBoolean && 
                            p.DestinationProperty != null && String.IsNullOrEmpty(p.ConstantValue.Value) 
                            select GetSimpleFieldMapping(p, ns),

                            from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                            where p.IsComplex && !p.IsCustomLogic && p.DestinationProperty != null
                            select GetComplexFieldMapping(map, p, ns),

                            from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                            where !String.IsNullOrEmpty(p.ConstantValue.Value) && p.DestinationProperty != null
                            select GetConstantMapping(p, ns),

                            from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                            where p.BooleanValue.IsBoolean
                            select GetConstantMapping(p, ns),

                            from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                            where p.IsCustomLogic
                            select GetCustomMapping(map, p, ns)
                            ));

                    var childMaps = new List<XElement>();

                    foreach (var item in map.ChildMaps)
                    {
                        var childMap = new XElement("MapDefinition", new XAttribute("Type", "Child"),
                            new XAttribute("SourceClass", item.SourceClass.FullName),
                            new XAttribute("DestinationClass", item.DestinationClass.FullName),
                            new XElement("Name", item.SourceClass + " To " + item.DestinationClass),
                            new XElement("Entries",
                                from p in item.Properties.OrderBy(e => e.DestinationProperty.Name)
                                where !p.IsComplex && p.DestinationProperty != null
                                select GetSimpleFieldMapping(p, ns),

                                from p in item.Properties.OrderBy(e => e.DestinationProperty.Name)
                                where p.IsComplex && p.DestinationProperty != null
                                select GetComplexFieldMapping(map, p, ns)
                                ));
                        childMaps.Add(childMap);
                    }

                    var mapDefinitions = parent.Element("MapDefinitions");

                    childMaps.ForEach(m => { if (mapDefinitions != null) mapDefinitions.Add(m); });

                    mapDefinitions.Add(parentMap);

                    var doc = new XDocument();
                    doc.Add(parent);

                    // doc.Save(Path.Combine(Configuration.ConfigurationHelper.MapsXmlPath, className + ".xml"));
                    doc.Save(Path.Combine(ConfigurationHelper.GetSetting(APPLICATION.XML_Base_Path), className + ".xml"));

                    if (
                        !this.ExistingMaps.Any(
                            em => em.SourceClass == map.SourceClass && em.DestinationClass == map.DestinationClass))
                    {
                        this.ExistingMaps.Add(map);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowMessage(ex);
                CustomLogger.LogException(ex, "Console");
            }
        }

        public void GenerateFileMap(Transformer map)
        {
            GenerateXmlFileMapping(map);
        }

        public void GenerateXmlFileMapping(Transformer map)
        {
            try
            {
                var className = string.Format("{0}_{1}Map", map.SourceClass.Name, map.DestinationClass.Name);
                XNamespace ns = "http://www.w3.org/2001/XMLSchema-instance";
                var parent = new XElement("MapTemplate");
                parent.Add(new XAttribute(XNamespace.Xmlns + "xsi", ns),
                    new XElement("MapDefinitions", new XAttribute("Type", this.Direction.ToString())));

                //parent.Add(name);
                var parentMap = new XElement("MapDefinition", new XAttribute("Type", "Parent"),
                    new XAttribute("SourceClass", map.SourceClass.FullName),
                    new XAttribute("DestinationClass", map.DestinationClass.FullName),
                    new XElement("Name", map.SourceClass + " To " + map.DestinationClass),
                    new XElement("Entries",
                        from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                        where !p.IsComplex && !p.IsCustomLogic && !p.BooleanValue.IsBoolean &&
                        p.DestinationProperty != null && String.IsNullOrEmpty(p.ConstantValue.Value)
                        select GetSimpleFieldMapping(p, ns),

                        from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                        where p.IsComplex && !p.IsCustomLogic && !p.BooleanValue.IsBoolean && 
                        p.DestinationProperty != null
                        select GetComplexFieldMapping(map, p, ns),

                        from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                        where !String.IsNullOrEmpty(p.ConstantValue.Value) && !p.BooleanValue.IsBoolean && 
                        p.DestinationProperty != null
                        select GetConstantMapping(p, ns),

                        from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                        where p.BooleanValue.IsBoolean
                        select GetConstantMapping(p, ns),

                        from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                        where p.IsCustomLogic
                        select GetCustomMapping(map, p, ns)
                        ));

                var childMaps = new List<XElement>();

                foreach (var item in map.ChildMaps)
                {
                    var childMap = new XElement("MapDefinition", new XAttribute("Type", "Child"),
                        new XAttribute("SourceClass", item.SourceClass.FullName),
                        new XAttribute("DestinationClass", item.DestinationClass.FullName),
                        new XElement("Name", item.SourceClass + " To " + item.DestinationClass),
                        new XElement("Entries",
                            from p in item.Properties.OrderBy(e => e.DestinationProperty.Name)
                            where !p.IsComplex && p.DestinationProperty != null
                            select GetSimpleFieldMapping(p, ns),

                            from p in item.Properties.OrderBy(e => e.DestinationProperty.Name)
                            where p.IsComplex && p.DestinationProperty != null
                            select GetComplexFieldMapping(map, p, ns)
                            ));
                    childMaps.Add(childMap);
                }

                var mapDefinitions = parent.Element("MapDefinitions");

                childMaps.ForEach(m => { if (mapDefinitions != null) mapDefinitions.Add(m); });

                mapDefinitions.Add(parentMap);

                var doc = new XDocument();
                doc.Add(parent);

                // doc.Save(Path.Combine(Configuration.ConfigurationHelper.MapsXmlPath, className + ".xml"));
                doc.Save(Path.Combine(ConfigurationHelper.GetSetting(APPLICATION.XML_Base_Path), className + ".xml"));               

                if (
                    !this.ExistingMaps.Any(
                        em => em.SourceClass == map.SourceClass && em.DestinationClass == map.DestinationClass))
                {
                    this.ExistingMaps.Add(map);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowMessage(ex);
                CustomLogger.LogException(ex, "Console");
            }
        }

        public XmlDocument GetXmlOfMapping(Transformer map)
        {
            var document = new XmlDocument();
            try
            {
                XNamespace ns = "http://www.w3.org/2001/XMLSchema-instance";
                var parent = new XElement("MapTemplate");
                parent.Add(new XAttribute(XNamespace.Xmlns + "xsi", ns),
                    new XElement("MapDefinitions", new XAttribute("Type", this.Direction.ToString())));

                //parent.Add(name);
                var parentMap = new XElement("MapDefinition", new XAttribute("Type", "Parent"),
                    new XAttribute("SourceClass", map.SourceClass.FullName),
                    new XAttribute("DestinationClass", map.DestinationClass.FullName),
                    new XElement("Name", map.SourceClass + " To " + map.DestinationClass),
                    new XElement("Entries",
                        from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                        where !p.IsComplex && !p.IsCustomLogic && !p.BooleanValue.IsBoolean &&
                        p.DestinationProperty != null && String.IsNullOrEmpty(p.ConstantValue.Value)
                        select GetSimpleFieldMapping(p, ns),

                        from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                        where p.IsComplex && !p.IsCustomLogic && !p.BooleanValue.IsBoolean &&
                        p.DestinationProperty != null
                        select GetComplexFieldMapping(map, p, ns),

                        from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                        where !String.IsNullOrEmpty(p.ConstantValue.Value) && !p.BooleanValue.IsBoolean &&
                        p.DestinationProperty != null
                        select GetConstantMapping(p, ns),

                        from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                        where p.BooleanValue.IsBoolean
                        select GetConstantMapping(p, ns),

                        from p in map.Properties.OrderBy(e => e.DestinationProperty.Name)
                        where p.IsCustomLogic
                        select GetCustomMapping(map, p, ns)
                        ));

                var childMaps = new List<XElement>();

                foreach (var item in map.ChildMaps)
                {
                    var childMap = new XElement("MapDefinition", new XAttribute("Type", "Child"),
                        new XAttribute("SourceClass", item.SourceClass.FullName),
                        new XAttribute("DestinationClass", item.DestinationClass.FullName),
                        new XElement("Name", item.SourceClass + " To " + item.DestinationClass),
                        new XElement("Entries",
                            from p in item.Properties.OrderBy(e => e.DestinationProperty.Name)
                            where !p.IsComplex && p.DestinationProperty != null
                            select GetSimpleFieldMapping(p, ns),

                            from p in item.Properties.OrderBy(e => e.DestinationProperty.Name)
                            where p.IsComplex && p.DestinationProperty != null
                            select GetComplexFieldMapping(map, p, ns)
                            ));
                    childMaps.Add(childMap);
                }

                var mapDefinitions = parent.Element("MapDefinitions");

                childMaps.ForEach(m => { if (mapDefinitions != null) mapDefinitions.Add(m); });

                mapDefinitions.Add(parentMap);

                var doc = new XDocument();
                doc.Add(parent);
                
                using (var reader = doc.CreateReader())
                {
                    document.Load(reader);
                }

            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowMessage(ex);
                CustomLogger.LogException(ex, "Console");
            }

            return document;
        }

        public Paragraph GetCodeOfMapping(Transformer map)
        {
            Paragraph paragraph = new Paragraph();

            /// AQ: Commented to get UI working
            //try
            //{
            //    var fileName = string.Format("{0}_{1}Map.cs", map.SourceClass.Name, map.DestinationClass.Name);
            //    var path = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.MapsPath;
            //    var files = new DirectoryInfo(path).GetFiles(fileName);

            //    if (files.Length > 0)
            //    {
            //        paragraph.Inlines.Add(System.IO.File.ReadAllText(files[0].FullName));
            //        paragraph.LineHeight = 24;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    ExceptionHelper.ShowMessage(ex);
            //    CustomLogger.LogException(ex, "Console");
            //}

            return paragraph;
        }

        private XElement GetSimpleFieldMapping(TransformerProperty property, XNamespace ns)
        {
            try
            {
                if (property.SourceProperty == null)
                {
                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                        new XAttribute("DataType", property.DestinationProperty.PropertyType.Name),
                        new XAttribute("MapType", MapTypes.None),
                        new XAttribute("Comment", property.Comment),
                        new XAttribute("IsComment", property.IsComment));
                    return element;
                }
                else
                {
                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                        new XAttribute("DataType", property.DestinationProperty.PropertyType.Name),
                        new XAttribute("MapType", MapTypes.SimpleToSimple),
                        new XAttribute("Comment", property.Comment),
                        new XAttribute("IsComment", property.IsComment),
                        new XElement("EntryValue", new XAttribute(ns + "type", "SourceValue"),
                            new XAttribute("FullName", property.SourceProperty.Name),
                            new XAttribute("DataType", property.SourceProperty.PropertyType.Name)));
                    return element;
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp);
            }

            return null;
        }

        private XElement GetComplexFieldMapping(Transformer transformer, TransformerProperty property, XNamespace ns)
        {
            try
            {
                if (property.SourceProperty == null)
                {
                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                       new XAttribute("DataType", property.DestinationProperty.PropertyType.Name),
                       new XAttribute("MapType", MapTypes.ObjectToObject),
                       new XAttribute("Comment", property.Comment),
                       new XAttribute("IsComment", property.IsComment),
                       new XElement("EntryValue", new XAttribute(ns + "type", "SourceValue"),
                           new XAttribute("FullName", transformer.SourceClass.Name),
                           new XAttribute("DataType", transformer.SourceClass.Name)));

                    return element;
                }
                else if ((property.SourceProperty.PropertyType.IsGenericType || property.SourceProperty.PropertyType.IsArray) &&
                    (property.DestinationProperty.PropertyType.IsGenericType || property.DestinationProperty.PropertyType.IsArray))
                {
                    var sourceType =
                        property.SourceProperty.PropertyType.GetGenericArguments().FirstOrDefault().FullName;
                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                        new XAttribute("DataType", property.DestinationProperty.PropertyType.FullName),
                        new XAttribute("MapType", MapTypes.CollectionToCollection),
                        new XAttribute("Comment", property.Comment),
                        new XAttribute("IsComment", property.IsComment),
                        new XElement("EntryValue", new XAttribute(ns + "type", "SourceValue"),
                            new XAttribute("FullName", property.SourceProperty.Name),
                            new XAttribute("DataType",
                                string.Format("IList<{0}>",
                                    property.SourceProperty.PropertyType.GetGenericArguments().FirstOrDefault().FullName))));

                    return element;
                }
                else if (!(property.SourceProperty.PropertyType.IsGenericType || property.SourceProperty.PropertyType.IsArray) &&
                    (property.DestinationProperty.PropertyType.IsGenericType || property.DestinationProperty.PropertyType.IsArray))
                {
                    var sourceType = property.SourceProperty.PropertyType;
                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                        new XAttribute("DataType", property.DestinationProperty.PropertyType.FullName),
                        new XAttribute("MapType", MapTypes.ObjectToCollection),
                        new XAttribute("Comment", property.Comment),
                        new XAttribute("IsComment", property.IsComment),
                        new XElement("EntryValue", new XAttribute(ns + "type", "SourceValue"),
                            new XAttribute("FullName", property.SourceProperty.Name),
                            new XAttribute("DataType", string.Format("{0}", sourceType))));

                    return element;
                }
                else if ((property.SourceProperty.PropertyType.IsGenericType || property.SourceProperty.PropertyType.IsArray) &&
                    !(property.DestinationProperty.PropertyType.IsGenericType || property.DestinationProperty.PropertyType.IsArray))
                {
                    var sourceType =
                        property.SourceProperty.PropertyType.GetGenericArguments().FirstOrDefault().FullName;
                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                        new XAttribute("DataType", property.DestinationProperty.PropertyType.FullName),
                        new XAttribute("MapType", MapTypes.CollectionToObject),
                        new XAttribute("Comment", property.Comment),
                        new XAttribute("IsComment", property.IsComment),
                        new XElement("EntryValue", new XAttribute(ns + "type", "SourceValue"),
                            new XAttribute("FullName", property.SourceProperty.Name),
                            new XAttribute("DataType", sourceType)));

                    return element;
                }
                else
                {
                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                        new XAttribute("DataType", property.DestinationProperty.PropertyType.FullName),
                        new XAttribute("MapType", MapTypes.ObjectToObject),
                        new XAttribute("Comment", property.Comment),
                        new XAttribute("IsComment", property.IsComment),
                        new XElement("EntryValue", new XAttribute(ns + "type", "SourceValue"),
                            new XAttribute("FullName", property.SourceProperty.Name),
                            new XAttribute("DataType", property.SourceProperty.PropertyType)));

                    return element;
                }
                //Old Code
                /*
                if (property.SourceProperty.PropertyType.IsGenericType &&
                    property.DestinationProperty.PropertyType.IsArray)
                {
                    var sourceType =
                        property.SourceProperty.PropertyType.GetGenericArguments().FirstOrDefault().FullName;
                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                        new XAttribute("DataType", property.DestinationProperty.PropertyType.FullName),
                        new XAttribute("MapType", MapTypes.CollectionToCollection),
                        new XElement("EntryValue", new XAttribute(ns + "type", "SourceValue"),
                            new XAttribute("FullName", property.SourceProperty.Name),
                            new XAttribute("DataType",
                                string.Format("IList<{0}>",
                                    property.SourceProperty.PropertyType.GetGenericArguments().FirstOrDefault().FullName))));

                    return element;
                }
                if (property.SourceProperty != null && !property.SourceProperty.PropertyType.IsGenericType &&
                    property.DestinationProperty.PropertyType.IsArray)
                {
                    var sourceType = property.SourceProperty.PropertyType;
                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                        new XAttribute("DataType", property.DestinationProperty.PropertyType.FullName),
                        new XAttribute("MapType", MapTypes.ObjectToCollection),
                        new XElement("EntryValue", new XAttribute(ns + "type", "SourceValue"),
                            new XAttribute("FullName", property.SourceProperty.Name),
                            new XAttribute("DataType", string.Format("IList<{0}>", sourceType))));

                    return element;
                }
                if (property.SourceProperty != null && property.SourceProperty.PropertyType.IsGenericType &&
                    !property.DestinationProperty.PropertyType.IsArray)
                {
                    var sourceType =
                        property.SourceProperty.PropertyType.GetGenericArguments().FirstOrDefault().FullName;
                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                        new XAttribute("DataType", property.DestinationProperty.PropertyType.FullName),
                        new XAttribute("MapType", MapTypes.CollectionToObject),
                        new XElement("EntryValue", new XAttribute(ns + "type", "SourceValue"),
                            new XAttribute("FullName", property.SourceProperty.Name),
                            new XAttribute("DataType", sourceType)));

                    return element;
                }
                else
                {
                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                        new XAttribute("DataType", property.DestinationProperty.PropertyType.FullName),
                        new XAttribute("MapType", MapTypes.ObjectToObject),
                        new XElement("EntryValue", new XAttribute(ns + "type", "SourceValue"),
                            new XAttribute("FullName", property.SourceProperty.Name),
                            new XAttribute("DataType", property.SourceProperty.PropertyType)));

                    return element;
                }
                */ 
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp);
            }

            return null;
        }

        private XElement GetConstantMapping(TransformerProperty property, XNamespace ns)
        {
            try
            {
                if (property.SourceProperty != null)
                {
                    if (property.ConstantValue.UseAsDefault)
                    {
                        var element = new XElement("Entry",
                            new XAttribute("FullName", property.DestinationProperty.Name),
                            new XAttribute("DataType", property.DestinationProperty.PropertyType.Name),
                            new XAttribute("MapType", MapTypes.FieldUsingDefault),
                            new XAttribute("Comment", property.Comment),
                            new XAttribute("IsComment", property.IsComment),
                            new XElement("EntryValue", new XAttribute(ns + "type", "Source"),
                                new XAttribute("DefaultValue", property.ConstantValue.UseAsDefault),
                                new XAttribute("FullName", property.SourceProperty.Name),
                                new XAttribute("Value", property.ConstantValue.Value)));
                        return element;
                    }
                    if (property.ConstantValue.IsKeyMapping)
                    {
                        var element = new XElement("Entry",
                            new XAttribute("FullName", property.DestinationProperty.Name),
                            new XAttribute("DataType", property.DestinationProperty.PropertyType.Name),
                            new XAttribute("MapType", MapTypes.KeyMapping),
                            new XAttribute("Comment", property.Comment),
                            new XAttribute("IsComment", property.IsComment),
                            new XElement("EntryValue", new XAttribute(ns + "type", "Source"),
                                new XAttribute("KeyMapping", property.ConstantValue.IsKeyMapping),
                                new XAttribute("FullName", property.SourceProperty.Name),
                                new XAttribute("Value", property.ConstantValue.Value)));
                        return element;
                    }
                    if (property.BooleanValue.IsBoolean)
                    {
                        var element = new XElement("Entry",
                                 new XAttribute("FullName", property.DestinationProperty.Name),
                                 new XAttribute("DataType", property.DestinationProperty.PropertyType.Name),
                                 new XAttribute("MapType", MapTypes.BooleanValues),
                                 new XAttribute("Comment", property.Comment),
                                 new XAttribute("IsComment", property.IsComment),
                                 new XElement("EntryValue", new XAttribute(ns + "type", "Source"),
                                     new XAttribute("BooleanValues", property.BooleanValue.IsBoolean),
                                     new XAttribute("FullName", property.SourceProperty.Name),
                                      new XAttribute("TrueValue", property.BooleanValue.TrueValue),
                                     new XAttribute("FalseValue", property.BooleanValue.FalseValue)));
                        return element;
                    }
                }
                else if(property.ConstantValue.IsConstant)
                {
                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                        new XAttribute("DataType", property.DestinationProperty.PropertyType.Name),
                        new XAttribute("MapType", MapTypes.FieldUsingConstant),
                        new XAttribute("Comment", property.Comment),
                        new XAttribute("IsComment", property.IsComment),
                        new XElement("EntryValue", new XAttribute(ns + "type", "Constant"),
                            new XAttribute("Constant", property.ConstantValue.IsConstant),
                            new XAttribute("Value", property.ConstantValue.Value)));
                    return element;
                }
                else if (property.BooleanValue.IsBoolean)
                {
                    var element = new XElement("Entry",
                             new XAttribute("FullName", property.DestinationProperty.Name),
                             new XAttribute("DataType", property.DestinationProperty.PropertyType.Name),
                             new XAttribute("MapType", MapTypes.BooleanValues),
                             new XAttribute("Comment", property.Comment),
                             new XAttribute("IsComment", property.IsComment),
                             new XElement("EntryValue", new XAttribute(ns + "type", "Source"),
                                 new XAttribute("BooleanValues", property.BooleanValue.IsBoolean),
                                 new XAttribute("FullName", property.SourceProperty.Name),
                                  new XAttribute("TrueValue", property.SourceProperty.Name),
                                 new XAttribute("FalseValue", property.BooleanValue.TrueValue)));
                    return element;
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp);
            }
            return null;
        }

        private XElement GetCustomMapping(Transformer transformer, TransformerProperty property, XNamespace ns)
        {
            try
            {
                if (property.SourceProperty != null && !property.CustomConditionalValue.IsAdvancedExpression)
                {
                    var sourceDataType = property.SourceProperty.PropertyType.Name;
                    if (property.SourceProperty.PropertyType.IsGenericType)
                    {
                        sourceDataType =
                            property.SourceProperty.PropertyType.GetGenericArguments().FirstOrDefault().Name;
                        sourceDataType = "List<" + sourceDataType + ">";
                    }

                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                        new XAttribute("DataType", property.DestinationProperty.PropertyType.Name),
                        new XAttribute("MapType", MapTypes.CustomExpression),
                        new XAttribute("Comment", property.Comment),
                        new XAttribute("IsComment", property.IsComment),
                        new XElement("EntryValue", new XAttribute(ns + "type", "Custom"),
                            new XAttribute("FullName", property.SourceProperty.Name),
                            new XAttribute("DataType", sourceDataType),
                            new XAttribute("IsCustomCondition", property.CustomConditionalValue.IsCustomCondition),
                            new XAttribute("Operator", property.CustomConditionalValue.OperatorValue != null ? property.CustomConditionalValue.OperatorValue : ""),
                            new XAttribute("ConditionValue", property.CustomConditionalValue.ConditionValue != null ? property.CustomConditionalValue.ConditionValue : ""),
                            new XAttribute("TrueValue", property.CustomConditionalValue.TrueValue != null ? property.CustomConditionalValue.TrueValue : ""),
                            new XAttribute("IsTrueSourceProperty", property.CustomConditionalValue.IsTrueSourceProperty),
                            new XAttribute("TrueSourceProperty", property.CustomConditionalValue.TrueSourceProperty != null ? property.CustomConditionalValue.TrueSourceProperty.Name : ""),
                            new XAttribute("TrueSourcePropertyType", property.CustomConditionalValue.TrueSourceProperty != null ? property.CustomConditionalValue.TrueSourceProperty.PropertyType.Name : ""),
                            new XAttribute("FalseValue", property.CustomConditionalValue.FalseValue != null ? property.CustomConditionalValue.FalseValue : ""),
                            new XAttribute("IsFalseSourceProperty", property.CustomConditionalValue.IsFalseSourceProperty),
                            new XAttribute("FalseSourceProperty", property.CustomConditionalValue.FalseSourceProperty != null ? property.CustomConditionalValue.FalseSourceProperty.Name : ""),
                            new XAttribute("FalseSourcePropertyType", property.CustomConditionalValue.FalseSourceProperty != null ? property.CustomConditionalValue.FalseSourceProperty.PropertyType.Name : "")
                            ));
                    return element;
                }
                else if (property.CustomConditionalValue.IsAdvancedExpression)
                {
                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                        new XAttribute("DataType", property.DestinationProperty.PropertyType.Name),
                        new XAttribute("MapType", MapTypes.CustomExpression),
                        new XAttribute("Comment", property.Comment),
                        new XAttribute("IsComment", property.IsComment),
                        new XElement("EntryValue", new XAttribute(ns + "type", "AdvancedCustom"),
                            new XAttribute("IsCustomCondition", property.CustomConditionalValue.IsCustomCondition),
                            new XAttribute("IsAdvancedExpression", property.CustomConditionalValue.IsAdvancedExpression),
                            new XAttribute("AdvancedExpression", property.CustomConditionalValue.AdvancedExpression)
                            ));
                    return element;
                }
                else
                {
                    var element = new XElement("Entry", new XAttribute("FullName", property.DestinationProperty.Name),
                        new XAttribute("DataType", property.DestinationProperty.PropertyType.Name),
                        new XAttribute("MapType", MapTypes.CustomExpression),
                        new XAttribute("Comment", property.Comment),
                        new XAttribute("IsComment", property.IsComment),
                        new XElement("EntryValue", new XAttribute(ns + "type", "Custom"),
                            new XAttribute("FullName", ""),
                            new XAttribute("DataType", transformer.SourceClass.Name)));
                    return element;
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp);
            }

            return null;
        }

        public Transformer LoadFile(string fileName)
        {
            return GetTransformerFromFile(fileName);
        }

        public void LoadTransformer()
        {
            try
            {
                if (this.ExistingMaps.Count > 0)
                {
                    this.ExistingMaps.Clear();
                }
                // var path = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.MapsXmlPath;
                var path = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.GetSetting(APPLICATION.XML_Base_Path);
                var files = new DirectoryInfo(path).GetFiles("*.xml");

                foreach (var file in files)
                {
                    var transformer = GetTransformerFromFile(file.DirectoryName + "\\" + file.Name);
                    if(transformer!= null)
                    {
                        this.ExistingMaps.Add(transformer);
                    }                    
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowMessage(ex);
                CustomLogger.LogException(ex, "Console");
            }
        }

        public void LoadFileTransformer(Transformer map)
        {
            try
            {
                // var path = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.MapsXmlPath;
                var path = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.GetSetting(APPLICATION.XML_Base_Path);
                var className = string.Format("{0}_{1}Map.xml", map.SourceClass.Name, map.DestinationClass.Name);
                var files = new DirectoryInfo(path).GetFiles(className);

                foreach (var file in files)
                {
                    var transformer = GetTransformerFromFile(file.DirectoryName + "\\" + file.Name);
                    if (transformer != null)
                    {
                        var currentMap = this.ExistingMaps.Where(em => em.SourceClass == map.SourceClass && em.DestinationClass == map.DestinationClass).FirstOrDefault();
                        int indexofSelectedTransformer = this.ExistingMaps.IndexOf(currentMap);
                        if (indexofSelectedTransformer >= 0)
                        {
                            this.ExistingMaps[indexofSelectedTransformer].ChildMaps = transformer.ChildMaps;
                            this.ExistingMaps[indexofSelectedTransformer].DestinationClass = transformer.DestinationClass;
                            this.ExistingMaps[indexofSelectedTransformer].SourceClass = transformer.SourceClass;
                            this.ExistingMaps[indexofSelectedTransformer].Properties = transformer.Properties;
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

        public Transformer GetTransformerFromFile(string fileName)
        {
            var transformer = new Transformer();
            try
            {
                if (File.Exists(fileName))
                {
                    var doc = XElement.Load(fileName);
                    var mappings = doc.Element("MapDefinitions");
                    var mapType = mappings.Attribute("Type").Value;
                    if (mapType != this.Direction.ToString())
                    {
                        transformer = null;
                        return transformer;
                    }
                    IEnumerable<XElement> childs =
                        (from el in mappings.Elements("MapDefinition")
                         where (string)el.Attribute("Type") == "Child"
                         select el).ToList();
                    var parent =
                        (from el in mappings.Elements("MapDefinition")
                         where (string)el.Attribute("Type") == "Parent"
                         select el).FirstOrDefault();

                    if (parent != null)
                    {
                        transformer.SourceClass =
                            this.SrcEntities.FirstOrDefault(
                                e => e.FullName.Equals(parent.Attributes("SourceClass").FirstOrDefault().Value));
                        transformer.DestinationClass =
                            this.DestEntities.FirstOrDefault(
                                e => e.FullName.Equals(parent.Attributes("DestinationClass").FirstOrDefault().Value));

                        var srcProperties = transformer.SourceClass.GetProperties();
                        var destProperties = transformer.DestinationClass.GetProperties();

                        var entries = parent.Elements("Entries").Elements("Entry").ToList();
                        entries.ForEach(entry => { FillProperty(transformer, srcProperties, destProperties, entry); });

                        destProperties.ToList().ForEach(d =>
                        {
                            if (!transformer.Properties.Any(tp => tp.DestinationProperty != null && tp.DestinationProperty.Name.Equals(d.Name)))
                            {
                                transformer.Properties.Add(new TransformerProperty(d)
                                {
                                    MapType = MapTypes.None
                                });
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowMessage(ex);
                CustomLogger.LogException(ex, "Console");
                transformer = null;
            }
            return transformer;
        }

        private static void FillProperty(Transformer transformer, PropertyInfo[] srcProperties,
            PropertyInfo[] destProperties, XElement entry)
        {
            try
            {
                var destProperty = entry.Attributes("FullName").FirstOrDefault().Value;
                var property = destProperties.FirstOrDefault(p => p.Name.Equals(destProperty));

                var mapType = entry.Attributes("MapType").FirstOrDefault().Value;
                var comment = entry.Attributes("Comment").FirstOrDefault().Value;
                var isComment = entry.Attributes("IsComment").FirstOrDefault().Value;
                if (mapType == MapTypes.None.ToString())
                {
                    transformer.Properties.Add(new TransformerProperty(property) {MapType = MapTypes.None});
                    return;
                }

                var srcProperty =
                    entry.Elements("EntryValue").FirstOrDefault().Attributes("FullName").FirstOrDefault() != null
                        ? entry.Elements("EntryValue").FirstOrDefault().Attributes("FullName").FirstOrDefault().Value
                        : null;
                var value = entry.Elements("EntryValue").FirstOrDefault().Attributes("Value").FirstOrDefault();
                var defaultValue =
                    entry.Elements("EntryValue").FirstOrDefault().Attributes("DefaultValue").FirstOrDefault();
                var booleanValues =
                 entry.Elements("EntryValue").FirstOrDefault().Attributes("BooleanValues").FirstOrDefault();

                XAttribute trueValue = null;
                XAttribute falseValue = null;
                if (booleanValues != null)
                {
                     trueValue = entry.Elements("EntryValue").FirstOrDefault().Attributes("TrueValue").FirstOrDefault();
                     falseValue = entry.Elements("EntryValue").FirstOrDefault().Attributes("FalseValue").FirstOrDefault();
                }
                var keyValue = entry.Elements("EntryValue").FirstOrDefault().Attributes("KeyMapping").FirstOrDefault();
                var constant = entry.Elements("EntryValue").FirstOrDefault().Attributes("Constant").FirstOrDefault();
                var complexType = entry.Attribute("MapType") != null
                                  && (entry.Attribute("MapType").Value.Equals(MapTypes.ObjectToObject.ToString())
                                      ||
                                      entry.Attribute("MapType")
                                          .Value.Equals(MapTypes.CollectionToCollection.ToString())
                                      || entry.Attribute("MapType").Value.Equals(MapTypes.CollectionToObject.ToString())
                                      || entry.Attribute("MapType").Value.Equals(MapTypes.ObjectToCollection.ToString()));

                var customExpression = entry.Attribute("MapType").Value.Equals(MapTypes.CustomExpression.ToString());
                var isCustomCondition =
                 entry.Elements("EntryValue").FirstOrDefault().Attributes("IsCustomCondition").FirstOrDefault();

                XAttribute IsAdvancedExpression = null;
                XAttribute AdvancedExpression = null;
                XAttribute operatorValue = null;
                XAttribute conditionValue = null;
                XAttribute cTrueValue = null;
                XAttribute cFalseValue = null;
                XAttribute isTrueSourceProperty = null;
                XAttribute isFalseSourceProperty = null;
                string trueSrcProperty = string.Empty;
                string falseSrcProperty = string.Empty;

                if (isCustomCondition != null)
                {
                    IsAdvancedExpression = entry.Elements("EntryValue").FirstOrDefault().Attributes("IsAdvancedExpression").FirstOrDefault();

                    if (IsAdvancedExpression != null && IsAdvancedExpression.Value == "true")
                    {
                        AdvancedExpression = entry.Elements("EntryValue").FirstOrDefault().Attributes("AdvancedExpression").FirstOrDefault();
                    }
                    else
                    {
                        operatorValue = entry.Elements("EntryValue").FirstOrDefault().Attributes("Operator").FirstOrDefault();
                        conditionValue = entry.Elements("EntryValue").FirstOrDefault().Attributes("ConditionValue").FirstOrDefault();
                        cTrueValue = entry.Elements("EntryValue").FirstOrDefault().Attributes("TrueValue").FirstOrDefault();
                        cFalseValue = entry.Elements("EntryValue").FirstOrDefault().Attributes("FalseValue").FirstOrDefault();
                        isTrueSourceProperty = entry.Elements("EntryValue").FirstOrDefault().Attributes("IsTrueSourceProperty").FirstOrDefault();
                        isFalseSourceProperty = entry.Elements("EntryValue").FirstOrDefault().Attributes("IsFalseSourceProperty").FirstOrDefault();

                        trueSrcProperty =
                        entry.Elements("EntryValue").FirstOrDefault().Attributes("TrueSourceProperty").FirstOrDefault() != null
                            ? entry.Elements("EntryValue").FirstOrDefault().Attributes("TrueSourceProperty").FirstOrDefault().Value
                            : null;

                        falseSrcProperty =
                        entry.Elements("EntryValue").FirstOrDefault().Attributes("FalseSourceProperty").FirstOrDefault() != null
                            ? entry.Elements("EntryValue").FirstOrDefault().Attributes("FalseSourceProperty").FirstOrDefault().Value
                            : null;
                    }
                }

                if (property != null)
                {
                    var transformerProperty = new TransformerProperty(property)
                    {
                        MapType = (MapTypes)Enum.Parse(typeof(MapTypes), mapType),
                        Comment = comment,
                        SourceProperty = srcProperties.FirstOrDefault(p => p.Name.Equals(srcProperty)),
                        IsComplex = complexType,
                        IsCustomLogic = customExpression,
                        IsComment = isComment != null && isComment == "true",
                        ConstantValue =
                        {
                            UseAsDefault = defaultValue != null && defaultValue.Value == "true",
                            IsKeyMapping = keyValue != null && keyValue.Value == "true",
                            IsConstant = constant != null && constant.Value == "true",
                             Value = value != null ? value.Value : null,                            
                        },
                        BooleanValue =
                        {
                            IsBoolean = booleanValues != null && booleanValues.Value == "true",
                            TrueValue = trueValue != null ? trueValue.Value : null,
                            FalseValue = falseValue != null ? falseValue.Value : null                       
                        },
                        CustomConditionalValue = 
                        { 
                            IsAdvancedExpression = IsAdvancedExpression != null && IsAdvancedExpression.Value == "true",
                            AdvancedExpression = AdvancedExpression!=null ? AdvancedExpression.Value : null,
                            IsCustomCondition = isCustomCondition != null && isCustomCondition.Value == "true",
                            IsFalseSourceProperty = isFalseSourceProperty != null && isFalseSourceProperty.Value == "true",
                            IsTrueSourceProperty = isTrueSourceProperty != null && isTrueSourceProperty.Value == "true",
                            OperatorValue = operatorValue != null ? operatorValue.Value : null,
                            ConditionValue = conditionValue != null ? conditionValue.Value : null,
                            TrueValue = cTrueValue != null ? cTrueValue.Value : null,
                            FalseValue = cFalseValue != null ? cFalseValue.Value : null,
                            TrueSourceProperty = srcProperties.FirstOrDefault(p => p.Name.Equals(trueSrcProperty)),
                            FalseSourceProperty = srcProperties.FirstOrDefault(p => p.Name.Equals(falseSrcProperty))
                        }
                    };

                    transformer.Properties.Add(transformerProperty);
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp);
            }
        }

        public void DeleteMap(Transformer map)
        {
            try
            {
                // var path = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.MapsXmlPath;
                var path = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.GetSetting(APPLICATION.XML_Base_Path);
                var className = string.Format("{0}_{1}Map.xml", map.SourceClass.Name, map.DestinationClass.Name);

                if(File.Exists(System.IO.Path.Combine(path, className)))
                {
                    var newFolder = "DeletedMaps";
                    var deletedPath = System.IO.Path.Combine(path, newFolder);

                    if (!Directory.Exists(deletedPath))
                    {
                        System.IO.Directory.CreateDirectory(deletedPath);
                    }
                    //Copying file in deleted folder
                    //TODO: Add DateTime & User info while delete file
                    File.Copy(System.IO.Path.Combine(path, className), System.IO.Path.Combine(deletedPath, className), true);
                    File.Delete(System.IO.Path.Combine(path, className));
                    //Removing map from UI
                    this.ExistingMaps.Remove(map);
                }    
            }
            catch (Exception exp)
            {
                ExceptionHelper.ShowMessage(exp);
                CustomLogger.LogException(exp);
            }
        }
    }
}