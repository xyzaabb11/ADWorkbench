using ICSharpCode.Core;
using System;
using System.IO;
using System.Threading;
using System.Xml;

namespace AD.Workbench.Serivces
{
    public sealed class CallbackOnDispose : IDisposable
    {
        Action action;

        public CallbackOnDispose(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            this.action = action;
        }

        public void Dispose()
        {
            Action a = Interlocked.Exchange(ref action, null);
            if (a != null)
            {
                a();
            }
        }
    }
    class PropertyService : PropertyServiceImpl
    {
        DirectoryName dataDirectory;
        DirectoryName configDirectory;
        FileName propertiesFileName;
        public PropertyService(DirectoryName configDirectory, DirectoryName dataDirectory, string propertiesName)
            : base(LoadPropertiesFromStream(configDirectory.CombineFile(propertiesName + ".xml")))
        {
            this.dataDirectory = dataDirectory;
            this.configDirectory = configDirectory;
            propertiesFileName = configDirectory.CombineFile(propertiesName + ".xml");
        }
        public override DirectoryName ConfigDirectory
        {
            get
            {
                return configDirectory;
            }
        }

        public override DirectoryName DataDirectory
        {
            get
            {
                return dataDirectory;
            }
        }

        static Properties LoadPropertiesFromStream(FileName fileName)
        {
            if (!File.Exists(fileName))
            {
                return new Properties();
            }
            try
            {
                using (LockPropertyFile())
                {
                    return Properties.Load(fileName);
                }
            }
            catch (XmlException ex)
            {
                ADService.MessageService.ShowError("Error loading properties: " + ex.Message + "\nSettings have been restored to default values.");
            }
            catch (IOException ex)
            {
                ADService.MessageService.ShowError("Error loading properties: " + ex.Message + "\nSettings have been restored to default values.");
            }
            return new Properties();
        }

        static IDisposable LockPropertyFile()
        {
            Mutex mutex = new Mutex(false, "PropertyServiceSave-30F32619-F92D-4BC0-BF49-AA18BF4AC313");
            mutex.WaitOne();
            return new CallbackOnDispose(
                delegate
                {
                    mutex.ReleaseMutex();
                    mutex.Close();
                });
        }
    }
}
