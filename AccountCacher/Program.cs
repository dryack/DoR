﻿/*code by leo228
 * Copyright (C) Dawn of Reckoning project since 2008-2019
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Common;
using FrameWork;

namespace AccountCacher
{
    class Program
    {
        static public AccountMgr AcctMgr = null;
        static public AccountConfigs Config = null;
        static public RpcServer Server;

        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(onError);

            Log.Texte("", "-------------------- Account Cacher -------------------", ConsoleColor.Blue);



            Log.Texte("", "---Dawn of Reckoning project since 2008-2019 BY LEO228---", ConsoleColor.Red);

            // Loading all configs files
            ConfigMgr.LoadConfigs();
            Config = ConfigMgr.GetConfig<AccountConfigs>();

            // Loading log level from file
            if (!Log.InitLog(Config.LogLevel, "AccountCacher"))
                ConsoleMgr.WaitAndExit(2000);

            AccountMgr.Database = DBManager.Start(Config.AccountDB.Total(), ConnectionType.DATABASE_MYSQL, "Accounts");
            if (AccountMgr.Database == null)
                ConsoleMgr.WaitAndExit(2000);

            Server = new RpcServer(Config.RpcInfo.RpcClientStartingPort, 1);
            if (!Server.Start(Config.RpcInfo.RpcIp, Config.RpcInfo.RpcPort))
                ConsoleMgr.WaitAndExit(2000);

            AcctMgr = Server.GetLocalObject<AccountMgr>();
            AcctMgr.LoadRealms();

            ConsoleMgr.Start();
        }

        static void onError(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error("OnError", e.ExceptionObject.ToString());
        }
    }
}
