using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.ViewModels;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    [DashboardActionFilter]
    [RoutePrefix("api/map")]
    public class MappingController : ApiBaseController
    {

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Entities", Name = "Entities")]
        public IHttpActionResult Entities()
        {
            try
            {
                IClassInfo connectorInfo = new VSI.EDGEAXConnector.ERPDataModels.ClassInfo();
                return Ok(connectorInfo.GetClassesInfo());
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Properties", Name = "Properties")]
        public IHttpActionResult Properties(string entity)
        {
            try
            {
                IClassInfo connectorInfo = new VSI.EDGEAXConnector.ERPDataModels.ClassInfo();
                var info = connectorInfo.GetClassesInfo();
                Type entityType = info.Where(m => m.Name == entity).FirstOrDefault();
                List<KeyValuePair<PropertyInfo, string>> propList = new List<KeyValuePair<PropertyInfo, string>>();
                foreach (var p in entityType.GetProperties().OrderBy(p => p.Name))
                {
                    KeyValuePair<PropertyInfo, string> prop = new KeyValuePair<PropertyInfo, string>(p, p.PropertyType.FullName);
                    propList.Add(prop);
                }
                return Ok(propList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Actions")]
        [Obsolete("Actions is deprecated, please use Actions with POST parameter instead.")]
        public IHttpActionResult Actions(bool generateXML)
        {
            List<KeyValuePair<string, string>> buttons = new List<KeyValuePair<string, string>>();

            if (generateXML)
            {
                KeyValuePair<string, string> btnCons = new KeyValuePair<string, string>("Constant Value", " constant-value=\"true\"");
                KeyValuePair<string, string> btnDefault = new KeyValuePair<string, string>("Default Value", " default-value=\"\"");
                KeyValuePair<string, string> btnRepeat = new KeyValuePair<string, string>("Repeat Node", " repeat=\"true\" data-source=\"\"");
                KeyValuePair<string, string> btnExpression = new KeyValuePair<string, string>("Expression", " expression=\"true\"");
                KeyValuePair<string, string> btnShow = new KeyValuePair<string, string>("Show Node", " show-node=\"true\"");
                KeyValuePair<string, string> btnData = new KeyValuePair<string, string>("Data Object", " data-object=\"\"");
                KeyValuePair<string, string> btnLower = new KeyValuePair<string, string>("To Lower", " to-lower=\"true\"");
                KeyValuePair<string, string> btnUpper = new KeyValuePair<string, string>("To Upper", " to-upper=\"true\"");
                KeyValuePair<string, string> btnAttribute = new KeyValuePair<string, string>("Attribute Expression", "{{Expression}}");
                KeyValuePair<string, string> btnConfiguration = new KeyValuePair<string, string>("ConfigurationHelper.Key", " ConfigurationHelper.KeyName");
                KeyValuePair<string, string> btnConstant = new KeyValuePair<string, string>("Custom Attribute Value", " custom-attribute-value=\"true\"");
                buttons.Add(btnCons);
                buttons.Add(btnDefault);
                buttons.Add(btnRepeat);
                buttons.Add(btnExpression);
                buttons.Add(btnData);
                buttons.Add(btnLower);
                buttons.Add(btnUpper);
                buttons.Add(btnAttribute);
                buttons.Add(btnConfiguration);
                buttons.Add(btnConstant);
                buttons.Add(btnShow);
            }
            else
            {
                KeyValuePair<string, string> btnCons = new KeyValuePair<string, string>("Constant Value", " constant-value=\"\"");
                KeyValuePair<string, string> btnDefault = new KeyValuePair<string, string>("Default Value", " default-value=\"\"");
                KeyValuePair<string, string> btnRepeat = new KeyValuePair<string, string>("Repeat Node", " target-source=\"\" repeat=\"true\"");
                KeyValuePair<string, string> btnProperties = new KeyValuePair<string, string>("Properties", " <Properties></Properties>");
                KeyValuePair<string, string> btnAttName = new KeyValuePair<string, string>("Attribute Name", " is-custom-attribute=\"true\" attribute-id=\"\"");
                KeyValuePair<string, string> btnIsCustom = new KeyValuePair<string, string>("Is Custom Attribute", " expression=\"true\"");
                KeyValuePair<string, string> btnConcatenate = new KeyValuePair<string, string>("Concatenate", " expression=\"true\"");
                buttons.Add(btnCons);
                buttons.Add(btnDefault);
                buttons.Add(btnRepeat);
                buttons.Add(btnProperties);
                buttons.Add(btnAttName);
                buttons.Add(btnIsCustom);
                buttons.Add(btnConcatenate);
            }
            return Ok(buttons);
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Actions")]
        public IHttpActionResult Actions([FromBody] GetActionsRequest ActionsRequest)
        {
            List<KeyValuePair<string, string>> buttons = new List<KeyValuePair<string, string>>();

            if (ActionsRequest.GenerateXML)
            {
                KeyValuePair<string, string> btnCons = new KeyValuePair<string, string>("Constant Value", " constant-value=\"true\"");
                KeyValuePair<string, string> btnDefault = new KeyValuePair<string, string>("Default Value", " default-value=\"\"");
                KeyValuePair<string, string> btnRepeat = new KeyValuePair<string, string>("Repeat Node", " repeat=\"true\" data-source=\"\"");
                KeyValuePair<string, string> btnExpression = new KeyValuePair<string, string>("Expression", " expression=\"true\"");
                KeyValuePair<string, string> btnShow = new KeyValuePair<string, string>("Show Node", " show-node=\"true\"");
                KeyValuePair<string, string> btnData = new KeyValuePair<string, string>("Data Object", " data-object=\"\"");
                KeyValuePair<string, string> btnLower = new KeyValuePair<string, string>("To Lower", " to-lower=\"true\"");
                KeyValuePair<string, string> btnUpper = new KeyValuePair<string, string>("To Upper", " to-upper=\"true\"");
                KeyValuePair<string, string> btnAttribute = new KeyValuePair<string, string>("Attribute Expression", "{{Expression}}");
                KeyValuePair<string, string> btnConfiguration = new KeyValuePair<string, string>("ConfigurationHelper.Key", " ConfigurationHelper.KeyName");
                KeyValuePair<string, string> btnConstant = new KeyValuePair<string, string>("Custom Attribute Value", " custom-attribute-value=\"true\"");
                buttons.Add(btnCons);
                buttons.Add(btnDefault);
                buttons.Add(btnRepeat);
                buttons.Add(btnExpression);
                buttons.Add(btnData);
                buttons.Add(btnLower);
                buttons.Add(btnUpper);
                buttons.Add(btnAttribute);
                buttons.Add(btnConfiguration);
                buttons.Add(btnConstant);
                buttons.Add(btnShow);
            }
            else
            {
                KeyValuePair<string, string> btnCons = new KeyValuePair<string, string>("Constant Value", " constant-value=\"\"");
                KeyValuePair<string, string> btnDefault = new KeyValuePair<string, string>("Default Value", " default-value=\"\"");
                KeyValuePair<string, string> btnRepeat = new KeyValuePair<string, string>("Repeat Node", " target-source=\"\" repeat=\"true\"");
                KeyValuePair<string, string> btnProperties = new KeyValuePair<string, string>("Properties", " <Properties></Properties>");
                KeyValuePair<string, string> btnAttName = new KeyValuePair<string, string>("Attribute Name", " is-custom-attribute=\"true\" attribute-id=\"\"");
                KeyValuePair<string, string> btnIsCustom = new KeyValuePair<string, string>("Is Custom Attribute", " expression=\"true\"");
                KeyValuePair<string, string> btnConcatenate = new KeyValuePair<string, string>("Concatenate", " expression=\"true\"");
                buttons.Add(btnCons);
                buttons.Add(btnDefault);
                buttons.Add(btnRepeat);
                buttons.Add(btnProperties);
                buttons.Add(btnAttName);
                buttons.Add(btnIsCustom);
                buttons.Add(btnConcatenate);
            }
            return Ok(buttons);
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("TemplateXML", Name = "TemplateXML")]
        public IHttpActionResult TemplateXML()
        {
            List<KeyValuePair<string, string>> templates = null;
            MappingTemplateDAL mapTem = null;
            try
            {
                templates = new List<KeyValuePair<string, string>>();
                mapTem = new MappingTemplateDAL(this.DbConnStr, this.StoreKey, this.User);
                List<MappingTemplate> tempList = mapTem.GetAllMappingTemplates();
                //List<XML> xmls = new List<XML>();
                //tempList.ForEach(m =>
                //{
                //    XML xml = new XML();
                //    xml.Name = m.Name;
                //    using (StringReader s = new StringReader(m.XML))
                //    {
                //        xml.Xml = XDocument.Load(s);
                //    }
                //    xmls.Add(xml);
                //});
                List<string> tempNames = tempList.Select(m => m.Name).ToList();
                tempNames.ForEach(m =>
                {
                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>(m, " file-source=\"" + m + "\" data-source=\"\"");
                    templates.Add(pair);
                });
                return Ok(templates);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Templates", Name = "Templates")]
        public IHttpActionResult Templates()
        {
            List<KeyValuePair<string, string>> templates = null;
            MappingTemplateDAL mapTem = null;
            List<MappingTemplate> tempList = null;
            try
            {
                templates = new List<KeyValuePair<string, string>>();
                mapTem = new MappingTemplateDAL(this.DbConnStr, this.StoreKey, this.User);
                tempList = mapTem.GetAllMappingTemplates();
                //List<XML> xmls = new List<XML>();
                //tempList.ForEach(m =>
                //{
                //    XML xml = new XML();
                //    xml.Name = m.Name;
                //    using (StringReader s = new StringReader(m.XML))
                //    {
                //        xml.Xml = XDocument.Load(s);
                //    }
                //    xmls.Add(xml);
                //});
                List<string> tempNames = tempList.Select(m => m.Name).ToList();
                return Ok(tempNames);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("ReadXML")]
        [Obsolete("ReadXML is deprecated, please use ReadXML with POST parameter instead.")]
        public IHttpActionResult ReadXML(string templateName)
        {
            List<KeyValuePair<string, string>> templates = null;
            MappingTemplateDAL mapTem = null;
            MappingTemplate mapTemplate = null;
            try
            {
                mapTem = new MappingTemplateDAL(this.DbConnStr, this.StoreKey, this.User);
                templates = new List<KeyValuePair<string, string>>();
                mapTemplate = mapTem.GetMappingTemplateByName(templateName);
                if (mapTemplate != null)
                {
                    return Ok(mapTemplate.XML);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("ReadXML")]
        public IHttpActionResult ReadXML([FromBody] GetReadXMLRequest ReadXMLRequest)
        {
            List<KeyValuePair<string, string>> templates = null;
            MappingTemplateDAL mapTem = null;
            MappingTemplate mapTemplate = null;
            try
            {
                mapTem = new MappingTemplateDAL(this.DbConnStr, this.StoreKey, this.User);
                templates = new List<KeyValuePair<string, string>>();
                mapTemplate = mapTem.GetMappingTemplateByName(ReadXMLRequest.TemplateName);
                if (mapTemplate != null)
                {
                    return Ok(mapTemplate.XML);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Delete")]
        [Obsolete("Delete is deprecated, please use Delete with POST parameter instead.")]
        public IHttpActionResult Delete(string templateName)
        {
            MappingTemplate mapTemp = null;
            MappingTemplateDAL mapTempDal = null;
            try
            {
                mapTempDal = new MappingTemplateDAL(this.DbConnStr, this.StoreKey, this.User);
                var done = mapTempDal.DeleteMappingTemplate(templateName);
                return Ok(done);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Delete")]
        public IHttpActionResult Delete([FromBody] GetDeleteRequest DeleteRequest)
        {
            MappingTemplate mapTemp = null;
            MappingTemplateDAL mapTempDal = null;
            try
            {
                mapTempDal = new MappingTemplateDAL(this.DbConnStr, this.StoreKey, this.User);
                var done = mapTempDal.DeleteMappingTemplate(DeleteRequest.TemplateName);
                return Ok(done);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Save", Name = "Save")]
        public IHttpActionResult Save(XMLMap map)
        {
            try
            {
                XDocument xml = new XDocument();
                using (StringReader s = new StringReader(map.XML))
                {
                    xml = XDocument.Load(s);
                }
                MappingTemplate mapTemp;
                MappingTemplateDAL mapTempDal = new MappingTemplateDAL(this.DbConnStr, this.StoreKey, this.User);
                mapTemp = mapTempDal.GetMappingTemplateByName(map.Name);
                if (mapTemp == null)
                {
                    mapTemp = new MappingTemplate();
                    mapTemp.Name = map.Name;
                    mapTemp.XML = map.XML;
                    mapTemp.IsActive = map.isActive;
                    mapTemp.MappingTemplateTypeId = 1; //SK - Only XML Mappings allowed so far.
                    mapTemp.SourceEntity = map.SourceEntity;
                    mapTemp.CreatedBy = this.User;
                    mapTemp.CreatedOn = DateTime.UtcNow;
                    mapTemp.ModifiedOn = mapTemp.CreatedOn;
                    mapTemp.ReadMode = map.Type;
                    mapTempDal.AddMappingTemplate(mapTemp);
                    return Ok("ADDED");

                }
                else
                {
                    mapTemp.ModifiedOn = DateTime.UtcNow;
                    mapTemp.Name = map.Name;
                    mapTemp.XML = map.XML;
                    mapTemp.IsActive = map.isActive;
                    mapTemp.MappingTemplateTypeId = 1; //SK - Only XML Mappings allowed so far.
                    mapTemp.SourceEntity = map.SourceEntity;
                    mapTemp.ModifiedBy = this.User;
                    mapTemp.ReadMode = map.Type;
                    mapTemp.ModifiedOn = mapTemp.CreatedOn;
                    mapTempDal.UpdateMappingTemplate(mapTemp);
                    return Ok("UPDATED");
                }

            }
            catch (XmlException ex)
            {
                return BadRequest("Invalid XML");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Get", Name = "Get")]
        public IHttpActionResult Get(string templateName)
        {
            MappingTemplate mapTemp = null;
            MappingTemplateDAL mapTempDal = null;
            try
            {
                mapTempDal = new MappingTemplateDAL(this.DbConnStr, this.StoreKey, this.User);
                mapTemp = mapTempDal.GetMappingTemplateByName(templateName);
                if (mapTemp != null)
                {
                    XMLMap map = new XMLMap();
                    map.isActive = mapTemp.IsActive;
                    map.Name = mapTemp.Name;
                    map.SourceEntity = mapTemp.SourceEntity;
                    map.Type = mapTemp.ReadMode;
                    map.XML = mapTemp.XML;
                    return Ok(map);
                }
                else
                {
                    return BadRequest("Template Not Found");
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Backup", Name = "Backup")]
        public dynamic Backup()
        {
            try
            {
                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Temp");
                var templatePath = mappedPath + "/Templates";
                DirectoryInfo backupDir = System.IO.Directory.CreateDirectory(mappedPath);
                DirectoryInfo temDir = System.IO.Directory.CreateDirectory(templatePath);

                MappingTemplateDAL mapTempDal = new MappingTemplateDAL(this.DbConnStr, this.StoreKey, this.User);
                List<MappingTemplate> templates = mapTempDal.GetAllMappingTemplates();
                string parentPath = temDir.FullName;
                templates.ForEach(m =>
                {
                    string fileName = m.Name;
                    string[] data = { m.XML };
                    System.IO.File.WriteAllLines(parentPath + "//" + fileName + ".xml", data);
                });
                var targetZip = mappedPath + "/" + DateTime.UtcNow.Ticks.ToString() + ".zip";
                System.IO.Compression.ZipFile.CreateFromDirectory(parentPath, targetZip);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(new FileStream(targetZip, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = "backup.zip";
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/zip");
                temDir.Delete(true);
                return response;
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        // [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Upload", Name = "Upload")]
        public async Task<IHttpActionResult> Upload()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.Contents)
                {
                    var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                    if (filename.Split('.').Last().Equals("xml"))
                    {
                        var buffer = await file.ReadAsByteArrayAsync();
                        var str = System.Text.Encoding.UTF8.GetString(buffer);
                        var fileParts = filename.Split('.');
                        if (fileParts.Length != 3)
                        {
                            return BadRequest("Invalid File");
                        }
                        var type = filename.Split('.')[0];
                        var entity = filename.Split('.')[1];

                        //Validate the xml
                        //XDocument xml = new XDocument();
                        //using (StringReader s = new StringReader(str))
                        //{
                        //    xml = XDocument.Load(s);
                        //}

                        MappingTemplate mapTemp;
                        MappingTemplateDAL mapTempDal = new MappingTemplateDAL(this.DbConnStr, this.StoreKey, this.User);
                        mapTemp = mapTempDal.GetMappingTemplateByName(type + "." + entity);

                        if (mapTemp != null)
                        {
                            mapTemp.IsActive = true;
                            mapTemp.Name = type + "." + entity;
                            mapTemp.XML = str;
                            mapTemp.SourceEntity = entity;
                            //mapTemp.Type = type;
                            mapTemp.ModifiedOn = DateTime.UtcNow;
                            var updateMap = mapTempDal.UpdateMappingTemplate(mapTemp);
                            if (updateMap == null)
                            {
                                return BadRequest("FAIL");
                            }
                        }
                        if (mapTemp == null)
                        {
                            mapTemp = new MappingTemplate();
                            mapTemp.Name = type + "." + entity;
                            mapTemp.XML = str;
                            mapTemp.IsActive = true;
                            mapTemp.MappingTemplateTypeId = 1; //SK - Only XML Mappings allowed so far.
                            //mapTemp.Type = type;
                            mapTemp.SourceEntity = entity;
                            mapTemp.CreatedBy = this.User;
                            mapTemp.CreatedOn = DateTime.UtcNow;
                            mapTemp.ModifiedOn = mapTemp.CreatedOn;
                            var updateMap = mapTempDal.AddMappingTemplate(mapTemp);
                            if (updateMap == null)
                            {
                                return BadRequest("FAIL");
                            }

                        }
                        return Ok(true);
                    }
                    else
                    {
                        return BadRequest("Invalid File");
                    }
                }
                return Ok();
            }
            catch (XmlException ex)
            {
                return BadRequest("Invalid XML");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        #region Integration Controller, Response classes

        public class GetActionsRequest
        {

            public bool GenerateXML { get; set; }

        }
        public class GetReadXMLRequest
        {

            public string TemplateName { get; set; }

        }

        public class GetDeleteRequest
        {

            public string TemplateName { get; set; }

        }


        #endregion
    }
}