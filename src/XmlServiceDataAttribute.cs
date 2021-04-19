/**
  * pgm data elements
  * <data type='data types'
  *       [dim='n'
  *       varying='on|off|2|4'
  *       enddo='label'
  *       setlen='label'
  *       offset='label'
  *       hex='on|off' before='cc1/cc2/cc3/cc4' after='cc4/cc3/cc2/cc1
  *       trim='on|off'
  *       next='label'
  *       ]>(value)</data>
  * ---
  * data        - data value name (tag)
  *  values     - value,
  * type
  *     3i0                   int8/byte     D myint8   3i 0
  *     5i0                   int16/short   D myint16  5i 0
  *     10i0                  int32/int     D myint32 10i 0
  *     20i0                  int64/int64   D myint64 20i 0
  *     3u0                   uint8/ubyte   D myint8   3u 0
  *     5u0                   uint16/ushort D myint16  5u 0
  *     10u0                  uint32/uint   D myint32 10u 0
  *     20u0                  uint64/uint64 D myint64 20u 0
  *     32a                   char          D mychar  32a
  *     32a   {varying2} varchar            D mychar  32a   varying
  *     32a   {varying4} varchar4           D mychar  32a   varying(4)
  *     12p2                  packed        D mydec   12p 2
  *     12s2                  zoned         D myzone  12s 2
  *     4f2                   float         D myfloat  4f
  *     8f4                   real/double   D myfloat  8f
  *     3b                    binary        D mybin   (any)
  *     40h                   hole (no out) D myhole  (any)
  * options
  *  dim
  *     n       - array dimension value (default dim1)
  *  varying
  *     on      - character varying data (same as varying2)
  *     off     - character non-varying data (default)
  *     2       - character varying data
  *     4       - character varying data
  *  enddou
  *     label   - match array dou terminate parm label (see ds)
  *  setlen (1.5.4)
  *     label   - match calculate length of ds parm lable (see ds)
  *  offset
  *     label   - match offset label (see overlay)
  *  hex (1.6.8)
  *     on      - input character hex (5147504C20202020)
  *  before
  *     cc(n)   - input ccsid1->ccsid2->ccsid3->ccsid4
  *  after
  *     cc(n)   - output ccsid1->ccsid2->ccsid3->ccsid4
  *  trim (1.7.1)
  *     on      - trim character (default)
  *     off     - no trim character
  *  next (1.9.2)
  *     label   - match next offset label (see overlay)
  */

using System;

namespace DotNetIToolkit
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class XmlServiceDataAttribute : Attribute
    {
        public ParmDirection Direction { get; set; } = ParmDirection.Both;
        public bool IsParameter { get; set; } = true;
        public string DataType { get; set; }
        public int Order { get; set; }

        /// <summary>
        /// Wrap data value in CDATA if it contains xml characters
        /// </summary>
        public bool CData { get; set; }
    }
}