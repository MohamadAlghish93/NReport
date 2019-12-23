using NReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace NReport
{
    [ValidateInput(false)]
    public class SettingController : Controller
    {
        // GET: Setting
        public ActionResult Index()
        {

            return View(new SettingModel());
        }

        [HttpPost]
        public ActionResult saveSetting(SettingModel settingModel)
        {
            if (Request.Params["btnUpdate"] == null)
            {
                ModelState.Clear();
                return View("Index", settingModel);
            }

            if (ModelState.IsValid)
            {

                // save in XML file
                bool isNew = true;
                string name = "ConnectionString";
                string path = Server.MapPath("~/Web.Config");
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNodeList list = doc.DocumentElement.SelectNodes(string.Format("connectionStrings/add[@name='{0}']", name));
                XmlNode node;
                isNew = list.Count == 0;
                if (isNew)
                {
                    node = doc.CreateNode(XmlNodeType.Element, "add", null);
                    XmlAttribute attribute = doc.CreateAttribute("name");
                    attribute.Value = name;
                    node.Attributes.Append(attribute);

                    attribute = doc.CreateAttribute("connectionString");
                    attribute.Value = "";
                    node.Attributes.Append(attribute);

                    attribute = doc.CreateAttribute("providerName");
                    attribute.Value = "System.Data.SqlClient";
                    node.Attributes.Append(attribute);
                }
                else
                {
                    node = list[0];
                }
                string conString = node.Attributes["connectionString"].Value;
                SqlConnectionStringBuilder conStringBuilder = new SqlConnectionStringBuilder(conString);
                conStringBuilder.InitialCatalog = settingModel.DatabaseName;
                conStringBuilder.DataSource = settingModel.ServerName;
                conStringBuilder.IntegratedSecurity = false;
                conStringBuilder.UserID = settingModel.UserName;
                conStringBuilder.Password = settingModel.Password;
                conStringBuilder.PersistSecurityInfo = true;
                node.Attributes["connectionString"].Value = conStringBuilder.ConnectionString;
                if (isNew)
                {
                    doc.DocumentElement.SelectNodes("connectionStrings")[0].AppendChild(node);
                }

                using (SqlConnection connection = new SqlConnection(conStringBuilder.ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        doc.Save(path);
                    }
                    catch (SqlException e)
                    {
                        ViewBag.MessageError = e.Message;
                        return View("Index", settingModel);
                    }
                }
                //

                return RedirectToAction("Index", "Designer");
            }
            return View("Index", settingModel);
        }
    }
}