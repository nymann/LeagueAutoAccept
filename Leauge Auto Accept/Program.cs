﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Leauge_Auto_Accept
{
    class Program
    {
        private static void Main()
        {
            // Start application using conhost (Windows Terminal do not support resize)
            // Ref: https://github.com/microsoft/terminal/issues/5094
            var parentProc = ParentProcessUtilities.GetParentProcess();
            var isDebug = Debugger.IsAttached;
            if (!isDebug && parentProc.ProcessName != "conhost")
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "conhost",
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                    Arguments = Process.GetCurrentProcess().MainModule.FileName,
                });
                return;
            }

            // Show initializing message
            UI.initializingWindow();

            // Initlize console size related stuff
            SizeHandler.initialize();

            // Set console title
            Console.Title = "League Auto Accept";

            // Set output to UTF8
            Console.OutputEncoding = Encoding.UTF8;

            // Attempt to load existing settings
            Settings.loadSettings();

            Updater.initialize();

            // Start a bunch of task
            var taskKeys = new Task(Navigation.ReadKeys);
            taskKeys.Start();
            var taskQueue = new Task(MainLogic.acceptQueue);
            taskQueue.Start();
            var taskLeagueAlive = new Task(LCU.CheckIfLeagueClientIsOpenTask);
            taskLeagueAlive.Start();
            var taskResizeHandler = new Task(SizeHandler.SizeReader);
            taskResizeHandler.Start();

            // Indefinitely await them
            var tasks = new[] { taskKeys, taskQueue, taskLeagueAlive, taskResizeHandler };
            Task.WaitAll(tasks);

            Console.ReadKey();
        }
    }
}