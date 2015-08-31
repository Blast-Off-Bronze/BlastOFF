define(['app'], function (app) {
    app.factory('fileReaderService', function ($q) {
        var fileReaderService = {};

        fileReaderService.readAsArrayBuffer = function (file, scope) {
            var deferrer = $q.defer();

            var reader = getReader(deferrer, scope);
            reader.readAsArrayBuffer(file);

            return deferrer.promise;
        };

        function onLoad(reader, deferrer, scope) {
            return function () {
                scope.$apply(function () {
                    deferrer.resolve(reader.result);
                });
            };
        }

        function onError(reader, deferrer, scope) {
            return function () {
                scope.$apply(function () {
                    deferrer.reject(reader.result);
                });
            };
        }

        function getReader(deferrer, scope) {
            var reader = new FileReader();
            reader.onload = onLoad(reader, deferrer, scope);
            reader.onerror = onError(reader, deferrer, scope);
            return reader;
        }

        return fileReaderService;
    });
});