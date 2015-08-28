define(['app'],

    function (app) {
        'use strict';

        app.directive('imageUpload', function(){
        	return {
        		restrict: 'AE',
        		link: function($scope, element, attributes) {
        			var mainSelector = '#' + attributes.imageUpload;
        			var inputElement = mainSelector + 'Input';
        			var buttonElement = mainSelector + 'Button';

        			$(buttonElement).on('click', function(event) {
        				event.preventDefault();
        				$(inputElement).click();
        			});

        			$(inputElement).on('change', function() {
        				var file = this.files[0],
        					reader;

        				if (file.type.match(/image\/.*/)) {
        					reader = new FileReader();

        					reader.onload = function() {
        						$(mainSelector).attr('src', reader.result);
        						$scope.edit[attributes.imageUpload + 'Data'] = reader.result;
        					};

        					reader.readAsDataURL(file);
        				} else {
        					// TODO: Display type-mismatch error message
        				}
        			});
        		}
        	};
        });
});