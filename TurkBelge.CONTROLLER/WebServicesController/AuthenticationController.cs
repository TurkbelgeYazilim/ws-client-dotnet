using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TurkBelge.COMMON;
using TurkBelge.CONTROLLER.RequestSection;
using TurkBelge.SERVICES.serviceOib;


namespace TurkBelge.CONTROLLER.Web_Services
{
    public class AuthenticationController
    {

        EFaturaOIBPortClient authenticationPortClient;

        public static string sesionID;


        public AuthenticationController()
        {
            authenticationPortClient = new EFaturaOIBPortClient();
        }





        public bool Login(string usurname, string password)
        {
            var req = new LoginRequest
            {
                REQUEST_HEADER = new REQUEST_HEADERType
                {
                    SESSION_ID = "-1",
                    APPLICATION_NAME = "TurkBelge.Application"
                },
                USER_NAME = usurname,
                PASSWORD = password
            };
            LoginResponse loginRes = authenticationPortClient.Login(req);

            if (loginRes.ERROR_TYPE == null)
            {
                sesionID = loginRes.SESSION_ID;
                return true;
            }
            else
            {
                MessageBox.Show(loginRes.ERROR_TYPE.ERROR_CODE + " " + loginRes.ERROR_TYPE.ERROR_SHORT_DES);
                return false;
            }
        }






    }
}
