using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurkBelge.SERVICES.serviceOib;


namespace TurkBelge.CONTROLLER.RequestSection
{
   public class SearchKey
    {

        public static GetInvoiceRequestINVOICE_SEARCH_KEY getSearchKeyInvoice;
        public static GetInvoiceWithTypeRequestINVOICE_SEARCH_KEY getSearchKeyInvoiceWithType;



        public static void createInvoiceSearchKey()
        {
            getSearchKeyInvoice = new GetInvoiceRequestINVOICE_SEARCH_KEY() //default degerler ısterse degısebılır
            {
                LIMIT = 10,
                LIMITSpecified =true,
                READ_INCLUDED = true,
                READ_INCLUDEDSpecified = true,              
            };
        }


        public static void createInvoiceSearchKeyGetInvoiceWithType()
        {
            getSearchKeyInvoiceWithType = new GetInvoiceWithTypeRequestINVOICE_SEARCH_KEY() //default degerler ısterse degısebılır
            {
                LIMIT = 10,
                LIMITSpecified = true,
                READ_INCLUDED = true,
                READ_INCLUDEDSpecified = true,          
            };
        }

    }
}
