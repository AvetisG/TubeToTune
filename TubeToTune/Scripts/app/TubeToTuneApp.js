var TubeToTuneApp = angular.module('TubeToTuneApp', ['angular-loading-bar']);

TubeToTuneApp.controller('TuneConvertController', function ($scope, $http) {

    $scope.ConvertToTune = function(youTubeVideoLink) {

        $scope.convertedAudioFilename = "";
        $scope.exceptionMessage = "";
        $scope.successfullyConverted = null;

        $http.defaults.headers.post["Content-Type"] = "application/x-www-form-urlencoded";
        
        $http.post('/api/convert',
            JSON.stringify(youTubeVideoLink),
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
                $scope.exceptionMessage = data.ExceptionMessage;
            });
    };
});