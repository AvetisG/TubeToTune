var TubeToTuneApp = angular.module('TubeToTuneApp', ['angular-loading-bar', 'ngRoute']);

TubeToTuneApp.config(function ($routeProvider) {

    $routeProvider
        .when('/',
        {
            templateUrl: '/Navigation/SingleTune'
        })
        .when('/singletune',
        {
            templateUrl: '/Navigation/SingleTune'
        })
        .when('/multipletunes',
        {
            templateUrl: '/Navigation/MultipleTunes'
        })
        .otherwise(
        {
            templateUrl: '/Navigation/ErrorPage'
        });
});

TubeToTuneApp.controller('TuneConvertController', function ($scope, $http) {

    $scope.ConvertToTune = function(youTubeVideoLink) {

        $scope.convertedAudioFilename = "";
        $scope.exceptionMessage = "";
        $scope.successfullyConverted = null;
        
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
                $scope.exceptionMessage = data.Message;
            });
    };
});