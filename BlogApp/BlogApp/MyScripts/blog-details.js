(function ()
{
	var app = angular.module("BlogAppModule");
	app.controller("DeleteController", ["$scope", "$http", function ($scope, $http)
	{
		var self = this;

		/* Delete post function.
		 */
		$scope.DeletePost = function (postId)
		{
			if (confirm("Are you sure you want to delete this post?\nThis action is irreversible."))
			{
				var url = "/Post/Delete?postId=" + postId;
				$http.delete(url).success(function ()
				{
					location.reload();

				}).error(function(data)
				{
					alert("Could not delete post: " + data);
				});
			}
		};
	}]);
}());