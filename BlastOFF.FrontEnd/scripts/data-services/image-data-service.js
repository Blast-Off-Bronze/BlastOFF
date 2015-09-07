define(['app', 'constants', 'request-headers', 'requester'], function (app) {
    'use strict';

    app.factory('imageDataService', function (constants, requestHeaders, requester) {
        var imageAlbumServiceUrl = constants.BASE_URL + "imageAlbums/";
        var imageServiceUrl = constants.BASE_URL + "images/";

        function addImageAlbum(imageAlbum) {

            var headers = new requestHeaders().get();

            return requester.post(headers, imageAlbumServiceUrl, imageAlbum);
        }

        function addImage(image) {

            var headers = new requestHeaders().get();

            return requester.post(headers, imageServiceUrl, image);
        }

        function deleteImageAlbum(imageAlbum) {

            var headers = new requestHeaders().get();

            return requester.delete(headers, imageAlbumServiceUrllb + imageAlbum);
        }

        function deleteImage(image) {

            var headers = new requestHeaders().get();

            return requester.delete(headers, imageServiceUrl + image);
        }

        function getMyAlbums() {

            var headers = new requestHeaders().get();

            return requester.get(headers, imageAlbumServiceUrl + 'my');
        }

        function modifyImageAlbum(imageAlbum) {

            var headers = new requestHeaders().get();

            return requester.put(headers, imageAlbumServiceUrllb + imageAlbum.Id, imageAlbum);
        }

        function modifyImage(image) {

            var headers = new requestHeaders().get();

            return requester.put(headers, imageServiceUrl + image.Id, image);
        }

        function followImageAlbum(imageAlbum){

            var headers = new requestHeaders.get();

            return requester.post(headers, imageAlbumServiceUrl + imageAlbum.Id + '/follow', null);
        }

        function unfollowImageAlbum(imageAlbum){

            var headers = new requestHeaders.get();

            return requester.delete(headers, imageAlbumServiceUrl + imageAlbum.Id + '/unfollow', null);
        }

        function likeImageAlbum(imageAlbum){

            var headers = new requestHeaders.get();

            return requester.post(headers, imageAlbumServiceUrl + imageAlbum.Id + '/like', null);
        }

        function unlikeImageAlbum(imageAlbum){

            var headers = new requestHeaders.get();

            return requester.delete(headers, imageAlbumServiceUrl + imageAlbum.Id + '/unlike', null);
        }

        function likeImage(image){

            var headers = new requestHeaders.get();

            return requester.post(headers, imageServiceUrl + image.Id + '/like', null);
        }

        function unlikeImage(image){

            var headers = new requestHeaders.get();

            return requester.delete(headers, imageServiceUrl + image.Id + '/unlike');
        }

        function commentImage(comment){

            var headers = new requestHeaders.get();

            return requester.post(headers, imageServiceUrl + comment.ImageId + "/comments", comment);
        }

        function commentImageAlbum(comment){

            var headers = new requestHeaders.get();

            return requester.post(headers, imageAlbumServiceUrl + comment.ImageAlbumId + "/comments", comment);
        }

        return {
            addImageAlbum: addImageAlbum,
            addImage: addImage,
            deleteImage: deleteImage,
            deleteImageAlbum: deleteImageAlbum,
            modifyImage: modifyImage,
            modifyImageAlbum: modifyImageAlbum,
            followImageAlbum: followImageAlbum,
            unfollowImageAlbum: unfollowImageAlbum,
            likeImageAlbum: likeImageAlbum,
            unlikeImageAlbum: unlikeImageAlbum,
            likeImage: likeImage,
            unlikeImage: unlikeImage,
            commentImage: commentImage,
            commentImageAlbum: commentImageAlbum,

            getMyAlbums: getMyAlbums
        }
    });
});