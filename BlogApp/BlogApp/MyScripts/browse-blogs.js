(function ()
{
	var app = angular.module("BlogAppModule");
	app.controller("BrowseController", ["$scope", "$http", function ($scope, $http)
	{
		var self = this;

		this.tags = [];
		this.blogs = [];

		$http.get("/Blogs/GetAllTags").success(function (data)
		{
			self.tags = data;
		});

		$http.get("/Blogs/GetAllBlogs").success(function (data)
		{
			self.blogs = data;
		}).error(function (data, status)
		{
			alert(status + ", " + data);
		});

		this.orderBy = "-Views";	// Default ordering is by views
		this.isReversed = false;	// from most viewed to least

		this.SetOrdering = function (orderBy)
		{
			if (self.orderBy == orderBy)
			{
				self.isReversed = !self.isReversed;
			}
			else
			{
				self.orderBy = orderBy;
				self.isReversed = false;
			}
		};
	}]);
}());