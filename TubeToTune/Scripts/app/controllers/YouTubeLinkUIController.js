TubeToTuneApp.controller('YouTubeLinkUIController', function ($scope, $http) {

    var youtubeId = 0;
    $scope.YouTubeLinks = [{ id: youtubeId, link: '' }];

	$scope.AreThereYouTubeLinks = function() {
		for (i = 0; i < $scope.YouTubeLinks.length; i++) {
			if ($scope.YouTubeLinks[i].link != "") {
				return true;
			}
		}
		return false;
	};

    $scope.AddLink = function () {
        youtubeId++;
        var singleYouTubeLink = { id: youtubeId, link: '' };
        $scope.YouTubeLinks.push(singleYouTubeLink);
    };

    $scope.RemoveLink = function (id) {

    	if ($scope.YouTubeLinks.length == 1) {
    		$scope.YouTubeLinks[0].link = '';
    		return;
	    }

        for (i = 0; i < $scope.YouTubeLinks.length; i++) {
            if ($scope.YouTubeLinks[i].id == id) {
                $scope.YouTubeLinks.splice(i, 1);
            }
        }
    };
});