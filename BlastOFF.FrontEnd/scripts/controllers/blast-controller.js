define(['app', 'blast-data-service', 'storage-service', 'notification-service'],

    function (app) {
        'use strict';

        app.controller('blastController',
            function ($scope, $routeParams, blastDataService, storageService, notificationService) {

                $scope.isLogged = storageService.isLogged();

//                $scope.noBlastsAvailable = false;
//                $scope.gettingBlasts = false;
//                $scope.previousBlastsExist = false;
//                $scope.allBlasts = [];
//
//                //blastDataService.getAllBlasts()
//                //.then(function(response) {
//                //    $scope.allBlasts = response;
//                //    console.log(response);
//                //}, function(error) {
//                //    console.log(error);
//                //});
//
//                // PAGED BLASTS
//                $scope.getPagedBlasts = function (startBlastId, pageSize) {
//                    if ($scope.gettingBlasts) {
//                        return;
//                    } else {
//                        $scope.gettingBlasts = true;
//
//                        blastDataService.getPagedBlasts("blqblq", 3, pageSize).then(
//                            function (response) {
//                                if (response.length < 1) {
//                                    $scope.noBlastsAvailable = true;
//                                } else {
//                                    $scope.allBlasts = response;
//                                    $scope.previousBlasts = true;
//                                }
//                                $scope.gettingBlasts = false;
//                            },
//                            function (error) {
//                                $scope.gettingBlasts = false;
//                                console.log(error);
//                                notificationService.alertError(error);
//                            });
//                    }
//                };

                $scope.commentBlast = function(blast) {
                    blastDataService.commentBlast(blast)
                    .then(function (response) {
                        blast.comments.unshift(response);
                        blast.commentModel.content = '';
                    }, function (error) {
                        console.log(error);
                    });
                };

                $scope.likeBlast = function(blast) {
                    console.log(blast);
                    blastDataService.likeBlast(blast)
                    .then(function (response) {
                        blast.isLiked = true;
                        blast.likesCount += 1;
                    }, function (error) {
                        console.log(error);
                    });
                };

                $scope.unlikeBlast = function(blast) {
                    blastDataService.unlikeBlast(blast)
                    .then(function (response) {
                        blast.isLiked = false;
                        blast.likesCount -= 1;
                    }, function (error) {
                        console.log(error);
                    });
                };

                $scope.deleteBlast = function(blast, collection) {
                    blastDataService.deleteBlast(blast)
                    .then(function (response) {
                        collection.splice(collection.indexOf(blast), 1);
                        console.log(response);
                    }, function (error) {
                        console.log(error);
                    });
                };

                $scope.modifyBlast = function(blast, collection) {
                    blastDataService.modifyBlast(blast)
                    .then(function (response) {
                        collection[collection.indexOf(blast)] = response;
                        console.log(response);
                    }, function (error) {
                        console.log(error);
                    });
                };
            });
    });