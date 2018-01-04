using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace DocuSign.ConsoleApp {
    public static class RestResponseGetter<T> {
        public static T GetPostResponse(string baseUrl, string resource, IDictionary<string, string> headerParams = null, IDictionary<string, string> formParams = null, Dictionary<string, object> body = null) {
            var result = default(T);
            headerParams = headerParams ?? new Dictionary<string, string>();
            formParams = formParams ?? new Dictionary<string, string>();
            try {
                var restRequest = new RestRequest(resource, Method.POST);
                foreach (var headerParam in headerParams) {
                    restRequest.AddHeader(headerParam.Key, headerParam.Value);
                }
                foreach (var formParam in formParams) {
                    restRequest.AddParameter(formParam.Key, formParam.Value);
                }
                if (body != null) {
                    restRequest.RequestFormat = DataFormat.Json;
                    restRequest.AddBody(body);
                }
                var restClient = new RestClient(new Uri(baseUrl));
                var restResponseBase = (RestResponseBase) restClient.Execute(restRequest);
                result = JsonConvert.DeserializeObject<T>(restResponseBase.Content);
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            return result;
        }
    }
}