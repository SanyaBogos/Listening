(function () {
    'use strict';

    angular.module('TextForListening')
        .controller('TextJoinedCtrl', function ($scope, $stateParams) {

            $scope.currentTextId = $stateParams.textId;
            $scope.title = $stateParams.title;
            $scope.subTitle = $stateParams.subTitle;
            $scope.audio = '/audio/' + $stateParams.audio;

            $scope.text = '';
            $scope.failedAttempts = [];

        });
})();