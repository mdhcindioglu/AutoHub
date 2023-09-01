using AutoHub.Client.Extenstions;
using AutoHub.Shared.Entities;
using AutoHub.Shared.Models;
using System.Net.Http.Json;

namespace AutoHub.Client.Services
{
    public interface IItemService
    {
        Task<Response<List<Item>>?> Items(int id);
        Task<Response<Item>?> Item(int id);
        Task<Response<Item>> Create(Item item);
        Task<Response<Item>> Update(Item item);
        Task<Response> Delete(int id);
        Task<bool> IsAdded(string title, int stepId, int id);
    }

    public class ItemService : IItemService
    {
        private readonly HttpClient _http;
        public ItemService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Response<List<Item>>?> Items(int id) => await _http.GetFromJsonAsync<Response<List<Item>>>($"API/Items?id={id}");

        public async Task<Response<Item>?> Item(int id) => await _http.GetFromJsonAsync<Response<Item>>($"API/Items/ById?id={id}");

        public async Task<Response<Item>> Create(Item item) => await _http.PostDataFromUrlAsync<Response<Item>>($"API/Items", item);

        public async Task<Response<Item>> Update(Item item) => await _http.PutDataFromUrlAsync<Response<Item>>($"API/Items", item);

        public async Task<Response> Delete(int id) => await _http.DeleteDataFromUrlAsync<Response>($"API/Items?id={id}");

        public async Task<bool> IsAdded(string title, int stepId, int id) => await _http.GetFromJsonAsync<bool>($"API/Items/IsAdded?title={title}&stepId={stepId}&id={id}");
    }
}
