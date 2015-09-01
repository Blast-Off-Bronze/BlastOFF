define(['app', 'notification-service', 'constants'], function (app) {
    app.factory('musicValidationService', function (notificationService, constants) {
        var validationService = {};

        validationService.validateSongFormat = function (song) {
            if (!song) {
                return true;
            }

            if (false) {
                var error = {
                    message: constants.INVALID_SONG_FORMAT_MESSAGE + constants.DEFAULT_SONG_FORMAT
                };

                notificationService.alertError(error);

                return false;
            }

            return true;
        };

        validationService.validateSongSize = function (song, maxSize) {
            if (song.size > maxSize) {
                var error = {
                    message: constants.INVALID_SONG_SIZE_MESSAGE + (maxSize / (constants.BYTE_SIZE)) + constants.DEFAULT_FILE_SIZE_UNIT
                };
                notificationService.alertError(error);
                return false;
            }
            return true;
        };

        return validationService;
    })
});