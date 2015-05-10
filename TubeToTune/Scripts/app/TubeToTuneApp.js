var TubeToTuneApp = angular.module('TubeToTuneApp', ['angular-loading-bar']);

TubeToTuneApp.controller('TuneConvertController', function ($scope, $http) {

    $scope.ConvertToTune = function(youTubeVideoLink) {

        var data = youTubeVideoLink;
        $scope.successfullyConverted = false;
        $scope.convertedAudioFilename = "";

        $http.post('/api/convert',
            JSON.stringify(data),
            {
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            .success(function(data) {
                if (data == "") {
                    $scope.successfullyConverted = false;
                } else {
                    $scope.successfullyConverted = true;
                    $scope.convertedAudioFilename = data;
                }
            })
            .error(function(data) {
                alert(data);
            });
    };
});