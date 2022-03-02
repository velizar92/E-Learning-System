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


        public CommentsController(ICommentService commentService, UserManager<User> userManagerService)
        {
            this.commentService = commentService;
            this.userManagerService = userManagerService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateComment(int lectureId, CommentFormModel commentModel)
        {
            string userId = User.Id();

            int commentId = await this.commentService.CreateComment(lectureId, userId, commentModel.Content);

            return RedirectToAction(nameof(AllComments));
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


        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            bool isDeleted = await this.commentService.DeleteComment(commentId);

            //TO DO Checks...

            return RedirectToAction(nameof(AllComments));
        }



        public async Task<IActionResult> AllComments(int _lectureId) 
        {
            var allComments = await this.commentService.GetLectureComments(_lectureId);

            return View(allComments);
        }

    }
}
