(function () {
    'use strict';

    angular.module('TextForListening')
        .controller('ButtonsWordCtrl', function ($scope, Word) {

            var self = this;

            $scope.letters = [];

            self.isGuessedWordFunc = function () {
                $scope.letterCountObj.isGuessed = $scope.letters.every(function (e) {
                    return !angular.isNumber(e.id);
                });
                //$scope.isGuessedWord = $scope.letters.every(function (e) {
                //    return !angular.isNumber(e.id);
                //});
                if ($scope.letterCountObj.isGuessed)
                    $scope.$emit('wordGuessed');

            };

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

            $scope.buildLetterArray = function (letterCountObj) {
                //$scope.isGuessedWord = Word.buildLettersArray($scope.letters, end);
                //$scope.letterCountObj = letterCountObj;
                //$scope.locator = locator;
                $scope.letterCountObj.isGuessed = Word.buildLettersArray($scope.letters, letterCountObj.val);
                //$scope.inputClass = Word.getInputClass(letterCountObj.val);
                //$scope.inputClass = self.inputClassBase = Word.getInputClass(letterCountObj.val);
            };

            $scope.buildLetterArray($scope.letterCountObj);
        })
        .directive('buttonsWord', function () {
            return {
                restrict: 'E',
                controller: 'ButtonsWordCtrl',
                scope: {
                    type: '=',
                    letterCountObj: '=',
                    locator: '=',
                    letters: '='
                },
                templateUrl: 'js/angular/app/TextForListening/templates/buttonsWord.html'
            };
        });
})();