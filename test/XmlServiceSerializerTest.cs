using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Xunit;

using DotNetIToolkit;
using DotNetIToolkitTest.Programs;
using DotNetIToolkitTest.Commands;

namespace DotNetIToolkitTest
{

    public class XmlServiceSerializerTest
    {
        private const string fixturesPath = @"fixtures";

        [Fact]
        public void SerializeProgramWithDataStructureParameterTest()
        {
            var zZCALL = new ZZCALL
            {
                InCharA = "Z",
                ZZCALLDs = new ZZCALLDs { DsCharA = "Z" }
            };

            var ser = new XmlServiceSerializer();
            string xmlInput = ser.Serialize(zZCALL);

            Assert.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>", xmlInput);
            Assert.Contains("<script>", xmlInput);
#if DEBUG
            Assert.Contains("<pgm name=\"ZZCALL\" lib=\"XMLSERVICE\">", xmlInput);
#endif
            Assert.Contains("<data type=\"1a\" prop=\"InCharA\">Z</data>", xmlInput);
            Assert.Contains("<data type=\"1a\" prop=\"DsCharA\">Z</data>", xmlInput);
        }

        [Fact]
        public void SerializeCommandTest()
        {
            var rTVJOBA = new RTVJOBA();
            var ser = new XmlServiceSerializer();

            string xmlInput = ser.Serialize(rTVJOBA);

            Assert.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>", xmlInput);
            Assert.Contains("<script>", xmlInput);
            Assert.Contains("<cmd exec=\"rexx\">RTVJOBA JOB(?) USER(?) NBR(?)</cmd>", xmlInput);
        }

        [Fact]
        public void SerializeListTest()
        {
            var rTVJOBA = new RTVJOBA();
            var zZCALL = new ZZCALL
            {
                InCharA = "Z",
                ZZCALLDs = new ZZCALLDs { DsCharA = "Z" }
            };

            var xmlServiceCalls = new IXmlServiceCall[]
            {
                rTVJOBA,
                zZCALL
            };

            var ser = new XmlServiceSerializer();
            string xmlInput = ser.Serialize(xmlServiceCalls);

            Assert.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>", xmlInput);
            Assert.Contains("<script>", xmlInput);
            Assert.Contains("<cmd exec=\"rexx\">RTVJOBA JOB(?) USER(?) NBR(?)</cmd>", xmlInput);
            Assert.Contains("<pgm name=\"ZZCALL\" lib=\"XMLSERVICE\">", xmlInput);
            Assert.Contains("<data type=\"1a\" prop=\"InCharA\">Z</data>", xmlInput);
        }

        [Fact]
        public void SerializeProgramListCDataTest()
        {
            var zZCALL = new ZZCALL
            {
                InCharA = "Z",
                InCharB = ">",
                ZZCALLDs = new ZZCALLDs { DsCharA = "Z", DsCharB = "<" }
            };

            var ser = new XmlServiceSerializer();
            string xmlInput = ser.Serialize(zZCALL);

            Assert.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>", xmlInput);
            Assert.Contains("<script>", xmlInput);
#if DEBUG
            Assert.Contains("<pgm name=\"ZZCALL\" lib=\"XMLSERVICE\">", xmlInput);
#endif
            Assert.Contains("<data type=\"1a\" prop=\"InCharA\">Z</data>", xmlInput);
            Assert.Contains("<data type=\"1a\" prop=\"InCharB\"><![CDATA[>]]></data>", xmlInput);
            Assert.Contains("<data type=\"1a\" prop=\"DsCharA\">Z</data>", xmlInput);
            Assert.Contains("<data type=\"1a\" prop=\"DsCharB\"><![CDATA[<]]></data>", xmlInput);
        }

        [Fact]
        public void SerializeProgramDeep()
        {
            var D5 = new INDS5
            {
                I1 = 1,
                C2 = "C",
                P1 = 0,
                Z2 = 0,
                R2 = 0,
                R3 = 0
            };

            var D4 = new INDS4
            {
                I1 = 1,
                C2 = "C",
                P1 = 0,
                Z2 = 0,
                D5 = D5
            };

            var D3 = new INDS3
            {
                I1 = 1,
                C2 = "C",
                P1 = 0,
                Z2 = 0,
                D4 = D4
            };

            var zZDeep = new ZZDEEP
            {
                INDS1 = new INDS1
                {
                    I1 = 1,
                    C2 = "C",
                    P1 = 0,
                    Z2 = 0,
                    D2 = new INDS2
                    {
                        I1 = 1,
                        C2 = "C",
                        P1 = 0,
                        Z2 = 0,
                        D3 = D3
                    },
                    D4 = D4,
                    D5 = D5,
                    D6 = new INDS6
                    {
                        I1 = 1,
                        C2 = "C",
                        P1 = 0,
                        Z2 = 0,
                        R2 = 0,
                        R3 = 0
                    }
                }
            };

            var ser = new XmlServiceSerializer();
            string xmlInput = ser.Serialize(zZDeep);

            Assert.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>", xmlInput);
            Assert.Contains("<script>", xmlInput);
#if DEBUG
            Assert.Contains("<pgm name=\"ZZDEEP\" lib=\"XMLSERVICE\">", xmlInput);
#endif
            Assert.Contains("<ds prop=\"INDS1\">", xmlInput);
            Assert.Contains("<ds prop=\"D2\">", xmlInput);
            Assert.Contains("<ds prop=\"D4\">", xmlInput);
        }

        [Fact]
        public void DeserializeProgramWithDataStructureParameterTest()
        {
            string xmlPath = Path.Combine(Environment.CurrentDirectory, fixturesPath, "ZZCALL.xml");

            var zZCALL = new ZZCALL
            {
                InCharA = "Z",
                ZZCALLDs = new ZZCALLDs { DsCharA = "Z" }
            };

            var xmlServiceCalls = new List<IXmlServiceCall> { zZCALL };

            var ser = new XmlServiceSerializer();
            string xmlInput = XDocument.Load(xmlPath).OuterXml();

            ser.Deserialize(xmlInput, xmlServiceCalls);

            Assert.Equal(11.1111m, zZCALL.InDec1);
            Assert.Equal(222.22m, zZCALL.InDec2);
            Assert.Equal(66.6666m, zZCALL.ZZCALLDs.DsDec1);
            Assert.Equal(77777.77m, zZCALL.ZZCALLDs.DsDec2);
        }

        [Fact]
        public void DeserializeProgramListCDataTest()
        {
            string xmlPath = Path.Combine(Environment.CurrentDirectory, fixturesPath, "ZZCALL.xml");

            var zZCALL = new ZZCALL
            {
                InCharA = "Z",
                ZZCALLDs = new ZZCALLDs { DsCharA = "Z" }
            };

            var xmlServiceCalls = new List<IXmlServiceCall> { zZCALL };

            var ser = new XmlServiceSerializer();
            string xmlInput = XDocument.Load(xmlPath).OuterXml();

            ser.Deserialize(xmlInput, xmlServiceCalls);

            Assert.Equal(">", zZCALL.InCharB);
            Assert.Equal("<", zZCALL.ZZCALLDs.DsCharB);
        }

        [Fact]
        public void DeserializeProgramDeepTest()
        {
            string xmlPath = Path.Combine(Environment.CurrentDirectory, fixturesPath, "ZZDEEP.xml");

            var D5 = new INDS5();
            var D4 = new INDS4 { D5 = D5 };
            var D3 = new INDS3 { D4 = D4 };

            var zZDeep = new ZZDEEP
            {
                INDS1 = new INDS1
                {
                    D2 = new INDS2 { D3 = D3 },
                    D4 = D4,
                    D5 = D5,
                    D6 = new INDS6()
                }
            };

            var xmlServiceCalls = new List<IXmlServiceCall> { zZDeep };

            var ser = new XmlServiceSerializer();
            string xmlInput = XDocument.Load(xmlPath).OuterXml();

            ser.Deserialize(xmlInput, xmlServiceCalls);

            Assert.Equal(1, zZDeep.INDS1.I1);
            Assert.Equal("C", zZDeep.INDS1.C2);
            Assert.Equal(1, zZDeep.INDS1.D2.I1);
            Assert.Equal("C", zZDeep.INDS1.D2.D3.D4.D5.C2);
        }

        [Fact]
        public void DeserializeCommandTest()
        {
            string xmlPath = Path.Combine(Environment.CurrentDirectory, fixturesPath, "RTVJOBA.xml");

            var rTVJOBA = new RTVJOBA();

            var xmlServiceCalls = new List<IXmlServiceCall> { rTVJOBA };

            var ser = new XmlServiceSerializer();
            string xmlInput = XDocument.Load(xmlPath).OuterXml();

            ser.Deserialize(xmlInput, xmlServiceCalls);

            Assert.Equal("QZDASOINIT", rTVJOBA.Job);
            Assert.Equal("QUSER", rTVJOBA.User);
            Assert.Equal("332973", rTVJOBA.Nbr);
        }

        [Fact]
        public void DefaultLibrarySetTest()
        {
            var zZCALL = new ZZCALL
            {
                InCharA = "Z",
                ZZCALLDs = new ZZCALLDs { DsCharA = "Z" }
            };

            var ser = new XmlServiceSerializer();
            string xmlInput = ser.Serialize(zZCALL);

#if DEBUG
            Assert.Contains("XMLSERVICE", xmlInput);
#else
            Assert.Contains("*LIBL", xmlInput);
#endif
        }
    }
}
