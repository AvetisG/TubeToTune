TubeToTuneApp.controller('ConvertController', function ($scope, $http) {

    $scope.ConvertToTunes = function (VideoConversionDetails) {

        $scope.convertedAudioFilename = "";
        $scope.exceptionMessages = "";
        $scope.successfullyConverted = null;

        var ConversionRequest = { VideoConversionDetails: VideoConversionDetails }

        $http.post('/api/convert',
            JSON.stringify(ConversionRequest),
            {
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            .success(function (data) {

                if (data.AllFailed == true)
                {
                    $scope.successfullyConverted = false;
                }
                else
                {
                    $scope.successfullyConverted = true;
                    $scope.convertedAudioFilename = data.ConvertedAudioFilename;
                }

                $scope.exceptionMessages = data.ErrorMessages;
            })
    };
});