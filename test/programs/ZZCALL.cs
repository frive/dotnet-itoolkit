/**
 * XMLSERVICE test program
 */

using DotNetIToolkit;

namespace DotNetIToolkitTest.Programs
{
    [XmlServiceProgram(Name = "ZZCALL", Library = "XMLSERVICE")]
    public class ZZCALL
        : IXmlServiceCall
    {
        [XmlServiceData(DataType = "1a", Order = 0)]
        public string InCharA { get; set; }

        [XmlServiceData(DataType = "1a", Order = 1, CData = true)]
        public string InCharB { get; set; }

        [XmlServiceData(DataType = "7p4", Order = 2)]
        public decimal InDec1 { get; set; }

        [XmlServiceData(DataType = "12p2", Order = 3)]
        public decimal InDec2 { get; set; }

        [XmlServiceDataStructure(Order = 4)]
        public ZZCALLDs ZZCALLDs { get; set; }
    }

    public class ZZCALLDs
    {
        [XmlServiceData(IsParameter = false, DataType = "1a", Order = 0)]
        public string DsCharA { get; set; }

        [XmlServiceData(IsParameter = false, DataType = "1a", Order = 1, CData = true)]
        public string DsCharB { get; set; }

        [XmlServiceData(IsParameter = false, DataType = "7p4", Order = 2)]
        public decimal DsDec1 { get; set; }

        [XmlServiceData(IsParameter = false, DataType = "12p2", Order = 3)]
        public decimal DsDec2 { get; set; }
    }
}
