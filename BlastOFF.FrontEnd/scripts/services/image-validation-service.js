define(['app', 'notification-service', 'constants'], function (app) {
    app.factory('imageValidationService', function (notificationService, constants) {
        var validationService = {};

        validationService.validateImageFormat = function (image) {
            if (!image) {
                return true;
            }

            if (image.type !== 'image/jpeg') {
                var error = {
                    message: constants.INVALID_IMAGE_FORMAT_MESSAGE + constants.DEFAULT_IMAGE_FORMAT
                };
                notificationService.alertError(error);
                return false;
            }

            return true;
        };

        validationService.validateImageSize = function (image, maxSize) {
            if (image.size > maxSize) {
                var error = {
                    message: constants.INVALID_COVER_IMAGE_SIZE_MESSAGE + (maxSize / constants.BYTE_SIZE) + constants.DEFAULT_FILE_SIZE_UNIT
                };

                notificationService.alertError(error);
                return false;
            }
            return true;
        };

        return validationService;
    })
});