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

    var youtubeId = 0;
    $scope.YouTubeLinks = [{ id: youtubeId, link: '' }];

    $scope.AreThereYouTubeLinks = function () {
        for (i = 0; i < $scope.YouTubeLinks.length; i++) {
            if ($scope.YouTubeLinks[i].link != "") {
                return true;
            }
        }
        return false;
    };

    $scope.ConvertToTunes = function (youtubeVideoLinks) {

        $scope.convertedAudioFilename = "";
        $scope.exceptionMessage = "";
        $scope.successfullyConverted = null;
        
        $http.post('/api/convert',
            JSON.stringify(youtubeVideoLinks),
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

    $scope.AddLink = function () {
        youtubeId++;
        var singleYouTubeLink = { id: youtubeId, link: '' };
        $scope.YouTubeLinks.push(singleYouTubeLink);
    };

    $scope.RemoveLink = function (id) {

        if ($scope.YouTubeLinks.length == 1) return;

        for (i = 0; i < $scope.YouTubeLinks.length; i++) {
            if ($scope.YouTubeLinks[i].id == id) {
                $scope.YouTubeLinks.splice(i, 1);
            }
        }
    };
});