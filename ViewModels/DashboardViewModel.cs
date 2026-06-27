namespace LearnerOpsLms.ViewModels
{
    public class DashboardViewModel
    {
        public int CourseCount { get; set; }

        public int LearnerCount { get; set; }

        public int AssessmentCount { get; set; }

        public int EvidenceItemCount { get; set; }

        public string EnvironmentName { get; set; } = string.Empty;

        public string ApplicationName { get; set; } = string.Empty;

        public string SqlServerName { get; set; } = string.Empty;

        public string SqlDatabaseName { get; set; } = string.Empty;

        public string DataAccess { get; set; } = "Entity Framework Core";

        public string DeploymentMethod { get; set; } = "Manual publish and ZIP deploy";
    }
}