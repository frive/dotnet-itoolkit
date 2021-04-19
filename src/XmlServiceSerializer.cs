using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace DotNetIToolkit
{
    public class XmlServiceSerializer
    {
        private const string SCRIPT_NODE = "script";
        private const string PGM_NODE = "pgm";
        private const string PGM_ATTRIBUTE_NAME = "name";
        private const string PGM_ATTRIBUTE_LIBRARY = "lib";
        private const string PGM_ATTRIBUTE_MODE = "mode";
        private const string PGM_ATTRIBUTE_ERROR = "error";
        private const string PGM_PARM_NODE = "parm";
        private const string PGM_PARM_ATTRIBUTE_IO = "io";
        private const string PGM_PARM_DS_NODE = "ds";
        private const string CMD_NODE = "cmd";
        private const string CMD_ATTRIBUTE_EXEC = "exec";
        private const string CMD_ROW_NODE = "row";
        private const string PARM_DATA_NODE = "data";
        private const string PARM_DATA_ATTRIBUTE_TYPE = "type";
        private const string PARM_DATA_ATTRIBUTE_PROPERTY = "prop";
        private const string PARM_DATA_ATTRIBUTE_DESC = "desc";
        private const string SUCCESS_NODE = "success";

        private readonly List<IXmlServiceCall> _xmlServiceCalls = new List<IXmlServiceCall>();

        public string Serialize(IXmlServiceCall xmlServiceCall)
        {
            _xmlServiceCalls.Add(xmlServiceCall);

            return SerializeCalls();
        }

        public string Serialize(IEnumerable<IXmlServiceCall> xmlServiceCalls)
        {
            _xmlServiceCalls.AddRange(xmlServiceCalls);

            return SerializeCalls();
        }

        public void Deserialize(
            string xmlDocument,
            IEnumerable<IXmlServiceCall> xmlServiceCallsInstance
        )
        {
            int index = 0;
            XDocument doc;
            XElement scriptNode = null;

            if (xmlServiceCallsInstance == null)
            {
                throw new ArgumentNullException(nameof(xmlServiceCallsInstance));
            }

            try
            {
                doc = XDocument.Parse(xmlDocument);
                scriptNode = doc.Element(SCRIPT_NODE);

                foreach (IXmlServiceCall call in xmlServiceCallsInstance)
                {
                    DeserializeCalls(scriptNode, call, index);
                    index++;
                }
            }
            catch (Exception e)
            {
                e.Data.Add("XMLServiceInput", SerializeCalls());
                e.Data.Add("XMLServiceOutput", xmlDocument);

                throw;
            }
        }

        private string SerializeCalls()
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", null)
            );
            var scriptNode = new XElement(SCRIPT_NODE);
            IEnumerable<XElement> callNodes = _xmlServiceCalls.Select(call => BuildServiceCall(call));

            scriptNode.Add(callNodes);
            doc.Add(scriptNode);

            return doc.OuterXml();
        }

        private XElement BuildServiceCall(object objectInstance)
        {
            Type type = objectInstance.GetType();
            XElement callNode = null;
            XmlServiceProgramAttribute programAttribute;
            XmlServiceCommandAttribute commandAttribute;
            string[] commandParameters;

            programAttribute = type.GetCustomAttribute<XmlServiceProgramAttribute>();
            commandAttribute = type.GetCustomAttribute<XmlServiceCommandAttribute>();

            if (programAttribute != null)
            {
                callNode = new XElement(PGM_NODE,
                    new XAttribute(PGM_ATTRIBUTE_NAME, programAttribute.Name),
                    new XAttribute(PGM_ATTRIBUTE_LIBRARY, programAttribute.Library)
                );

                if (programAttribute.Mode != PgmMode.Ile)
                {
                    callNode.Add(
                        new XAttribute(
                            PGM_ATTRIBUTE_MODE,
                            programAttribute.Mode.ToString().ToLower()
                        )
                    );
                }

                if (programAttribute.Error != PgmError.Off)
                {
                    callNode.Add(
                        new XAttribute(PGM_ATTRIBUTE_ERROR, programAttribute.Error.ToString().ToLower())
                    );
                }

                BuildProgramParameter(callNode, objectInstance);
            }

            if (commandAttribute != null)
            {
                callNode = new XElement(CMD_NODE);

                if (commandAttribute.Exec != CmdExec.Cmd)
                {
                    callNode.Add(
                        new XAttribute(
                            CMD_ATTRIBUTE_EXEC,
                            commandAttribute.Exec.ToString().ToLower()
                        )
                    );
                }

                commandParameters = BuildCommandParameter(type.GetProperties());

                callNode.Add(
                    new XText($"{commandAttribute.Command} {string.Join(" ", commandParameters)}")
                );
            }

            return callNode;
        }

        private void BuildProgramParameter(XElement parentNode, object objectInstance)
        {
            Type type = objectInstance.GetType();
            PropertyInfo[] sortedProperties = SortTypeProperties(type);

            foreach (PropertyInfo parameterProperty in sortedProperties)
            {
                var parameterNode = new XElement(PGM_PARM_NODE);
                XElement dataNode = null;
                var dataAttr = parameterProperty.GetCustomAttribute<XmlServiceDataAttribute>();
                var dsAttr = parameterProperty.GetCustomAttribute<XmlServiceDataStructureAttribute>();

                if (dataAttr != null)
                {
                    if (dataAttr.Direction != ParmDirection.Both)
                    {
                        parameterNode.Add(
                            new XAttribute(
                                PGM_PARM_ATTRIBUTE_IO,
                                dataAttr.Direction.ToString().ToLower()
                            )
                        );
                    }

                    dataNode = BuildParameterData(objectInstance, parameterProperty);

                    if (dataAttr.IsParameter && parentNode.Name != PGM_PARM_DS_NODE)
                    {
                        parameterNode.Add(dataNode);
                        parentNode.Add(parameterNode);
                    }
                    else
                    {
                        parentNode.Add(dataNode);
                    }
                }

                if (dsAttr != null)
                {
                    object dsObject = parameterProperty.GetValue(objectInstance);
                    var dsNode = new XElement(PGM_PARM_DS_NODE);

                    if (dsAttr.IsParameter)
                    {
                        parameterNode.Add(dsNode);
                        parentNode.Add(parameterNode);
                    }
                    else
                    {
                        parentNode.Add(dsNode);
                    }

                    dsNode.Add(new XAttribute(PARM_DATA_ATTRIBUTE_PROPERTY, parameterProperty.Name));

                    BuildProgramParameter(dsNode, dsObject);
                }
            }
        }

        private XElement BuildParameterData(object objectInstance, PropertyInfo parameterProperty)
        {
            var dataNode = new XElement(PARM_DATA_NODE);
            XText textNode;

            XmlServiceDataAttribute paramAttr =
                parameterProperty.GetCustomAttribute<XmlServiceDataAttribute>();

            dataNode.Add(new XAttribute(PARM_DATA_ATTRIBUTE_TYPE, paramAttr.DataType));
            dataNode.Add(new XAttribute(PARM_DATA_ATTRIBUTE_PROPERTY, parameterProperty.Name));

            // XText requires a string and property values can be null
            // convert property values to string.
            //
            // $"{null}" - succint version of calling `ToString()` of a null object
            textNode = new XText($"{parameterProperty.GetValue(objectInstance)}");
            if (paramAttr.CData)
            {
                textNode = new XCData($"{parameterProperty.GetValue(objectInstance)}");
            }

            dataNode.Add(textNode);

            return dataNode;
        }

        private string[] BuildCommandParameter(PropertyInfo[] parameterProperties)
        {
            XmlServiceDataAttribute paramAttr;

            return parameterProperties
                .Select(property =>
                {
                    paramAttr = property.GetCustomAttribute<XmlServiceDataAttribute>();

                    if (paramAttr == null) return "";
                    if (property.PropertyType == typeof(decimal))
                    {
                        return $"{property.Name.ToUpper()}(?N)";
                    }

                    return $"{property.Name.ToUpper()}(?)";
                }).ToArray();
        }

        private object DeserializeCalls(
            XElement scriptNode,
            object objectInstance,
            int index
        )
        {
            XElement callNode;
            XElement commandNode;
            IEnumerable<XElement> parameterNodes;
            XmlServiceProgramAttribute programAttribute;
            XmlServiceCommandAttribute commandAttribute;

            Type type = objectInstance.GetType();

            programAttribute = type.GetCustomAttribute<XmlServiceProgramAttribute>();
            commandAttribute = type.GetCustomAttribute<XmlServiceCommandAttribute>();

            if (programAttribute != null)
            {
                callNode = scriptNode
                    .Elements(PGM_NODE)
                    .ElementAt(index);

                MapServiceCallParameter(callNode, objectInstance);
            }

            if (commandAttribute != null)
            {
                commandNode = scriptNode
                    .Elements(CMD_NODE)
                    .Elements(SUCCESS_NODE)
                    .First(el => el.Value.Contains(commandAttribute.Command));

                parameterNodes = commandNode.Ancestors(CMD_NODE).Elements(CMD_ROW_NODE);

                foreach (PropertyInfo prop in type.GetProperties())
                {
                    MapCommandParameterDataToProperty(objectInstance, prop, parameterNodes);
                }
            }

            return objectInstance;
        }

        private void MapServiceCallParameter(
            XElement callNode,
            object objectInstance
        )
        {
            Type type = objectInstance.GetType();
            XElement parameterNode;
            XElement dataNode;
            XElement dsNode;

            foreach (PropertyInfo prop in type.GetProperties())
            {
                var dataAttr = prop.GetCustomAttribute<XmlServiceDataAttribute>();
                var dsAttr = prop.GetCustomAttribute<XmlServiceDataStructureAttribute>();

                if (dataAttr != null && dataAttr.Direction != ParmDirection.In)
                {
                    if (dataAttr.IsParameter)
                    {
                        parameterNode = callNode
                            .Elements(PGM_PARM_NODE)
                            .Elements(PARM_DATA_NODE)
                            .First(el => (string)el.Attribute(PARM_DATA_ATTRIBUTE_PROPERTY) == prop.Name);

                        MapParameterDataToProperty(objectInstance, prop, parameterNode);
                    }
                    else
                    {
                        dataNode = callNode
                            .Elements(PARM_DATA_NODE)
                            .First(el => (string)el.Attribute(PARM_DATA_ATTRIBUTE_PROPERTY) == prop.Name);

                        MapParameterDataToProperty(objectInstance, prop, dataNode);
                    }
                }

                if (dsAttr == null || dsAttr.Direction == ParmDirection.In) continue;

                object dsObject = prop.GetValue(objectInstance);

                if (dsAttr.IsParameter)
                {
                    dsNode = callNode
                        .Elements(PGM_PARM_NODE)
                        .Elements(PGM_PARM_DS_NODE)
                        .First(el => (string)el.Attribute(PARM_DATA_ATTRIBUTE_PROPERTY) == prop.Name);

                    MapServiceCallParameter(dsNode, dsObject);
                }
                else
                {
                    dsNode = callNode
                        .Elements(PGM_PARM_DS_NODE)
                        .First(el => (string)el.Attribute(PARM_DATA_ATTRIBUTE_PROPERTY) == prop.Name);

                    MapServiceCallParameter(dsNode, dsObject);
                }
            }
        }

        private void MapParameterDataToProperty(
            object objectInstance,
            PropertyInfo property,
            XElement parentDataNode
        )
        {
            object dataValue = Convert.ChangeType(
                parentDataNode.Value,
                property.PropertyType,
                CultureInfo.InvariantCulture);

            property.SetValue(objectInstance, dataValue);
        }

        private void MapCommandParameterDataToProperty(
            object xmlServiceCallInstance,
            PropertyInfo property,
            IEnumerable<XElement> parameterNodes
        )
        {
            object dataValue;
            string propertyName = property.Name.ToUpper();

            XElement dataNode = parameterNodes
                .Elements(PARM_DATA_NODE)
                .First(el => (string)el.Attribute(PARM_DATA_ATTRIBUTE_DESC) == propertyName);

            dataValue = Convert.ChangeType(
                dataNode.Value,
                property.PropertyType,
                CultureInfo.InvariantCulture);

            property.SetValue(xmlServiceCallInstance, dataValue);
        }

        private PropertyInfo[] SortTypeProperties(Type type)
        {
            return type
                .GetProperties()
                .OrderBy(p =>
                {
                    var dataAttr = p.GetCustomAttribute<XmlServiceDataAttribute>();
                    var dsAttr = p.GetCustomAttribute<XmlServiceDataStructureAttribute>();

                    if (dataAttr != null) return dataAttr.Order;
                    if (dsAttr != null) return dsAttr.Order;

                    return int.MaxValue;
                })
                .ToArray();
        }
    }
}