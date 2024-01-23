﻿using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

class DIShims
{
    void Configuration()
    {
        IServiceCollection services = new ServiceCollection();

#pragma warning disable CS0618 // Type or member is obsolete
        #region core-8to9-di-shims
        // 1
        services.ConfigureComponent(typeof(MyDependency), DependencyLifecycle.SingleInstance);
        // 2
        services.ConfigureComponent<MyDependency>(DependencyLifecycle.SingleInstance);
        // 3
        services.ConfigureComponent<MyDependency>(DependencyLifecycle.InstancePerUnitOfWork);
        // 4
        services.ConfigureComponent<MyDependency>(DependencyLifecycle.InstancePerCall);
        // 5
        services.RegisterSingleton(new MyDependency());
        // 6
        if(services.HasComponent<MyDependency>())
        {
            // do something
        }
        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }

    class MyDependency { }
}