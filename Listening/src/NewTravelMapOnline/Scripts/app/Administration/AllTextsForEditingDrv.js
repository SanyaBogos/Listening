(function () {
    'use strict';

    angular.module('Administration')
        .controller('AllTextsForEditingCtrl', function ($scope, $state) {

            $scope.clickForEditing = function (text) {
                $state.go('administrationEdit', {
                    textId: text.textId
                });
            };

            $scope.clickForAdding = function (text) {
                $state.go('administrationEdit');
            };

        })
        .directive('allTextsForEditing', function () {
            return {
                restrict: 'E',
                controller: 'allTextsForEditingCtrl',
                //scope: {
                //    textId: '='
                //},
                templateUrl: 'js/angular/app/Administration/templates/textEdit.html'
            };
        });
})();