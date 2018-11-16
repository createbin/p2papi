using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Core.Enums;

namespace ZFCTAPI.Data.Customers
{
    public class ChangePasswordRequest
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public bool ValidateRequest { get; set; }
        public PasswordFormat PasswordFormat { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }

        public ChangePasswordRequest(int Id, bool validateRequest, PasswordFormat passwordFormat, string newPassword, string oldPassword = "")
        {
            this.Id = Id;
            this.ValidateRequest = validateRequest;
            this.PasswordFormat = PasswordFormat;
            this.NewPassword = newPassword;
            this.OldPassword = oldPassword;
        }
    }
}