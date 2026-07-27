using System.Net.Http;
using System.Net.Http.Json;
using CAPA_ENTITY;

namespace Tracking_Tool_System.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ApiService(
            HttpClient httpClient,
            IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            _httpClient.BaseAddress =
                new Uri(_config["ApiSettings:BaseUrl"]);
        }

        // GET
        public async Task<List<T>> GetAsync<T>(
            string endpoint)
        {
            return await _httpClient
                .GetFromJsonAsync<List<T>>(endpoint)
                ?? new List<T>();
        }

        public async Task<T?> GetByIdAsync<T>(
            string endpoint)
        {
            return await _httpClient
                .GetFromJsonAsync<T>(endpoint);
        }

        // POST
        public async Task<HttpResponseMessage> PostAsync<T>(
            string endpoint,
            T data)
        {
            return await _httpClient
                .PostAsJsonAsync(endpoint, data);
        }

        // PUT
        public async Task<HttpResponseMessage> PutAsync<T>(
            string endpoint,
            T data)
        {
            return await _httpClient
                .PutAsJsonAsync(endpoint, data);
        }

        // GET Single
        public async Task<T?> GetSingleAsync<T>(
            string endpoint)
        {
            return await _httpClient
                .GetFromJsonAsync<T>(endpoint);
        }

        // DELETE
        public async Task<HttpResponseMessage> DeleteAsync(
            string endpoint)
        {
            return await _httpClient
                .DeleteAsync(endpoint);
        }

        public async Task<List<ReportMoldEntity>> GetReportMolds(
            int? moldID = null,
            string? moldStatus = null)
        {
            string url = "ReportMold";

            List<string> parameters = new();

            if (moldID.HasValue)
            {
                parameters.Add(
                    $"moldID={moldID.Value}"
                );
            }

            if (!string.IsNullOrWhiteSpace(moldStatus))
            {
                parameters.Add(
                    $"moldStatus=" +
                    $"{Uri.EscapeDataString(moldStatus.Trim())}"
                );
            }

            if (parameters.Any())
            {
                url += "?" + string.Join(
                    "&",
                    parameters
                );
            }

            return await GetAsync<ReportMoldEntity>(
                url
            );
        }

        public async Task<List<MoldEvaluationEntity>>
            GetReportMoldEvaluations(
                int? moldID,
                DateTime? startDate,
                DateTime? endDate
            )
        {
            string url = "MoldEvaluation/report";

            List<string> parameters = new();

            if (moldID.HasValue)
            {
                parameters.Add(
                    $"moldID={moldID.Value}"
                );
            }

            if (startDate.HasValue)
            {
                parameters.Add(
                    $"startDate=" +
                    $"{startDate.Value:yyyy-MM-dd}"
                );
            }

            if (endDate.HasValue)
            {
                parameters.Add(
                    $"endDate=" +
                    $"{endDate.Value:yyyy-MM-dd}"
                );
            }

            if (parameters.Any())
            {
                url += "?" + string.Join(
                    "&",
                    parameters
                );
            }

            return await GetAsync<MoldEvaluationEntity>(
                url
            );
        }

        public async Task<MoldEvaluationEntity?>
            GetReportMoldEvaluationDetail(
                int evaluationID)
        {
            string url =
                $"MoldEvaluation/report/detail/" +
                $"{evaluationID}";

            return await GetSingleAsync<MoldEvaluationEntity>(
                url
            );
        }

        public async Task<List<ReportItemBomEntity>>
        GetReportItemBom()
        {
            return await GetAsync<ReportItemBomEntity>(
                "ReportItemBom"
            );
        }

        public async Task<List<ReportPendingEvaluationEntity>>
    GetPendingEvaluationsReport(
        int? year = null,
        bool detail = false)
        {
            string endpoint =
                $"ReportPendingEvaluation?year={year}";

            if (detail)
            {
                endpoint += "&detail=true";
            }

            return await GetAsync<ReportPendingEvaluationEntity>(
                endpoint);
        }
    }
}