using TurkBelge.CONTROLLER.Singleton;
using TurkBelge.MODEL.Data;
using TurkBelge.MODEL.Entities;
using TurkBelge.SERVICES.serviceArchive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurkBelge.CONTROLLER.DAL
{
    public class ArchiveReportsDal
    {

        public List<ArchiveReports> getReportList()
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                return databaseContext.archiveReports.ToList();
            }
        }


        public int addArcReportFromReportArr(REPORT[] reportArr)
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                ArchiveReports arcReport;

                foreach (var rep in reportArr)
                {
                    arcReport = new ArchiveReports();

                    if (databaseContext.archiveInvoices.Find(arcReport.reportNo) == null)
                    {
                        arcReport.reportNo = rep.REPORT_NO;
                        arcReport.status = rep.REPORT_SUB_STATUS;

                        databaseContext.archiveReports.Add(arcReport);
                    }
                }
               return databaseContext.SaveChanges();
            }
        }




    }
}
