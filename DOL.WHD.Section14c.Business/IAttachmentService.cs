﻿using System;
using System.IO;
using DOL.WHD.Section14c.Domain.Models.Submission;
using DOL.WHD.Section14c.Domain.ViewModels;

namespace DOL.WHD.Section14c.Business
{
    public interface IAttachmentService : IDisposable
    {
        Attachment UploadAttachment(string EIN, MemoryStream memoryStream, string fileName, string fileType);
        AttachementDownload DownloadAttachment(MemoryStream memoryStream, string EIN, Guid fileId);
        void DeleteAttachement(string EIN, Guid fileId);
    }
}