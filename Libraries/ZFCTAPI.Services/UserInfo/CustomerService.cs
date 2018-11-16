using System;
using System.Linq;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.Customers;
using ZFCTAPI.Services.Repositorys;
using ZFCTAPI.Services.Security;
using System.Collections.Generic;
using ZFCTAPI.Core;
using ZFCTAPI.Data.CST;

namespace ZFCTAPI.Services.UserInfo
{
    public partial interface ICustomerService : IRepository<Customer>
    {
        /// <summary>
        /// 通過手机号获取用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Customer GetCustomerByCondition(string username = null, string phone = null, string code = null, string email = null);

        /// <summary>
        /// 插入訪客
        /// </summary>
        /// <returns></returns>
        Customer InsertGuestCustomer();

        /// <summary>
        /// 更改登陆密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PasswordChangeResult ChangeLoginPassword(ChangePasswordRequest request);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request);

        /// <summary>
        /// 企业注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CustomerRegistrationResult RegisterCompany(CustomerRegistrationRequest request);

        CustomerLoginResults ValidateUser(string usernameOrPhone, string password);

        string CreateYqm();

        /// <summary>
        /// 查询用户数量
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<Customer> GetCustomerByParentId(int parentId);
        /// <summary>
        /// 根据用户名或手机号获取用户
        /// </summary>
        /// <param name="phoneOrName"></param>
        /// <returns></returns>
        Customer GetCustomerByPhoneOrName(string phoneOrName);
    }

    public partial class CustomerService : Repository<Customer>, ICustomerService
    {
        private readonly IEncryptionService _encryptionService;

        public CustomerService(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        public Customer GetCustomerByCondition(string username = null, string phone = null, string code = null, string email = null)
        {
            #region 查询条件

            var sqlstence = "1=1";
            if (!string.IsNullOrEmpty(username))
            {
                sqlstence += string.Format(" and (UserName='{0}' or Phone='{0}')", username);
            }
            if (!string.IsNullOrEmpty(phone))
            {
                sqlstence += string.Format(" and Phone='{0}'", phone);
            }
            if (!string.IsNullOrEmpty(code))
            {
                sqlstence += string.Format(" and cst_invitation_code='{0}'", code);
            }
            if (!string.IsNullOrEmpty(email))
            {
                sqlstence += string.Format(" and Email='{0}'", email);
            }

            #endregion 查询条件

            var builder = new StringBuilder();
            builder.Append("select * from customer");
            builder.Append(" where " + sqlstence);
            var result = _Conn.Query<Customer>(builder.ToString()).ToList();
            if (result.Count == 1)
                return result.FirstOrDefault();
            if (result.Count > 1)
                return result.FirstOrDefault(p => !p.IsAdminAccount);
            return null;
        }

        public Customer InsertGuestCustomer()
        {
            var customer = new Customer()
            {
                CustomerGuid = Guid.NewGuid(),
                Active = true,
                CreatedOnUtc = DateTime.Now,
                LastActivityDateUtc = DateTime.Now,
            };
            //add to 'Guests' role
            var guestRole = GetCustomerRoleBySystemName(SystemCustomerRoleNames.Guests);
            if (guestRole == null)
                return null;

            _Conn.Insert<Customer>(customer);
            var customered = _Conn.QueryFirst<Customer>("select * from customer where CustomerGuid='" + customer.CustomerGuid + "'");
            AddCustomerRole(customered.Id, guestRole.Id);
            return customered;
        }

        public CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request)
        {
            var result = new CustomerRegistrationResult();
            var customered = _Conn.QueryFirst<Customer>("select * from customer where CustomerGuid='" + request.Customer.CustomerGuid + "'");
            if (customered == null)
            {
                result.AddError("用户不存在");
                return result;
            }
            customered.Username = request.Username;
            customered.Phone = request.Phone;
            customered.Email = request.Email;
            customered.PasswordFormat = request.PasswordFormat;

            #region 直接使用hash对用户密码进行加密

            string saltKey = _encryptionService.CreateSaltKey(5);
            customered.PasswordSalt = saltKey;
            customered.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey, "SHA1");

            #endregion 直接使用hash对用户密码进行加密

            customered.Active = request.IsApproved;
            customered.cst_invitation_code = request.Customer.cst_invitation_code;
            #region 添加注册用户角色

            var registeredRole = GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered);
            AddCustomerRole(customered.Id, registeredRole.Id);

            #endregion 添加注册用户角色

            #region 如果通过邀请码过来的 cst_parent_id 赋值

            if (!string.IsNullOrEmpty(request.Code))
            {
                var parentCustomer = GetCustomerByCondition(code: request.Code);
                if (parentCustomer != null)
                {
                    customered.cst_parent_id = parentCustomer.Id;
                }
            }

            #endregion 如果通过邀请码过来的 cst_parent_id 赋值

            _Conn.Update<Customer>(customered);
            return result;
        }

        public CustomerRegistrationResult RegisterCompany(CustomerRegistrationRequest request)
        {
            var result = RegisterCustomer(request);
            if (result.Success)
            {
                var customerInfo =_Conn.QueryFirst(
                        "select * from customer where CustomerGuid='" + request.Customer.CustomerGuid + "'");
                //构建实体类
                var cstUser = new CST_user_info
                {
                    cst_customer_id = customerInfo.Id, //Customer表ID
                    cst_user_phone = request.Phone, //手机号
                    cst_user_email = "", //邮箱
                    cst_add_date = DateTime.Now, //添加时间
                    cst_mobile_date = DateTime.Now, //手机验证日期
                    cst_user_name = customerInfo.Username, //用户名
                    cst_user_type = DataDictionary.projecttype_Enterprise //用户类型（企业）
                };
                //Customer添加成功 添加Cst_user_info 数据
                _Conn.Insert<CST_user_info>(cstUser);
                var userInfo = _Conn.QueryFirst<CST_user_info>("select * from CST_user_info where cst_customer_id="+cstUser.cst_customer_id);
                var company = new Cst_Company_Info
                {
                    UserId = userInfo.Id,
                    UserName = "",
                    CompanyName = "",
                    RealName = request.Username,
                    InstuCode = "",
                    BusiCode = "",
                    TaxCode = "",
                    AuditState = 1,
                    IsDelete = false,
                    ContactPhone = request.Customer.Phone,
                    ContactUser = request.CompanyRegisterModel.ContactUser,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                if (request.CompanyRegisterModel.IsOne)
                {
                    company.InstuCode = request.CompanyRegisterModel.SocialCredit;
                }
                else
                {
                    company.InstuCode = request.CompanyRegisterModel.OrganizationCode;
                    company.BusiCode = request.CompanyRegisterModel.BusinessLicense;
                    company.TaxCode = request.CompanyRegisterModel.TaxCode;
                }
                _Conn.Insert<Cst_Company_Info>(company);
                var companyInfo= _Conn.QueryFirst<Cst_Company_Info>("select * from Cst_Company_Info where UserId=" + userInfo.Id);
                //添加accountInfo数据
                var account = new CST_account_info
                {
                    act_user_id = userInfo.Id,
                    act_user_type = DataDictionary.projecttype_Enterprise, //用户类型（个人）
                    act_user_phone = userInfo.cst_user_phone,
                    invest_platform_id = CommonHelper.CreatePlatFormId(userInfo.Id,UserAttributes.Invester),
                    financing_platform_id = CommonHelper.CreatePlatFormId(userInfo.Id,UserAttributes.Financer),
                    JieSuan = false,
                    BoHai=false,
                };
                _Conn.Insert<CST_account_info>(account);
                var accountInfo = _Conn.QueryFirst<CST_account_info>("select * from CST_account_info where act_user_id=" + userInfo.Id);
                userInfo.CompanyId = companyInfo.Id;
                userInfo.cst_account_id = accountInfo.Id;
                _Conn.Update<CST_user_info>(userInfo);
            }
            return result;
        }

        public CustomerLoginResults ValidateUser(string usernameOrPhone, string password)
        {
            var customer = GetCustomerByPhoneOrName(usernameOrPhone);
            if (customer == null)
                return CustomerLoginResults.CustomerNotExist;
            if (customer.Deleted)
                return CustomerLoginResults.Deleted;
            if (!customer.Active)
                return CustomerLoginResults.NotActive;
            //only registered can login
            if (!IsRegistered(customer))
                return CustomerLoginResults.NotRegistered;
            var pwd = _encryptionService.CreatePasswordHash(password, customer.PasswordSalt, CustomerSettings.HashedPasswordFormat);
            var isValid = pwd == customer.Password;
            if (isValid)
            {
                customer.LastLoginDateUtc = DateTime.Now;
                Update(customer);
                return CustomerLoginResults.Successful;
            }
            else
                return CustomerLoginResults.WrongPassword;
        }


        public string CreateYqm()
        {
            var flag = true;
            string yqm = string.Empty;
            while (flag)
            {
                yqm = ProjectHelper.GenerateCheckCode(8);
                var customer = _Conn.QueryFirstOrDefault("select * from customer where cst_invitation_code=@YQM", new { YQM = yqm });
                if (customer == null)
                {
                    flag = false;
                }
            }
            return yqm;
        }

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="systemName">Customer role system name</param>
        /// <returns>Customer role</returns>
        public virtual CustomerRole GetCustomerRoleBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;
            return _Conn.QueryFirstOrDefault<CustomerRole>("select * from CustomerRole where SystemName='" + systemName + "'");
        }

        public void AddCustomerRole(int customerId, int roleId)
        {
            var customerRole = new Customer_CustomerRole_Mapping
            {
                Customer_Id = customerId,
                CustomerRole_Id = roleId
            };
            _Conn.Insert<Customer_CustomerRole_Mapping>(customerRole);
        }

        public bool IsRegistered(Customer customer)
        {
            var role = GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered);
            var sqlStence = new StringBuilder();
            sqlStence.Append("select * from Customer_CustomerRole_Mapping");
            sqlStence.Append(" where Customer_Id='" + customer.Id + "'");
            sqlStence.Append(" and CustomerRole_Id='" + role.Id + "'");
            var isInRole = _Conn.QueryFirstOrDefault<Customer_CustomerRole_Mapping>(sqlStence.ToString());
            return isInRole != null;
        }

        public PasswordChangeResult ChangeLoginPassword(ChangePasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var result = new PasswordChangeResult();

            //判断新密码是否为空
            if (String.IsNullOrWhiteSpace(request.NewPassword))
            {
                result.AddError("新密码不能为空！");
                return result;
            }

            var customer = new Customer();
            customer = Find(request.Id);

            //根据用户获取
            if (customer == null)
            {
                result.AddError("用户不存在！");//用户不存在
                return result;
            }

            var requestIsValid = false;
            if (request.ValidateRequest)
            {
                string oldPwd = _encryptionService.CreatePasswordHash(request.OldPassword, customer.PasswordSalt, "SHA1");
                bool oldPasswordIsValid = oldPwd == customer.Password;
                if (!oldPasswordIsValid)
                {
                    result.AddError("旧密码不正确！");
                    return result;
                }
                if (oldPasswordIsValid)
                    requestIsValid = true;
            }
            else
                requestIsValid = true;

            if (requestIsValid)
            {
                string saltKey = _encryptionService.CreateSaltKey(5);
                customer.PasswordSalt = saltKey;
                customer.Password = _encryptionService.CreatePasswordHash(request.NewPassword, saltKey, "SHA1");
            }
            customer.PasswordFormat = request.PasswordFormat;
            Update(customer);
            return result;
        }

        public List<Customer> GetCustomerByParentId(int parentId)
        {
            var sqlStr = "SELECT * FROM Customer WHERE cst_parent_id="+parentId;

            return _Conn.Query<Customer>(sqlStr).ToList();
        }

        public Customer GetCustomerByPhoneOrName(string phoneOrName)
        {
            return _Conn.QueryFirstOrDefault<Customer>("select * from Customer where (username=@usernameOrPhone or phone=@usernameOrPhone) AND IsAdminAccount=0 ", new { usernameOrPhone = phoneOrName });
        }
    }
}