(function ()
{
	var app = angular.module("BlogAppModule");
	app.controller("CommentController", ["$scope", "$http", function ($scope, $http)
	{
		var self = this;

		$scope.ToggleBox = function (box)
		{
			$scope.box = !$scope.box;
		};

		this.PostCommentUrl = "";
		this.DeleteCommentUrl = "";
				
		this.Submit = function (authorId, postId, body)
		{
			var url = self.PostCommentUrl + "?AuthorId=" + authorId + "&PostId=" + postId + "&Body=" + body;
			$http.post(url, null).success(function (data)
			{
				/* If this is the first comment, remove the text stating there are no comments. */
				$("#noComments").remove();

				var html = "<p></p>" + data.Html;
				$("#showComments-" + postId).prepend(html);
			});
		};

		this.Delete = function (userId, commentId)
		{
			//if (confirm("Are you sure you want to permanently delete this comment?"))
			{
				var url = self.DeleteCommentUrl + "?userId=" + userId + "&commentId=" + commentId;
				$http.delete(url).success(function (data)
				{
					if (data.Success == true)
					{
						$("#" + commentId).remove();
					}
					else
					{
						alert(data.Message);
					}
				});
			}
		};
	}]);
}());