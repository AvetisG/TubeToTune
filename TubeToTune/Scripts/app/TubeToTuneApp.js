var TubeToTuneApp = angular.module('TubeToTuneApp', ['angular-loading-bar', 'ngRoute']);

TubeToTuneApp.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/', {
            templateUrl: '/Home/Conversion',
        }).
        otherwise({
            templateUrl: '/Home/ErrorPage'
        });
  }]);

TubeToTuneApp.controller('TuneConvertController', function ($scope, $http) {

    $scope.YouTubeLinks = [{ link: '' }];

    $scope.ConvertToTune = function (youTubeVideoLink) {

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

    $scope.AddOneMoreLink = function() {
        var singleYouTubeLink = {link: ''};
        $scope.YouTubeLinks.push(singleYouTubeLink);
    };
});