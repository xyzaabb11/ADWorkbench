using System;
using System.Collections.Generic;

namespace AD.Workbench.Startup
{
    [Serializable]
    sealed class StartupSettings
    {
        bool useSharpDevelopErrorHandler = true;
        string applicationName = "AD.Workbench";
        string applicationRootPath;
        bool allowAddInConfigurationAndExternalAddIns = true;
        bool allowUserAddIns;
        string propertiesName;
        string configDirectory;
        string dataDirectory;
        string domPersistencePath;
        string resourceAssemblyName = "AD.Workbench";
        internal List<string> addInDirectories = new List<string>();
        internal List<string> addInFiles = new List<string>();

        public string ResourceAssemblyName
        {
            get { return resourceAssemblyName; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                resourceAssemblyName = value;
            }
        }
        public bool UseSharpDevelopErrorHandler
        {
            get { return useSharpDevelopErrorHandler; }
            set { useSharpDevelopErrorHandler = value; }
        }

        public bool AllowAddInConfigurationAndExternalAddIns
        {
            get { return allowAddInConfigurationAndExternalAddIns; }
            set { allowAddInConfigurationAndExternalAddIns = value; }
        }

        public bool AllowUserAddIns
        {
            get { return allowUserAddIns; }
            set { allowUserAddIns = value; }
        }

        public string ApplicationName
        {
            get { return applicationName; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                applicationName = value;
            }
        }

        public string ApplicationRootPath
        {
            get { return applicationRootPath; }
            set { applicationRootPath = value; }
        }

        public string ConfigDirectory
        {
            get { return configDirectory; }
            set { configDirectory = value; }
        }

        public string DataDirectory
        {
            get { return dataDirectory; }
            set { dataDirectory = value; }
        }

        public string PropertiesName
        {
            get { return propertiesName; }
            set { propertiesName = value; }
        }

        public string DomPersistencePath
        {
            get { return domPersistencePath; }
            set { domPersistencePath = value; }
        }

        public void AddAddInsFromDirectory(string addInDir)
        {
            if (addInDir == null)
                throw new ArgumentNullException("addInDir");
            addInDirectories.Add(addInDir);
        }

        public void AddAddInFile(string addInFile)
        {
            if (addInFile == null)
                throw new ArgumentNullException("addInFile");
            addInFiles.Add(addInFile);
        }
    }
}
