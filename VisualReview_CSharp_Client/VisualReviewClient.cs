using System;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace VisualReview_CSharp_Client
{
    public class VisualReviewClient
    {
        private static readonly string HOSTNAME = "localhost";
        private static readonly string PORT = "7000";

        public static string CreateRun(string projectName, string suiteName)
        {
            var client = new RestClient($"http://{HOSTNAME}:{PORT}/api");
            var createRunRequest = new RestRequest($"/runs", Method.POST);
            createRunRequest.AddParameter("application/json", $"{{\"projectName\":\"{projectName}\", \"suiteName\":\"{suiteName}\"}}",
                ParameterType.RequestBody);
            var createRunResponce = client.Execute(createRunRequest);
            return JObject.Parse(createRunResponce.Content)["id"].ToString();
        }

        public static IRestResponse SendScreenshot(
            string runId,
            byte[] screenshotBytes,
            string filePath,
            string screenshotName = "NoNameScreenshot",
            string properties = "{}",
            string meta = "{}",
            string compareSettings = null,
            string mask = null)
        {
            var client = new RestClient($"http://{HOSTNAME}:{PORT}/api");
            var request = new RestRequest($"/runs/{runId}/screenshots", Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFileBytes("file", screenshotBytes, filePath, "image/png");
            request.AlwaysMultipartFormData = true;
            request.AddParameter("screenshotName", screenshotName, ParameterType.GetOrPost);
            request.AddParameter("properties", properties, ParameterType.GetOrPost);
            request.AddParameter("meta", meta, ParameterType.GetOrPost);

            if (compareSettings != null) request.AddParameter("compareSettings", compareSettings, ParameterType.GetOrPost);
            if (mask != null) request.AddParameter("mask", mask, ParameterType.GetOrPost);

            return client.Execute(request);
        }

        public static IRestResponse GetAnalysis(string runId)
        {
            var client = new RestClient($"http://{HOSTNAME}:{PORT}/api");
            var request = new RestRequest($"/runs/{runId}/analysis", Method.GET);

            return client.Execute(request);
        }
    }
}
