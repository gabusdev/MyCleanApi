using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Messaging;

public interface ICommand: ICommand<Unit> { }
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

public interface IQuery : IQuery<Unit> { }
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}


