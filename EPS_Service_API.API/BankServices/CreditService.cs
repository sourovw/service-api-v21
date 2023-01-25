using EPS_Service_API.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace EPS_Service_API.API.BankServices
{
    public class CreditService : ICreditService
    {
        private readonly HttpClient httpClient;

        //public CreditService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        //{
        //    this.httpClient = httpClientFactory.CreateClient(configuration.GetValue<string>("CurrentBank"));
        //    this.httpClient.DefaultRequestHeaders.Add("x-hash", "83b4d2700144a5119985ddbe54b6c16d2d4081b1d04c20efe4268ad34a681a2a");
        //}



        public async Task<CreditTransactionModel> ValidateCredit(CreditTransactionModel inputModel)
        {
            CreditTransactionModel _objResponseModel = new CreditTransactionModel();
            try
            {
                var GetBankServiceEndPoint = "";
                GetBankServiceEndPoint = "http://localhost:2342/api/Credit/ValidateCredit";

                inputModel.IsSuccess = false;
                CreditTransactionModel BankModel = new CreditTransactionModel
                {
                    Amount = inputModel.Amount,
                    // AccountNo = customerFromAccount.AccountNumber,
                    TransactionType = inputModel.TransactionType,
                    Hash = "AF316ECB91A8EE7AE99210702B2D4758F30CDDE3BF61E3D8E787D74681F90A6E",

                    FromAccountId = inputModel.FromAccountId,
                    ToAccountId = inputModel.ToAccountId,
                    FromBankId = inputModel.FromBankId,
                    ToBankId = inputModel.ToBankId
                };

                var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync<CreditTransactionModel>(GetBankServiceEndPoint, BankModel);

                //var response = await Policy
                //            .Handle<HttpRequestException>(ex =>
                //            {
                //                Debug.WriteLine($"► {ex.GetType().Name} (ValidateCredit) : {ex.Message}");
                //                return true;
                //            })
                //            .WaitAndRetryAsync
                //            (
                //                2, retryAttempt => TimeSpan.FromSeconds(2)
                //            )
                //            .ExecuteAsync(async () =>
                //                //  await httpClient.PostAsJsonAsync<CreditTransactionModel>(GetBankServiceEndPoint, BankModel)
                //                await httpClient.PostAsJsonAsync<CreditTransactionModel>("http://localhost:2342/api/Credit/ValidateCredit", BankModel)
                //            );

                //Transfer transfer = new Transfer
                //{
                //    Id = inputModel.TransferId,
                //    FromAccountId = inputModel.FromAccountId,
                //    Amount = inputModel.Amount,
                //    ChargeAmount = inputModel.ChargeAmount,
                //    VatAmount = inputModel.VatAmount,
                //    ToAccountId = inputModel.ToAccountId
                //};

                string CreditTransactionId = "";
                decimal amount = 0;
                string accountNumber = "";
                string transactionDate = DateTime.Now.ToString();
                int StatusCode = 0;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        using (HttpContent content = response.Content)
                        {
                            string Success_Result = await content.ReadAsStringAsync();
                            dynamic jsonObject = JObject.Parse(Success_Result);

                            CreditTransactionId = jsonObject.TransactionID;
                            StatusCode = jsonObject.StatusCode;

                            if (StatusCode == 0)
                            {


                                // If Success
                                _objResponseModel.APIVersion = "0.1";
                                _objResponseModel.TransferId = Convert.ToInt32(CreditTransactionId);
                                _objResponseModel.StatusCode = 0;
                                _objResponseModel.ErrorDescription = "Data get successfully";
                                _objResponseModel.Bankresult = Success_Result;
                                _objResponseModel.IsSuccess = true;
                                return _objResponseModel;

                            }
                            else
                            {
                                //inputModel.TransferId = Convert.ToInt32(CreditTransactionId);
                                //inputModel.IsSuccess = false;

                                _objResponseModel.APIVersion = "0.1";
                                _objResponseModel.TransferId = Convert.ToInt32(CreditTransactionId);
                                _objResponseModel.StatusCode = 0;
                                _objResponseModel.ErrorDescription = "Data get not successfully";
                                _objResponseModel.Bankresult = Success_Result;
                                _objResponseModel.IsSuccess = false;
                                return _objResponseModel;
                            }
                        }
                        break;
                    case HttpStatusCode.BadRequest:
                        // Handle status
                        // Save data to  transfer Table for (ValidateCredit)
                        inputModel.TransferId = 0;
                        inputModel.IsSuccess = false;

                        break;
                    case HttpStatusCode.NotFound:
                        // Handle status

                        inputModel.TransferId = 0;
                        inputModel.IsSuccess = false;

                        break;
                    case HttpStatusCode.Forbidden:
                        // Handle status
                        // Save data to  transfer Table for (ValidateCredit)
                        inputModel.TransferId = 0;
                        inputModel.IsSuccess = false;

                        break;
                    case HttpStatusCode.TooManyRequests:
                        // Handle status
                        // Save data to  transfer Table for (ValidateCredit)
                        inputModel.TransferId = 0;
                        inputModel.IsSuccess = false;

                        break;
                    case HttpStatusCode.UnavailableForLegalReasons:
                        // Handle status
                        // Save data to  transfer Table for (ValidateCredit)
                        inputModel.TransferId = 0;
                        inputModel.IsSuccess = false;


                        break;
                    case HttpStatusCode.RequestTimeout:
                        // Handle status
                        // Save data to  transfer Table for (ValidateCredit)
                        inputModel.TransferId = 0;
                        inputModel.IsSuccess = false;

                        break;
                    case HttpStatusCode.InternalServerError:
                        // Handle status
                        // Save data to  transfer Table for (ValidateCredit)
                        inputModel.TransferId = 0;
                        inputModel.IsSuccess = false;

                        break;
                    default:
                        // Handle default case
                        // Save data to  transfer Table for (ValidateCredit)
                        inputModel.TransferId = 0;
                        inputModel.IsSuccess = false;

                        break;
                }

                return inputModel;
            }
            catch (Exception ex)
            {
                inputModel.TransferId = 0;
                inputModel.IsSuccess = false;

                return inputModel;

                // Debug.WriteLine($"► {ex.GetType().Name} (Catch, ValidateCredit) : {ex.Message}");
                //  throw;
            }
            return null;
        }




    }
}
