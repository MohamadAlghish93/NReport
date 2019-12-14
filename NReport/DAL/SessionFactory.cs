using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace NReport.DAL
{
    public static class SessionFactory
    {
        static readonly IDataLayer dataLayer;

        static SessionFactory()
        {
            var dictionary = new ReflectionDictionary();
            dictionary.GetDataStoreSchema(typeof(SessionFactory).Assembly);

            var connectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            var dataStore = XpoDefault.GetConnectionProvider(connectionString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);

            dataLayer = new ThreadSafeDataLayer(dictionary, dataStore);
        }

        public static UnitOfWork Create()
        {
            return new UnitOfWork(dataLayer);
        }
    }
}