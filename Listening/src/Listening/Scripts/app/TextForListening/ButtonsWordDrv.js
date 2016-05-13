(function () {
    'use strict';

    angular.module('TextForListening')
        .controller('ButtonsWordCtrl', function ($scope, Word, ButtonClass) {

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

            self.addListeners = function () {
                $scope.$on('wordGueesedJoined', function (event, data) {
                    if (!!data) {
                        var locators = data.locators;
                        var word = data.word;

                        if (!!locators) {
                            for (var j = 0; j < locators.length; j++) {
                                if ($scope.locator.wordIndex === locators[j].wordIndex
                                    && $scope.locator.paragraphIndex === locators[j].paragraphIndex) {
                                    for (var i = 0; i < $scope.letters.length; i++) {
                                        if (angular.isNumber($scope.letters[i].id)) {
                                            $scope.letters[i].id = i === 0 && locators[j].isCapital ? word[i].toUpperCase() : word[i];
                                            $scope.letters[i].class = ButtonClass.guessed;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //angular.forEach(data.locators, function (value) {
                    //    if ($scope.locator.wordIndex === value.wordIndex && $scope.locator.paragraphIndex === value.paragraphIndex) {
                    //        for (var i = 0; i < $scope.letters.length; i++) {
                    //            if (angular.isNumber($scope.letters[i].id)) {
                    //                $scope.letters[i].id = word[i];
                    //                $scope.letters[i].class = ButtonClass.guessed;
                    //            }
                    //        }
                    //    }
                    //});

                });
            }

            self.init();
            self.addFunctions();
            self.addListeners();
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