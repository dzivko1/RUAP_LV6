﻿

// This code requires the Nuget package Microsoft.AspNet.WebApi.Client to be installed.
// Instructions for doing this in Visual Studio:
// Tools -> Nuget Package Manager -> Package Manager Console
// Install-Package Microsoft.AspNet.WebApi.Client

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CallRequestResponseService
{

	public class StringTable
	{
		public string[] ColumnNames { get; set; }
		public string[,] Values { get; set; }
	}

	class Program
	{
		static void Main(string[] args)
		{
			InvokeRequestResponseService().Wait();
			Console.ReadLine();
		}

		static async Task InvokeRequestResponseService()
		{
			using (var client = new HttpClient())
			{
				var scoreRequest = new {

					Inputs = new Dictionary<string, StringTable>() {
						{
							"input1",
							new StringTable()
							{
								ColumnNames = new string[] {"Recency", "Frequency", "Monetary", "Time", "Class"},
								Values = new string[,] {  { "2", "6", "1500", "22", "0" }, { "4", "2", "500", "4", "1" },  }
							}
						},
					},
					GlobalParameters = new Dictionary<string, string>() {
					}
				};
				const string apiKey = "DPPIoVfrdEn07Pi3ESCNl2dfRUpFYGnkxYa7zLqA3Gd8b5uUOuSHB5BL2bPwhkp9+RfkzLpn4u0fGv1F3A769w=="; // Replace this with the API key for the web service
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

				client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/ca191a792ede4730a7fd88a9666ab74f/services/3b26c34c727c4dcb9465c6ca05bdc780/execute?api-version=2.0&details=true");

				// WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
				// One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
				// For instance, replace code such as:
				//      result = await DoSomeTask()
				// with the following:
				//      result = await DoSomeTask().ConfigureAwait(false)


				HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

				if (response.IsSuccessStatusCode)
				{
					string result = await response.Content.ReadAsStringAsync();
					Console.WriteLine("Result: {0}", result);
				}
				else
				{
					Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

					// Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
					Console.WriteLine(response.Headers.ToString());

					string responseContent = await response.Content.ReadAsStringAsync();
					Console.WriteLine(responseContent);
				}
			}
		}
	}
}

