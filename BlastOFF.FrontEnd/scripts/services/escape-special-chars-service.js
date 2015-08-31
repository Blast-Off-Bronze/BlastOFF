define(['app'], function (app) {
    'use strict';

    app.factory('escapeSpecialCharsService', function () {

        function escapeSpecialCharacters(input, isString) {
            if (isString) {
                return escapeHtmlSpecialCharacters(input);
            } else {
                var output = {};

                for (var property in input) {
                    if (input.hasOwnProperty(property)) {
                        if (property !== 'wantsToBeRemembered') {
                            output[property] = escapeHtmlSpecialCharacters(input[property]);
                        } else {
                            output[property] = input[property];
                        }
                    }
                }
                return output;
            }
        }

        function escapeHtmlSpecialCharacters(text) {
            var map = {
                '<': '&lt;',
                '>': '&gt;'
            };

            return text.replace(/[<>]/g, function (m) {
                return map[m];
            });
        }

        return {
            escapeSpecialCharacters: escapeSpecialCharacters
        }
    });
});