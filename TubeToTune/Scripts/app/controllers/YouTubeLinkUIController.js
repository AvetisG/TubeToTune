TubeToTuneApp.controller('YouTubeLinkUIController', function ($scope, $http) {

    var youtubeId = 0;
    $scope.VideoConversionDetails = [{ VideoId: youtubeId, VideoLink: '' }];

	$scope.AreThereYouTubeLinks = function() {
	    for (i = 0; i < $scope.VideoConversionDetails.length; i++) {
		    if ($scope.VideoConversionDetails[i].VideoLink != "") {
				return true;
			}
		}
		return false;
	};

    $scope.AddLink = function () {
        youtubeId++;
        var singleYouTubeLink = { VideoId: youtubeId, VideoLink: '' };
        $scope.VideoConversionDetails.push(singleYouTubeLink);
    };

    $scope.RemoveLink = function (id) {

        if ($scope.VideoConversionDetails.length == 1) {
    	    $scope.VideoConversionDetails[0].VideoLink = '';
    		return;
	    }

    	for (i = 0; i < $scope.VideoConversionDetails.length; i++) {
            if ($scope.VideoConversionDetails[i].VideoId == id) {
                $scope.VideoConversionDetails.splice(i, 1);
            }
        }
    };
});