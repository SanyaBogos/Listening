(function () {
    'use strict';

    angular.module('AdditionFunctionality')
        .controller('CatchKeyCtrl', function ($scope, $window) {

            var self = this;

            self.init = function () {
                self.keysPressed = [];
                self.keyConbination = {
                    ctrlLeft: [17, 37],
                    ctrlRight: [17, 39],
                    ctrlSpace: [17, 32],
                    tab: [9],
                    shiftTab: [16, 9],
                    enter: [13],
                    alt: [18]
                };
                self.audioEventCombinationDictionary = [
                    { eventName: 'minus', keyCombination: self.keyConbination.ctrlLeft },
                    { eventName: 'plus', keyCombination: self.keyConbination.ctrlRight },
                    { eventName: 'playStopAudio', keyCombination: self.keyConbination.ctrlSpace },
                    { eventName: 'nextWord', keyCombination: self.keyConbination.tab },
                    { eventName: 'prevWord', keyCombination: self.keyConbination.shiftTab }
                ];
                                               
            };

            self.addFunctions = function () {

                $scope.catchKeysDown = function (event) {

                    self.keysPressed.push(event.keyCode);

                    angular.forEach(self.audioEventCombinationDictionary, function (value) {
                        if (angular.equals(self.keysPressed, value.keyCombination))
                            $scope.$broadcast(value.eventName);
                    });

                }

                $scope.clean = function (event) {
                    var indexToRemove = self.keysPressed.indexOf(event.keyCode);
                    while (indexToRemove !== -1) {
                        self.keysPressed.splice(indexToRemove, 1);
                        indexToRemove = self.keysPressed.indexOf(event.keyCode);
                    }
                };

                $scope.cleanAll = function () {
                    self.keysPressed.splice(0, self.keysPressed.length);
                };

                $window.onfocus = function () {
                    $scope.cleanAll();
                }

            };

            self.addListeners = function () {
                $scope.$on('textPageOn', function (event) {
                    event.currentScope.cleanAll();
                });
            };

            self.init();
            self.addFunctions();
            self.addListeners();

        });
})();