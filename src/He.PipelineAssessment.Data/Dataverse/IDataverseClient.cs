namespace He.PipelineAssessment.Data.Dataverse
{
    public interface IDataverseClient
    {
        DataverseResults RunFetchXML(string fetchXML);
    }
}
