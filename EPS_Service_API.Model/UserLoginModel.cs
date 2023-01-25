using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EPS_Service_API.Model
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Please enter 'Mobile Number'.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter 'Password'.")]
        public string Password { get; set; }


        // API Related Information 
        public string DeviceTypeID { get; set; }
        public string DeviceID { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceOS { get; set; }
        public string DeviceDetails { get; set; }
        public string LocationLattitude { get; set; }
        public string LocationLongitude { get; set; }
        public string IP_Address { get; set; }

        public string BrowserDetails { get; set; }
        public string ActionMethod { get; set; }
        public string ActionID { get; set; }
        public string UserId { get; set; }
        // API Related Information 
    }


    public class objUserLoginProfileModel
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object UserProfile { set; get; }
        public string Token { set; get; }
        public string TokenExpiration { set; get; }

        //Additional data for Refresh Token
        //    public string RefreshToken { get; set; }
        //    public DateTime RefreshTokenExpiration { get; set; }
        //Additional data for Refresh Token
    }
    public class UserProfile
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string MobileNumber { get; set; }
        public string ProfileImage { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
    }


    //For Refresh token
    

    public class RefreshToken
    {
        [Key]
        public int ID { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
    }


    //For Refresh token

    public class objUserLogoutModel
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public string Message { get; set; }

    }


    public class RefreshToken_2: RefreshToken
    {
        public string ApplicationUserId { get; set; }
    }


    public class objRenewAccessToken
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public string Token { get; set; }

    }


}