namespace VolleyMS.BusinessLogic.Features.Users.GetTaskTrends
{
    public sealed record TaskTrendsDto
    {
        public List<string> Labels { get; set; } = new();
        public List<int> TasksTrend { get; set; } = new();
        public List<int> MatchesTrend { get; set; } = new();
    }
}
