/**
 * http://yips.idevcloud.com/wiki/index.php/XMLService/XMLSERVICEQuick#cmd
 *
 * exec
 *   cmd     - qcmdexe only return true/false (default)
 *   system  - system utility return CPFxxxx
 *   rexx    - rexx output parms and return CPFxxxx
 *             (?) character type
 *             (?N) explicit cast numeric
 */

namespace DotNetIToolkit
{
    public enum CmdExec
    {
        Cmd,
        System,
        Rexx
    }
}