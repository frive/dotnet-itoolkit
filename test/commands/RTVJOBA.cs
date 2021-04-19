using DotNetIToolkit;

namespace DotNetIToolkitTest.Commands
{
    [XmlServiceCommand(Command = "RTVJOBA", Exec = CmdExec.Rexx)]
    public class RTVJOBA
        : IXmlServiceCall
    {
        [XmlServiceData]
        public string Job { get; set; }

        [XmlServiceData]
        public string User { get; set; }

        [XmlServiceData]
        public string Nbr { get; set; }
    }
}