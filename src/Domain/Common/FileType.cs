using System.ComponentModel;

namespace Domain.Common;

public enum FileType
{
    [Description(".jpg,.png,.jpeg")]
    Image,
    [Description(".pdf")]
    Application,
    [Description(".txt,.plain")]
    Text
}