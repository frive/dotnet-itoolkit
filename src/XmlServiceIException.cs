using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DotNetIToolkit
{
    [Serializable]
    public class XmlServiceIException : Exception
    {
        private readonly XDocument _xmlDocumentOutput;

        public IEnumerable<XmlServiceIError> Errors { get; private set; }
        public JobInfo JobInfo { get; private set; }
        public JobLogScan JobLogScan { get; private set; }
        public string JobLog { get; private set; }
        public string XmlServiceInput { get; }
        public string XmlServiceOutput { get; }

        public XmlServiceIException(string xmlInput, string xmlOutput)
        {
            XmlServiceInput = xmlInput;
            XmlServiceOutput = xmlOutput;

            try
            {
                _xmlDocumentOutput = XDocument.Parse(xmlOutput);
                MapXmlOutput();
            }
            catch (Exception ex)
            {
                ex.Data.Add("XMLServiceInput", XmlServiceInput);
                ex.Data.Add("XMLServiceOutput", XmlServiceOutput);

                throw;
            }
        }

        public XmlServiceIException()
        {
        }

        public XmlServiceIException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected XmlServiceIException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }

        private void MapXmlOutput()
        {
            MapErrors();
            MapJobInfo();
            MapJobLogScan();

            XElement jobLogNode = _xmlDocumentOutput.Descendants("joblog").FirstOrDefault();
            JobLog = (jobLogNode == null) ? "" : jobLogNode.Value;
        }

        private void MapErrors()
        {
            IEnumerable<XElement> errorNodes = _xmlDocumentOutput.Descendants("error");

            if (errorNodes == null) return;

            Errors = errorNodes
                // Only get error nodes with child
                .Where(e => e.Element("errnoxml") != null)
                .Select(e =>
                {
                    XElement errNoNode = e.Element("errnoxml");
                    XElement errMessageNode = e.Element("xmlerrmsg");
                    XElement hintNode = e.Element("xmlhint");

                    return new XmlServiceIError
                    {
                        Number = (errNoNode == null) ? 0 : Convert.ToInt32(errNoNode.Value),
                        Message = (errMessageNode == null) ? "" : errMessageNode.Value,
                        Hint = (hintNode == null) ? "" : hintNode.Value
                    };
                })
                .ToList();
        }

        private void MapJobInfo()
        {
            XElement jobInfoNode = _xmlDocumentOutput.Descendants("jobinfo").First();

            if (jobInfoNode == null) return;

            XElement jobIpcNode = jobInfoNode.Element("jobipc");
            XElement jobIpcsKeyNode = jobInfoNode.Element("jobipcskey");
            XElement jobNameNode = jobInfoNode.Element("jobname");
            XElement jobUserNode = jobInfoNode.Element("jobuser");
            XElement jobNbrNode = jobInfoNode.Element("jobnbr");
            XElement jobStsNode = jobInfoNode.Element("jobsts");
            XElement curUserNode = jobInfoNode.Element("curuser");
            XElement ccsidNode = jobInfoNode.Element("ccsid");
            XElement dftCcsidNode = jobInfoNode.Element("dftccsid");
            XElement paseCcsidNode = jobInfoNode.Element("paseccsid");
            XElement langIdNode = jobInfoNode.Element("langid");
            XElement cntryIdNode = jobInfoNode.Element("cntryid");
            XElement sbsNameNode = jobInfoNode.Element("sbsname");
            XElement sbsLibNode = jobInfoNode.Element("sbslib");
            XElement curLibNode = jobInfoNode.Element("curlib");
            XElement sysLiblNode = jobInfoNode.Element("syslibl");
            XElement usrLiblNode = jobInfoNode.Element("usrlibl");
            XElement jobCpfFindNode = jobInfoNode.Element("jobcpffind");

            JobInfo = new JobInfo
            {
                Ipc = (jobIpcNode == null) ? "" : jobIpcNode.Value,
                IpcsKey = (jobIpcsKeyNode == null) ? "" : jobIpcsKeyNode.Value,
                Name = (jobNameNode == null) ? "" : jobNameNode.Value,
                User = (jobUserNode == null) ? "" : jobUserNode.Value,
                Number = (jobNbrNode == null) ? 0 : Convert.ToInt32(jobNbrNode.Value),
                Status = (jobStsNode == null) ? "" : jobStsNode.Value,
                CurrentUser = (curUserNode == null) ? "" : curUserNode.Value,
                Ccsid = (ccsidNode == null) ? "" : ccsidNode.Value,
                DefaultCcsid = (dftCcsidNode == null) ? "" : dftCcsidNode.Value,
                PaseCcsid = (paseCcsidNode == null) ? "" : paseCcsidNode.Value,
                LanguageId = (langIdNode == null) ? "" : langIdNode.Value,
                CountryId = (cntryIdNode == null) ? "" : cntryIdNode.Value,
                SubSystemName = (sbsNameNode == null) ? "" : sbsNameNode.Value,
                SubSystemLibrary = (sbsLibNode == null) ? "" : sbsLibNode.Value,
                CurrentLibrary = (curLibNode == null) ? "" : curLibNode.Value,
                SystemLibraryList = (sysLiblNode == null) ? "" : sysLiblNode.Value,
                UserLibraryList = (usrLiblNode == null) ? "" : usrLiblNode.Value,
                CpfFind = (jobCpfFindNode == null) ? "" : jobCpfFindNode.Value
            };
        }

        private void MapJobLogScan()
        {
            IEnumerable<XElement> jobLogRecNodes = _xmlDocumentOutput.Descendants("joblogrec");

            if (jobLogRecNodes == null) return;

            JobLogScan = new JobLogScan();
            JobLogScan.JobLogRecords = jobLogRecNodes
                .Select(r =>
                {
                    XElement cpfNode = r.Element("jobcpf");
                    XElement timeNode = r.Element("jobtime");
                    XElement textNode = r.Element("jobtime");

                    return new JobLogRecord
                    {
                        Cpf = (cpfNode == null) ? "" : cpfNode.Value,
                        Time = (timeNode == null) ? "" : timeNode.Value,
                        Text = (textNode == null) ? "" : textNode.Value
                    };
                })
                .ToList();
        }
    }

    public struct XmlServiceIError
    {
        public int Number { get; set; }
        public string Message { get; set; }
        public string Hint { get; set; }
    }

    public struct JobInfo
    {
        public string Ipc { get; set; }
        public string IpcsKey { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public int Number { get; set; }
        public string Status { get; set; }
        public string CurrentUser { get; set; }
        public string Ccsid { get; set; }
        public string DefaultCcsid { get; set; }
        public string PaseCcsid { get; set; }
        public string LanguageId { get; set; }
        public string CountryId { get; set; }
        public string SubSystemName { get; set; }
        public string SubSystemLibrary { get; set; }
        public string CurrentLibrary { get; set; }
        public string SystemLibraryList { get; set; }
        public string UserLibraryList { get; set; }
        public string CpfFind { get; set; }
    }

    public class JobLogScan
    {
        public IEnumerable<JobLogRecord> JobLogRecords { get; set; }
    }

    public struct JobLogRecord
    {
        public string Cpf { get; set; }
        public string Time { get; set; }
        public string Text { get; set; }
    }
}