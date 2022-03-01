namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Comment;
    
   
    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;
        private readonly UserManager<User> userManagerService;


        public CommentsController(ICommentService _commentService, UserManager<User> _userManagerService)
        {
            this.commentService = _commentService;
            this.userManagerService = _userManagerService;
        }



        [HttpPost]
        public async Task<IActionResult> CreateComment(int _lectureId, CommentFormModel _commentModel)
        {
            var user = await this.userManagerService.GetUserAsync(HttpContext.User);

            int commentId = await this.commentService.CreateComment(_lectureId, user.Id, _commentModel.Content);

            return RedirectToAction(nameof(AllComments));
        }



        [HttpGet]
        public async Task<IActionResult> EditComment(int _commentId)
        {
            var comment = await this.commentService.GetCommentById(_commentId);
           
            return View(comment);
        }


        [HttpPost]
        public async Task<IActionResult> EditComment(int _commentId, string _content)
        {
            bool isEdited = await this.commentService.EditComment(_commentId, _content);

            //TO DO Checks...

            return RedirectToAction(nameof(AllComments));
        }


        [HttpPost]
        public async Task<IActionResult> DeleteComment(int _commentId)
        {
            bool isDeleted = await this.commentService.DeleteComment(_commentId);

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
