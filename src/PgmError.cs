/**
 * http://yips.idevcloud.com/wiki/index.php/XMLService/XMLSERVICEQuick#pgm
 *
 * error (1.7.6)
 *   on      - script stops, full error report
 *   off     - script continues, job error log (default)
 *   fast    - script continues, brief error log
 */

namespace DotNetIToolkit
{
    public enum PgmError
    {
        On,
        Off,
        Fast
    }
}