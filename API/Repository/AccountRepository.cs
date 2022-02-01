using API.Context;
using API.Models;
using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace API.Repository
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext myContext;
        public AccountRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }

        public LoginDataVM TokenPayload(string email)
        {
            var query = (from employee in myContext.Set<Employee>()
                         where employee.Email == email
                         join account in myContext.Set<Account>()
                             on employee.NIK equals account.NIK
                         join accountRole in myContext.Set<AccountRole>()
                            on account.NIK equals accountRole.AccountId
                         join role in myContext.Set<Role>()
                            on accountRole.RoleId equals role.Id
                         select new
                         {
                             role.Name
                         }).ToList();

            List<string> roles = new List<string>();

            foreach (var item in query)
            {
                roles.Add(item.Name);
            }

            LoginDataVM payload = new LoginDataVM
            {
                Email = email,
                Roles = roles
            };

            return payload;
        }

        public int Login(string email, string password)
        {
            var checkData = (from employee in myContext.Set<Employee>()
                            where employee.Email == email
                            join account in myContext.Set<Account>()
                                on employee.NIK equals account.NIK
                            select new
                            {
                                employee.NIK,
                                employee.Email,
                                account.Password
                            }).FirstOrDefault();
            //check email di tabel employee dan nik di tabel account
            if (checkData == null)
            {
                return 2; //Akun tidak ditemukan
            }

            // check a password
            bool validPassword = BCrypt.Net.BCrypt.Verify(password, checkData.Password);
            if (!validPassword)
            {
                return 3; //Pasword salah
            }
            else
            { 
                return 1; //Berhasil login
            }
        }

        public int ForgotPassword(string email)
        {
            var checkData = (from employee in myContext.Set<Employee>()
                             where employee.Email == email
                             join account in myContext.Set<Account>()
                                 on employee.NIK equals account.NIK
                             select new
                             {
                                 employee.NIK,
                                 employee.Email,
                                 account.Password
                             }).FirstOrDefault();

            if (checkData == null)
            {
                return 2;
            }

            var thisAccount = myContext.Accounts.Where(a => a.NIK == checkData.NIK).FirstOrDefault();
            Random rd = new Random();
            thisAccount.OTP = rd.Next(100000, 999999);
            thisAccount.IsUsed = false;
            thisAccount.ExpiredToken = DateTime.Now.AddMinutes(5);

            try
            {
                myContext.SaveChanges();
                SendOTPEmail(email, thisAccount.OTP, thisAccount.ExpiredToken);
            }
            catch (Exception)
            {
                return 3;
            }

            return thisAccount.OTP;
        }

        public int SendOTPEmail(string email, int otp, DateTime expiredToken)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Admin", "strway001@gmail.com");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress("User", email);
            message.To.Add(to);

            message.Subject = "Forgot Password - OTP Request";
            BodyBuilder bodyBuilder = new BodyBuilder();
            //bodyBuilder.HtmlBody = $"<h1>{otp.ToString()}!</h1>";
            bodyBuilder.TextBody = 
                $"Your OTP {otp.ToString()}, please use your OTP to change your password at" +
                $" https://localhost:44367/api/accounts/changepassword/ and provide email, OTP, " +
                $"and NewPassword, please use your OTP before {expiredToken}";
            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            client.Authenticate("strway001@gmail.com", "bismillah99!");

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();

            return 1;
        }

        public int ChangePassword(string email, int otp, string newPassword)
        {
            var checkData = (from e in myContext.Set<Employee>()
                             where e.Email == email
                             join a in myContext.Set<Account>()
                                 on e.NIK equals a.NIK
                             select new
                             {
                                 a.NIK
                             }).FirstOrDefault();
            if (checkData == null)
            {
                return 2; // Account not found
            }
            var account = myContext.Accounts.Find(checkData.NIK);

            if (account.OTP != otp)
            {
                return 3; // Invalid OTP
            }
            if (DateTime.Now > account.ExpiredToken)
            {
                return 4; //OTP Expired
            }
            if (account.IsUsed)
            {
                return 5; //OTP Already been used
            }
            account.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            account.IsUsed = true;
            int update;
            try
            {
               update = Update(account);
            }
            catch (Exception)
            {
                return 7; // Update error
            }
            if (update != 1)
            {
                return 6; //update failed
            }
            return 1; //success
        }


    }
}
