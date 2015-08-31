define(['app', 'file-reader-service', 'music-validation-service', 'constants'], function (app) {
    app.directive('songUpload', function ($timeout, fileReaderService, musicValidationService, constants) {
        return {
            scope: {
                ngModel: '='
            },
            link: function ($scope, element) {
                function getFile(file) {
                    fileReaderService.readAsBinaryString(file, $scope)
                        .then(function (result) {
                            $timeout(function () {
                                $scope.ngModel = result;
                            });
                        });
                }

                element.bind('change', function (event) {
                    var file = (event.srcElement || event.target).files[0];
                    if (musicValidationService.validateSongFormat(file) &&
                        musicValidationService.validateSongSize(file, constants.SONG_SIZE_LIMIT * constants.BYTE_SIZE)) {
                        getFile(file);
                    }
                });
            }
        };
    });
});