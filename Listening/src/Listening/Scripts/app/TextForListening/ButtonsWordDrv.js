(function () {
    'use strict';

    angular.module('TextForListening')
        .controller('ButtonsWordCtrl', function ($scope, Word) {

            var self = this;

            self.init = function () {
                $scope.letters = [];

                self.isGuessedWordFunc = function () {
                    $scope.letterCountObj.isGuessed = $scope.letters.every(function (e) {
                        return !angular.isNumber(e.id);
                    });

                    if ($scope.letterCountObj.isGuessed)
                        $scope.$emit('wordGuessed');
                };

                $scope.letterCountObj.isGuessed = Word.buildLettersArray($scope.letters, $scope.letterCountObj.val);
            };

            self.addFunctions = function () {
                $scope.getCorrectLetter = function (letterIndex, event) {
                    console.log($scope.locator);
                    var newLocator = jQuery.extend(true, {}, $scope.locator);
                    newLocator.letterIndex = letterIndex;

                    if (!isNaN(parseInt(event.currentTarget.innerText))) {
                        Word.hintLetter(newLocator, $scope.letters, self.isGuessedWordFunc);
                    }
                };

                $scope.setWordIndex = function () {
                    $scope.$emit('setWordIndex', $scope.locator);
                };
            };

            self.init();
            self.addFunctions();
        })
        .directive('buttonsWord', function ($templateCache) {
            return {
                restrict: 'E',
                controller: 'ButtonsWordCtrl',
                scope: {
                    type: '=',
                    letterCountObj: '=',
                    locator: '=',
                    letters: '='
                },
                templateUrl: 'TextForListening/buttonsWord.html'
            };
        });
})();