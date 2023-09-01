using AutoHub.Client.Extenstions;
using AutoHub.Shared.Entities;
using AutoHub.Shared.Models;
using System.Net.Http.Json;

namespace AutoHub.Client.Services
{
    public interface IStepService
    {
        Task<Response<List<Step>>?> Steps();
        Task<Response<Step>?> Step(int id);
        Task<Response<Step>> Create(Step step);
        Task<Response<Step>> Update(Step step);
        Task<Response> Delete(int id);
        Task<bool> IsAdded(string title, int id);
    }

    public class StepService : IStepService
    {
        private readonly HttpClient _http;
        public StepService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Response<List<Step>>?> Steps() => await _http.GetFromJsonAsync<Response<List<Step>>>($"API/Steps");

        public async Task<Response<Step>?> Step(int id) => await _http.GetFromJsonAsync<Response<Step>>($"API/Steps?id={id}");

        public async Task<Response<Step>> Create(Step step) => await _http.PostDataFromUrlAsync<Response<Step>>($"API/Steps", step);

        public async Task<Response<Step>> Update(Step step) => await _http.PutDataFromUrlAsync<Response<Step>>($"API/Steps", step);

        public async Task<Response> Delete(int id) => await _http.DeleteDataFromUrlAsync<Response>($"API/Steps?id={id}");

        public async Task<bool> IsAdded(string title, int id) => await _http.GetFromJsonAsync<bool>($"API/Steps/IsAdded?title={title}&id={id}");
    }
}
