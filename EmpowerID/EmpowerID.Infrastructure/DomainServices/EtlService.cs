using Azure.Identity;
using EmpowerID.Domain.DomainServices;
using EmpowerID.Domain.Settings;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmpowerID.Infrastructure.DomainServices
{
    public class EtlService : IEtlService
    {
        private readonly DataFactoryManagementClient _dataFactoryManagementClient;
        private readonly ILogger<EtlService> _logger;
        private readonly EtlServiceSettings _settings;

        public EtlService(DataFactoryManagementClient client, IOptions<EtlServiceSettings> etlSettings, ILogger<EtlService> logger)
        {

            _logger = logger;
            _settings = etlSettings.Value;
            _dataFactoryManagementClient = client;

        }

        public async Task RunPipelineAsync()
        {
            var resourceGroupName = _settings.ResourceGroupName;
            var factoryName = _settings.FactoryName;
            var pipelineName = _settings.PipelineName;

            _logger.LogDebug("Triggering the pipeline...");
            var runResponse = await _dataFactoryManagementClient.Pipelines.CreateRunAsync(resourceGroupName, factoryName, pipelineName);

            string runId = runResponse.RunId;
            _logger.LogDebug("Pipeline run ID: {runId}", runId);

            // Monitor the pipeline run status
            PipelineRun pipelineRun;
            do
            {
                pipelineRun = await _dataFactoryManagementClient.PipelineRuns.GetAsync(resourceGroupName, factoryName, runId);
                _logger.LogDebug("Status: {Status}", pipelineRun.Status);
                await Task.Delay(10000); // Wait for 10 seconds before checking the status again
            } while (pipelineRun.Status == "InProgress");

            _logger.LogDebug("Pipeline run finished with status: {pipelineRun.Status}", pipelineRun.Status);
        }
    }
}
