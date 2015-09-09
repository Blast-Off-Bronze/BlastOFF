define(['app', 'comment-data-service', 'storage-service', 'notification-service'],

    function (app) {
        'use strict';

        app.controller('commentController',
            function ($scope, $routeParams, commentDataService, storageService, notificationService) {
                $scope.likeComment = function(comment) {
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

                $scope.deleteComment = function(comment, commentedObject) {
                    commentDataService.deleteComment(comment)
                    .then(function (response) {
                        commentedObject.comments.splice(commentedObject.comments.indexOf(comment), 1);
                        console.log(response);
                    }, function (error) {
                        console.log(error);
                    });
                };

                $scope.modifyComment = function(comment, commentedObject) {
                    commentDataService.modifyComment(comment)
                    .then(function (response) {
                        commentedObject.comments[commentedObject.comments.indexOf(comment)] = response;
                        console.log(response);
                    }, function (error) {
                        console.log(error);
                    });
                };
            });
    });