(function () {
    'use strict';

    angular.module('Administration')
        .controller('TextEditCtrl', function ($scope, $stateParams, $state, Upload, TextSvcRest, FileSvcRest, GenerateIdSvc) {
            var self = this;

            self.init = function () {
                $scope.textDto = {};
                $scope.successMessage = '';
                $scope.errorsFromServer = [];

                $scope.textDto.textId = $stateParams.textId || GenerateIdSvc.generateId();
                $scope.isNewElement = !$stateParams.textId;

                self.getText = function (id) {
                    $scope.textDto = TextSvcRest.get({ id: id });
                }

                if (!$scope.isNewElement) {
                    self.getText($scope.textDto.textId);
                }
            };

            self.addListeners = function () {
                $scope.$watch('file.name', function (newValue, oleValue) {
                    $scope.textDto.audioName = newValue;
                });
            };

            $scope.save = function () {
                $scope.errorsFromServer.splice(0, $scope.errorsFromServer.length);

                var dto = {
                    textId: $scope.textDto.textId,
                    title: $scope.textDto.title,
                    subTitle: $scope.textDto.subTitle,
                    text: $scope.textDto.text,
                    audioName: $scope.textDto.audioName,
                }

                if ($scope.file) {
                    //Upload.upload({
                    //    url: 'api/File/' + $scope.textDto.textId,
                    //    method: "PUT",
                    //    data: { files: $scope.file }
                    //}).then(function (resp) {
                    //    $scope.successMessage = 'File uploaded successfully!';
                    //}, function (resp) {
                    //    $scope.errorsFromServer.push(resp.data);
                    //});
                    FileSvcRest.upload($scope.textDto.textId, $scope.file);/*.then(function (resp) {
                        $scope.successMessage = 'File uploaded successfully!';
                    }, function (resp) {
                        $scope.errorsFromServer.push(resp.data);
                    });*/
                }

                if (!!$scope.errorsFromServer && !$scope.errorsFromServer.length) {
                    if ($scope.isNewElement) {
                        TextSvcRest.insert(dto);
                        $scope.isNewElement = true;
                    }
                    else {
                        TextSvcRest.update({ id: $scope.textDto.textId },
                            dto);
                    }
                }

            };

            $scope.remove = function () {

                TextSvcRest.remove({ id: $scope.textDto.textId });
                FileSvcRest.remove($scope.textDto.audioName);/*.then(function (resp) {
                    $scope.successMessage = 'File uploaded successfully!';
                }, function (resp) {
                    $scope.errorsFromServer.push(resp.data);
                });*/
                $state.go('administration');
            };

            self.init();
            self.addListeners();

        })
        .directive('textEdit', function () {
            return {
                restrict: 'E',
                controller: 'TextEditCtrl',
                //scope: {
                //    type: '=',
                //    textId: '='
                //},
                templateUrl: 'js/angular/app/Administration/templates/textEdit.html'
            };
        });
})();