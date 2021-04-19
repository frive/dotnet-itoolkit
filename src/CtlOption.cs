/**
 * Control(CTL) keywords for operator control over XMLSERVICE jobs.
 *
 * http://yips.idevcloud.com/wiki/index.php/XMLService/XMLSERVICEQuick#ctl
 *
 *--- very high peformance ignore all flag parsing (loop calls, etc.)
 * *ignore
 *    - do not parse flags (high performance)
 *      example: $ctl="*ignore";
 *----------------------------------------------------
 *--- kill XMLSERVICE job
 * *immed
 *    - end server immed destroy IPC
 *      example: $ctl="*immed";
 *----------------------------------------------------
 *--- misc functions XMLSERVICE
 * *license
 *    - return license for this code
 * *session
 *    - retrieve session key (IPC name /tmp/fred042)
 * *clear
 *    - clear internal XMLSERVICE caches,
 *                       but will not deactivate loaded PGM/SRVPGM, etc.
 *----------------------------------------------------
 * -- if you need to fix result set return drivers (iPLUGRxxx)
 *    hack for pesky DB2 drivers adding "junk" to back of records.
 * *hack
 *    - add </hack> each record of a result set
 *      example: $ctl="*hack";
 *      - iPLUGRxxx XMLSERVICE stored procedures
 *        return result sets of 3000 byte records,
 *        you must loop fetch and concat records
 *        to recreate output XML ... except
 *        some DB2 drivers have junk end ...
 *      - enable easy rec truncate ill behaved drivers
 *        remove all past </hack> during concat
 *        loop fetch records 1-n
 *        rec1: <script>....</hack> (3000 bytes)
 *        rec2: ............</hack> (3000 bytes)
 *        recn: ...</script></hack> (<3000 bytes)
 *----------------------------------------------------
 * -- pause XMLSERVICE job(s) for debugger attach (message to qsysopr)
 * *debug
 *    - stop call server with message qsysopr (XMLSERVICE)
 *      example: $ctl="*debug";
 * *debugproc
 *    - stop stored proc with message qsysopr (client QSQSRVR)
 * *debugcgi
 *    - stop CGI with message qsysopr         (XMLCGI only)
 * *test[(n)]
 *    - test parse XML in/out and report n level information
 *----------------------------------------------------
 * -- override default XMLSERVICE client/server spawn child behaviour
 * *sbmjob[(lib/jobd/job/asp)]
 *    - sbmjob job (instead of XMLSERVICE default spawn)
 *      example: $ctl="*sbmjob";
 *      example: $ctl="*sbmjob(QSYS/QSRVJOB/XTOOLKIT)";
 *      example: $ctl="*sbmjob(ZENDSVR/ZSVR_JOBD/XTOOLKIT)";
 *      - default values provided plugconf.rpgle
 *      - optional asp INLASPGRP(ASP1) (added 1.6.5)
 *      -- Notes:
 *         - See embedded XML overrides for user full control
 *           of XMLSERVICE start behavior SBMJOB settings
 * *here
 *    - run stateless in stored proc job (client only job)
 *      example: $ctl="*here";
 *      - commonly known as running in PHP job, but in fact
 *        more likely runs in database job you connected
 *        on/off machine DRDA/ODBC/PASE (QSQSRVR, etc.)
 *      - generally runs slower using "one process"
 *        because XMLSERVICE has to restart itself,
 *        wake up PASE, find/load your PGM call, etc.
 * *nostart
 *    - disallow spawn and sbmjob (web not start anything)
 *      example: $ctl="*nostart";
 *      - probably prestart all your XMLSERVICE jobs
 *        SBMJOB CMD(CALL PGM(XMLSERVICE/XMLSERVICE)
 *        PARM('/tmp/db2ipc042')) USER(DB2)
 *      - consider using a custom plugconf to disable
 *        issues with timeout defaults (*idle/*wait)
 * *java (1.9.2)
 *    - start JVM allowing user classpath
 *      <cmd>ADDENVVAR ENVVAR(CLASSPATH) VALUE('$ours')
 *           REPLACE(*YES)</cmd>
 *      <pgm>... calling my RPG w/JAVA ... </pgm>
 * *sqljava or *dbgjava (port 30000) (1.9.2)
 *    - start JVM allowing DB2 classpath (no user control)
 *       SQLJ.INSTALL_JAR into schema
 *       /QIBM/UserData/OS400/SQLLib/Function/jar/(schema)
 */

namespace DotNetIToolkit
{
    public static class CtlOption
    {
        /// <summary>
        /// Do not parse flags (high performance)
        /// </summary>
        public const string IGNORE = "*ignore";

        /// <summary>
        /// Kill XMLService job
        /// End server immed destroy IPC
        /// </summary>
        public const string IMMED = "*immed";

        /// <summary>
        /// Return license for this code
        /// </summary>
        public const string LICENSE = "*license";

        /// <summary>
        /// Retrieve session key (IPC name /tmp/fred042)
        /// </summary>
        public const string SESSION = "*session";

        /// <summary>
        /// Clear internal XMLService caches, but will not deactivate loaded PGM/SRVPGM, etc.
        /// </summary>
        public const string CLEAR = "*clear";

        /// <summary>
        /// Pause XMLService job(s) for debugger attach (message to qsysopr)
        /// Stop call server with message qsysopr (XMLService)
        /// </summary>
        public const string DEBUG = "*debug";

        /// <summary>
        /// Stop stored proc with message qsysopr (client qsqsrvr)
        /// </summary>
        public const string DEBUG_PROC = "*debugproc";

        /// <summary>
        /// Test parse XML in/out and report n level information
        /// </summary>
        public const string TEST = "*test";

        /// <summary>
        /// Run stateless in stored proc job (client only job)
        /// </summary>
        public const string HERE = "*here";
    }
}
