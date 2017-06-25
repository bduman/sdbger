﻿namespace SDBGer
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Security.Policy;

    internal class Program
    {
        #region Static Fields

        private static AppDomain runnerDomain;

        private static SpecflowManager specManager;

        #endregion

        #region Public Methods and Operators

        public static void InitDomain(string assemblyPath)
        {
            File.Copy(Path.Combine(Path.GetDirectoryName(assemblyPath), "Log4NetConfiguration.xml"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4NetConfiguration.xml"), true);
            File.Copy(Path.Combine(Path.GetDirectoryName(assemblyPath), "Log4NetConfiguration.xml"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4NetConfiguration.xml"), true);
            var stp = new AppDomainSetup();
            stp.ConfigurationFile = assemblyPath + ".config";
            runnerDomain = AppDomain.CreateDomain("SpecflowDebugRunner", new Evidence(), stp);

            var type = typeof(SpecflowManager);

            var value = (SpecflowManager)runnerDomain.CreateInstanceAndUnwrap(
                type.Assembly.FullName,
                type.FullName,
                false,
                BindingFlags.Default,
                null,
                new object[] { assemblyPath },
                null,
                null);

            specManager = value;
        }

        #endregion

        #region Methods

        private static void Main(string[] args)
        {
            InitDomain(@"c:/open/Source/Wilco.UITest/Wilco.UITest.Spec/bin/Debug/Wilco.UITest.Spec.dll");
            specManager.BeforeTestRun();
            specManager.InitFeature("Login page basic functionality");
            specManager.BeforeFeature();
            specManager.InitScenario("The managed user with able to log in");
            specManager.BeforeScenario();
            specManager.RunStep("the user has logged in as admin", "given");
        }

        #endregion
    }
}