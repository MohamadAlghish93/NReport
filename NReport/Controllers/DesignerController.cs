using NReport.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.DataAccess.Sql;
using System.Web.Configuration;
using NReport.DAL;
using System.Web.Hosting;
using System.IO;
using HelperNVS.FileManagement;
using HelperNVS.ClassesHelper;
using NReport.Models;
using DevExpress.Xpo;

namespace NReport.Controllers
{
    public class DesignerController : BaseController
    {
        [HttpGet]
        public ActionResult Index(string token)
        {
            List<ReportModel> reportModels = new List<ReportModel>();
            var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            if (config.AppSettings.Settings["TarasolLink"] != null)
            {
                if (bool.Parse(config.AppSettings.Settings["TarasolLink"].Value) && (token != ApplicationConstant.TOKEN_AUTH))
                {
                    return View("ErrorPage");
                }
            }

            var connectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"];
            if (connectionString == null)
            {
                return RedirectToAction("Index", "Setting");
            }
            if (string.IsNullOrWhiteSpace(connectionString.ConnectionString))
            {
                return RedirectToAction("Index", "Setting");
            }

            using (var session = SessionFactory.Create())
            {
                string currentPath = HostingEnvironment.ApplicationPhysicalPath;
                string messageError = string.Empty;
                currentPath = Path.Combine(currentPath, ApplicationConstant.LOCATION_REPORT_SAVE_NAME);
                FileManagement fileManagement = new FileManagement();
                List<ClassesHelper.FliesName> reportsFile = fileManagement.GetFilesWithoutExtensionFromDirectory(currentPath, ref messageError);

                if (reportsFile != null)
                {
                    var reports = session.Query<TSOUTERREPORT>()
                    .Select(x => new ReportModel
                    {
                        Url = x.Url,
                        Id = x.Id

                    })
                    .ToArray();
                    if (reportsFile.Count != reports.Count())
                    {

                        foreach (var item in reports)
                        {
                            DeletReport(item.Id);
                        }

                        foreach (var elem in reportsFile)
                        {
                            var reportEntity = new TSOUTERREPORT(session)
                            {
                                Url = elem.Url
                            };
                            session.CommitChanges();
                        }
                        reports = session.Query<TSOUTERREPORT>()
                        .Select(x => new ReportModel
                        {
                            Url = x.Url,
                            Id = x.Id
                        })
                        .ToArray();
                    }

                    reportModels = reports.ToList();
                }

                return View("Index", reportModels);
            }
        }

        public ActionResult GridViewPartial()
        {
            List<ReportModel> reportModels = new List<ReportModel>();
            using (var session = SessionFactory.Create())
            {
                string currentPath = HostingEnvironment.ApplicationPhysicalPath;
                string messageError = string.Empty;
                currentPath = Path.Combine(currentPath, ApplicationConstant.LOCATION_REPORT_SAVE_NAME);
                FileManagement fileManagement = new FileManagement();
                List<ClassesHelper.FliesName> reportsFile = fileManagement.GetFilesWithoutExtensionFromDirectory(currentPath, ref messageError);

                if (reportsFile != null)
                {
                    var reports = session.Query<TSOUTERREPORT>()
                    .Select(x => new ReportModel
                    {
                        Url = x.Url,
                        Id = x.Id

                    })
                    .ToArray();
                    if (reportsFile.Count != reports.Count())
                    {

                        foreach (var item in reports)
                        {
                            DeletReport(item.Id);
                        }

                        foreach (var elem in reportsFile)
                        {
                            var reportEntity = new TSOUTERREPORT(session)
                            {
                                Url = elem.Url
                            };
                            session.CommitChanges();
                        }
                        reports = session.Query<TSOUTERREPORT>()
                        .Select(x => new ReportModel
                        {
                            Url = x.Url,
                            Id = x.Id
                        })
                        .ToArray();
                    }

                    reportModels = reports.ToList();
                }

                return PartialView("GridViewPartial", reportModels);
            }
        }


        public void Delete(long id)
        {

            TSOUTERREPORT obj = DeletReport(id);

            if (obj != null)
            {
                string currentPath = HostingEnvironment.ApplicationPhysicalPath;
                string messageError = string.Empty;
                string url = obj.Url;
                currentPath = Path.Combine(currentPath, ApplicationConstant.LOCATION_REPORT_SAVE_NAME);
                url = url + ApplicationConstant.EXETENSTION_REPORT_SAVE_NAME;
                currentPath = Path.Combine(currentPath, url);
                FileManagement fileManagement = new FileManagement();
                fileManagement.DeleteFileByPath(currentPath, ref messageError);
            }
        }

        public TSOUTERREPORT DeletReport(long id)
        {
            try
            {
                using (var session = SessionFactory.Create())
                {
                    var report = session.GetObjectByKey<TSOUTERREPORT>(id);
                    session.Delete(report);
                    session.CommitChanges();
                    return report;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        public ActionResult Design(string url)
        {
            ViewBag.ShowBackButton = true;
            return View("Design", new DesignModel { Url = url, DataSource = CreateSqlDataSource() });
        }

        [ValidateAntiForgeryToken]
        public ActionResult GridViewUpdatePartial(ReportModel model)
        {
            ViewBag.ShowBackButton = true;
            return View("Design", new DesignModel { Url = model.Url, DataSource = CreateSqlDataSource() });
        }

        [ValidateAntiForgeryToken]
        public ActionResult GridViewCustomActionPartial(string customAction)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return GridViewPartial();
        }
        private void PerformDelete()
        {
            if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
            {
                String selectedRowIds = Request.Params["SelectedRows"];
                List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
                foreach (long item in selectedIds)
                {
                    Delete(item);
                }
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult GridViewAddNewPartial(ReportModel model)
        {
            return View("New", new DesignModel { Url = string.Empty, DataSource = CreateSqlDataSource() });
        }

        [HttpGet]
        public ActionResult New()
        {
            ViewBag.ShowBackButton = true;
            return View("New", new DesignModel { Url = string.Empty, DataSource = CreateSqlDataSource() });
        }

        [HttpGet]
        public ActionResult ViewExist(string url)
        {
            ViewBag.ShowBackButton = true;
            return View("ViewExist", new DesignModel { Url = url, DataSource = CreateSqlDataSource() });
        }

        SqlDataSource CreateSqlDataSource()
        {
            SqlDataSource ds = new SqlDataSource("ConnectionString");
            ds.Name = "TarasolDB";
            //ds.Queries.Add(new CustomSqlQuery("CUSTOMER", "SELECT * FROM [CUSTOMER]"));
            ds.RebuildResultSchema();
            return ds;
        }

        public TSOUTERREPORT GetObjectByUrl(string url)
        {
            using (var session = SessionFactory.Create())
            {
                var reports = session.Query<TSOUTERREPORT>()
                    .Where(e => e.Url == url).FirstOrDefault();
                return reports;
            }
        }
    }
}