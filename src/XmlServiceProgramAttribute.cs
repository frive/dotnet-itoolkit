/**
  * pgm name (*PGM or *SRVPGM)
  * <pgm name=''
  *      [lib=''
  *       func=''
  *       mode='opm|ile'
  *       error='on|off|fast'
  *       ]>values (see <parm> and <return>) </pgm>
  * ---
  * pgm         - IBM i *PGM or *SRVPGM name (tag)
  *  values     - (see parm and return)
  * options
  *  lib
  *     library - IBM i library name
  *  func
  *     function- IBM i *SRVPGM function name
  *  mode
  *     ile     - ILE and PASE memory (default)
  *     opm     - ILE only memory (PASE can not view)
  *  error (1.7.6)
  *     on      - script stops, full error report
  *     off     - script continues, job error log (default)
  *     fast    - script continues, brief error log
 */

using System;

namespace DotNetIToolkit
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class XmlServiceProgramAttribute : Attribute
    {
        private const string DEFAULT_LIBRARY = "*LIBL";
        private string _library = DEFAULT_LIBRARY;

        public string Name { get; set; }
        public string Library
        {
            get
            {
                return _library;
            }
            set
            {
#if DEBUG
                _library = value;
#else
                _library = DEFAULT_LIBRARY;
#endif
            }
        }
        public PgmMode Mode { get; set; }
        public PgmError Error { get; set; } = PgmError.Off;

        public XmlServiceProgramAttribute()
        {
        }

        public XmlServiceProgramAttribute(string name)
        {
            Name = name;
        }

        public XmlServiceProgramAttribute(string name, string library)
        {
            Name = name;
            Library = library;
        }

        public XmlServiceProgramAttribute(string name, string library, PgmMode mode)
        {
            Name = name;
            Library = library;
            Mode = mode;
        }

        public XmlServiceProgramAttribute(string name, string library, PgmMode mode, PgmError error)
        {
            Name = name;
            Library = library;
            Mode = mode;
            Error = error;
        }
    }
}