/**
 * http://yips.idevcloud.com/wiki/index.php/XMLService/XMLSERVICEQuick#pgm
 *
 *  io
 *    in      - input only
 *    out     - output only
 *    both    - input/output only (default)
 *    omit    - omit (1.2.3)
 */

namespace DotNetIToolkit
{
    public enum ParmDirection
    {
        In,
        Out,
        Both,
        Omit
    }
}