using System.Collections.Generic;
using JenkinsRestClient.Data;

namespace JenkinsRestClient
{
    public interface IJenkinsClient
    {
        IEnumerable<Job> GetJobs();
        JenkinsData GetJenkinsData();
        void StartJob(Job job);
        IEnumerable<SlaveData> GetAllSlaves();
        JobDetail GetJobDetails(Job job);
    }
}