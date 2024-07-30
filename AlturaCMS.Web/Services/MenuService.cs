using AlturaCMS.Web.DataModels;

namespace AlturaCMS.Web.Services;

public class MenuService
{
    private readonly HttpClient _httpClient;

    public MenuService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<MenuItem>> GetMenuItems()
    {
        return await _httpClient.GetFromJsonAsync<List<MenuItem>>("api/menu");
    }
}