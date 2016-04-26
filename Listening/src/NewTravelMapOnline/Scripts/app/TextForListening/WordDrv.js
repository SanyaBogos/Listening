(function () {
    'use strict';

    angular.module('TextForListening')
        .controller('WordCtrl', function ($scope, /*$http,*/ $templateCache, $timeout, Word) {

            var self = this;

            //$scope.isGuessedWord = false;
            self.init = function () {
                self.inputClassBase = '';

                $scope.letters = [];
                $scope.inputClass = '';
                $scope.word = '';
                $scope.failedAttempts = [];
                $templateCache.put('failedAttemptsTooltip.html', '<div ng-repeat="attempt in failedAttempts track by $index">{{attempt}}</div>');

                self.successGuessed = function () {
                    $scope.letterCountObj.isGuessed = true;
                    $scope.$emit('wordGuessed');
                    //$scope.isGuessedWord = true;
                };

                self.failGuessed = function (unguessedWord) {
                    $scope.failedAttempts.push(unguessedWord);
                    $scope.inputClass = self.inputClassBase + ' ' + 'red-bg-color';

                    $timeout(function () {
                        $scope.inputClass = self.inputClassBase + ' ' + 'transition-to-white-bg';
                    }, 50);
                };
            };

            //$scope.letterCountObject = {};
            //$scope.locator = {};


            /*self.isGuessedWordFunc = function () {
                $scope.letterCountObj.isGuessed = $scope.letters.every(function (e) {
                    return !angular.isNumber(e.id);
                });
                //$scope.isGuessedWord = $scope.letters.every(function (e) {
                //    return !angular.isNumber(e.id);
                //});
                if ($scope.letterCountObj.isGuessed)
                    $scope.$emit('wordGuessed');

            };*/

            //self.isGuessedTrue = function () {
            //    $scope.isGuessedWord = true;
            //};



            /*$scope.buildLetterArray = function (letterCountObj) {
                //$scope.isGuessedWord = Word.buildLettersArray($scope.letters, end);
                $scope.letterCountObj = letterCountObj;
                //$scope.locator = locator;
                $scope.letterCountObj.isGuessed = Word.buildLettersArray($scope.letters, letterCountObj.val);
                //$scope.inputClass = Word.getInputClass(letterCountObj.val);
                $scope.inputClass = self.inputClassBase = Word.getInputClass(letterCountObj.val);
            };*/

            /*$scope.getCorrectLetter = function (letterIndex, event) {
                console.log($scope.locator);
                var newLocator = jQuery.extend(true, {}, $scope.locator);
                newLocator.letterIndex = letterIndex;

                if (!isNaN(parseInt(event.currentTarget.innerText))) {
                    Word.hintLetter(newLocator, $scope.letters, self.isGuessedWordFunc);
                }
            };*/

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

            //$scope.classInit = function () {
            //};

            $scope.$watch("letterCountObj", function (newObj, oldObj) {
                if (newObj == null || newObj.val == null)
                    return;

                $scope.inputClass = self.inputClassBase = Word.getInputClass(newObj.val);
            });

            /*$scope.setWordIndex = function () {
                $scope.$emit('setWordIndex', $scope.locator);
            };*/

            //$scope.pressShiftDelete = function (event) {
            //    if (event.which === 16 && event.which === 46) {
            //        $scope.discardChanges();
            //    }
            //};

            self.init();
        })

        .directive('word', function () {
            return {
                restrict: 'E',
                controller: 'WordCtrl',
                scope: {
                    type: '=',
                    letterCountObj: '=',
                    locator: '=',
                    //setWordIndex: '&',
                    checkWordIndex: '&'
                },
                templateUrl: 'js/angular/app/TextForListening/templates/word.html'
            };
        });
})();