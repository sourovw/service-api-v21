using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dapper;
using System.Net.Mail;
using Azure;

namespace EPS_Service_API.API.Repositories.Notification
{
    public class SMS_Service: ISMS_Service
    {
        private readonly IConfiguration _config;
        public SMS_Service(IConfiguration config)
        {
            _config=config;
        }


        public async Task SendSMSNotification(string MobileNumber,string Message)
        {
           // return;

            try
            {
                string GatewayURL = _config["SMS_Portal:URL"];
                string GatewayAPI= _config["SMS_Portal:apikey"];
                string GatewayUser = _config["SMS_Portal:sender"];

                string CheckFirst3= MobileNumber.Substring(0, 3);

                if (CheckFirst3 == "+88")
                {
                    MobileNumber=MobileNumber.Substring(3);
                }

                string FinalePushURL = GatewayURL + "?apikey=" + GatewayAPI + "&sender=" + GatewayUser + "&msisdn=" + MobileNumber + "&smstext=" + Message;

                using (var httpClient = new HttpClient())
                {

                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), FinalePushURL))
                    {
                      
                        try
                        {
                            var response = await httpClient.SendAsync(request);
                            if (response.IsSuccessStatusCode == true)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    string Ser_success = await content.ReadAsStringAsync();
                                    dynamic jsonObject = JObject.Parse(Ser_success);
                                    // bool status = Convert.ToBoolean(jsonObject.IsValid);
                                    if (jsonObject.status != 0)
                                    {
                                        await SendSMSNotification_Alternate(MobileNumber, Message);
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                          
                        }
                    }
                }

               

            }
            catch (Exception e1)
            {
               
            }
        }



        private async Task SendSMSNotification_Alternate(string MobileNumber, string Message)
        {
            try
            {
                string GatewayURL = _config["SMS_Portal:URL"];
                string GatewayAPI = _config["SMS_Portal:apikey"];
                string GatewayUser = _config["SMS_Portal:sender2"];

                string CheckFirst3 = MobileNumber.Substring(0, 3);

                if (CheckFirst3 == "+88")
                {
                    MobileNumber = MobileNumber.Substring(3);
                }

                string FinalePushURL = GatewayURL + "?apikey=" + GatewayAPI + "&sender=" + GatewayUser + "&msisdn=" + MobileNumber + "&smstext=" + Message;

                using (var httpClient = new HttpClient())
                {

                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), FinalePushURL))
                    {

                        try
                        {
                            var response = await httpClient.SendAsync(request);
                            if (response.IsSuccessStatusCode == true)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    string Ser_success = await content.ReadAsStringAsync();
                                    dynamic jsonObject = JObject.Parse(Ser_success);
                                    // bool status = Convert.ToBoolean(jsonObject.IsValid);

                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }



            }
            catch (Exception e1)
            {

            }

        }

        public async Task SendSMSNotification_Test(string MobileNumber, string Message)
        {
            //   return;

            try
            {
                string GatewayURL = _config["SMS_Portal:URL"];
                string GatewayAPI = _config["SMS_Portal:apikey"];
                string GatewayUser = _config["SMS_Portal:sender"];

                string CheckFirst3 = MobileNumber.Substring(0, 3);

                if (CheckFirst3 == "+88")
                {
                    MobileNumber = MobileNumber.Substring(3);
                }

                string FinalePushURL = GatewayURL + "?apikey=" + GatewayAPI + "&sender=" + GatewayUser + "&msisdn=" + MobileNumber + "&smstext=" + Message;

                using (var httpClient = new HttpClient())
                {

                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), FinalePushURL))
                    {

                        try
                        {
                            var response = await httpClient.SendAsync(request);
                            if (response.IsSuccessStatusCode == true)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    string Ser_success = await content.ReadAsStringAsync();
                                    dynamic jsonObject = JObject.Parse(Ser_success);
                                    // bool status = Convert.ToBoolean(jsonObject.IsValid);

                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }



            }
            catch (Exception e1)
            {

            }
        }


    }
}
