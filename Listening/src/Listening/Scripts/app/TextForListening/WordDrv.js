(function () {
    'use strict';

    angular.module('TextForListening')
        .controller('WordCtrl', function ($scope, $timeout, Word) {

            var self = this;

            self.init = function () {
                self.inputClassBase = '';

                $scope.letters = [];
                $scope.inputClass = '';
                $scope.word = '';
                $scope.failedAttempts = [];

                self.successGuessed = function () {
                    $scope.letterCountObj.isGuessed = true;
                    $scope.$emit('wordGuessed');
                };

                self.failGuessed = function (unguessedWord) {
                    $scope.failedAttempts.push(unguessedWord);
                    $scope.inputClass = self.inputClassBase + ' ' + 'red-bg-color';

                    $timeout(function () {
                        $scope.inputClass = self.inputClassBase + ' ' + 'transition-to-white-bg';
                    }, 50);
                };
            };

            $scope.checkWord = function () {
                console.log($scope.locator);
                if ($scope.failedAttempts.indexOf($scope.word) === -1)
                    Word.checkWord(
                        $scope.locator,
                        $scope.letters,
                        $scope.word,
                        self.successGuessed,
                        self.failGuessed);
            };

            $scope.discardChanges = function () {
                $scope.word = '';
            };

            $scope.isOkBtnDisabled = function () {
                return $scope.letterCountObj.val == $scope.word.length ? false : true;
            };

            $scope.pressEnter = function (event) {
                if (event.which === 13 && !$scope.isOkBtnDisabled($scope.letterCountObj.val)) {
                    $scope.checkWord($scope.locator);
                }
            };

            $scope.$watch("letterCountObj", function (newObj, oldObj) {
                if (newObj == null || newObj.val == null)
                    return;

                $scope.inputClass = self.inputClassBase = Word.getInputClass(newObj.val);
            });

            self.init();
        })

        .directive('word', function ($templateCache) {
            return {
                restrict: 'E',
                controller: 'WordCtrl',
                scope: {
                    type: '=',
                    letterCountObj: '=',
                    locator: '=',
                    checkWordIndex: '&'
                },
                templateUrl: 'TextForListening/word.html'
            };
        });
})();