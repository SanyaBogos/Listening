(function () {
    'use strict';

    angular.module('TextForListening')
        .controller('TextDescriptionCtrl', function ($scope, $mdDialog, $mdMedia) {

            $scope.info = function () {

            };

            $scope.customFullscreen = $mdMedia('xs') || $mdMedia('sm');
            $scope.showAlert = function (ev) {
                // Appending dialog to document.body to cover sidenav in docs app
                // Modal dialogs should fully cover application
                // to prevent interaction outside of dialog
                $mdDialog.show(
                  $mdDialog.alert()
                    .parent(angular.element(document.querySelector('#popupContainer')))
                    .clickOutsideToClose(true)
                    .title('This is an alert title')
                    .textContent('You can specify some description text in here.')
                    .ariaLabel('Alert Dialog Demo')
                    .ok('Got it!')
                    .targetEvent(ev)
                );
            };

        })
        .directive('textDescription', function ($templateCache) {
            return {
                restrict: 'E',
                controller: 'TextDescriptionCtrl',
                scope: {
                    description: '='
                },
                templateUrl: 'TextForListening/textDescription.html'
            };
        });
})();