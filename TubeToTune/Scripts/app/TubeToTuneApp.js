var TubeToTuneApp = angular.module('TubeToTuneApp', [])

TubeToTuneApp.controller('TuneConvertController', function ($scope, $http) {
    
    $scope.ConvertToTune = function (youTubeVideoLink) {
        var data = { "link": youTubeVideoLink };
        $http.post('/api/tubetotune',
                JSON.stringify(data),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).
           success(function (data) {
               alert(data);
           })
    }
});