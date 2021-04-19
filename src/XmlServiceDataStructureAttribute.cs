/**
 * pgm data structure
 * <ds [dim='n' dou='label'
 *      len='label'
 *      data='records'
 *      ]>values (see <ds> or <data>)</ds>
 * ---
 * ds          - data structure tag
 * values      - (see ds or data)
 * options
 *  dim
 *   n         - array dimension value (default dim1)
 *  dou
 *   label     - match array dou terminate parm label (see data)
 *  len (1.5.4)
 *   label     - match calculate length of ds parm lable (see data)
 *  data (1.7.5)
 *   records   - data in records tag
 *
 */

using System;

namespace DotNetIToolkit
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class XmlServiceDataStructureAttribute : Attribute
    {
        public ParmDirection Direction { get; set; } = ParmDirection.Both;
        public bool IsParameter { get; set; } = true;
        public int Order { get; set; }

        /// <summary>
        /// Wrap data value in CDATA if it contains xml characters
        /// </summary>
        public bool CData { get; set; }
    }
}