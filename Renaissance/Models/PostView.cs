using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Renaissance.Models
{
    public class PostView
    {
        public string CommentBody { get; set; }

        public Post Posts { get; set; }

        public Comment comments { get; set; }

        public virtual List<Comment> Comments { get; set; }
    }
}