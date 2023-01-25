
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
using EPS_Service_API.Model;
using System.Net.Http.Json;

namespace EPS_Service_API.API.BankServices
{
    public class DebitService : IDebitService
    {
        private readonly HttpClient httpClient;

        public DebitService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            //  this.httpClient = httpClientFactory.CreateClient(configuration.GetValue<string>("CurrentBank"));
            //  this.httpClient.DefaultRequestHeaders.Add("x-hash", "50e16bdd9480b5ca1847c7f21d87f7906d4b7f455d45fbd5d75c8bcfccd81f6e");
        }



        //  public async Task<DebitTransactionModel> ValidateDebit(DebitTransactionModel inputModel)
        public async Task<CreditTransactionModel> ValidateDebit(CreditTransactionModel inputModel)
        {
            CreditTransactionModel _objResponseModel = new CreditTransactionModel();
            try
            {
                var GetBankServiceEndPoint = "";
                GetBankServiceEndPoint = "http://localhost:2342/api/Debit/ValidateDebit";


                inputModel.IsSuccess = false;

                // var customeToAccountinfoDebit = db.Customeraccount.Where(x => x.Id == inputModel.ToAccountId).FirstOrDefault();

                CreditTransactionModel BankModel = new CreditTransactionModel
                {
                    Amount = inputModel.Amount,
                    // AccountNo = customeToAccountinfoDebit.AccountNumber,
                    TransactionType = inputModel.TransactionType,
                    Hash = "E7BF382F6E5915B3F88619B866223EBF1D51C4C5321CCCDE2E9FF700A3259086",
                    TransferId = inputModel.TransferId,
                    ToAccountId = inputModel.ToAccountId,
                    FromBankId = inputModel.FromBankId,

                };


                var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync<CreditTransactionModel>(GetBankServiceEndPoint, BankModel);


                //// var response = await httpClient.PostAsJsonAsync<DebitTransactionModel>("http://localhost:5002/api/Debit/ValidateDebit", BankModel);
                //var response = await Policy
                //             .Handle<HttpRequestException>(ex =>
                //             {
                //                 Debug.WriteLine($"► {ex.GetType().Name} (ValidateDebit) : {ex.Message}");
                //                 return true;
                //             })
                //             .WaitAndRetryAsync
                //             (
                //                 2, retryAttempt => TimeSpan.FromSeconds(2)
                //             )
                //             .ExecuteAsync(async () =>
                //                 await httpClient.PostAsJsonAsync<DebitTransactionModel>(GetBankServiceEndPoint, BankModel)
                //             );


                //Transfer transfer = new Transfer
                //{
                //    //FromAccountId = inputModel.FromAccountId,
                //    //Amount = inputModel.Amount,
                //    //ChargeAmount = inputModel.ChargeAmount,
                //    //VatAmount = inputModel.VatAmount,
                //    ToAccountId = inputModel.ToAccountId,
                //    Id = inputModel.TransferId
                //};

                string transactionID = "";
                decimal amount = 0;
                string accountNumber = "";
                string otpReferenceID = "";
                string otp = "";
                string transactionDate = DateTime.Now.ToString();
                int StatusCode = 0;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        using (HttpContent content = response.Content)
                        {
                            string Success_Result = await content.ReadAsStringAsync();
                            dynamic jsonObject = JObject.Parse(Success_Result);

                            transactionID = jsonObject.TransactionID;
                            //amount = jsonObject.amount;
                            //accountNumber = jsonObject.accountNumber;
                            //transactionDate = jsonObject.transactionDate;
                            //otpReferenceID = jsonObject.otpReferenceID;
                            //otp = jsonObject.otp;
                            StatusCode = jsonObject.StatusCode;
                            if (StatusCode == 0)
                            {
                                // inputModel.TransferId = Convert.ToInt32(transactionID);

                                _objResponseModel.IsSuccess = true;
                                _objResponseModel.APIVersion = "0.1";
                                _objResponseModel.TransferId = Convert.ToInt32(transactionID);
                                _objResponseModel.StatusCode = 0;
                                _objResponseModel.ErrorDescription = "Data get successfully";
                                _objResponseModel.Bankresult = Success_Result;

                                return _objResponseModel;
                                //  return Created("Credit", _objResponseModel);
                            }
                            else
                            {
                                //inputModel.TransferId = Convert.ToInt32(transactionID);
                                //inputModel.IsSuccess = false;

                                _objResponseModel.IsSuccess = false;
                                _objResponseModel.APIVersion = "0.1";
                                _objResponseModel.TransferId = Convert.ToInt32(transactionID);
                                _objResponseModel.StatusCode = 1;
                                _objResponseModel.ErrorDescription = "Data get not successfully";
                                _objResponseModel.Bankresult = Success_Result;
                            }
                        }

                        break;
                    case HttpStatusCode.BadRequest:
                        // Handle status
                        //inputModel.TransferId = 0;
                        //inputModel.IsSuccess = false;

                        _objResponseModel.IsSuccess = false;
                        _objResponseModel.APIVersion = "0.1";
                        _objResponseModel.TransferId = Convert.ToInt32(transactionID);
                        _objResponseModel.StatusCode = 1;
                        _objResponseModel.ErrorDescription = "BadRequest";
                        _objResponseModel.Bankresult = "";


                        break;
                    case HttpStatusCode.NotFound:
                        // Handle status
                        //inputModel.TransferId = 0;
                        //inputModel.IsSuccess = false;

                        _objResponseModel.IsSuccess = false;
                        _objResponseModel.APIVersion = "0.1";
                        _objResponseModel.TransferId = Convert.ToInt32(transactionID);
                        _objResponseModel.StatusCode = 1;
                        _objResponseModel.ErrorDescription = "NotFound";
                        _objResponseModel.Bankresult = "";


                        break;
                    case HttpStatusCode.Forbidden:
                        // Handle status
                        //inputModel.TransferId = 0;
                        //inputModel.IsSuccess = false;

                        _objResponseModel.IsSuccess = false;
                        _objResponseModel.APIVersion = "0.1";
                        _objResponseModel.TransferId = Convert.ToInt32(transactionID);
                        _objResponseModel.StatusCode = 1;
                        _objResponseModel.ErrorDescription = "Forbidden";
                        _objResponseModel.Bankresult = "";


                        break;
                    case HttpStatusCode.TooManyRequests:
                        // Handle status
                        //inputModel.TransferId = 0;
                        //inputModel.IsSuccess = false;

                        _objResponseModel.IsSuccess = false;
                        _objResponseModel.APIVersion = "0.1";
                        _objResponseModel.TransferId = Convert.ToInt32(transactionID);
                        _objResponseModel.StatusCode = 1;
                        _objResponseModel.ErrorDescription = "TooManyRequests";
                        _objResponseModel.Bankresult = "";



                        break;
                    case HttpStatusCode.UnavailableForLegalReasons:
                        // Handle status
                        //inputModel.TransferId = 0;
                        //inputModel.IsSuccess = false;

                        _objResponseModel.IsSuccess = false;
                        _objResponseModel.APIVersion = "0.1";
                        _objResponseModel.TransferId = Convert.ToInt32(transactionID);
                        _objResponseModel.StatusCode = 1;
                        _objResponseModel.ErrorDescription = "UnavailableForLegalReasons";
                        _objResponseModel.Bankresult = "";


                        break;
                    case HttpStatusCode.RequestTimeout:
                        // Handle status
                        //inputModel.TransferId = 0;
                        //inputModel.IsSuccess = false;

                        _objResponseModel.IsSuccess = false;
                        _objResponseModel.APIVersion = "0.1";
                        _objResponseModel.TransferId = Convert.ToInt32(transactionID);
                        _objResponseModel.StatusCode = 1;
                        _objResponseModel.ErrorDescription = "RequestTimeout";
                        _objResponseModel.Bankresult = "";

                        break;
                    case HttpStatusCode.InternalServerError:
                        // Handle status
                        //inputModel.TransferId = 0;
                        //inputModel.IsSuccess = false;

                        _objResponseModel.IsSuccess = false;
                        _objResponseModel.APIVersion = "0.1";
                        _objResponseModel.TransferId = Convert.ToInt32(transactionID);
                        _objResponseModel.StatusCode = 1;
                        _objResponseModel.ErrorDescription = "InternalServerError";
                        _objResponseModel.Bankresult = "";

                        break;
                    default:
                        // Handle default case

                        //inputModel.TransferId = 0;
                        //inputModel.IsSuccess = false;

                        _objResponseModel.IsSuccess = false;
                        _objResponseModel.APIVersion = "0.1";
                        _objResponseModel.TransferId = Convert.ToInt32(transactionID);
                        _objResponseModel.StatusCode = 1;
                        _objResponseModel.ErrorDescription = "default";
                        _objResponseModel.Bankresult = "";

                        break;
                }
                return _objResponseModel;

            }
            catch (Exception ex)
            {


                // If Success
                //inputModel.TransferId = 0;
                //inputModel.IsSuccess = false;
                //return inputModel;

                _objResponseModel.IsSuccess = false;
                _objResponseModel.APIVersion = "0.1";
                _objResponseModel.TransferId = 0;
                _objResponseModel.StatusCode = 1;
                _objResponseModel.ErrorDescription = ex.Message.ToString();
                _objResponseModel.Bankresult = "";
                return _objResponseModel;

                //Debug.WriteLine($"► {ex.GetType().Name} (Catch, ValidateDebit) : {ex.Message}");
                //throw;
            }
        }





    }
}