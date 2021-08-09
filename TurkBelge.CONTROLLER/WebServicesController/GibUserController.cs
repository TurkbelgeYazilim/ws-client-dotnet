using TurkBelge.COMMON;
using TurkBelge.COMMON.FileControl;
using TurkBelge.CONTROLLER.RequestSection;
using TurkBelge.CONTROLLER.Model;
using TurkBelge.CONTROLLER.Singleton;
using TurkBelge.SERVICES.serviceOib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace TurkBelge.CONTROLLER.WebServicesController
{
    public class GibUserController
    {

        private EFaturaOIBPortClient authenticationPortClient = new EFaturaOIBPortClient();


        public GibUserController()
        {
            RequestHeader.createRequestHeaderAuth();
        }




        public string getGibUserList(string ProductType)
        {
            using (new OperationContextScope(authenticationPortClient.InnerChannel))
            {
                GetUserListBinaryRequest req = new GetUserListBinaryRequest();
                req.REQUEST_HEADER = RequestHeader.getRequestHeaderAuth;
                req.DOCUMENT_TYPE = ProductType;//type INVOICE verileri cok buyuk ıslemı gormek
                                                //için "DESPATCHADVICE" secebılırsınız ya da despatch sayfasında da   
                                                //gib userlist alma ıslemıne bakabılırsınız
                req.REGISTER_TIME_START = DateTime.Now;
                GetUserListBinaryResponse response = authenticationPortClient.GetUserListBinary(req);
                if (response.Item == null)
                {
                    return "basarısız işlem";
                }
                else
                {
                    base64Binary content = (base64Binary)response.Item;
                    if (content.Value != null)
                    {
                        byte[] unCompressedContent = Compress.UncompressFile(content.Value);
                        string xmlInputData = Encoding.UTF8.GetString(unCompressedContent);

                        //xml duzenlenmesı
                        xmlInputData = xmlInputData.Replace(" xmlns=\"http://schemas.i2i.com/ei/wsdl\"", "").Trim();
                        //serilaze islemı
                        GibUserListXmlModel gibUserList = XmlSerilazer.Deserialize<GibUserListXmlModel>(xmlInputData);
                        //db ye ekleme
                        if (Singl.gibUsersDalGet.addGibUserList(gibUserList) != gibUserList.Items.Count)//db ye kayıt basarısızsa
                        {
                            return "DB ye kayıt basarısız";
                        }
                    }
                    return null;
                }
            }
        }





    }
}
