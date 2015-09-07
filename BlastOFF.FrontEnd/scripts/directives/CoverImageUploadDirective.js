define(['app', 'file-reader-service', 'image-validation-service', 'constants'], function (app) {
    app.directive('coverImageUpload', function ($timeout, fileReaderService, imageValidationService, constants) {
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
                    if (imageValidationService.validateImageFormat(file) &&
                        imageValidationService.validateImageSize(file, constants.COVER_IMAGE_SIZE_LIMIT * constants.BYTE_SIZE)
                    ) {
                        getFile(file);
                    }
                });
            }
        };
    });
});