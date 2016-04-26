(function () {
    'use strict';

    angular.module('AdditionFunctionality')
        .controller('EmbedCtrl', function ($scope, $element) {

            $scope.$on('plus', function () {
                $element[0].currentTime = $element[0].currentTime + 1;
            })

            $scope.$on('minus', function () {
                $element[0].currentTime = $element[0].currentTime - 1;
            })

            $scope.$on('playStopAudio', function () {
                if ($element[0].paused)
                    $element[0].play();
                else
                    $element[0].pause();
            })

        })
        .directive('embedSrc', function () {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    var current = element;
                    scope.$watch(function () { return attrs.embedSrc; }, function () {
                        var clone = element
                                      .clone()
                                      .attr('src', attrs.embedSrc);
                        current.replaceWith(clone);
                        current = clone;
                    });
                }
            };
        });

})();