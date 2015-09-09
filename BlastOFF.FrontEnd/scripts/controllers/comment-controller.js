define(['app', 'comment-data-service', 'storage-service', 'notification-service'],

    function (app) {
        'use strict';

        app.controller('commentController',
            function ($scope, $routeParams, commentDataService, storageService, notificationService) {
                $scope.likeComment = function(comment) {
                console.log(comment);
                    commentDataService.likeComment(comment)
                    .then(function (response) {
                        comment.isLiked = true;
                        comment.likesCount += 1;
                    }, function (error) {
                        console.log(error);
                    });
                };

                $scope.unlikeComment = function(comment) {
                    commentDataService.unlikeComment(comment)
                    .then(function (response) {
                        comment.isLiked = false;
                        comment.likesCount -= 1;
                    }, function (error) {
                        console.log(error);
                    });
                };

                $scope.deleteComment = function(comment) {
                    commentDataService.deleteComment(comment)
                    .then(function (response) {

                    }, function (error) {

                    });
                };
            });
    });