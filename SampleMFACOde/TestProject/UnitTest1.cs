using Newtonsoft.Json;
using NUnit.Framework;
using SampleMFACOde;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestCases;

namespace TestProject
{
    public class Tests
    {
        public TestResults testResults;
        public Class1 class1;
        public Dictionary<string, TestCaseResultDto> testCaseResults;
        public string customValue;
        public string UniqueGuid = "18f69543-da90-412c-8a01-4825f31340bb";
        [SetUp]
        public void Setup()
        {
            class1 = new Class1();
            testResults = new TestResults();
            testCaseResults = new Dictionary<string, TestCaseResultDto>();
            customValue = System.IO.File.ReadAllText("../../../../custom.ih");
            testResults.CustomData = customValue;
        }

        [Test]
        public void Test1()
        {
            try
            {
                string a = "Hello ", b = "there";
                var result = class1.ConcatString(a, b);
                Assert.Equals(result, "Hello there");
                testCaseResults.Add("18f69543-da90-412c-8a01-4825f31340bb", new TestCaseResultDto
                {
                    MethodName = "test1",
                    MethodType = "functional",
                    EarnedScore = 25,
                    ActualScore = 25,
                    Status = "Passed",
                    IsMandatory = true
                });
            }
            catch (Exception ex)
            {
                testCaseResults.Add("18f69543-da90-412c-8a01-4825f31340bb", new TestCaseResultDto
                {
                    MethodName = "test1",
                    MethodType = "functional",
                    EarnedScore = 25,
                    ActualScore = 0,
                    Status = "Passed",
                    IsMandatory = true
                });
            }
        }

        [TearDown]
        public async Task SendTestCaseResults()
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                testResults.TestCaseResults = JsonConvert.SerializeObject(testCaseResults);
                var testResultsJson = JsonConvert.SerializeObject(testResults);
                await _httpClient.PostAsync("https://yaksha-stage-sbfn.azurewebsites.net/api/YakshaMFAEnqueue?code=JSssTES1yvRyHXshDwx6m405p0uSwbqnA937NaLAGX7zazwdLPC4jg==", new StringContent(testResultsJson, Encoding.UTF8, "application/json"));
            }
        }
    }
}
