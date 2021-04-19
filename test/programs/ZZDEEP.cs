/**
 * XMLSERVICE test program
 */

using DotNetIToolkit;

namespace DotNetIToolkitTest.Programs
{
    [XmlServiceProgram(Name = "ZZDEEP", Library = "XMLSERVICE")]
    public class ZZDEEP
        : IXmlServiceCall
    {
        [XmlServiceDataStructure]
        public INDS1 INDS1 { get; set; }
    }

    public class BaseDS
    {
        [XmlServiceData(IsParameter = false, DataType = "10i0", Order = 0)]
        public int I1 { get; set; }

        [XmlServiceData(IsParameter = false, DataType = "10a", Order = 1)]
        public string C2 { get; set; }

        [XmlServiceData(IsParameter = false, DataType = "12p2", Order = 2)]
        public decimal P1 { get; set; }

        [XmlServiceData(IsParameter = false, DataType = "12s2", Order = 3)]
        public decimal Z2 { get; set; }
    }

    public class INDS5 : BaseDS
    {
        [XmlServiceData(IsParameter = false, DataType = "8f", Order = 4)]
        public float R2 { get; set; }

        [XmlServiceData(IsParameter = false, DataType = "4f", Order = 5)]
        public float R3 { get; set; }
    }

    public class INDS6 : INDS5
    {
    }

    public class INDS4 : BaseDS
    {
        [XmlServiceDataStructure(IsParameter = false, Order = 4)]
        public INDS5 D5 { get; set; }
    }

    public class INDS3 : BaseDS
    {
        [XmlServiceDataStructure(IsParameter = false, Order = 4)]
        public INDS4 D4 { get; set; }
    }

    public class INDS2 : BaseDS
    {
        [XmlServiceDataStructure(IsParameter = false, Order = 4)]
        public INDS3 D3 { get; set; }
    }

    public class INDS1 : BaseDS
    {
        [XmlServiceDataStructure(IsParameter = false, Order = 4)]
        public INDS2 D2 { get; set; }

        [XmlServiceDataStructure(IsParameter = false, Order = 5)]
        public INDS4 D4 { get; set; }

        [XmlServiceDataStructure(IsParameter = false, Order = 6)]
        public INDS5 D5 { get; set; }

        [XmlServiceDataStructure(IsParameter = false, Order = 7)]
        public INDS6 D6 { get; set; }

        [XmlServiceData(IsParameter = false, DataType = "8f", Order = 8)]
        public float R2 { get; set; }

        [XmlServiceData(IsParameter = false, DataType = "4f", Order = 9)]
        public float R3 { get; set; }

        [XmlServiceData(IsParameter = false, DataType = "60a", Order = 10)]
        public string C3 { get; set; }

        [XmlServiceData(IsParameter = false, DataType = "12s3", Order = 11)]
        public decimal Z3 { get; set; }

        [XmlServiceData(IsParameter = false, DataType = "12s4", Order = 12)]
        public decimal Z4 { get; set; }
    }
}
