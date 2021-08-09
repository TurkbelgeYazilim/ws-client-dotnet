using TurkBelge.COMMON;
using TurkBelge.CONTROLLER.Web_Services;
using TurkBelge.SERVICES.serviceOib;
using TurkBelge.SERVICES.serviceArchive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurkBelge.CONTROLLER.RequestSection
{
    public  class RequestHeader
    {


        public static SERVICES.serviceOib.REQUEST_HEADERType getRequestHeaderAuth;
        public static SERVICES.serviceOib.REQUEST_HEADERType getRequestHeaderOib;
        public static SERVICES.serviceArchive.REQUEST_HEADERType getRequestHeaderArchive;





        public static void createRequestHeaderAuth()
        {
            getRequestHeaderAuth = new SERVICES.serviceOib.REQUEST_HEADERType() //default degerler ısterse degısebılır
            {
                SESSION_ID = AuthenticationController.sesionID,
                APPLICATION_NAME = "TurkBelge.Application",
                COMPRESSED = EI.ActiveOrPasive.Y.ToString()
            };
        }



        public static void createRequestHeaderOib()
        {
            getRequestHeaderOib = new SERVICES.serviceOib.REQUEST_HEADERType() //default degerler ısterse degısebılır
            {
                SESSION_ID = AuthenticationController.sesionID,
                APPLICATION_NAME = "TurkBelge.Application",
                COMPRESSED = EI.ActiveOrPasive.Y.ToString()
            };
        }


        public static void createRequestHeaderArchive()
        {
            getRequestHeaderArchive = new SERVICES.serviceArchive.REQUEST_HEADERType() //default degerler ısterse degısebılır
            {
                SESSION_ID = AuthenticationController.sesionID,
                APPLICATION_NAME = "TurkBelge.Application",
                COMPRESSED = EI.ActiveOrPasive.Y.ToString()
            };
        }




    }
}
