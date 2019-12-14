using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Xpo;

namespace NReport.DAL
{
    [DeferredDeletion(false)]
    public class TSOUTERREPORT : XPCustomObject
    {
        long id;
        string url;
        string name;
        byte[] layout;

        public TSOUTERREPORT(Session session)
            : base(session)
        {
        }

        [Key(AutoGenerate = true)]
        public long Id
        {
            get { return id; }
            set { SetPropertyValue("Id", ref id, value); }
        }

        public string Url
        {
            get { return url; }
            set { SetPropertyValue("Url", ref url, value); }
        }

        public byte[] Layout
        {
            get { return layout; }
            set { SetPropertyValue("Layout", ref layout, value); }
        }
    }
}