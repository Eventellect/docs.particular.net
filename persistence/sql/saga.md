---
title: Saga Persister
summary: How sagas are persisted using SQL Persistence
component: SqlPersistence
related:
 - samples/sql-persistence/simple
 - samples/sql-persistence/transitioning-correlation-ids
 - samples/saga/sql-sagafinder
 - persistence/sql/saga-concurrency
 - persistence/sql/sqlsaga
redirects:
 - nservicebus/sql-persistence/saga
reviewed: 2025-04-09
---

SQL persistence supports sagas using the core [NServiceBus.Saga](/nservicebus/sagas/) API or an [experimental API unique to SQL persistence](sqlsaga.md) that provides a simpler mapping API.

partial: sqlsaga-required-in-some-versions


## Table structure


### Table name

The name used for a saga table consists of two parts:

 * The prefix of the table name is the [table prefix](/persistence/sql/install.md#table-prefix) defined at the endpoint level.
 * The suffix of the table name is **either** the saga [Type.Name](https://msdn.microsoft.com/en-us/library/system.type.name.aspx) **or**, if defined, the table suffix defined at the saga level.

partial: tablesuffix-snippets

> [!NOTE]
> Using [delimited identifiers](https://technet.microsoft.com/en-us/library/ms176027.aspx) in the TableSuffix is currently **not** supported.


### Columns


#### ID

The value of `IContainSagaData.Id`. Primary Key.


#### Metadata

A JSON-serialized dictionary containing all NServiceBus-managed information about the saga.


#### Data

The JSON-serialized saga data.


#### PersistenceVersion

The assembly version of the SQL persister.


#### SagaTypeVersion

The version of the assembly where the saga exists.


#### Correlation columns

Between 0 and 2 correlation ID columns named `Correlation_[PROPERTYNAME]`. The type will correspond to the .NET type of the mapped property on the saga data.

For each correlation ID there will be a corresponding index named `Index_Correlation_[PROPERTYNAME]`.


## Correlation IDs

[Saga message correlation](/nservicebus/sagas/message-correlation.md) is implemented by promoting the correlation property to the level of a column on the saga table. When saga data is persisted, the correlation property is copied from the instance and duplicated in a column named, by convention, (`Correlation_[PROPERTYNAME]`).

partial: correlation-property


### Correlation types

Each correlation property type has an equivalent SQL data type.

include: correlationpropertytypes

The following .NET types are interpreted as `CorrelationPropertyType.Int`:

 * [Int16](https://msdn.microsoft.com/en-us/library/system.int16.aspx)
 * [Int32](https://msdn.microsoft.com/en-us/library/system.int32.aspx)
 * [Int64](https://msdn.microsoft.com/en-us/library/system.int64.aspx)
 * [UInt16](https://msdn.microsoft.com/en-us/library/system.uint16.aspx)
 * [UInt32](https://msdn.microsoft.com/en-us/library/system.uint32.aspx)
 * [UInt64](https://msdn.microsoft.com/en-us/library/system.uint64.aspx)


## Json.NET settings

SQL persistence uses the [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) package to serialize saga data and metadata.

The saga data can be queried manually by taking advantage of the [JSON querying capababilities of SQL Server](https://docs.microsoft.com/en-us/sql/relational-databases/json/json-data-sql-server). The persister itself does not use any JSON querying capababilities.

### Custom settings

An instance of [JsonSerializerSettings](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm) can be used to customize serialization. 

In this snippet, a custom DateTime converter is included and the `DefaultValueHandling` setting is changed to `Include` (by default, the `DefaultValueHandling` setting is set to `Ignore` to minimize the amount of data stored in the database).

snippet: SqlPersistenceCustomSettings


#### Version-specific / type-specific deserialization settings

The type and saga assembly version are persisted as part of the saga data. It is possible to explicitly control the deserialization of sagas based on version and/or type. This allows the serialization approach to evolve while avoiding migrations.

snippet: SqlPersistenceJsonSettingsForVersion


### Custom reader

Customize the creation of the [JsonReader](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonReader.htm).

snippet: SqlPersistenceCustomReader


### Custom writer

Customize the creation of the [JsonWriter](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonWriter.htm).

snippet: SqlPersistenceCustomWriter
