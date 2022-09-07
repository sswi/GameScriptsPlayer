using System;
using System.Collections.Generic;
using System.Text;

namespace XE.Commands.Abstraction
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
