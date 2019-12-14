using System.Collections.Generic;
using DevExpress.XtraReports.Web.Extensions;
using System.Linq;
using System.IO;
using System.Web.Hosting;
using DevExpress.Xpo;
using NReport.DAL;
using NReport.Constant;
using HelperNVS.FileManagement;

namespace NReport
{
    public class ReportStorageWebExtension1 : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        public TSOUTERREPORT GetObjectByUrl(string url)
        {
            using (var session = SessionFactory.Create())
            {
                var reports = session.Query<TSOUTERREPORT>()
                    .Where(e => e.Url == url).FirstOrDefault();
                return reports;
            }
        }

        public override bool CanSetData(string url)
        {
            // Check if the URL is available in the report storage.
            //using (var session = SessionFactory.Create())
            //{
            //    return session.GetObjectByKey<TSOUTERREPORT>(url) != null;
            //}
            return GetObjectByUrl(url) != null;
        }
        public override bool IsValidUrl(string url)
        {
            // Check if the specified URL is valid for the current report storage.
            // In this example, a URL should be a string containing a numeric value that is used as a data row primary key.
            return true;
        }

        public override byte[] GetData(string url)
        {
            // Get the report data from the storage.
            using (var session = SessionFactory.Create())
            {
                MemoryStream vs = this.ReadFromFile(url);
                return vs.ToArray();
                //return report.Layout;
            }
        }

        public override Dictionary<string, string> GetUrls()
        {
            // Get URLs and display names for all reports available in the storage
            using (var session = SessionFactory.Create())
            {
                return session.Query<TSOUTERREPORT>().ToDictionary<TSOUTERREPORT, string, string>(report => report.Url, report => report.Url);
            }
        }

        public override void SetData(DevExpress.XtraReports.UI.XtraReport report, string url)
        {
            // Write a report to the storage under the specified URL.
            using (var session = SessionFactory.Create())
            {
                var reportEntity = GetObjectByUrl(url); // session.GetObjectByKey<TSOUTERREPORT>(url);


                MemoryStream ms = new MemoryStream();
                report.SaveLayout(ms);
                //reportEntity.Layout = ms.ToArray();
                this.SaveToFile(reportEntity.Url, ms);

                session.CommitChanges();
            }
        }

        public override string SetNewData(DevExpress.XtraReports.UI.XtraReport report, string defaultUrl)
        {
            // Save a report to the storage under a new URL. 
            // The defaultUrl parameter contains the report display name specified by a user.
            if (CanSetData(defaultUrl))
                SetData(report, defaultUrl);
            else
                using (var session = SessionFactory.Create())
                {
                    MemoryStream ms = new MemoryStream();
                    report.SaveLayout(ms);

                    var reportEntity = new TSOUTERREPORT(session)
                    {
                        Url = defaultUrl
                    };
                    this.SaveToFile(reportEntity.Url, ms);
                    session.CommitChanges();
                }
            return defaultUrl;
        }

        public void SaveToFile(string name, MemoryStream ms)
        {
            string currentPath = HostingEnvironment.ApplicationPhysicalPath;
            string messageError = string.Empty;
            string directoryPath = Path.Combine(currentPath, ApplicationConstant.LOCATION_REPORT_SAVE_NAME);
            name = name + ApplicationConstant.EXETENSTION_REPORT_SAVE_NAME;
            currentPath = Path.Combine(directoryPath, name);
            FileManagement fileManagement = new FileManagement();
            if (fileManagement.MemoryStreamWrite(currentPath, directoryPath, ms, ref messageError))
            {

            }
        }

        public MemoryStream ReadFromFile(string name)
        {
            string currentPath = HostingEnvironment.ApplicationPhysicalPath;
            string messageError = string.Empty;
            currentPath = Path.Combine(currentPath, ApplicationConstant.LOCATION_REPORT_SAVE_NAME);
            name = name + ApplicationConstant.EXETENSTION_REPORT_SAVE_NAME;
            currentPath = Path.Combine(currentPath, name);
            FileManagement fileManagement = new FileManagement();
            return (fileManagement.MemoryStreamRead(currentPath, ref messageError));
        }
    }
}
