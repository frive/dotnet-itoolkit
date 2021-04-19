using System.Collections.Generic;

namespace DotNetIToolkit
{
    public interface IIbmiService
    {
        void Call(IXmlServiceCall xmlServiceCall);
        void Call(IEnumerable<IXmlServiceCall> xmlServiceCalls);
    }
}
