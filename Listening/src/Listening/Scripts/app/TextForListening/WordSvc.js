(function () {
    'use strict';

    angular.module('TextForListening')
        .constant('ButtonClass', {
            unguessed: 'btn-default',
            sign: 'btn-primary',
            hinted: 'btn-warning',
            guessed: 'btn-success'
        })
        .factory('Word', function (ButtonClass, WordSvcRest) {
            var word = {};

            var setHintLetter = function (locator, letter, checkIsGuessedMethod) {
                WordSvcRest.getLetter(locator)
                    .then(function (responce) {
                        letter.id = responce.data;
                        checkIsGuessedMethod();
                    }, function (responce) {
                        throw DOMException('error ' + responce.status + ' ' + responce.data);
                    });
            };

            word.getInputClass = function (wordLength) {
                if (wordLength > 14)
                    return 'edit-box-length-14';
                else
                    return 'edit-box-length-' + wordLength;
            };

            word.buildLettersArray = function (letters, wordLength) {
                if (!angular.isNumber(wordLength)) {
                    letters.push({ id: wordLength, class: ButtonClass.sign });
                    return true;
                }
                else {
                    var range = _.range(1, wordLength + 1);
                    _.each(range, function (item) {
                        letters.push({ id: item, class: ButtonClass.unguessed });
                    });
                    return false;
                }
            };

            word.hintLetter = function (locator, letters, checkIsGuessedMethod) {
                var result = letters.filter(function (e) {
                    return e.id == locator.letterIndex;
                })[0];

                setHintLetter(locator, result, checkIsGuessedMethod);
                result.class = ButtonClass.hinted;
            };

            word.checkWord = function (locator, letters, word, success, fail) {
                WordSvcRest.postWord(locator, word.replace("'","`"))
                    .then(function (response) {
                        if (response.data == true) {
                            for (var i = 0; i < letters.length; i++) {
                                if (angular.isNumber(letters[i].id)) {
                                    letters[i].id = word[i];
                                    letters[i].class = ButtonClass.guessed;
                                }
                            }
                            success();
                        }
                        else {
                            fail(word);
                        }
                    }, function (response) {
                        throw DOMException('error ' + responce.status + ' ' + responce.data);
                    });
            };

            return word;
        });
})();