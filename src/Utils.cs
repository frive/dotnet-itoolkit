using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace DotNetIToolkit
{
    public static class Extensions
    {
        /// <summary>
        /// Extension method that gets the markup containing this node and all its child nodes.
        /// Like <see cref="XmlDocument"/> OuterXml property.
        /// </summary>
        /// <param name="doc"><see cref="XDocument"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="doc"/> is <c>null</c>.</exception>
        public static string OuterXml(this XDocument doc)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            using (var sw = new Utf8StringWriter())
            {
                doc.Save(sw);
                return sw.ToString();
            }
        }
    }

    /// <summary>
    /// <see cref="StringWriter"/> with utf8 encoding.
    /// </summary>
    /// <remarks>
    /// Subclass of <see cref="StringWriter"/> to use utf8 encoding.
    /// </remarks>
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }

}