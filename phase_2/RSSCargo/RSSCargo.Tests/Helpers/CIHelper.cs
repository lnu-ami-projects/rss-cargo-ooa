using System;

namespace RSSCargo.Tests.Helpers
{
    /// <summary>
    /// Helper class for CI-specific configurations and settings
    /// </summary>
    public static class CIHelper
    {
        /// <summary>
        /// Determines if the tests are running in a CI environment
        /// </summary>
        public static bool IsRunningInCI => 
            !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI")) || 
            !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"));
            
        /// <summary>
        /// Gets the connection string for tests running in CI
        /// </summary>
        public static string GetTestConnectionString()
        {
            if (IsRunningInCI)
            {
                // CI PostgreSQL connection string
                return "Host=localhost;Port=5432;Database=rsscargo_test;Username=postgres;Password=postgres";
            }
            
            // Default local development connection string
            return "Host=localhost;Port=5432;Database=rsscargo_test_local;Username=postgres;Password=postgres";
        }
    }
}