(function () {
    'use strict';

    angular.module('TextForListening')
        .controller('AllTextsCtrl', function ($scope, $state, TextSvcRest) {

            var self = this;

            self.init = function () {
                $scope.textsDescription = [];

                //$scope.clickTextDescription = function (text) {
                //    $state.go('currentText', {
                //        textId: text.textId,
                //        title: text.title,
                //        subTitle: text.subTitle,
                //        audio: text.audioName
                //    });
                //};

                TextSvcRest.query({ id: '' }, function (texts) {
                    if ($scope.textsDescription.length > 0) {
                        $scope.textsDescription.splice(0, $scope.textsDescription.length);
                    }

                    angular.copy(texts, $scope.textsDescription);
                });
            };

            self.init();
        })
        .directive('allTexts', function () {
            return {
                restrict: 'E',
                controller: 'AllTextsCtrl',
                scope: {
                    actionToDo: '&'
                },
                templateUrl: 'js/angular/app/TextForListening/templates/allTexts.html'
            };
        });
})();