### Topology configuration

The [default topology](/transports/azure-service-bus/topology.md) (using topic-per-event approach) can be overridden by adding topology options to the Application configuration.

The functions integration looks for either topology options placed into `AzureServiceBus:TopologyOptions` or `AzureServiceBus:MigrationTopologyOptions` for migration scenarios.

snippet: ASBFunctionsWorker-topology-options

Using the default topology

snippet: ASBFunctionsWorker-topology-options-json

Using the migration topology

snippet: ASBFunctionsWorker-topology-migration-options-json