var TubeToTuneApp = angular.module('TubeToTuneApp', ['angular-loading-bar', 'ngRoute']);

TubeToTuneApp.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/', {
            templateUrl: '/Home/Conversion',
        }).
        when('/index', {
            templateUrl: '/Home/Conversion',
        }).
        when('/terms', {
            templateUrl: '/Home/TermsOfUse',
        }).
        otherwise({
            templateUrl: '/Home/ErrorPage'
        });
  }]);