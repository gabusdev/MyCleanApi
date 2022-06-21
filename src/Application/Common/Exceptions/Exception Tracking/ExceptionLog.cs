using Domain.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions.Exception_Tracking;
public class ExceptionLog: IEntity<Guid>
{
    public Guid Id { get; set; }
    public string? UserId { get; set; }
    public DateTime DataTime { get; set; }
    public string? Reference { get; set; }
    public string? IPInfo { get; set; }
    public string? ErrorDescription { get; set; }
    public string? Data { get; set; }
    public string? StackTrace { get; set; }
}
