﻿using System;
using Microsoft.Data.SqlClient;
using NServiceBus;

Console.Title = "Samples.SqlPersistence.EndpointSqlServer";

#region sqlServerConfig

var endpointConfiguration = new EndpointConfiguration("Samples.SqlPersistence.EndpointSqlServer");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlPersistence;Integrated Security=True;Encrypt=false
var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlPersistence;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";
persistence.SqlDialect<SqlDialect.MsSqlServer>();
persistence.ConnectionBuilder(
    connectionBuilder: () => new SqlConnection(connectionString));
var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));

#endregion

SqlHelper.EnsureDatabaseExists(connectionString);

endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();

var endpointInstance = await Endpoint.Start(endpointConfiguration)
    .ConfigureAwait(false);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop()
    .ConfigureAwait(false);
