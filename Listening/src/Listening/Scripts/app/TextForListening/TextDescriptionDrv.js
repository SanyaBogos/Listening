(function () {
    'use strict';

    angular.module('TextForListening')
        .controller('TextDescriptionCtrl', function ($scope, $uibModal, $templateCache) {

            $scope.animationsEnabled = true;

            $scope.showAlert = function (ev) {
                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    template: $templateCache.get('TextForListening/info.html'),
                    controller: 'InfoCtrl'
                });

                modalInstance.result.then();
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