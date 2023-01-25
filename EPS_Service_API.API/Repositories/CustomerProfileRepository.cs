using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using EPS_Service_API.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EPS_Service_API.Repositories;
using System;
using EPS_Service_API.API.Data;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using EPS_Service_API.API.V1.Controllers;
using Microsoft.Extensions.Logging;
using System.IO;
//using System.Drawing;
using System.Net;
using SharpCompress.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Amazon.Runtime.Internal.Transform;
using System.Text.Json.Nodes;

namespace EPS_Service_API.API.Repositories
{
    public class CustomerProfileRepository: ICustomerProfileRepository
    {
        private DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        private readonly ILogger<CustomerProfileRepository> _logger;

        public CustomerProfileRepository(DataAccessHelper dataAccessHelper, IConfiguration config, ILogger<CustomerProfileRepository> logger)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
            _logger = logger;
        }

        public async Task<List<CustomerProfileModel>> CustomerProfile_GetAll(int customerID)
        {
            return (await _dataAccessHelper.QueryData<CustomerProfileModel, dynamic>("CustomerProfile_GetAll", new { CustomerID_in = customerID })).ToList();
        }


        public async Task<CxProfile> CustomerProfile_GetByID(int customerID)
        {
            return (await _dataAccessHelper.QueryData<CxProfile, dynamic>("CustomerProfile_GetByID", new { CustomerID_in = customerID })).FirstOrDefault();
        }

        public async Task<CustomerProfileModel> CustomerProfile_GetByNId(string customerNId)
        {
            return (await _dataAccessHelper.QueryData<CustomerProfileModel, dynamic>("CustomerProfile_GetByNId", new { CustomerNId_in = customerNId })).FirstOrDefault();
        }


        public async Task<int> Customer_Update(CustomerProfileModel CP, CxImageURLs cxImgUrl)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Cus_ID", CP.CusId);
            p.Add("Cus_Name", CP.Name);
            p.Add("Cus_Address", CP.Address);
            p.Add("Cus_Email", CP.Email);
            p.Add("Cus_NIDNumber", CP.NIDNumber);
            p.Add("Cus_DOB", CP.DOB);

            if (CP.matched.Length > 1)
            {
                p.Add("matched_pori", CP.matched);
            }

            else
            {
                p.Add("matched_pori", "0");
            }


            if (CP.percentage.Length > 1)
            {
                p.Add("percentage_pori", CP.percentage);
            }

            else
            {
                p.Add("percentage_pori", "0");
            }


            //p.Add("Cus_ProfilePic", CP.Base64ImagePhoto);
            //p.Add("Cus_NIDPic", CP.Base64ImageNID);

            if (cxImgUrl.ProfileImageUrl == null)
            {
                p.Add("Cus_ProfileImage", "default.png");
            }
            else
            {
                p.Add("Cus_ProfileImage", cxImgUrl.ProfileImageUrl);
            }

            if (cxImgUrl.NID_ImageFrontUrl == null)
            {
                p.Add("Cus_NID_Front", "default.png");
            }
            else
            {
                p.Add("Cus_NID_Front", cxImgUrl.NID_ImageFrontUrl);
            }

            if (cxImgUrl.NID_ImageBackUrl == null)
            {
                p.Add("Cus_NID_Back", "default.png");
            }
            else
            {
                p.Add("Cus_NID_Back", cxImgUrl.NID_ImageBackUrl);
            }





            return await _dataAccessHelper.ExecuteData("Customer_Update", p);
        }


        public async Task<int> Customer_Insert(CustomerProfileModel CP)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Cus_Name", CP.Name);
            p.Add("Cus_Address", CP.Address);
            p.Add("Cus_Email", CP.Email);
            p.Add("Cus_NIDNumber", CP.NIDNumber);
            p.Add("Cus_ProfilePic", CP.Base64ImagePhoto);
            p.Add("Cus_NIDPic", CP.Base64ImageNID);

            await _dataAccessHelper.ExecuteData("Customer_Insert", p);
            return p.Get<int>("Id");
        }


        public async Task<int> Porichoy_Customer_Status_Update(string Cus_ID, string Cus_matched, string Cus_percentage)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Cus_ID", Cus_ID);
            p.Add("Cus_matched", Cus_matched);
            p.Add("Cus_percentage", Cus_percentage);

            return await _dataAccessHelper.ExecuteData("Porichoy_Customer_Status_Update", p);
        }


        public async Task<int> EPS_Service_Delete_IncompleteCustomerSU(string Cus_MobileNumber)
        {
            DynamicParameters p = new DynamicParameters();
            
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Cus_MobileNumber", Cus_MobileNumber);

            await _dataAccessHelper.ExecuteData("EPS_Service_Delete_IncompleteCustomerSU", p);
            return p.Get<int>("Id");
        }


        public async Task<int> EPS_Service_SignUp_Customer(string Cus_MobileNumber)
        {
            DynamicParameters p = new DynamicParameters();

            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Cus_MobileNumber", Cus_MobileNumber);

            await _dataAccessHelper.ExecuteData("EPS_Service_SignUp_Customer", p);
            return p.Get<int>("Id");
        }


        public async Task<int> EPS_Service_OTP_Generation(string Cus_MobileNumber)
        {
            try
            {

            
            DynamicParameters p = new DynamicParameters();

            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Cus_MobileNumber", Cus_MobileNumber);

            await _dataAccessHelper.ExecuteData("EPS_Service_OTP_Generation", p);
            return p.Get<int>("Id");
            }
            catch (Exception Ex)
            {
                return 0;
            }
        }

        public async Task<int> EPS_Service_OTP_Verification(string Cus_MobileNumber, string Cus_OTP)
        {
            DynamicParameters p = new DynamicParameters();

            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Cus_MobileNumber", Cus_MobileNumber);
            p.Add("Cus_OTP", Cus_OTP);

            await _dataAccessHelper.ExecuteData("EPS_Service_OTP_Verification", p);
            return p.Get<int>("Id");
        }




        public async Task<ApplicationUser> ApplicationUserInformationGet(string MobileNumber)
        {
            return (await _dataAccessHelper.QueryData<ApplicationUser, dynamic>("EPS_Service_Identity_User_Get", new { MobileNumner_in = MobileNumber })).FirstOrDefault();
        }



        public async Task<List<HelpAndFAQ>> EPS_Common_HelpAndFAQ_Get(int customerID)
        {
            return (await _dataAccessHelper.QueryData<HelpAndFAQ, dynamic>("EPS_Common_HelpAndFAQ_Get", new { CustomerID_in = customerID })).ToList();
        }


        public async Task<List<Support>> EPS_Common_Support_Get(int customerID)
        {
            return (await _dataAccessHelper.QueryData<Support, dynamic>("EPS_Common_Support_Get", new { CustomerID_in = customerID })).ToList();
        }


        public async Task<List<TermsAndCondition>> EPS_Common_TermsAndCondition_Get(int customerID)
        {
            return (await _dataAccessHelper.QueryData<TermsAndCondition, dynamic>("EPS_Common_TermsAndCondition_Get", new { CustomerID_in = customerID })).ToList();
        }


        public async Task<List<limitsettings>> EPS_Common_limitsettings_Get(int customerID)
        {
            return (await _dataAccessHelper.QueryData<limitsettings, dynamic>("EPS_Common_limitsettings_Get", new { CustomerID_in = customerID })).ToList();
        }





        public async Task<CxImageURLs> SaveImagesToFileServer(string Base64PxImage, string Base64NIDImageFront, string Base64NIDImageBack)
        {

            var FolderLoc = _config["ImageUpload:UploadURL_live"];
            //FolderLoc = FolderLoc + @"/Images";

            CxImageURLs cxImageURLs = new CxImageURLs();

            #region Upload Profile Image
            try
            {
                if (Base64PxImage.Length < 6 || Base64PxImage == null)
                {
                    //No Image for Update
                }

                else
                {
                    string datetime = DateTime.Now.ToString("ddMMyyyyHHmm");
                    string datetime_img = DateTime.Now.ToString("ddMMyyyyHHmmss");
                    Random r = new Random();
                    int RandomNumber = r.Next(10000, 99999);

                  //       var encodedImage = "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";
                    var encodedImage = Base64PxImage;
                    var decodedImage = Convert.FromBase64String(encodedImage);
                    
                    var uniqueFileName = Guid.NewGuid().ToString();

                    string filePath = "";
                    string uploadFolder = "";

                    //Upload Original
                    string ServerIP = _config["ImageUpload:ServerIP"];

                    uploadFolder = Path.Combine(_config["ImageUpload:SharedFolder"], "profile_image\\original");
                    filePath = Path.Combine(uploadFolder, uniqueFileName +@".png");

                    using (Image image = Image.Load(decodedImage))
                    {
                        image.Save("\\\\"+ ServerIP + filePath);
                    }

                    //Upload Large
                    uploadFolder = Path.Combine(_config["ImageUpload:SharedFolder"], "profile_image\\large");
                    filePath = Path.Combine(uploadFolder, uniqueFileName + @".png");

                    using (Image image = Image.Load(decodedImage))
                    {
                        int width = image.Width / 2;
                        int height = image.Height / 2;
                        image.Mutate(x => x.Resize(width, height));

                        image.Save("\\\\" + ServerIP + filePath);
                    }


                    //Upload small
                    uploadFolder = Path.Combine(_config["ImageUpload:SharedFolder"], "profile_image\\small");
                    filePath = Path.Combine(uploadFolder, uniqueFileName + @".png");

                    using (Image image = Image.Load(decodedImage))
                    {
                        int width = image.Width / 4;
                        int height = image.Height / 4;
                        image.Mutate(x => x.Resize(width, height));

                        image.Save("\\\\" + ServerIP + filePath);
                    }


                    cxImageURLs.ProfileImageUrl = uniqueFileName + @".png";



                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in Upload Profile Image.");
            }

            #endregion

            #region Upload NID Image Front

            //Upload NID Image Front
            try
            {
                if (Base64NIDImageFront.Length < 6 || Base64NIDImageFront == null)
                {
                    //No Image for Update
                }

                else
                {
                    string datetime = DateTime.Now.ToString("ddMMyyyyHHmm");
                    string datetime_img = DateTime.Now.ToString("ddMMyyyyHHmmss");
                    Random r = new Random();
                    int RandomNumber = r.Next(10000, 99999);

                  //       var encodedImage = "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";
                    var encodedImage = Base64NIDImageFront;
                    var decodedImage = Convert.FromBase64String(encodedImage); 

                    var uniqueFileName = Guid.NewGuid().ToString();

                    string filePath = "";
                    string uploadFolder = "";

                    //Upload Original
                    string ServerIP = _config["ImageUpload:ServerIP"];

                    uploadFolder = Path.Combine(_config["ImageUpload:SharedFolder"], "nid_image\\original");
                    filePath = Path.Combine(uploadFolder, uniqueFileName + @".png");

                    using (Image image = Image.Load(decodedImage))
                    {
                        image.Save("\\\\" + ServerIP + filePath);
                    }

                    //Upload Large
                    uploadFolder = Path.Combine(_config["ImageUpload:SharedFolder"], "nid_image\\large");
                    filePath = Path.Combine(uploadFolder, uniqueFileName + @".png");

                    using (Image image = Image.Load(decodedImage))
                    {
                        int width = image.Width / 2;
                        int height = image.Height / 2;
                        image.Mutate(x => x.Resize(width, height));

                        image.Save("\\\\" + ServerIP + filePath);
                    }


                    //Upload small
                    uploadFolder = Path.Combine(_config["ImageUpload:SharedFolder"], "nid_image\\small");
                    filePath = Path.Combine(uploadFolder, uniqueFileName + @".png");

                    using (Image image = Image.Load(decodedImage))
                    {
                        int width = image.Width / 4;
                        int height = image.Height / 4;
                        image.Mutate(x => x.Resize(width, height));

                        image.Save("\\\\" + ServerIP + filePath);
                    }

                    cxImageURLs.NID_ImageFrontUrl = uniqueFileName + @".png";




                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in NID Image Front.");
            }

            #endregion

            #region Upload NID Image Back

            //Upload NID Image Back
            try
            {
                if (Base64NIDImageBack.Length < 6 || Base64NIDImageBack == null)
                {
                    //No Image for Update
                }

                else
                {
                    string datetime = DateTime.Now.ToString("ddMMyyyyHHmm");
                    string datetime_img = DateTime.Now.ToString("ddMMyyyyHHmmss");
                    Random r = new Random();
                    int RandomNumber = r.Next(10000, 99999);

                  //      var encodedImage = "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";
                    var encodedImage = Base64NIDImageBack;
                    var decodedImage = Convert.FromBase64String(encodedImage);

                    var uniqueFileName = Guid.NewGuid().ToString();

                    string filePath = "";
                    string uploadFolder = "";

                    //Upload Original
                    string ServerIP = _config["ImageUpload:ServerIP"];

                    uploadFolder = Path.Combine(_config["ImageUpload:SharedFolder"], "nid_image\\original");
                    filePath = Path.Combine(uploadFolder, uniqueFileName + @".png");

                    using (Image image = Image.Load(decodedImage))
                    {
                        image.Save("\\\\" + ServerIP + filePath);
                    }

                    //Upload Large
                    uploadFolder = Path.Combine(_config["ImageUpload:SharedFolder"], "nid_image\\large");
                    filePath = Path.Combine(uploadFolder, uniqueFileName + @".png");

                    using (Image image = Image.Load(decodedImage))
                    {
                        int width = image.Width / 2;
                        int height = image.Height / 2;
                        image.Mutate(x => x.Resize(width, height));

                        image.Save("\\\\" + ServerIP + filePath);
                    }


                    //Upload small
                    uploadFolder = Path.Combine(_config["ImageUpload:SharedFolder"], "nid_image\\small");
                    filePath = Path.Combine(uploadFolder, uniqueFileName + @".png");

                    using (Image image = Image.Load(decodedImage))
                    {
                        int width = image.Width / 4;
                        int height = image.Height / 4;
                        image.Mutate(x => x.Resize(width, height));

                        image.Save("\\\\" + ServerIP + filePath);
                    }

                    cxImageURLs.NID_ImageBackUrl = uniqueFileName + @".png";
                  

                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in NID Image Back.");
            }

            #endregion




            // Image process 01..
            //    var PaymentImage = RetailerPayment.PaymentImage;
            // return await Task.FromResult<CxImageURLs>(null);
            return cxImageURLs;
        }




        public async Task<CxImageURLs> ImageUploadToServer(ImageUploadModel _data)
        {
            CxImageURLs cxImageURLs = new CxImageURLs();

            try
            {
                string baseURL = _config["ImageUpload:UploadServer"];

                //var _model = new Dictionary<string, string>();
                //_model.Add("Content-Type", "application/x-www-form-urlencoded");
                //_model.Add("Base64PxImage", _data.Base64PxImage);
                //_model.Add("Base64NIDImageFront", _data.Base64NIDImageFront);
                //_model.Add("Base64NIDImageBack", _data.Base64NIDImageBack);



                //using (var httpClient = new HttpClient())
                //{
                //    using (var request = new HttpRequestMessage(new HttpMethod("POST"), baseURL) { Content = new FormUrlEncodedContent(_model) })
                //    {
                //        var response = await httpClient.SendAsync(request);
                //        if (response.IsSuccessStatusCode == true)
                //        {
                //            using (HttpContent content = response.Content)
                //            {
                //                string Ser_success = await content.ReadAsStringAsync();
                //                dynamic jsonObject = JObject.Parse(Ser_success);
                //                cxImageURLs.ProfileImageUrl = jsonObject.profileImageUrl;
                //                cxImageURLs.NID_ImageFrontUrl = jsonObject.niD_ImageFrontUrl;
                //                cxImageURLs.NID_ImageBackUrl = jsonObject.niD_ImageBackUrl;
                //                return cxImageURLs;
                //            }
                //        }
                //    }
                //}

                using (var httpClient = new HttpClient())
                {

                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), baseURL))
                    {
                        string reqString = JsonConvert.SerializeObject(_data);
                        request.Content = new StringContent(reqString, Encoding.UTF8, "application/json");
                        try
                        {
                            var response = await httpClient.SendAsync(request);
                            if (response.IsSuccessStatusCode == true)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    string Ser_success = await content.ReadAsStringAsync();
                                    dynamic jsonObject = JObject.Parse(Ser_success);
                                    cxImageURLs.ProfileImageUrl = jsonObject.profileImageUrl;
                                    cxImageURLs.NID_ImageFrontUrl = jsonObject.niD_ImageFrontUrl;
                                    cxImageURLs.NID_ImageBackUrl = jsonObject.niD_ImageBackUrl;
                                    return cxImageURLs;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return cxImageURLs;
                        }
                    }
                }




                cxImageURLs.ProfileImageUrl = "";
                cxImageURLs.NID_ImageFrontUrl = "";
                cxImageURLs.NID_ImageBackUrl = "";
                return cxImageURLs;
            }
            catch (Exception e1)
            {
                cxImageURLs.ProfileImageUrl = "";
                cxImageURLs.NID_ImageFrontUrl = "";
                cxImageURLs.NID_ImageBackUrl = "";
                return cxImageURLs;
            }
        }




        public async Task<CxImageURLs> AlternateProcessImageUpload(ImageUploadModel _data)
        {
            CxImageURLs cxImageURLs = new CxImageURLs();

            try
            {
                string baseURL = _config["AlternateImageUpload:API_URL"];
                using (var httpClient = new HttpClient())
                {

                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), baseURL))
                    {
                        string reqString = JsonConvert.SerializeObject(_data);
                        request.Content = new StringContent(reqString, Encoding.UTF8, "application/json");
                        try
                        {
                            var response = await httpClient.SendAsync(request);
                            if (response.IsSuccessStatusCode == true)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    string Ser_success = await content.ReadAsStringAsync();
                                    dynamic jsonObject = JObject.Parse(Ser_success);
                                    cxImageURLs.ProfileImageUrl = jsonObject.ProfileImageUrl;
                                    cxImageURLs.NID_ImageFrontUrl = jsonObject.NID_ImageFrontUrl;
                                    cxImageURLs.NID_ImageBackUrl = jsonObject.NID_ImageBackUrl;
                                    return cxImageURLs;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return cxImageURLs;
                        }
                    }
                }

                return cxImageURLs;

            }
            catch (Exception e1)
            {
                return cxImageURLs;
            }
        }









    }
}
