using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace EPS_Service_API.Model
{

    public class objUserProfile
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object CxProfile { set; get; }

    }

    public class CustomerProfileModel
    {
        public int CusId { get; set; }

        [Required(ErrorMessage = "Please enter 'Name'.")]
        [MaxLength(150, ErrorMessage = "Maximum length of Name is 150 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter 'Address'.")]
        [MaxLength(150, ErrorMessage = "Maximum length of Name is 500 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter 'Email'.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Not Valid Email Address!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter 'NIDNumber'.")]
        [MaxLength(25, ErrorMessage = "Maximum length of Name is 25 characters.")]
        public string NIDNumber { get; set; }

        public string DOB { get; set; } //format 2000-11-30

        public string matched { get; set; } //from Porichoy API

        public string percentage { get; set; } //from Porichoy API

        [Required(ErrorMessage = "Please Upload Front Image of 'NID'.")]
        public string Base64ImageNID { get; set; }

        [Required(ErrorMessage = "Please Upload Back Image of 'NID'.")]
        public string Base64ImageNID_Back { get; set; }

        [Required(ErrorMessage = "Please Upload 'Personal Image'.")]
        public string Base64ImagePhoto { get; set; }

        //public IFormFile ProfileImage { get; set; }
        //public IFormFile ProfileImage { get; set; }
        //public IFormFile NID_Image { get; set; }

        // API Related Information 
        public string DeviceID { get; set; }

        // API Related Information 
    }

    public class CxProfile
    {
        public int CusId { get; set; }

        [Required(ErrorMessage = "Please enter 'Name'.")]
        [MaxLength(150, ErrorMessage = "Maximum length of Name is 150 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter 'Address'.")]
        [MaxLength(150, ErrorMessage = "Maximum length of Name is 500 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter 'Email'.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Not Valid Email Address!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter 'NIDNumber'.")]
        [MaxLength(25, ErrorMessage = "Maximum length of Name is 25 characters.")]
        public string NIDNumber { get; set; }

        public string DOB { get; set; }

        //[Required(ErrorMessage = "Please Upload Image of 'NID'.")]
        //public string Base64ImageNID { get; set; }

        //[Required(ErrorMessage = "Please Upload 'Personal Image'.")]
        //public string Base64ImagePhoto { get; set; }
        public string ProfileImage { get; set; }
        public string NID_Front { get; set; }
        public string NID_Back { get; set; }


        public bool IsActive { get; set; }



    }


    public class ImageUploadModel
    {
        public string Base64PxImage { get; set; }
        public string Base64NIDImageFront { get; set; }
        public string Base64NIDImageBack { get; set; }
    }



    public class objStatusOfOperation
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }

        public bool IsActive { get; set; }

    }

    public class GetCustomerProfileEntity
    {
        [Required(ErrorMessage = "Please enter 'CustomerID'.")]
        //[MinLength(1, ErrorMessage = "Maximum length of CustomerID is 1 characters.")]
        //[MaxLength(40, ErrorMessage = "Maximum length of CustomerID is 40 characters.")]
        [Range(1,2147483647)]
        public int CusId { get; set; }


        // API Related Information 
        [Required(ErrorMessage = "Please enter 'DeviceID'.")]
        [MinLength(1, ErrorMessage = "Minimum length of DeviceID is 1 characters.")]
        [MaxLength(150, ErrorMessage = "Maximum length of DeviceID is 150 characters.")]
        public string DeviceID { get; set; }

        // API Related Information 

    }



    public class CxImageURLs
    {
        public string ProfileImageUrl { get; set; }     
        public string NID_ImageFrontUrl { get; set; }
        public string NID_ImageBackUrl { get; set; }

    }



}
