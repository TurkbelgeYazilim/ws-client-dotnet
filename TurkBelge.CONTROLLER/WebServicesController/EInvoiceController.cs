﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TurkBelge.MODEL.Entities;
using TurkBelge.SERVICES.serviceOib;
using TurkBelge.COMMON;
using TurkBelge.CONTROLLER.Singleton;
using TurkBelge.CONTROLLER.RequestSection;
using TurkBelge.COMMON.FileControl;
using System.IO;
using TurkBelge.COMMON.UblSerializer;
using UblInvoice;
using TurkBelge.MODEL.Data;

namespace TurkBelge.CONTROLLER.Web_Services
{



    public class EInvoiceController
    {

        private EFaturaOIBPortClient eInvoiceOIBPortClient = new EFaturaOIBPortClient();

        List<INVOICE> invoiceList = new List<INVOICE>();




        public EInvoiceController()
        {
            SearchKey.createInvoiceSearchKey();
            SearchKey.createInvoiceSearchKeyGetInvoiceWithType();
            RequestHeader.createRequestHeaderOib();
        }


        /// <summary>
        /// servisten getInvoice ardından mark ınvoıce ve db ye kaydetme ıslemını yapar
        /// </summary>
        /// 
        public string getInvoiceOnServiceAndSaveDb(string direction)
        {
            using (new OperationContextScope(eInvoiceOIBPortClient.InnerChannel))
            {
                var req = new GetInvoiceRequest(); //sistemdeki gelen efatura listesi için request parametreleri
                req.REQUEST_HEADER = RequestHeader.getRequestHeaderOib;
                req.REQUEST_HEADER.COMPRESSED = EI.ActiveOrPasive.Y.ToString();
                req.INVOICE_SEARCH_KEY = SearchKey.getSearchKeyInvoice;
                req.HEADER_ONLY = EI.ActiveOrPasive.N.ToString();

                if (direction.Equals(nameof(EI.Direction.DRAFT))) //direction taslak fatura ıse
                {
                    req.INVOICE_SEARCH_KEY.DIRECTION = EI.Direction.OUT.ToString();
                    req.INVOICE_SEARCH_KEY.DRAFT_FLAG = EI.ActiveOrPasive.Y.ToString();
                }
                else
                {
                    req.INVOICE_SEARCH_KEY.DIRECTION = direction;
                }

                GetInvoiceResponse getInvoiceResponse = eInvoiceOIBPortClient.GetInvoice(req);
                INVOICE[] invoiceArray = getInvoiceResponse.INVOICE;
                if (invoiceArray != null && invoiceArray.Length > 0)
                {
                    string markErrorMessage = invoiceMarkRead(invoiceArray);
                    if (markErrorMessage != null) //mark despatch dan donen error message varsa
                    {
                        return markErrorMessage;
                    }
                    //getirilen faturaları db ye kaydet
                    if (Singl.invoiceDalGet.addInvoiceToDbAndSaveContentOnDisk(invoiceArray, direction) != invoiceArray.Length)
                    {
                        return "DataBase'e kaydetme işlemi başarısız";
                    }
                    return null;
                }
                return "servisten getirilecek fatura bulunamdı";
            }
        }





        /// <summary>
        /// basarılıysa ınvoice content doner basarısızsa null doner 
        /// </summary>
        public string getInvoiceWithUuidOnService(string uuid, string direction)
        {
            using (new OperationContextScope(eInvoiceOIBPortClient.InnerChannel))
            {
                GetInvoiceRequest req = new GetInvoiceRequest(); //sistemdeki gelen efatura listesi için request parametreleri
                req.REQUEST_HEADER = RequestHeader.getRequestHeaderOib;
                req.REQUEST_HEADER.COMPRESSED = EI.ActiveOrPasive.Y.ToString();
                req.INVOICE_SEARCH_KEY = SearchKey.getSearchKeyInvoice;
                req.INVOICE_SEARCH_KEY.UUID = uuid;
                req.INVOICE_SEARCH_KEY.READ_INCLUDED = true;
                req.INVOICE_SEARCH_KEY.READ_INCLUDEDSpecified = true;
                req.HEADER_ONLY = EI.ActiveOrPasive.N.ToString();

                if (direction.Equals(nameof(EI.Direction.DRAFT))) //direction taslak fatura ıse
                {
                    req.INVOICE_SEARCH_KEY.DIRECTION = EI.Direction.OUT.ToString();
                    req.INVOICE_SEARCH_KEY.DRAFT_FLAG = EI.ActiveOrPasive.Y.ToString();
                }
                else
                {
                    req.INVOICE_SEARCH_KEY.DIRECTION = direction;
                }
                INVOICE[] invoiceArray = eInvoiceOIBPortClient.GetInvoice(req).INVOICE;

                if (invoiceArray != null && invoiceArray.Length != 0)
                {
                    //getirilen faturanın contentını zipten cıkar , string halınde dondur
                    return Encoding.UTF8.GetString(Compress.UncompressFile(invoiceArray[0].CONTENT.Value));
                }

                return null;
            }
        }



        private string invoiceMarkRead(INVOICE[] invoiceList)
        {
            using (new OperationContextScope(eInvoiceOIBPortClient.InnerChannel))
            {
                var markReq = new MarkInvoiceRequest(); //sistemdeki gelen efatura listesi için request parametreleri

                markReq.REQUEST_HEADER = RequestHeader.getRequestHeaderOib;
                markReq.MARK = new MarkInvoiceRequestMARK();

                List<INVOICE> listInvoiceMark = new List<INVOICE>();
                for (int i = 0; i < invoiceList.Length; i++)
                {
                    INVOICE inv = new INVOICE();
                    inv.ID = invoiceList[i].ID;
                    inv.UUID = invoiceList[i].UUID;

                    listInvoiceMark.Add(inv);
                }

                markReq.MARK.INVOICE = listInvoiceMark.ToArray();
                markReq.MARK.value = MarkInvoiceRequestMARKValue.READ;
                markReq.MARK.valueSpecified = true;

                MarkInvoiceResponse markRes = eInvoiceOIBPortClient.MarkInvoice(markReq);

                if (markRes.REQUEST_RETURN != null && markRes.REQUEST_RETURN.RETURN_CODE == 0)//basarılıysa
                {
                    return null;
                }
                else
                {
                    return "mark ınvoice basarısız";
                }
            }
        }




        public void createInvListWithContent(bool withZip, string xmlStr)
        {
            INVOICE invoice = new INVOICE();
            base64Binary contentByte = new base64Binary();

            if (withZip) //zipli gönderilmek isteniyorsa
            {
                contentByte.Value = Compress.compressFile(xmlStr);
            }
            else  //zipsiz content value
            {
                contentByte.Value = Encoding.UTF8.GetBytes(xmlStr);
            }
            invoice.CONTENT = contentByte;

            invoiceList.Add(invoice);
        }






        public int sendInvoice(string receiverAlias, bool isWithZip)
        {
            using (new OperationContextScope(eInvoiceOIBPortClient.InnerChannel))
            {
                var req = new SendInvoiceRequest();
                req.REQUEST_HEADER = RequestHeader.getRequestHeaderOib;
                if (isWithZip)
                {
                    req.REQUEST_HEADER.COMPRESSED = EI.ActiveOrPasive.Y.ToString();
                }
                else
                {
                    req.REQUEST_HEADER.COMPRESSED = EI.ActiveOrPasive.N.ToString();
                }
                req.SENDER = new SendInvoiceRequestSENDER();
                req.RECEIVER = new SendInvoiceRequestRECEIVER();
                req.RECEIVER.alias = receiverAlias;

                req.INVOICE = invoiceList.ToArray();

                invoiceList.Clear();

                var res = eInvoiceOIBPortClient.SendInvoice(req);
                if (res.REQUEST_RETURN != null)
                {
                    return res.REQUEST_RETURN.RETURN_CODE;
                }
                return -1;
            }
        }



        public int loadInvoice(bool isWithZip)
        {
            using (new OperationContextScope(eInvoiceOIBPortClient.InnerChannel))
            {
                LoadInvoiceRequest req = new LoadInvoiceRequest();

                req.REQUEST_HEADER = RequestHeader.getRequestHeaderOib;
                if (isWithZip) //zipli gonderılmek ıstenıyorsa
                {
                    req.REQUEST_HEADER.COMPRESSED = EI.ActiveOrPasive.Y.ToString();
                }
                else //zipsiz ise
                {
                    req.REQUEST_HEADER.COMPRESSED = EI.ActiveOrPasive.N.ToString();
                }
                req.INVOICE = invoiceList.ToArray();
                invoiceList.Clear();
                var res = eInvoiceOIBPortClient.LoadInvoice(req);
                if (res.REQUEST_RETURN != null)
                {
                    return res.REQUEST_RETURN.RETURN_CODE;
                }
                return -1;
            }
        }



        public int sendInvoiceResponse(string status, List<string> description)
        {
            using (new OperationContextScope(eInvoiceOIBPortClient.InnerChannel))
            {
                SendInvoiceResponseWithServerSignRequest req = new SendInvoiceResponseWithServerSignRequest()
                {
                    REQUEST_HEADER = RequestHeader.getRequestHeaderOib,
                    STATUS = status,
                    INVOICE = invoiceList.ToArray(),
                    DESCRIPTION = description.ToArray(),
                };
                invoiceList.Clear();

                var res= eInvoiceOIBPortClient.SendInvoiceResponseWithServerSign(req);
                if (res.REQUEST_RETURN != null)
                {
                    return res.REQUEST_RETURN.RETURN_CODE;
                }
                return -1;
            }
        }



        public void createInvListWithUuid(string uuid)
        {
            INVOICE invoice = new INVOICE();
            invoice.UUID = uuid;
            invoiceList.Add(invoice);
        }



        /// <summary>
        /// islem basarılıysa status doner degılse null doner
        /// </summary>
        public GetInvoiceStatusResponseINVOICE_STATUS getInvoiceStatatus(string invoiceUuid)
        {
            INVOICE invoice = new INVOICE();
            invoice.UUID = invoiceUuid;

            using (new OperationContextScope(eInvoiceOIBPortClient.InnerChannel))
            {
                GetInvoiceStatusRequest req = new GetInvoiceStatusRequest()
                {
                    REQUEST_HEADER = RequestHeader.getRequestHeaderOib,
                    INVOICE = invoice,
                };

              return eInvoiceOIBPortClient.GetInvoiceStatus(req).INVOICE_STATUS;
            }
        }




        /// <summary>
        /// IMZALI XML DISKE KAYDET
        /// </summary>

        public bool isGetInvoiceSingnedXml(string direction)
        {
            using (new OperationContextScope(eInvoiceOIBPortClient.InnerChannel))
            {
                GetInvoiceRequest req = new GetInvoiceRequest();
                req.REQUEST_HEADER = RequestHeader.getRequestHeaderOib;
                req.REQUEST_HEADER.COMPRESSED = nameof(EI.ActiveOrPasive.Y);
                req.INVOICE_SEARCH_KEY = SearchKey.getSearchKeyInvoice;
                req.INVOICE_SEARCH_KEY.READ_INCLUDED = true; //okunmusları da al
                req.INVOICE_SEARCH_KEY.READ_INCLUDEDSpecified = true;
                req.HEADER_ONLY = EI.ActiveOrPasive.N.ToString();
                req.INVOICE_SEARCH_KEY.DIRECTION = direction;
                if (direction == nameof(EI.Direction.DRAFT))
                {
                    req.INVOICE_SEARCH_KEY.DIRECTION = nameof(EI.Direction.OUT);
                    req.INVOICE_SEARCH_KEY.DRAFT_FLAG = nameof(EI.ActiveOrPasive.Y);
                }

                INVOICE[] invoiceArr = eInvoiceOIBPortClient.GetInvoice(req).INVOICE;
                if (invoiceArr == null || invoiceArr.Length != 0)
                {
                    foreach (var invoice in invoiceArr)
                    {
                        FolderControl.saveInvDocContentWithByte(Compress.UncompressFile(invoice.CONTENT.Value), direction, invoice.ID, nameof(EI.DocumentType.XML));
                    }
                    //db ye kaydetmedıgım ıcın aldıgım faturaları mark ınv yapmadım

                    if (invoiceArr.Length == 100) //butun okunmamısları almak ıcın ... limit 100 vermıstık
                    {
                        isGetInvoiceSingnedXml(direction);
                    }
                    return true;
                }
                return false;
            }
        }



        /// <summary>
        /// basarılıysa ınv content doner degılse null
        /// </summary>

        public byte[] getInvoiceType(string invoiceUuid, string documentType, string direction)
        {
            using (new OperationContextScope(eInvoiceOIBPortClient.InnerChannel))
            {
                GetInvoiceWithTypeRequest req = new GetInvoiceWithTypeRequest();
                req.REQUEST_HEADER = RequestHeader.getRequestHeaderOib;
                req.REQUEST_HEADER.COMPRESSED = nameof(EI.ActiveOrPasive.Y);
                req.INVOICE_SEARCH_KEY = SearchKey.getSearchKeyInvoiceWithType;
                req.INVOICE_SEARCH_KEY.READ_INCLUDED = true;
                req.INVOICE_SEARCH_KEY.READ_INCLUDEDSpecified = true;
                req.INVOICE_SEARCH_KEY.UUID = invoiceUuid;
                req.INVOICE_SEARCH_KEY.TYPE = documentType;//XML,PDF 
                req.HEADER_ONLY = EI.ActiveOrPasive.N.ToString();
                req.INVOICE_SEARCH_KEY.DIRECTION = direction;
                if (direction == nameof(EI.Direction.DRAFT))
                {
                    req.INVOICE_SEARCH_KEY.DIRECTION = nameof(EI.Direction.OUT);
                    req.INVOICE_SEARCH_KEY.DRAFT_FLAG = nameof(EI.ActiveOrPasive.Y);
                }

                INVOICE[] invoice = eInvoiceOIBPortClient.GetInvoiceWithType(req).INVOICE;
                if (invoice == null || invoice.Length != 0)
                {
                    return Compress.UncompressFile(invoice[0].CONTENT.Value);
                }
                return null;
            }
        }



        public string getInvoiceContentXml(string uuid, string direction)
        {
            //db den pathı getırdı
            string xmlPath = Singl.invoiceDalGet.getInvoice(uuid, direction).folderPath;

            if (!FolderControl.xmlFileIsInFolder(xmlPath)) // xml dosyası verılen pathde bulunmuyorsa
            {
                //servisten, gonderilen uuıd ye aıt faturanın contentını getır
                return getInvoiceWithUuidOnService(uuid, direction);
            }
            else
            {
                return File.ReadAllText(xmlPath);
            }
        }













    }
}



