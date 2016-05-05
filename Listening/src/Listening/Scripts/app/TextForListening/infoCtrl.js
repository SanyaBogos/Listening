(function () {
    'use strict';

    angular.module('TextForListening')
        .controller('InfoCtrl', function ($scope, $uibModalInstance) {

            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            };

        });
})();