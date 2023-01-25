using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPS_Service_API.API.Data;
using EPS_Service_API.Model;
using Microsoft.AspNetCore.Identity;

namespace EPS_Service_API.API.Repositories
{
    public interface ICustomerProfileRepository
    {
        Task<List<CustomerProfileModel>> CustomerProfile_GetAll(int customerID);
        Task<CxProfile> CustomerProfile_GetByID(int customerID);
        Task<CustomerProfileModel> CustomerProfile_GetByNId(string customerNId);
        Task<int> Customer_Update(CustomerProfileModel CP, CxImageURLs cxImgUrl);
        Task<int> Customer_Insert(CustomerProfileModel CP);

        Task<int> Porichoy_Customer_Status_Update(string Cus_ID, string Cus_matched, string Cus_percentage);


        Task<int> EPS_Service_Delete_IncompleteCustomerSU(string Cus_MobileNumber);
        Task<int> EPS_Service_SignUp_Customer(string Cus_MobileNumber);
        Task<int> EPS_Service_OTP_Generation(string Cus_MobileNumber);
        Task<int> EPS_Service_OTP_Verification(string Cus_MobileNumber, string Cus_OTP);


        Task<ApplicationUser> ApplicationUserInformationGet(string MobileNumber);




        Task<List<HelpAndFAQ>> EPS_Common_HelpAndFAQ_Get(int customerID);
        Task<List<Support>> EPS_Common_Support_Get(int customerID);
        Task<List<TermsAndCondition>> EPS_Common_TermsAndCondition_Get(int customerID);
        Task<List<limitsettings>> EPS_Common_limitsettings_Get(int customerID);


        Task<CxImageURLs> SaveImagesToFileServer(string Base64PxImage, string Base64NIDImageFront, string Base64NIDImageBack);
        Task<CxImageURLs> AlternateProcessImageUpload(ImageUploadModel _data);

        Task<CxImageURLs> ImageUploadToServer(ImageUploadModel _data);



    }
}
