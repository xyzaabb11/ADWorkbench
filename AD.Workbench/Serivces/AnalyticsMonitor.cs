using ICSharpCode.Core;
using System;

namespace AD.Workbench.Serivces
{
    sealed class AnalyticsMonitor : IAnalyticsMonitor, IDisposable
    {
        public static readonly AnalyticsMonitor Instance = new AnalyticsMonitor();
        public AnalyticsMonitor()
        {
//             ADService.Services.AddService(typeof(IAnalyticsMonitor), this);
//             dbFileName = Path.Combine(PropertyService.ConfigDirectory, "usageData.dat");
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void TrackException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public IAnalyticsMonitorTrackedFeature TrackFeature(string featureName, string activationMethod = null)
        {
//             TrackedFeature feature = new TrackedFeature();
//             lock (lockObj)
//             {
//                 if (session != null)
//                 {
//                     feature.Feature = session.AddFeatureUse(featureName, activationMethod);
//                 }
//             }
            return null;
        }

        public IAnalyticsMonitorTrackedFeature TrackFeature(Type featureClass, string featureName = null, string activationMethod = null)
        {
            throw new NotImplementedException();
        }
    }
}
