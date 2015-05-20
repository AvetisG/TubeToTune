TubeToTuneApp.controller('ConvertController', function ($scope, $http) {

    $scope.ConvertToTunes = function (youtubeVideoLinks) {

        $scope.convertedAudioFilename = "";
        $scope.exceptionMessage = "";
        $scope.successfullyConverted = null;

	    var links = [];

	    for (i = 0; i < youtubeVideoLinks.length; i++) {
	    	links.push(youtubeVideoLinks[i].link);
        }

        $http.post('/api/convert',
            JSON.stringify(links),
            {
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            .success(function(data) {     
                $scope.successfullyConverted = true;
                $scope.convertedAudioFilename = data;
                
            })
            .error(function (data) {
                $scope.successfullyConverted = false;
                $scope.exceptionMessage = data.Message;
            });
    };
});