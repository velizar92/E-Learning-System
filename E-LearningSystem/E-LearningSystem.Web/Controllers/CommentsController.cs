namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Comment;
    using E_LearningSystem.Infrastructure.Extensions;

    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;
        private readonly UserManager<User> userManagerService;


        public CommentsController(
            ICommentService commentService,
            UserManager<User> userManagerService)
        {
            this.commentService = commentService;
            this.userManagerService = userManagerService;
        }


        [HttpGet]
        public async Task<IActionResult> CreateComment(int lectureId)
        {
            var user = await userManagerService.GetUserAsync(HttpContext.User);

            return View(new CommentFormModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImageUrl = user.ProfileImageUrl,
                LectureId = lectureId
            });
        }


        [HttpPost]
        public async Task<IActionResult> CreateComment(int id, CommentFormModel commentModel)
        {
            string userId = User.Id();
            var user = await userManagerService.GetUserAsync(HttpContext.User);

            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            ModelState.Remove("ProfileImageUrl");

            commentModel.ProfileImageUrl = user.ProfileImageUrl;
            commentModel.FirstName = user.FirstName;
            commentModel.LastName = user.LastName;

            if (!ModelState.IsValid)
            {
                return View(commentModel);
            }

            int commentId = await this.commentService.CreateComment(commentModel.LectureId, userId, commentModel.Content);

            return RedirectToAction(nameof(AllComments), new { commentModel.LectureId });
        }



        [HttpGet]
        public async Task<IActionResult> EditComment(int commentId)
        {
            var comment = await this.commentService.GetCommentById(commentId);

            return View(comment);
        }


        [HttpPost]
        public async Task<IActionResult> EditComment(int commentId, string content)
        {
            bool isEdited = await this.commentService.EditComment(commentId, content);

            //TO DO Checks...

            return RedirectToAction(nameof(AllComments));
        }



        public async Task<IActionResult> DeleteComment(int commentId)
        {
            bool isDeleted = await this.commentService.DeleteComment(commentId);

            //TO DO Checks...

            return RedirectToAction(nameof(AllComments));
        }



        public async Task<IActionResult> AllComments(int lectureId)
        {
            var allCommentsServiceModel = await this.commentService.GetLectureComments(lectureId);
            List<CommentViewModel> comments = new List<CommentViewModel>();

            foreach (var commentModel in allCommentsServiceModel)
            {
                var user = await userManagerService.FindByIdAsync(commentModel.UserId);

                CommentViewModel commentsViewModel = new CommentViewModel
                {
                    Id = commentModel.Id,
                    LectureId = commentModel.LectureId,
                    Content = commentModel.Content,
                    CreatedOn = commentModel.CreatedOn,
                    UpdatedOn = commentModel.UpdatedOn,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfileImageUrl = user.ProfileImageUrl,           
                };

                comments.Add(commentsViewModel);
            }

            return View(comments);
        }

    }
}
