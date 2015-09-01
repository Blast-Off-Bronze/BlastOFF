define(['app', 'file-reader-service', 'cover-image-validation-service', 'constants'], function (app) {
    app.directive('coverImageUpload', function ($timeout, fileReaderService, coverImageValidationService, constants) {
        return {
            scope: {
                ngModel: '='
            },
            link: function ($scope, element) {
                function getFile(file) {
                    fileReaderService.readAsDataURL(file, $scope)
                        .then(function (result) {
                            $timeout(function () {
                                $scope.ngModel = result;
                            });
                        });
                }

                element.bind('change', function (event) {
                    var file = (event.srcElement || event.target).files[0];
                    if (coverImageValidationService.validateImageFormat(file) &&
                        coverImageValidationService.validateImageSize(file, constants.COVER_IMAGE_SIZE_LIMIT * constants.BYTE_SIZE)
                    ) {
                        getFile(file);
                    }
                });
            }
        };
    });
});