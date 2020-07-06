using Structure.Application.Adapters;
using Structure.Data.Filtering;
using Structure.DependencyInjection;
using Structure.Domain.Notifications;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Structure.Application
{
    public abstract class ApplicationService : IApplicationService, IServiceProviderAccessor, IObjectAdapterSupport
    {
        protected bool isDisposed = false;

        //TODO: lazy load services
        public IUnitOfWorkManager UowManager { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        public IObjectAdapter Adapter { get; set; }
        public IDataFilterHandler DataFilterHandler { get; set; }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
        }

        protected void Notify(INotification notification)
        {
            ServiceProvider.GetService<INotificationCollection>().Add(notification);
        }

        protected T Default<T>(INotification notification = null)
        {
            if (notification != null)
            {
                Notify(notification);
            }

            return default;
        }

        protected T GetService<T>()
            where T : class
        {
            return ServiceProvider.GetService<T>();
        }

        //protected TService LazyGetRequiredService<TService>(ref TService reference)
        //   => LazyGetRequiredService(typeof(TService), ref reference);

        //protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        //{
        //    if (reference == null)
        //    {
        //        lock (serviceProviderLock)
        //        {
        //            if (reference == null)
        //            {
        //                reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
        //            }
        //        }
        //    }

        //    return reference;
        //}
    }
}
