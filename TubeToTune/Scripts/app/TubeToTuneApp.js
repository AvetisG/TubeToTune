var TubeToTuneApp = angular.module('TubeToTuneApp', ['angular-loading-bar'])

TubeToTuneApp.controller('TuneConvertController', function ($scope, $http) {
    
    $scope.ConvertToTune = function (youTubeVideoLink) {

        var data = { "link": youTubeVideoLink };

        $http.post('/api/tubetotune',
                JSON.stringify(data),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
           .success(function (data) {
               alert(data);
           })
           .error(function (data) {
               alert(data);
           })
    }
});