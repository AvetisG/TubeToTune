var TubeToTuneApp = angular.module('TubeToTuneApp', ['angular-loading-bar'])

TubeToTuneApp.controller('TuneConvertController', function ($scope, $http) {
    
    $scope.ConvertToTune = function (youTubeVideoLink) {

        var data = { "link": youTubeVideoLink };
        $scope.successfullyConverted = false;
        $scope.convertedYouTubeAudioPath = "";

        $http.post('/api/tubetotune',
                JSON.stringify(data),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
           .success(function (data) {
           		if (data == "") {
					$scope.successfullyConverted = false;
				} else {
           			$scope.successfullyConverted = true;
			        $scope.convertedYouTubeAudioPath = data;
		        }
	        })
           .error(function (data) {
               alert(data);
           })
    }
});