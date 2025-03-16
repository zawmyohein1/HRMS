using HRMS.Model.Responses;
using HRMS.Models.View;
using Newtonsoft.Json;
using System.Text;

namespace HRMS.UI.Helper
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiBaseUrl"];
            _httpClient.BaseAddress = new Uri(_apiBaseUrl); // Ensure BaseAddress is set
        }

        /// <summary>
        ///  GET Request that returns a **list** of items wrapped in `ResponseModel<T>`.
        /// </summary>
        public async Task<ResponseModel<List<T>>> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{endpoint}");

            if (!response.IsSuccessStatusCode)
            {
                return new ResponseModel<List<T>>((int)response.StatusCode, "Request failed.");
            }

            var content = await response.Content.ReadAsStringAsync();

            //  Deserialize as `ResponseModel<List<T>>`
            var responseObject = JsonConvert.DeserializeObject<ResponseModel<List<T>>>(content);

            return responseObject ?? new ResponseModel<List<T>>(500, "Failed to deserialize response.");
        }



        /// <summary>
        ///  GET Request that returns a **single** object wrapped in `ResponseModel<T>`.
        /// </summary>
        public async Task<ResponseModel<T>> GetSingleAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{endpoint}");

            if (!response.IsSuccessStatusCode)
            {
                return new ResponseModel<T>((int)response.StatusCode, "Request failed.");
            }

            var content = await response.Content.ReadAsStringAsync();

            //  Deserialize as `ResponseModel<List<T>>`
            var responseObject = JsonConvert.DeserializeObject<ResponseModel<T>>(content);

            return responseObject ?? new ResponseModel<T>(500, "Failed to deserialize response.");
        }

        /// <summary>
        ///  POST Request to create a new record.
        /// </summary>
        public async Task<ResponseModel<T>> PostAsync<T>(string endpoint, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/{endpoint}", content);

            return await HandleResponse<T>(response);
        }

        /// <summary>
        ///  PUT Request to update an existing record.
        /// </summary>
        public async Task<ResponseModel<T>> PutAsync<T>(string endpoint, T data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{endpoint}", content);

            return await HandleResponse<T>(response);
        }

        /// <summary>
        ///  DELETE Request to remove a record.
        /// </summary>
        public async Task<ResponseModel<T>> DeleteAsync<T>(string endpoint)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{endpoint}");

            return await HandleResponse<T>(response);
        }

        /// <summary>
        ///  Centralized Response Handling to **parse JSON and return ResponseModel<T>**.
        /// </summary>
        private async Task<ResponseModel<T>> HandleResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                // Deserialize the base ResponseModel structure first
                var baseResponse = JsonConvert.DeserializeObject<ResponseModel<object>>(content);

                if (baseResponse == null)
                    return new ResponseModel<T>((int)response.StatusCode, "Failed to deserialize response.");

                // If T is IEnumerable<EmployeeDTO>, convert data to the correct list type
                if (typeof(T) == typeof(IEnumerable<EmployeeModel>))
                {
                    var listData = JsonConvert.DeserializeObject<List<EmployeeModel>>(JsonConvert.SerializeObject(baseResponse.Data));
                    return new ResponseModel<T>(baseResponse.StatusCode, baseResponse.Message, (T)(object)listData, baseResponse.Error);
                }
                // If T is a single object, deserialize it correctly
                else if (typeof(T) == typeof(EmployeeModel))
                {
                    var objData = JsonConvert.DeserializeObject<EmployeeModel>(JsonConvert.SerializeObject(baseResponse.Data));
                    return new ResponseModel<T>(baseResponse.StatusCode, baseResponse.Message, (T)(object)objData, baseResponse.Error);
                }

                // Default deserialization
                var result = JsonConvert.DeserializeObject<ResponseModel<T>>(content);
                return result ?? new ResponseModel<T>((int)response.StatusCode, "Failed to deserialize response.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Deserialization error: {ex.Message}");
                return new ResponseModel<T>((int)response.StatusCode, "Error while processing the response.", default, ex.Message);
            }
        }

        public async Task<ResponseModel<T>> PostAsyncT2<T>(string endpoint, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/{endpoint}", content);
            var result = await response.Content.ReadAsStringAsync();

            //  Deserialize the base response structure first
            var baseResponse = JsonConvert.DeserializeObject<ResponseModel<object>>(result);

            Console.WriteLine("Raw API Response: " + result); // Debugging output

            //  Handle single object vs. list scenario
            object objData;
            if (baseResponse.Data is Newtonsoft.Json.Linq.JArray)
            {
                objData = JsonConvert.DeserializeObject<List<EmployeeModel>>(JsonConvert.SerializeObject(baseResponse.Data));
            }
            else
            {
                var singleEmployee = JsonConvert.DeserializeObject<EmployeeModel>(JsonConvert.SerializeObject(baseResponse.Data));
                objData = new List<EmployeeModel> { singleEmployee };
            }

            return new ResponseModel<T>(baseResponse.StatusCode, baseResponse.Message, (T)objData, baseResponse.Error);
        }
    }
}
