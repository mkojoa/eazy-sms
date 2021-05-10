using eazy.sms.Core.Providers.MnotifyHelpers.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eazy.sms.Core.Providers.MnotifyHelpers.Helpers
{
    public class ApiCallHelper<TResult>
        where TResult : class
    {
        public static async Task<TResult> Campaign(string apiUrl, DataDto data)
        {
            TResult result = null;

            var jsonString = JsonConvert.SerializeObject(data);

            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("POST"), apiUrl)
            {
                Content = new StringContent(jsonString),
                //("{\"recipient\":[\"0249706365\",\"0203698970\"], \"sender\":\"mNotify\", \"message\":\"API messaging is fun!\", \"is_schedule\":false, \"schedule_date\":\"\"}")

            };
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync().ContinueWith(x =>
            {
                if (x.IsFaulted)
                    if (x.Exception != null)
                        throw x.Exception;

                result = JsonConvert.DeserializeObject<TResult>(x.Result);
            });

            return result;
        }
        public static async Task<TResult> CampaignWithVoice(string apiUrl, DataDto data)
        {
            TResult result = null;

            using (var httpClient = new HttpClient())
            {
                using var request = new HttpRequestMessage(new HttpMethod("POST"), $"{apiUrl}");
                var multipartContent = new MultipartFormDataContent();

                for (int i = 0; i < data.Recipient.Length; i++) // get all recipient http:localhost:887
                {
                    multipartContent.Add(new StringContent(data.Recipient[i]), "recipient[]");
                }

                var voiceId = (data.VoiceId == null) ? "" : data.VoiceId;
                var scheduleDate = (data.ScheduleDate == null) ? "" : data.ScheduleDate;
                 
                multipartContent.Add(new ByteArrayContent(File.ReadAllBytes(data.File)), "file", Path.GetFileName(data.File));
                multipartContent.Add(new StringContent(voiceId), "voice_id");
                multipartContent.Add(new StringContent(data.IsSchedule.ToString()), "is_schedule");
                multipartContent.Add(new StringContent(scheduleDate), "schedule_date");
                multipartContent.Add(new StringContent(data.Campaign), "campaign");
                request.Content = multipartContent;

                var response = await httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync().ContinueWith(x =>
                {
                    if (x.IsFaulted)
                        if (x.Exception != null)
                            throw x.Exception;

                    result = JsonConvert.DeserializeObject<TResult>(x.Result);
                });
            }
            return result;
        }
    }
}
