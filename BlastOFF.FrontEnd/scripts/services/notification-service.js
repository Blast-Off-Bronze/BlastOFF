define(['app', 'noty'], function (app, noty) {
	'use strict';

	app.factory('notificationService', function () {

		function alert(type, text) {
			noty({
				type: type,
				text: text,
				layout: 'top',
				theme: 'relax',
				animation: {
					open: 'animated fadeIn',
					close: 'animated fadeOut'
				},
				closeWith: ['click'],
				timeout: 2500
			});
		}

		return {
			alertSuccess: function (text) {
				return alert('success', text);
			},
			alertError: function (error) {
				var errors = [];

				if (error && error.message) {
					errors.push(error.message);
				}

				if (error && error.error_description) {
					errors.push(error.error_description);
				}

				if (error && error.modelState) {
					var modelStateErrors = error.modelState;

					for (var error in modelStateErrors) {

						var errorMessages = modelStateErrors[error];

						for (var index = 0; index < errorMessages.length; index++) {
							var currentError = errorMessages[index];
							errors.push(currentError);
						}
					}
				}

				if (errors.length > 0) {
					var text = errors.join("<br>");
				}

				return alert('error', text);
			}
		};
	})
});