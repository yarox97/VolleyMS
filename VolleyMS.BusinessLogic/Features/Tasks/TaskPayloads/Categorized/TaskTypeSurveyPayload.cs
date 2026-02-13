namespace VolleyMS.BusinessLogic.Features.Tasks.TaskPayloads.Categorized
{
    public sealed record TaskTypeSurveyPayload()
    {
        public required string surveyUrl { get; set; }
    }
}
