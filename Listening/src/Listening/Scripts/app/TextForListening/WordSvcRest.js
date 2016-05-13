(function () {
    'use strict';

    angular.module('TextForListening')
        .factory('WordSvcRest', function ($http) {
            var urlBase = 'api/Word/';
            var wordRest = {};

            wordRest.getHiddenText = function (id) {
                return $http({
                    method: 'GET',
                    url: urlBase + id
                });
            };

            wordRest.getLetter = function (locator) {
                return $http({
                    method: 'GET',
                    url: urlBase + 'letter/' + locator.textId + '/' + locator.paragraphIndex + '/'
                                + locator.wordIndex + '/' + (locator.letterIndex - 1).toString()
                });
            };

            wordRest.postWord = function (locator, word) {
                return $http({
                    method: 'GET',
                    url: urlBase + 'wordCorrectness/' + locator.textId + '/' + locator.paragraphIndex + '/' + locator.wordIndex + '/' + word
                });
            };

            wordRest.postWordsArray = function (textId, words) {
                return $http({
                    method: 'POST',
                    url: urlBase + 'wordsForCheck/' + textId,
                    data: words
                });
            };

            return wordRest;
        });
})();