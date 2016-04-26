﻿(function () {
    'use strict';

    angular.module('TextForListening')
        .controller('AllTextsForGuessingCtrl', function ($scope, $state) {

            $scope.clickTextDescription = function (text) {
                $state.go('currentText', {
                    textId: text.textId,
                    title: text.title,
                    subTitle: text.subTitle,
                    audio: text.audioName
                });

                $scope.$emit('textPageOn');
            };

        })
        .directive('allTextsForGuessing', function () {
            return {
                restrict: 'E',
                controller: 'AllTextsForGuessingCtrl',
                templateUrl: 'js/angular/app/TextForListening/templates/AllTextsForGuessing.html'
            };
        });
})();