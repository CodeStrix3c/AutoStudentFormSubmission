using System;

namespace ASFS.Domain.Entities
{
    public class FormAttachment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FormId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty; // or Blob URL
        public string ContentType { get; set; } = string.Empty;
        public Guid? UploadedBy { get; set; } // optional link to Users table
        public DateTimeOffset UploadedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
