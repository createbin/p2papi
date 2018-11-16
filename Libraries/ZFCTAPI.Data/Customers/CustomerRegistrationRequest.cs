using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Data.ApiModels.SubmitModels;

namespace ZFCTAPI.Data.Customers
{
    public class CustomerRegistrationRequest
    {
        public Customer Customer { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        public string Phone { get; set; }
        public string Password { get; set; }
        public PasswordFormat PasswordFormat { get; set; }
        public bool IsApproved { get; set; }

        public string Code { get; set; }
        public SCompanyRegisterModel CompanyRegisterModel { get; set; }

        public CustomerRegistrationRequest(Customer customer, string email, string username, string phone,
            string password,
            PasswordFormat passwordFormat,
            string code, SCompanyRegisterModel companyRegisterModel,
            bool isApproved = true)
        {
            this.Customer = customer;
            this.Email = email;
            this.Username = username;
            this.Password = password;
            this.PasswordFormat = passwordFormat;
            this.IsApproved = isApproved;
            this.Phone = phone;
            this.Code = code;
            this.CompanyRegisterModel = companyRegisterModel;
        }
    }
}
