using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Renaissance.Models
{
    public class UserSignIn
    {
        public int userId { get; set; }

        [DisplayName("Username")]
        [Required(ErrorMessage = "*")]
        public string userName { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Password")]
        [Required(ErrorMessage = "*")]
        public string password { get; set; }

        public string GetUserPassword(string loginName)
        {
            using (dbModels db = new dbModels())
            {
                var user = db.Users.Where(o => o.userName.ToLower().Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().password;
                else return string.Empty;
            }
        }

        public int GetUserId(string loginName)
        {
            using (dbModels db = new dbModels())
            {
                var user = db.Users.Where(o => o.userName.ToLower().Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().userId;
                else return 0;
            }
        }

        public string GetUsername(int uid)
        {
            using (dbModels db = new dbModels())
            {
                var user = db.Users.Where(o => o.userId.Equals(uid));
                    return user.FirstOrDefault().userName;
                
            }
        }


    }
}