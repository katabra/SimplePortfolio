using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PFWSWPFClient
{
    /// <summary>
    /// 간단한 RestApi Get, Post 호출
    /// </summary>
    class ApiCall
    {
        HttpClient client = new HttpClient();

        public async Task CallGetApi(string uri)
        {
            var result = await client.GetAsync(uri);
            if (result.IsSuccessStatusCode)
            {
                MessageBox.Show(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task CallPostApi(string uri)
        {
            var result = await client.PostAsync(uri, null);
            if (result.IsSuccessStatusCode)
            {
                MessageBox.Show(await result.Content.ReadAsStringAsync());
            }
        }
    }
}
