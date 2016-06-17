(function () {
    'use strict';

    angular.module('TextForListening')
        .factory('TextSvc', function () {

            var self = this;

            self.fillParagraph = function (word) {
                var intValue = parseInt(word);
                if (!isNaN(intValue))
                    self.array[self.array.length - 1]
                        .push({ val: intValue, isGuessed: false });
                else
                    self.array[self.array.length - 1]
                        .push({ val: word, isGuessed: true });
            };

            return {
                buildHiddenArrays: function (promise, array) {
                    self.array = array;
                    promise
                        .then(function (response) {
                            array.splice(0, array.length);
                            _.each(response.data, function (paragraph) {
                                array.push([]);
                                _.each(paragraph, self.fillParagraph);
                            });
                            //$scope.errorMessage = '';
                        }, function (response) {
                            //$scope.errorMessage = 'Request failed';
                        });
                }
            };

        });
})();