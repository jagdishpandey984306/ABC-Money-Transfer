using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System.Text.Json;


namespace MoneyTransfer.BLL.Services
{
    public class CountryService
    {
        public async Task<List<SelectListItem>> CountryList()
        {

            string apiUrl = "https://restcountries.com/v3.1/all";
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                JArray countriesArray;
                try
                {
                    countriesArray = JArray.Parse(responseBody);
                }
                catch (JsonException ex)
                {
                    throw new InvalidOperationException("Failed to parse JSON response", ex);
                }

                var list = new List<SelectListItem>();

                foreach (var country in countriesArray)
                {
                    var countryNameToken = country["name"]?["common"];
                    if (countryNameToken != null)
                    {
                        string countryName = countryNameToken.ToString();
                        list.Add(new SelectListItem
                        {
                            Text = countryName,
                            Value = countryName
                        });
                    }
                }

                return list;
            }
        }
    }
}
