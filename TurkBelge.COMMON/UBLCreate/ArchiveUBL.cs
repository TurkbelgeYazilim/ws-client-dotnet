﻿using TurkBelge.COMMON.Ubl_Tr;
using TurkBelge.CONTROLLER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;
using UblInvoice;

namespace TurkBelge.COMMON.UBLCreate
{
    public class ArchiveUBL : BaseInvoiceUBL
    {



        public ArchiveUBL(string archiveSendingType, string profileId, string invoiceTypeCode)
            : base(profileId, invoiceTypeCode)
        {
            addAdinationalDocRefXslt();
            createArhiveUbl(archiveSendingType);
        }



        private void addAdinationalDocRefXslt()
        {
            var idRef = new DocumentReferenceType();
            idRef.ID = new IDType { Value = Guid.NewGuid().ToString() };
            idRef.IssueDate = baseInvoiceUBL.IssueDate;
            idRef.DocumentType = new DocumentTypeType { Value = nameof(EI.DocumentType.XSLT) };
            idRef.Attachment = new AttachmentType();
            idRef.Attachment.EmbeddedDocumentBinaryObject = new EmbeddedDocumentBinaryObjectType();
            idRef.Attachment.EmbeddedDocumentBinaryObject.characterSetCode = "UTF-8";
            idRef.Attachment.EmbeddedDocumentBinaryObject.encodingCode = "Base64";
            idRef.Attachment.EmbeddedDocumentBinaryObject.filename = baseInvoiceUBL.ID.Value.ToString() + ".xslt";
            idRef.Attachment.EmbeddedDocumentBinaryObject.mimeCode = "application/xml";
            //invoice olusturuldugunda xslt invoice olarak verılecegı ıcın
            idRef.Attachment.EmbeddedDocumentBinaryObject.Value = Convert.FromBase64String(Xslt.xsltGibArchive);

            docRefList.Add(idRef);
            baseInvoiceUBL.AdditionalDocumentReference = docRefList.ToArray();
        }

    
        /// <summary>
        /// e-Arşiv UBL de fatura ye ek olarak eklenecek alanların eklenmesi
        /// </summary>
        private void createArhiveUbl( string archiveSendingType)
        {
            addAdditionalDocumentReference(archiveSendingType,"SendingType");
        }

        






    }
}
