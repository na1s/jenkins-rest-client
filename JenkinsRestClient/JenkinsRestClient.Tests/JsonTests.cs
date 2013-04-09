using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using JenkinsRestClient.Data;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json;

namespace JenkinsRestClient.Tests
{
    [TestFixture]
    public class JsonTests
    {
        [Test] 
        public void TestJobs()
        {
            var fakeServer = new Mock<IJenkinsServerApi>();
            fakeServer.Setup(t => t.Get<JenkinsData>("/api/json")).Returns(GetJson<JenkinsData>("jenkinsData1.json"));
            var client = new JenkinsClient(fakeServer.Object);
            var jobs = client.GetJobs().ToList();
            Assert.AreEqual(1,jobs.Count());
            var job = jobs.FirstOrDefault();
            Assert.AreEqual("asdsa", job.Name);
            Assert.AreEqual("http://127.0.0.1:8080/job/asdsa/", job.Url);
            Assert.IsTrue( job.IsSuccess());
        }
        [Test]
        public void TestMainData()
        {
            var fakeServer = new Mock<IJenkinsServerApi>();
            fakeServer.Setup(t => t.Get<JenkinsData>("/api/json")).Returns(GetJson<JenkinsData>("jenkinsData1.json"));
            var client = new JenkinsClient(fakeServer.Object);
            Assert.AreEqual(2, client.GetJenkinsData().NumExecutors);
        }
        private static T GetJson<T>(string name)
        {
            Assembly thisExe = Assembly.GetExecutingAssembly();
            var stream = thisExe.GetManifestResourceStream("JenkinsRestClient.Tests.JsonResults." + name);
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(result);
            }
        }
    }
}