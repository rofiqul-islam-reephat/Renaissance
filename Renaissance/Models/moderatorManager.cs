using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Renaissance.Models
{
    public class moderatorManager
    {
        public int postId { get; set; }
        public Nullable<bool> status { get; set; }
        public string GetModeratorPassword(string loginName)
        {
            using (dbModels db = new dbModels())
            {
                var user = db.Moderators.Where(o => o.userName.ToLower().Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().password;
                else return string.Empty;
            }
        }
    }
}