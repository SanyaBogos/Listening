(function () {
    'use strict';

    angular.module('TextForListening')
        .controller('TextCtrl', function ($scope, $stateParams, WordSvcRest) {

            var self = this;

            self.init = function () {
                $scope.currentTextId = $stateParams.textId;
                $scope.title = $stateParams.title;
                $scope.subTitle = $stateParams.subTitle;
                $scope.audio = '/audio/' + $stateParams.audio;
                $scope.hiddenWordsArrayInParagraphs = [];
                $scope.errorMessage = '';

                $scope.myPromise = WordSvcRest.getHiddenText($scope.currentTextId);

                $scope.currentWordIndex = {
                    paragraphIndex: 0,
                    wordIndex: 0
                };

                self.findFromPosition = function (index) {
                    for (var i = index.paragraphIndex; i < $scope.hiddenWordsArrayInParagraphs.length; i++) {
                        for (var j = index.wordIndex; j < $scope.hiddenWordsArrayInParagraphs[i].length; j++)
                            if (!$scope.hiddenWordsArrayInParagraphs[i][j].isGuessed)
                                return { paragraphIndex: i, wordIndex: j };
                        index.wordIndex = 0;
                    }
                    return false;
                };

                self.reverseFindFromPosition = function (index) {
                    var iIndex;
                    for (var i = index.paragraphIndex ; i >= 0 ; i--) {
                        for (var j = index.wordIndex; j >= 0 ; j--)
                            if (!$scope.hiddenWordsArrayInParagraphs[i][j].isGuessed)
                                return { paragraphIndex: i, wordIndex: j };
                        iIndex = i === 0 ? $scope.hiddenWordsArrayInParagraphs.length : i;
                        index.wordIndex = $scope.hiddenWordsArrayInParagraphs[iIndex - 1].length - 1;
                    }
                    return false;
                };

                self.fillParagraph = function (word) {
                    var intValue = parseInt(word);
                    if (!isNaN(intValue))
                        $scope.hiddenWordsArrayInParagraphs[$scope.hiddenWordsArrayInParagraphs.length - 1]
                            .push({ val: intValue, isGuessed: false });
                    else
                        $scope.hiddenWordsArrayInParagraphs[$scope.hiddenWordsArrayInParagraphs.length - 1]
                            .push({ val: word, isGuessed: true });
                };

                self.getHiddenText = function () {
                    console.log('id=' + $scope.currentTextId);
                    WordSvcRest.getHiddenText($scope.currentTextId)
                        .then(function (response) {
                            $scope.hiddenWordsArrayInParagraphs.splice(0, $scope.hiddenWordsArrayInParagraphs.length);
                            _.each(response.data, function (paragraph) {
                                $scope.hiddenWordsArrayInParagraphs.push([]);
                                _.each(paragraph, self.fillParagraph);
                            });
                            $scope.errorMessage = '';
                        }, function (response) {
                            $scope.errorMessage = 'Request failed';
                        });
                };

                self.getHiddenText();
            };

            self.addFunctions = function () {
                $scope.setWordIndex = function (index) {
                    $scope.currentWordIndex.paragraphIndex = index.paragraphIndex;
                    $scope.currentWordIndex.wordIndex = index.wordIndex;
                };

                $scope.checkWordIndex = function (index) {
                    if ($scope.currentWordIndex.paragraphIndex === index.paragraphIndex
                            && $scope.currentWordIndex.wordIndex === index.wordIndex)
                        return true;
                    else
                        return false;
                };

                $scope.findNext = function (index) {
                    var result = self.findFromPosition(index);
                    if (!result)
                        result = self.findFromPosition({ wordIndex: 0, paragraphIndex: 0 });
                    return result;
                };

                $scope.findPrev = function (currentIndex, lastIndex) {
                    var result = self.reverseFindFromPosition(currentIndex);
                    if (!result)
                        result = self.reverseFindFromPosition(lastIndex);
                    return result;
                };
            };

            self.addListeners = function () {
                $scope.$on('setWordIndex', function (event, data) {
                    event.currentScope.setWordIndex(data);
                });

                $scope.$on('wordGuessed', function (event) {
                    event.currentScope.currentWordIndex.wordIndex += 1;
                    var result = event.currentScope.findNext(event.currentScope.currentWordIndex);
                    if (result)
                        event.currentScope.setWordIndex(result);
                });

                $scope.$on('nextWord', function (event) {
                    event.currentScope.currentWordIndex.wordIndex += 1;
                    var result = event.currentScope.findNext(event.currentScope.currentWordIndex);
                    if (result)
                        event.currentScope.setWordIndex(result);
                });

                $scope.$on('prevWord', function (event) {
                    event.currentScope.currentWordIndex.wordIndex -= 1;
                    var arr = event.currentScope.hiddenWordsArrayInParagraphs;
                    var lastIndex = {
                        paragraphIndex: arr.length - 1,
                        wordIndex: arr[arr.length - 1].length - 1
                    };
                    var result = event.currentScope.findPrev(event.currentScope.currentWordIndex, lastIndex);
                    if (result)
                        event.currentScope.setWordIndex(result);
                });
            };

            self.init();
            self.addFunctions();
            self.addListeners();
        })
        .directive('text', function () {
            return {
                restrict: 'E',
                controller: 'TextCtrl',
                scope: {
                    textId: '='
                },
                templateUrl: 'js/angular/app/TextForListening/templates/text.html'
            };
        });
})();