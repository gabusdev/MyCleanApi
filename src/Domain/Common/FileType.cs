using System.ComponentModel;

namespace Domain.Common;

public enum FileType
{
    [Description(".jpg,.png,.jpeg")]
    Image,
    [Description(".txt,.doc,.docx,.pdf")]
    Document
}