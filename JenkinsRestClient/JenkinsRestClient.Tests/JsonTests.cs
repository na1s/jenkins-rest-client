using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static T GetJson<T>(string name)
        {
            Assembly thisExe = Assembly.GetExecutingAssembly();
            Stream stream = thisExe.GetManifestResourceStream("JenkinsRestClient.Tests.JsonResults." + name);
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        [Test]
        public void TestJenkinsComputerData()
        {
            var fakeServer = new Mock<IJenkinsServerApi>();
            fakeServer.Setup(t => t.Get<JenkinsComputerData>("/computer/api/json")).Returns(
                GetJson<JenkinsComputerData>("jenkinsComputerData1.json"));
            var client = new JenkinsClient(fakeServer.Object);
            var slaveDatas = client.GetComputerData().Computer.ToList();
            Assert.AreEqual(2, slaveDatas.Count);
            Assert.IsFalse(slaveDatas[0].Offline);
            Assert.IsTrue(slaveDatas[1].Offline);
        }

        [Test]
        public void TestJobs()
        {
            var fakeServer = new Mock<IJenkinsServerApi>();
            fakeServer.Setup(t => t.Get<JenkinsData>("/api/json")).Returns(GetJson<JenkinsData>("jenkinsData1.json"));
            var client = new JenkinsClient(fakeServer.Object);
            List<Job> jobs = client.GetJobs().ToList();
            Assert.AreEqual(1, jobs.Count());
            Job job = jobs.FirstOrDefault();
            Assert.AreEqual("asdsa", job.Name);
            Assert.AreEqual("http://127.0.0.1:8080/job/asdsa/", job.Url);
            Assert.IsTrue(job.IsSuccess());
        }

        [Test]
        public void TestMainData()
        {
            var fakeServer = new Mock<IJenkinsServerApi>();
            fakeServer.Setup(t => t.Get<JenkinsData>("/api/json")).Returns(GetJson<JenkinsData>("jenkinsData1.json"));
            var client = new JenkinsClient(fakeServer.Object);
            Assert.AreEqual(2, client.GetJenkinsData().NumExecutors);
        }
    }
}