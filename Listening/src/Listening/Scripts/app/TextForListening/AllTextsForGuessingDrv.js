(function () {
    'use strict';

    angular.module('TextForListening')
        .controller('AllTextsForGuessingCtrl', function ($scope, $state) {

            var self = this;
            self.separated = 'Separated';
            self.joined = 'Joined';

            $scope.mode = self.separated;

            $scope.clickTextDescription = function (text) {

                switch ($scope.mode) {
                    case self.separated:
                        $state.go('currentText', {
                            textId: text.textId,
                            title: text.title,
                            subTitle: text.subTitle,
                            audio: text.audioName
                        }); break;
                    case self.joined:
                        $state.go('currentTextJoined', {
                            textId: text.textId,
                            title: text.title,
                            subTitle: text.subTitle,
                            audio: text.audioName
                        }); break;
                    default: break;

                }
                
                $scope.$emit('textPageOn');
            };

        })
        .directive('allTextsForGuessing', function ($templateCache) {
            return {
                restrict: 'E',
                controller: 'AllTextsForGuessingCtrl',
                templateUrl: 'TextForListening/allTextsForGuessing.html'
            };
        });
})();