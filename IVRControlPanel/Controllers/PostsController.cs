using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IVRControlPanel.Models;
using IVRControlPanel.Helpers;
using System.Text;

namespace IVRControlPanel.Controllers
{
    public class PostsController : Controller
    {
        private IVRControlPanelEntities model = new IVRControlPanelEntities();
        public IVRControlPanelRepository repository = new IVRControlPanelRepository();

        private const int PostsPerPage = 4;
        public ActionResult Index(int? id)
        {
            int pageNumber = id ?? 0;
            IEnumerable<Post> posts =
                (from post in model.Posts
                 where post.DateTime < DateTime.Now
                 orderby post.DateTime descending
                 select post).Skip(pageNumber * PostsPerPage).Take(PostsPerPage + 1);
            ViewBag.IsPreviousLinkVisible = pageNumber > 0;
            ViewBag.IsNextLinkVisible = posts.Count() > PostsPerPage;
            ViewBag.PageNumber = pageNumber;
            ViewBag.IsAdmin = IsAdmin;
            ViewData["CurrentPage"] = "Post";
            return View(posts.Take(PostsPerPage));
        }

        public ActionResult Details(int id)
        {
            Post post = GetPost(id);
            ViewBag.IsAdmin = IsAdmin;
            return View(post);
        }

        /* [ValidateInput(false)]
          [Authorize]
          public ActionResult Comment(int id, string name, string email, string body)
          {
              Post post = GetPost(id);
              Comment comment = new Comment();
              User CurrentUser = repository.GetUser(User.Identity.Name);
              comment.UserID = CurrentUser.ID;
              comment.Post = post;
              comment.DateTime = DateTime.Now;
              comment.Name = User.Identity.Name;
              comment.Email = email;
              comment.Body = body;
              model.AddToComments(comment);
              model.SaveChanges();
              return RedirectToAction("Details", new { id = id });

          } */

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Comment(int id, string email, string body)
        {
            if (Request.IsAuthenticated)
            {
                Post post = GetPost(id);
                Comment comment = new Comment();
                User CurrentUser = repository.GetUser(User.Identity.Name);
                comment.UserID = CurrentUser.ID;
                comment.Post = post;
                comment.DateTime = DateTime.Now;
                comment.Name = User.Identity.Name;
                comment.Email = email;
                comment.Body = body;
                model.AddToComments(comment);
                model.SaveChanges();
                return RedirectToAction("Details", new { id = id });
            }

            return RedirectToAction("LogOn", "Account", new { ReturnUrl = Url.Action("Comment", "Posts", new { id = id }) });
        }

        public ActionResult Comment(int id)
        {
            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult Tags(string id)
        {
            Tag tag = GetTag(id);
            ViewBag.IsAdmin = IsAdmin;
            return View("Index", tag.Posts);
        }

        [ValidateInput(false)]
        public ActionResult Update(int? id, string title, string body, string tags)
        {
            if (!IsAdmin)
            {
                return RedirectToAction("Index");
            }

            User CurrentUser = repository.GetUser(User.Identity.Name);
            int userid = CurrentUser.ID;

            Post post = GetPost(id);
            post.Title = title;
            post.Body = body;
            post.DateTime = DateTime.Now;
            post.UserID = userid;
            post.Tags.Clear();

            tags = tags ?? string.Empty;
            string[] tagNames = tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tagName in tagNames)
            {
                post.Tags.Add(GetTag(tagName));
            }
            if (!id.HasValue)
            {
                model.AddToPosts(post);
            }
            model.SaveChanges();
            return RedirectToAction("Details", new { id = post.ID });
        }

        public ActionResult Delete(int id)
        {
            if (IsAdmin)
            {
                Post post = GetPost(id);
                model.DeleteObject(post);
                model.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteComment(int id)
        {
            if (IsAdmin)
            {
                Comment comment = model.Comments.Where(x => x.ID == id).First();
                int PostID = comment.PostID;
                model.DeleteObject(comment);
                model.SaveChanges();
                return RedirectToAction("Details", new { id = PostID });

            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int? id)
        {
            Post post = GetPost(id);
            StringBuilder tagList = new StringBuilder();
            foreach (Tag tag in post.Tags)
            {
                tagList.AppendFormat("{0} ", tag.Name);
            }
            ViewBag.Tags = tagList.ToString();
            return View(post);
        }
        private Tag GetTag(string tagName)
        {
            return model.Tags.Where(x => x.Name == tagName).FirstOrDefault() ?? new Tag() { Name = tagName };
        }

        private Post GetPost(int? id)
        {
            return id.HasValue ? model.Posts.Where(x => x.ID == id).First() : new Post() { ID = -1 };
        }

        public IVRControlPanelRoleProvider roleModel = new IVRControlPanelRoleProvider();

        public bool IsAdmin
        {
            get
            {
                return roleModel.IsUserInRole(User.Identity.Name, "administrator");
            }
        }


    }
}
