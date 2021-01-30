using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Renaissance.Models;


namespace Renaissance.Controllers
{
    public class HomeController : Controller
    {
        int currentpost;
        public ActionResult Index()
        {
            if (Session["userName"] == null)
                return View();
            return RedirectToAction("Homepage", "Home");
        }

        [Route("home/signin")]
        public ActionResult SignIn()
        {
            if (Session["userName"] == null)
                   return View();
            return RedirectToAction("Homepage", "Home");
        }

        [HttpPost]
        public ActionResult SignIn(UserSignIn usi, string returnUrl)
        {
            if (Session["userName"]==null)
            {
                if (ModelState.IsValid)
                {
                    UserSignIn UM = new UserSignIn();
                    moderatorManager mm = new moderatorManager();

                    string password = UM.GetUserPassword(usi.userName);
                    string password2 = mm.GetModeratorPassword(usi.userName);

                    if (string.IsNullOrEmpty(password) && string.IsNullOrEmpty(password2))
                        ViewBag.noPasswordMessage = "The user login or password provided is incorrect.";
                    else if(usi.password.Equals(password))
                    {
                       
                            FormsAuthentication.SetAuthCookie(usi.userName, true);
                            Session["userName"] = usi.userName.ToString();
                            return RedirectToAction("Homepage", "Home", usi.userName);
                       
                       
                    }
                    else if(usi.password.Equals(password2))
                    {
                        FormsAuthentication.SetAuthCookie(usi.userName, true);
                        Session["userName"] = usi.userName.ToString();
                        return RedirectToAction("AdminHomepage", "Home", usi.userName);
                    }
                    else
                    {
                        ViewBag.wrongPasswordMessage = "The password provided is incorrect.";
                    }
                }
                return View(usi);
            }
            return RedirectToAction("Homepage", "Home");

            // If we got this far, something failed, redisplay form return View(ULV);
        }

        //[Authorize]
        public ActionResult SignOut()
        {
            Session["userName"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Homepage()
        {
            if(Session["userName"] != null)
            {
                using (dbModels dbModel = new dbModels())
                {
                    return View(dbModel.Posts.ToList().Where(x=>x.status==true));
                }
            }
            else
                return RedirectToAction("SignIn", "Home");

        }
        
        public ActionResult MakePost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MakePost(Post post)
        {

            if (Session["userName"] != null)
            {
                using (dbModels dbModel = new dbModels())
                {
                    UserSignIn UM = new UserSignIn();
                    int uid = UM.GetUserId(Session["userName"].ToString());
                    post.userId = uid;
                    post.creationDate = DateTime.Now;

                    ViewBag.SuccessMessage = "Post Added";
                    dbModel.Posts.Add(post);
                    dbModel.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.SuccessMessage = "Post Added";
                return View("MakePost", new Post());

            }
            return RedirectToAction("SignIn", "Home");

        }

        
         public ActionResult PostDetails(int id=0)
         {
            if (Session["userName"] != null)
            {
                using (dbModels dbModel = new dbModels())
                {
                    PostView p = new PostView();
                    p.Posts  = dbModel.Posts.SingleOrDefault(x => x.postId == id);
                    UserSignIn u = new UserSignIn();
                    ViewBag.postdetails = p.Posts.body;
                    ViewBag.creationDate = p.Posts.creationDate;
                    ViewBag.title = p.Posts.title;
                    ViewBag.postId = p.Posts.postId;
                    ViewBag.bloggerName = u.GetUsername(p.Posts.userId);
                    //currentpost = ViewBag.postdetails.postId;
                    return View();
                     
                }
            }
            return RedirectToAction("SignIn", "Home");
        }

        [HttpPost]
        public ActionResult PostComment( PostView pv, int id)
        {

            using (dbModels dbModel = new dbModels())
            {
                UserSignIn UM = new UserSignIn();
                Comment cm = new Comment();
                int uid = UM.GetUserId(Session["userName"].ToString());
                cm.userId = uid;
                cm.PostId = id;
                cm.creationDate = DateTime.Now;
                cm.commentBody = pv.CommentBody;
                //ViewBag.postId = id;
                ViewBag.SuccessComment = "Comment Successful.";
                dbModel.Comments.Add(cm);
                dbModel.SaveChanges();
            }
            ModelState.Clear();
            return RedirectToAction("Homepage", "Home");

        }



        public ActionResult AdminHomepage()
        {
            if (Session["userName"] != null)
            {
                using (dbModels dbModel = new dbModels())
                {
                    return View(dbModel.Posts.ToList().Where(x=>x.status==null));
                }
            }
            else
                return RedirectToAction("SignIn", "Home");
        }

        public ActionResult Adminpostmanage(int id=0)
        {
            if (Session["userName"] != null)
            {
                using (dbModels dbModel = new dbModels())
                {
                    Post post = dbModel.Posts.SingleOrDefault(x => x.postId == id);
                    UserSignIn u = new UserSignIn();
                    ViewBag.postdetails = post;
                    ViewBag.bloggerName = u.GetUsername(ViewBag.postdetails.userId);
                    currentpost = ViewBag.postdetails.postId;
                    return View(post);
                }
            }
            return RedirectToAction("SignIn", "Home");
        }

        public ActionResult PostApproved(int id=0)
        {
            using (dbModels dbModel = new dbModels())
            {
               
                var newpost = dbModel.Posts.Find(id);

                if (TryUpdateModel(newpost))
                {
                    newpost.status = true;
                    dbModel.SaveChanges();
                    ViewBag.updatesuccess = "Update Successful";
                    return RedirectToAction("AdminHomepage", "Home");
                }
            }
            return RedirectToAction("AdminHomepage", "Home");
        }

        public ActionResult PostDisApproved(int id = 0)
        {
            using (dbModels dbModel = new dbModels())
            {

                var newpost = dbModel.Posts.Find(id);

                if (TryUpdateModel(newpost))
                {
                    newpost.status = false;
                    dbModel.SaveChanges();
                    ViewBag.updatesuccess = "Update Successful";
                    return RedirectToAction("AdminHomepage", "Home");
                }
            }
            return RedirectToAction("AdminHomepage", "Home");
        }

      
        public ActionResult Contact()
        {
            ViewBag.themessage = "having troube? send us a message";
            return View();
        }
        [HttpPost]
        public ActionResult Contact(string message)
        {
            ViewBag.themessage = "thanks we got the message";
            return View();
        }

    }
}