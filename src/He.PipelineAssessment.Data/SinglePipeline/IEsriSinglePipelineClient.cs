namespace He.PipelineAssessment.Data.SinglePipeline
{
    public interface IEsriSinglePipelineClient
    {
        Task<string?> GetSinglePipelineData(string spid);
    }
}
