(function () {
    'use strict';

    angular.module('TextForListening')
        .controller('TextJoinedCtrl', function ($scope, $stateParams, WordSvcRest, TextSvc) {

            var self = this;

            self.init = function () {
                $scope.currentTextId = $stateParams.textId;
                $scope.title = $stateParams.title;
                $scope.subTitle = $stateParams.subTitle;
                $scope.audio = '/audio/' + $stateParams.audio;
                $scope.hiddenWordsArrayInParagraphs = [];

                $scope.text = '';
                $scope.failedAttempts = [];

                $scope.myPromise = WordSvcRest.getHiddenText($scope.currentTextId);
                TextSvc.buuildHiddenArrays($scope.myPromise, $scope.hiddenWordsArrayInParagraphs);
            };


            self.init();
        });
})();