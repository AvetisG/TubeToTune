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