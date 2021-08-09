using TurkBelge.CONTROLLER.DAL;
using TurkBelge.CONTROLLER.Web_Services;
using TurkBelge.CONTROLLER.WebServicesController;
using TurkBelge.MODEL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TurkBelge.CONTROLLER.Singleton
{
   public class Singl
    {
      
        private static AuthenticationController authenticationController = null;
        private static EInvoiceController invoiceController = null;
        private static ArchiveController archiveController = null;
        private static GibUserController gibUserController = null;
        private static InvoiceDal invoiceDal = null;
        private static ArchiveInvoicesDal archiveInvoiceDal = null;
        private static ArchiveReportsDal archiveReportsDal = null;
        private static IdSerilazeDal idSerilazeDal = null;
        private static UserInformationDal userInformationDal = null;
        private static GibUsersDal gibUsersDal = null;

       


        public static GibUsersDal gibUsersDalGet
        {
            get
            {
                if (null == gibUsersDal)
                {
                    gibUsersDal = new GibUsersDal();
                }

                return gibUsersDal;
            }
        }

        public static UserInformationDal userInformationDalGet
        {
            get
            {
                if (null == userInformationDal)
                {
                    userInformationDal = new UserInformationDal();
                }

                return userInformationDal;
            }
        }


        public static IdSerilazeDal invIdSerilazeDalGet
        {
            get
            {
                if (null == idSerilazeDal)
                {
                    idSerilazeDal = new IdSerilazeDal();
                }

                return idSerilazeDal;
            }
        }

        public static ArchiveInvoicesDal archiveInvoiceDalGet
        {
            get
            {
                if (null == archiveInvoiceDal)
                {
                    archiveInvoiceDal = new  ArchiveInvoicesDal();
                }

                return archiveInvoiceDal;
            }
        }


        public static ArchiveReportsDal ArchiveReportsDalGet
        {
            get
            {
                if (null == archiveReportsDal)
                {
                    archiveReportsDal = new ArchiveReportsDal();
                }

                return archiveReportsDal;
            }
        }


      


        public static InvoiceDal invoiceDalGet
        {
            get
            {
                if (null == invoiceDal)
                {
                    invoiceDal = new InvoiceDal();
                }

                return invoiceDal;
            }
        }



        public static ArchiveController archiveControllerGet
        {
            get
            {
                if (null == archiveController)
                {
                    archiveController = new ArchiveController();
                }

                return archiveController;
            }
        }

        public static AuthenticationController authControllerGet
        {
            get
            {
                if (null == authenticationController)
                {
                    authenticationController = new AuthenticationController();
                }

                return authenticationController;
            }
        }

        public static EInvoiceController invoiceControllerGet
        {
            get
            {
                if (null == invoiceController)
                {
                    invoiceController = new EInvoiceController();
                }

                return invoiceController;
            }
        }




        public static GibUserController GibUserControllerGet
        {
            get
            {
                if (null == gibUserController)
                {
                    gibUserController = new GibUserController();
                }

                return gibUserController;
            }
        }




    }
}
