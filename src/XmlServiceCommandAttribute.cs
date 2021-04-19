using System;

namespace DotNetIToolkit
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class XmlServiceCommandAttribute : Attribute
    {
        public string Command { get; set; }
        public CmdExec Exec { get; set; }
    }
}