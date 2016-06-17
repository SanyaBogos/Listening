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
                self.failedAttempts = [];
                $scope.failedAttemptsText = '';

                $scope.myPromise = WordSvcRest.getHiddenText($scope.currentTextId);
                TextSvc.buildHiddenArrays($scope.myPromise, $scope.hiddenWordsArrayInParagraphs);
            };

            self.addFunctions = function () {
                $scope.checkWords = function () {
                    var formattedText = $scope.text.replace(/[^a-zA-Z0-9' ]/g, '')
                                           .replace(/ +(?= )/g, '');

                    var wordsToSend = formattedText.replace("'", "`").split(" ");


                    var uniqueWords = formattedText.split(" ").filter(function (item, index, inputArray) {
                        return inputArray.indexOf(item) == index;
                    });

                    WordSvcRest.postWordsArray($scope.currentTextId, wordsToSend).then(function (response) {
                        //console.log(response);
                        var correctWords = [];

                        angular.forEach(response.data, function (wordWithLocator) {
                            //console.log(wordWithLocator);
                            correctWords.push(wordWithLocator.word);
                            $scope.$broadcast('wordGueesedJoined', wordWithLocator);
                        });


                        var diff = _.difference(uniqueWords, correctWords);
                        var intersect = _.intersection(self.failedAttempts, diff);
                        self.failedAttempts = self.failedAttempts.concat(_.difference(diff, intersect));

                        $scope.text = '';
                        $scope.failedAttemptsText = self.failedAttempts.join(' ');

                    }, function (response) {

                    });
                };

                $scope.keyPress = function (ev) {
                    if (ev.keyCode === 13)
                        $scope.checkWords();
                };
            };

            self.init();
            self.addFunctions();
        });
})();